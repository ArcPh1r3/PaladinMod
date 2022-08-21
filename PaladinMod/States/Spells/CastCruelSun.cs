using EntityStates;
using PaladinMod.Misc;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.States.Spell
{
    public class CastCruelSun : BaseCastChanneledSpellState
    {
        protected virtual GameObject sunPrefab => Modules.Assets.paladinSunPrefab;
        protected GameObject sunInstance;
        private Vector3? sunSpawnPosition;

        public override void OnEnter() {
            this.baseDuration = this.overrideDuration = StaticValues.cruelSunDuration;
            this.muzzleflashEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/ExplosionSolarFlare");
            this.projectilePrefab = null;
            this.castSoundString = Modules.Sounds.CastTorpor;

            SetCancelSkillOverride();

            base.OnEnter();

            if (NetworkServer.active) {
                this.sunSpawnPosition = this.characterBody.corePosition + new Vector3(0f, 11f, 0f);
                if (sunPrefab && sunSpawnPosition != null) sunInstance = SpawnPaladinSun(sunPrefab, sunSpawnPosition.Value);
            }

            //What does this do??? It's VFX but
            Transform modelTransform = base.GetModelTransform();
            if (modelTransform) {
                TemporaryOverlay temporaryOverlay = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay.duration = this.baseDuration;
                temporaryOverlay.animateShaderAlpha = true;
                temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay.destroyComponentOnEnd = true;
                temporaryOverlay.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matGrandparentTeleportOutBoom");
                temporaryOverlay.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
            }

            base.cameraTargetParams.RemoveParamsOverride(camParamsOverrideHandle, 1f);
            camParamsOverrideHandle = Modules.CameraParams.OverridePaladinCameraParams(base.cameraTargetParams, PaladinCameraParams.CRUEL_SUN, 1f);
        }

        protected virtual void SetCancelSkillOverride(bool shouldOverride = true) {

            if (shouldOverride) {
                skillLocator.special.SetSkillOverride(this, Modules.Skills.cancelSunSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            } else {
                skillLocator.special.UnsetSkillOverride(this, Modules.Skills.cancelSunSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            }
        }

        public override void FixedUpdate()
        {
            characterBody.isSprinting = false;

            if (base.isAuthority && base.inputBank && base.fixedAge >= 0.2f)
            {
                if (base.inputBank.sprint.justPressed && !PaladinPlugin.autoSprintInstalled) {
                    this.outer.SetNextStateToMain();
                    return;
                }
            }

            base.FixedUpdate();
        }

        protected override void PlayCastAnimation()
        {
            base.PlayAnimation("Gesture, Override", "CastSun", "Spell.playbackRate", 0.25f);
        }

        protected override void OnChanneledSpellExit()
        {

            if (NetworkServer.active) {

                //Effect moved here so that Scepter version doesn't get an explosion when starting the throw.
                if ((bool)Modules.Assets.paladinSunSpawnPrefab) {
                    EffectManager.SimpleImpactEffect(Modules.Assets.paladinSunSpawnPrefab, sunInstance.transform.position, Vector3.up, transmit: true);
                }
                this.sunInstance.GetComponent<GenericOwnership>().ownerObject = null;
                this.sunInstance = null;
            }

            SetCancelSkillOverride(false);

            base.PlayAnimation("Gesture, Override", "CastSunEnd", "Spell.playbackRate", 0.8f);
        }

        private GameObject SpawnPaladinSun(GameObject prefab, Vector3 spawnPosition)
        {
            GameObject sun = UnityEngine.Object.Instantiate<GameObject>(prefab, spawnPosition, Quaternion.identity, transform);
            sun.GetComponent<GenericOwnership>().ownerObject = base.gameObject;
            NetworkServer.Spawn(sun);
            sun.GetComponent<PaladinSunNetworkController>().RpcPosition(gameObject);
            return sun;
        }
    }
}