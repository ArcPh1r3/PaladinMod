using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using static RoR2.CameraTargetParams;

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
        public Material overrideAreaIndicatorMat;

        public bool zooming = true;
        private bool zoomRequested;
        private CameraParamsOverrideHandle camParamsOverrideHandle;
        private AimRequest aimRequest;

        private bool hasCharged;
        protected bool castAtMaxCharge;
        private GameObject defaultCrosshairPrefab;
        private uint loopSoundInstanceId;
        private float duration { get; set; }
        private Animator animator { get; set; }
        private ChildLocator childLocator { get; set; }
        private GameObject chargeEffectInstance { get; set; }
        protected GameObject areaIndicatorInstance { get; set; }

        protected bool castSuccess;

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

            this.PlayChannelAnimation();
            this.loopSoundInstanceId = Util.PlayAttackSpeedSound(this.chargeSoundString, base.gameObject, this.attackSpeedStat);
            this.defaultCrosshairPrefab = base.characterBody._defaultCrosshairPrefab;

            if (this.crosshairOverridePrefab)
            {
                base.characterBody._defaultCrosshairPrefab = this.crosshairOverridePrefab;
            }

            if (NetworkServer.active) base.characterBody.AddBuff(RoR2Content.Buffs.Slow50);

            if (EntityStates.Huntress.ArrowRain.areaIndicatorPrefab)
            {
                this.areaIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(EntityStates.Huntress.ArrowRain.areaIndicatorPrefab);
                this.areaIndicatorInstance.transform.localScale = Vector3.zero;

                if (this.overrideAreaIndicatorMat) this.areaIndicatorInstance.GetComponentInChildren<MeshRenderer>().material = this.overrideAreaIndicatorMat;
            }

            if (this.zooming) {

                //aimRequest = base.cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);
                camParamsOverrideHandle = Modules.CameraParams.OverridePaladinCameraParams(base.cameraTargetParams, PaladinCameraParams.CHANNEL, 0.5f);
            }
        }

        protected virtual void PlayChannelAnimation()
        {
            if (this.characterBody.outOfCombat && this.characterBody.outOfDanger) base.PlayAnimation("Gesture, Override", "ChannelSpellRest", "Spell.playbackRate", 0.85f);
            else base.PlayAnimation("Gesture, Override", "ChannelSpell", "Spell.playbackRate", 0.85f);
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

        protected float CalcCharge()
        {
            return Mathf.Clamp01(base.fixedAge / this.duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterBody.isSprinting = false;
            base.StartAimMode(0.5f, false);
            base.characterBody.outOfCombatStopwatch = 0f;

            float charge = this.CalcCharge();

            if (this.areaIndicatorInstance)
            {
                float size = Util.Remap(charge, 0, 1, 0, this.maxSpellRadius);

                this.areaIndicatorInstance.transform.localScale = new Vector3(size, size, size);
            }

            if (charge >= 0.8f && this.zooming && !zoomRequested)
            {
                zoomRequested = true;
                base.cameraTargetParams.RemoveParamsOverride(camParamsOverrideHandle, 1f);
                camParamsOverrideHandle = Modules.CameraParams.OverridePaladinCameraParams(base.cameraTargetParams, PaladinCameraParams.CHANNEL_FULL, 0.4f);
            }

            if (charge >= 1f)
            {
                if (!this.hasCharged)
                {
                    this.hasCharged = true;
                    Util.PlaySound(Modules.Sounds.ChannelMax, base.gameObject);
                }
            }

            if (base.isAuthority && base.inputBank && base.fixedAge >= 0.2f)
            {
                if (base.inputBank.sprint.wasDown)
                {
                    base.characterBody.isSprinting = true;
                    this.RefundCooldown();
                    castSuccess = false;
                    this.outer.SetNextStateToMain();
                    return;
                }
            }

            if (base.isAuthority && (!base.IsKeyDownAuthority() || castAtMaxCharge ) && charge >= 1f)
            {
                castSuccess = true;
                BaseCastChanneledSpellState nextState = this.GetNextState();

                Transform indicatorTransform = this.areaIndicatorInstance ? this.areaIndicatorInstance.transform : transform;

                nextState.spellPosition = indicatorTransform.position;
                nextState.spellRotation = indicatorTransform.rotation;
                nextState.camParamsOverrideHandle = camParamsOverrideHandle;
                nextState.aimRequest = aimRequest;

                this.outer.SetNextState(nextState);
            }
        }

        public override void Update() {
            base.Update();
            this.UpdateAreaIndicator();
        }

        public override void OnExit() {
            if (this.crosshairOverridePrefab) {
                base.characterBody._defaultCrosshairPrefab = this.defaultCrosshairPrefab;
            } else {
                base.characterBody.hideCrosshair = false;
            }

            if (this.areaIndicatorInstance) {
                EntityState.Destroy(this.areaIndicatorInstance.gameObject);
            }

            AkSoundEngine.StopPlayingID(this.loopSoundInstanceId);

            if (!this.outer.destroying) {
                this.EndAnimation();
            }

            if (this.zooming && !castSuccess) {

                //base.cameraTargetParams.RemoveRequest(aimRequest);
                base.cameraTargetParams.RemoveParamsOverride(camParamsOverrideHandle, 0.3f);
            }

            if (NetworkServer.active) base.characterBody.RemoveBuff(RoR2Content.Buffs.Slow50);

            if (this.chargeEffectInstance) EntityState.Destroy(this.chargeEffectInstance);

            base.OnExit();
        }

        protected virtual void EndAnimation() {
            base.PlayAnimation("Gesture, Override", "BufferEmpty");
        }

        private void RefundCooldown()
        {
            base.activatorSkillSlot.rechargeStopwatch = (0.9f * base.activatorSkillSlot.finalRechargeInterval);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}