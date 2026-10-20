using EntityStates.Acolyte.Weapon;
using HG;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinMod.States
{
    //98% stolen from acolyte AimSacrifice, including its ESC parameters
    //the other 2% is to graft onto our BaseChannelSpellState
    public abstract class BaseChannelSpellStateTargeted : BaseChannelSpellState
    {
        // Token: 0x04004270 RID: 17008
        private HurtBox trackingTarget;
        // Token: 0x04004271 RID: 17009
        private BullseyeSearch search;
        // Token: 0x0400426F RID: 17007
        private Indicator indicator;
        // Token: 0x04004273 RID: 17011
        private List<Renderer> targetRenderers;

        private float trackerUpdateStopwatch;


        public override void OnEnter()
        {
            chargeEffectPrefab = null;
            castAtMaxCharge = false;
            base.OnEnter();
            this.search = new BullseyeSearch();
            this.indicator = new Indicator(base.gameObject, AimSacrifice.indicatorPrefab);
            this.indicator.active = true;
            this.targetRenderers = CollectionPool<Renderer, List<Renderer>>.RentCollection();
            if (base.isAuthority)
            {
                OutlineHighlight.onPreRenderOutlineHighlight = (Action<OutlineHighlight>)Delegate.Combine(OutlineHighlight.onPreRenderOutlineHighlight, new Action<OutlineHighlight>(this.OnPreRenderOutlineHighlight));
            }

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority)
            {
                FixedUpdateAuthority();
            }
        }

        public override void PerformCastSuccess(BaseCastChanneledSpellState nextState)
        {
            if (!trackingTarget)
            {
                castSuccess = false;
                this.outer.SetNextStateToMain();
                return;
            }
            BaseCastChanneledSpellStateTargeted nextStateTargeted = nextState as BaseCastChanneledSpellStateTargeted;
            if(nextStateTargeted == null)
            {
                PaladinPlugin.logger.LogError($"{GetNextState().GetType()} is not of type BaseCastChanneledSpellStateTargeted.");
            }
            nextStateTargeted.trackingTarget = trackingTarget;

            base.PerformCastSuccess(nextStateTargeted);
        }

        private void FixedUpdateAuthority()
        {
            if (IsKeyDownAuthority())
            {
                trackerUpdateStopwatch -= Time.fixedDeltaTime;
                if (trackerUpdateStopwatch <= 0f)
                {
                    trackerUpdateStopwatch += 1f / AimSacrifice.trackerUpdateFrequency;
                    Ray aimRay = new Ray(base.inputBank.aimOrigin, base.inputBank.aimDirection);
                    SearchForTarget(aimRay);
                    indicator.targetTransform = (trackingTarget ? trackingTarget.transform : null);
                }
            }
            else
            {
                trackerUpdateStopwatch = 0f;
            }
        }

        private void OnPreRenderOutlineHighlight(OutlineHighlight outlineHighlight)
        {
            if (targetRenderers == null || targetRenderers.Count <= 0)
            {
                return;
            }
            foreach (Renderer targetRenderer in targetRenderers)
            {
                outlineHighlight.AddHighlight(targetRenderer, base.hasCharged ? AimSacrifice.targetHighlightColor : Color.gray);
            }
        }


        // Token: 0x06003205 RID: 12805 RVA: 0x000FB2FC File Offset: 0x000F94FC
        public override void OnExit()
        {
            base.OnExit();
            this.indicator.active = false;
            Util.PlaySound(AimSacrifice.exitSoundString, base.gameObject);

            if (base.isAuthority && !this.outer.destroying && !this.trackingTarget)
            {
                GenericSkill activatorSkillSlot = base.activatorSkillSlot;
                int stock = activatorSkillSlot.stock;
                activatorSkillSlot.stock = stock + 1;
            }
            OutlineHighlight.onPreRenderOutlineHighlight = (Action<OutlineHighlight>)Delegate.Remove(OutlineHighlight.onPreRenderOutlineHighlight, new Action<OutlineHighlight>(this.OnPreRenderOutlineHighlight));
            this.SetTarget(null);
            this.targetRenderers = CollectionPool<Renderer, List<Renderer>>.ReturnCollection(this.targetRenderers);
        }
        // Token: 0x0600320A RID: 12810 RVA: 0x000FB724 File Offset: 0x000F9924
        private void SetTarget(HurtBox newTarget)
        {
            this.targetRenderers.Clear();
            if (this.trackingTarget)
            {
                this.trackingTarget.healthComponent.isSacrificeTarget = false;
            }
            if (newTarget && newTarget.healthComponent)
            {
                newTarget.healthComponent.isSacrificeTarget = true;
                ModelLocator modelLocator = newTarget.healthComponent.body.modelLocator;
                CharacterModel characterModel;
                if (modelLocator && modelLocator.modelTransform && modelLocator.modelTransform.TryGetComponent<CharacterModel>(out characterModel))
                {
                    foreach (CharacterModel.RendererInfo rendererInfo in characterModel.baseRendererInfos)
                    {
                        if (!rendererInfo.ignoreOverlays)
                        {
                            this.targetRenderers.Add(rendererInfo.renderer);
                        }
                    }
                }
            }
            this.trackingTarget = newTarget;
        }

        // Token: 0x06003209 RID: 12809 RVA: 0x000FB52C File Offset: 0x000F972C
        public virtual void SearchForTarget(Ray aimRay)
        {
            HurtBox hurtBox2 = null;
            if (Util.CharacterSpherecast(base.gameObject, aimRay, AimSacrifice.sphereCastRadius, out var hitInfo, AimSacrifice.searchDistance, LayerIndex.CommonMasks.bullet, QueryTriggerInteraction.Ignore) && hitInfo.collider.TryGetComponent<HurtBox>(out var component) && (bool)component.healthComponent && component.healthComponent.alive && IsValidTarget(component))
            {
                if ((bool)component.hurtBoxGroup && (bool)component.hurtBoxGroup.mainHurtBox)
                {
                    component = component.hurtBoxGroup.mainHurtBox;
                }
                hurtBox2 = component;
            }
            if (hurtBox2 == null)
            {
                search.teamMaskFilter = TeamMask.all;
                search.filterByLoS = true;
                search.searchOrigin = aimRay.origin;
                search.searchDirection = aimRay.direction;
                search.sortMode = BullseyeSearch.SortMode.Angle;
                search.maxDistanceFilter = AimSacrifice.searchDistance;
                search.maxAngleFilter = AimSacrifice.searchAngle;
                search.RefreshCandidates();
                search.FilterOutGameObject(base.gameObject);
                List<HurtBox> result;
                CollectionPool<HurtBox, List<HurtBox>>.DisposableRental disposableRental = CollectionPool<HurtBox, List<HurtBox>>.RentCollection(out result);
                try
                {
                    search.GetResults(result);
                    foreach (HurtBox item in result)
                    {
                        if ((bool)item && (bool)item.healthComponent && item.healthComponent.alive && IsValidTarget(item))
                        {
                            hurtBox2 = item;
                            break;
                        }
                    }
                }
                finally
                {
                    disposableRental.Dispose();
                }
            }
            if (hurtBox2 != trackingTarget)
            {
                SetTarget(hurtBox2);
            }
        }

        bool IsValidTarget(HurtBox hurtBox)
        {
            if (!hurtBox || !hurtBox.healthComponent)
            {
                return false;
            }
            if (hurtBox.healthComponent.godMode)
            {
                return false;
            }
            if ((bool)hurtBox.healthComponent.GetComponent<ProjectileController>())
            {
                return false;
            }
            return VerifyValidTarget(hurtBox);
        }

        protected virtual bool VerifyValidTarget(HurtBox hurtBox)
        {
            TeamMask teamFilter = TeamMask.GetUnprotectedTeams(base.teamComponent.teamIndex);
            bool flag = teamFilter.HasTeam(hurtBox.teamIndex);
            return flag;
        }
    }
}