using PaladinMod.Misc;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.States.Spell
{
    public class CastCruelSun : BaseCastChanneledSpellState
    {
        private GameObject sunInstance;
        public Vector3? sunSpawnPosition;
        protected PaladinSwordController swordController;

        public override void OnEnter()
        {
            this.baseDuration = this.overrideDuration = StaticValues.cruelSunDuration;
            this.muzzleflashEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/ExplosionSolarFlare");
            this.projectilePrefab = null;
            this.castSoundString = Modules.Sounds.CastTorpor;
            this.swordController = base.gameObject.GetComponent<PaladinSwordController>();

            base.OnEnter();

            if (NetworkServer.active)
            {
                this.sunSpawnPosition = this.characterBody.corePosition + new Vector3(0f, 10f, 0f);
                if (this.sunSpawnPosition != null) this.sunInstance = this.SpawnPaladinSun(this.sunSpawnPosition.Value);
            }

            //What does this do???
            Transform modelTransform = base.GetModelTransform();
            if (modelTransform)
            {
                TemporaryOverlay temporaryOverlay = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay.duration = this.baseDuration;
                temporaryOverlay.animateShaderAlpha = true;
                temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay.destroyComponentOnEnd = true;
                temporaryOverlay.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matGrandparentTeleportOutBoom");
                temporaryOverlay.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
            }

            base.camParamsOverrideHandle = Modules.CameraParams.OverridePaladinCameraParams(base.cameraTargetParams, PaladinCameraParams.CRUEL_SUN, 1f);
        }

        public override void FixedUpdate()
        {
            this.characterBody.isSprinting = false;
            //if (NetworkServer.active) sunInstance.transform.position = this.characterBody.transform.position + new Vector3(0f, 10f, 0f);
            base.FixedUpdate();
        }

        protected override void PlayCastAnimation()
        {
            base.PlayAnimation("Gesture, Override", "CastSun", "Spell.playbackRate", 0.25f);
        }

        public override void OnExit()
        {
            if (NetworkServer.active && this.sunInstance)
            {
                this.sunInstance.GetComponent<GenericOwnership>().ownerObject = null;
                this.sunInstance = null;
            }

            base.OnExit();

            base.PlayAnimation("Gesture, Override", "CastSunEnd", "Spell.playbackRate", 0.8f);
        }

        private GameObject SpawnPaladinSun(Vector3 sunSpawnPosition)
        {
            GameObject sun = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.paladinSunPrefab, sunSpawnPosition, Quaternion.identity, this.characterBody.transform);
            sun.GetComponent<GenericOwnership>().ownerObject = base.gameObject;
            NetworkServer.Spawn(sun);
            return sun;
        }
    }
}