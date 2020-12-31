using EntityStates;
using PaladinMod.Misc;
using RoR2;
using UnityEngine;

namespace PaladinMod.States
{
    public abstract class BaseChargeSpellState : BaseSkillState
    {
        protected abstract BaseThrowSpellState GetNextState();
        public GameObject chargeEffectPrefab;
        public string chargeSoundString;
        public float baseDuration = 1.5f;
        public float minBloomRadius;
        public float maxBloomRadius;
        public GameObject crosshairOverridePrefab;
        protected static readonly float minChargeDuration = 0.5f;

        private GameObject defaultCrosshairPrefab;
        private uint loopSoundInstanceId;
        private float duration { get; set; }
        private Animator animator { get; set; }
        private ChildLocator childLocator { get; set; }
        private GameObject chargeEffectInstance { get; set; }
        private PaladinSwordController swordController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.animator = base.GetModelAnimator();
            this.childLocator = base.GetModelChildLocator();
            this.swordController = base.GetComponent<PaladinSwordController>();

            if (this.swordController) this.swordController.attacking = true;

            if (this.childLocator)
            {
                Transform transform = this.childLocator.FindChild("HandL");

                if (transform && this.chargeEffectPrefab)
                {
                    this.chargeEffectInstance = UnityEngine.Object.Instantiate<GameObject>(this.chargeEffectPrefab, transform.position, transform.rotation);
                    this.chargeEffectInstance.transform.parent = transform;
                    ScaleParticleSystemDuration component = this.chargeEffectInstance.GetComponent<ScaleParticleSystemDuration>();
                    ObjectScaleCurve component2 = this.chargeEffectInstance.GetComponent<ObjectScaleCurve>();
                    if (component)
                    {
                        component.newDuration = this.duration;
                    }
                    if (component2)
                    {
                        component2.timeMax = this.duration;
                    }
                }
            }

            base.PlayAnimation("Gesture, Override", "ChargeSpell", "ChargeSpell.playbackRate", 0.4f * this.duration);
            this.loopSoundInstanceId = Util.PlayScaledSound(this.chargeSoundString, base.gameObject, this.attackSpeedStat);
            this.defaultCrosshairPrefab = base.characterBody.crosshairPrefab;

            if (this.crosshairOverridePrefab)
            {
                base.characterBody.crosshairPrefab = this.crosshairOverridePrefab;
            }

            base.StartAimMode(this.duration + 2f, false);
        }

        public override void OnExit()
        {
            if (base.characterBody)
            {
                base.characterBody.crosshairPrefab = this.defaultCrosshairPrefab;
            }

            AkSoundEngine.StopPlayingID(this.loopSoundInstanceId);

            if (!this.outer.destroying)
            {
                base.PlayAnimation("Gesture, Override", "BufferEmpty");
            }

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
            //base.characterBody.isSprinting = false;

            float charge = this.CalcCharge();
            if (base.isAuthority && ((!base.IsKeyDownAuthority() && base.fixedAge >= BaseChargeSpellState.minChargeDuration) || base.fixedAge >= this.duration))
            {
                BaseThrowSpellState nextState = this.GetNextState();
                nextState.charge = charge;
                this.outer.SetNextState(nextState);
            }
        }

        public override void Update()
        {
            base.Update();
            base.characterBody.SetSpreadBloom(Util.Remap(this.CalcCharge(), 0f, 1f, this.minBloomRadius, this.maxBloomRadius), true);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
