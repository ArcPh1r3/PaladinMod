﻿using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.States
{
    public abstract class BaseChannelSpellState : BaseSkillState
    {
        protected abstract BaseCastChanneledSpellState GetNextState();
        public GameObject chargeEffectPrefab;
        public string chargeSoundString;
        public GameObject crosshairOverridePrefab;
        public float maxSpellRadius;
        public float baseDuration = 3f;

        private bool hasCharged;
        private GameObject defaultCrosshairPrefab;
        private uint loopSoundInstanceId;
        private float duration { get; set; }
        private Animator animator { get; set; }
        private ChildLocator childLocator { get; set; }
        private GameObject chargeEffectInstance { get; set; }
        private GameObject areaIndicatorInstance { get; set; }

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.animator = base.GetModelAnimator();
            this.childLocator = base.GetModelChildLocator();

            if (this.childLocator)
            {
                Transform transform = this.childLocator.FindChild("HandL");

                if (transform && this.chargeEffectPrefab)
                {
                    this.chargeEffectInstance = UnityEngine.Object.Instantiate<GameObject>(this.chargeEffectPrefab, transform.position, transform.rotation);
                    this.chargeEffectInstance.transform.parent = transform;

                    ScaleParticleSystemDuration scaleParticleSystemDuration = this.chargeEffectInstance.GetComponent<ScaleParticleSystemDuration>();
                    ObjectScaleCurve scaleCurve = this.chargeEffectInstance.GetComponent<ObjectScaleCurve>();

                    if (scaleParticleSystemDuration) scaleParticleSystemDuration.newDuration = this.duration;
                    if (scaleCurve) scaleCurve.timeMax = this.duration;
                }
            }

            base.PlayAnimation("Gesture, Override", "ChannelSpell", "Spell.playbackRate", 0.85f);
            this.loopSoundInstanceId = Util.PlayScaledSound(this.chargeSoundString, base.gameObject, this.attackSpeedStat);
            this.defaultCrosshairPrefab = base.characterBody.crosshairPrefab;

            if (base.cameraTargetParams)
            {
                base.cameraTargetParams.aimMode = CameraTargetParams.AimType.AimThrow;
            }

            if (this.crosshairOverridePrefab)
            {
                base.characterBody.crosshairPrefab = this.crosshairOverridePrefab;
            }

            if (NetworkServer.active) base.characterBody.AddBuff(BuffIndex.Slow50);

            if (EntityStates.Huntress.ArrowRain.areaIndicatorPrefab)
            {
                this.areaIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(EntityStates.Huntress.ArrowRain.areaIndicatorPrefab);
                this.areaIndicatorInstance.transform.localScale = Vector3.zero;
            }
        }

        private void UpdateAreaIndicator()
        {
            if (this.areaIndicatorInstance)
            {
                float maxDistance = 128f;

                Ray aimRay = base.GetAimRay();
                RaycastHit raycastHit;
                if (Physics.Raycast(aimRay, out raycastHit, maxDistance, LayerIndex.CommonMasks.bullet))
                {
                    this.areaIndicatorInstance.transform.position = raycastHit.point;
                    this.areaIndicatorInstance.transform.up = raycastHit.normal;
                }
                else
                {
                    this.areaIndicatorInstance.transform.position = aimRay.GetPoint(maxDistance);
                    this.areaIndicatorInstance.transform.up = -aimRay.direction;
                }
            }
        }

        public override void OnExit()
        {
            if (this.crosshairOverridePrefab)
            {
                base.characterBody.crosshairPrefab = this.defaultCrosshairPrefab;
            }
            else
            {
                base.characterBody.hideCrosshair = false;
            }

            if (this.areaIndicatorInstance)
            {
                EntityState.Destroy(this.areaIndicatorInstance.gameObject);
            }

            AkSoundEngine.StopPlayingID(this.loopSoundInstanceId);

            if (!this.outer.destroying)
            {
                base.PlayAnimation("Gesture, Override", "BufferEmpty");
            }

            if (base.cameraTargetParams)
            {
                base.cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;
            }

            if (NetworkServer.active) base.characterBody.RemoveBuff(BuffIndex.Slow50);

            EntityState.Destroy(this.chargeEffectInstance);
            base.OnExit();
        }

        protected float CalcCharge()
        {
            return Mathf.Clamp01(base.fixedAge / this.duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterBody.isSprinting = false;
            base.StartAimMode(0.5f, false);

            float charge = this.CalcCharge();

            if (this.areaIndicatorInstance)
            {
                float size = Util.Remap(charge, 0, 1, 0, this.maxSpellRadius);

                this.areaIndicatorInstance.transform.localScale = new Vector3(size, size, size);
            }

            if (charge >= 1f)
            {
                if (!this.hasCharged)
                {
                    this.hasCharged = true;
                    Util.PlaySound(Modules.Sounds.ChannelMax, base.gameObject);

                    if (base.cameraTargetParams)
                    {
                        base.cameraTargetParams.aimMode = CameraTargetParams.AimType.Aura;
                    }
                }
            }

            if (base.isAuthority && base.inputBank && base.fixedAge >= 0.2f)
            {
                if (base.inputBank.sprint.wasDown)
                {
                    this.RefundCooldown();
                    this.outer.SetNextStateToMain();
                    return;
                }
            }

            if (base.isAuthority && !base.IsKeyDownAuthority() && charge >= 1f)
            {
                BaseCastChanneledSpellState nextState = this.GetNextState();
                if (this.areaIndicatorInstance)
                {
                    nextState.spellPosition = this.areaIndicatorInstance.transform.position;
                    nextState.spellRotation = this.areaIndicatorInstance.transform.rotation;
                }
                else
                {
                    nextState.spellPosition = base.transform.position;
                    nextState.spellRotation = this.areaIndicatorInstance.transform.rotation;
                }
                this.outer.SetNextState(nextState);
            }
        }

        private void RefundCooldown()
        {
            base.skillLocator.special.RunRecharge(0.9f * 18f);
        }

        public override void Update()
        {
            base.Update();
            this.UpdateAreaIndicator();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
