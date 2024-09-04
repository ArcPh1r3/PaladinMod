using EntityStates;
using RoR2;
using UnityEngine;

namespace PaladinMod.States.Quickstep
{
    public class QuickstepSimple : BaseState
    {
        protected Vector3 slipVector = Vector3.zero;
        public float duration = 0.25f;
        public float speedCoefficient = 5.5f;
        private Vector3 cachedForward;

        public override void OnEnter()
        {
            base.OnEnter();
            this.slipVector = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
            this.cachedForward = this.characterDirection.forward;

            Animator anim = this.GetModelAnimator();

            Vector3 rhs = base.characterDirection ? base.characterDirection.forward : this.slipVector;
            Vector3 rhs2 = Vector3.Cross(Vector3.up, rhs);
            float num = Vector3.Dot(this.slipVector, rhs);
            float num2 = Vector3.Dot(this.slipVector, rhs2);
            anim.SetFloat("dashF", num);
            anim.SetFloat("dashR", num2);

            base.PlayCrossfade("FullBody, Override", "Dash", "Whirlwind.playbackRate", this.duration,  0.05f);
            //base.PlayAnimation("Gesture, Override", "BufferEmpty");

            Util.PlaySound(EntityStates.BrotherMonster.BaseSlideState.soundString, base.gameObject);

            this.ApplyBuff();
            this.CreateDashEffect();
        }

        public virtual void ApplyBuff()
        {
            if (base.isAuthority) base.healthComponent.AddBarrierAuthority(StaticValues.dashBarrierAmount * base.healthComponent.fullBarrier);
        }

        public virtual void CreateDashEffect()
        {
            EffectData effectData = new EffectData();
            effectData.rotation = Util.QuaternionSafeLookRotation(this.slipVector);
            effectData.origin = base.characterBody.corePosition;

            EffectManager.SpawnEffect(Modules.Asset.dashFX, effectData, false);
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterMotor.velocity = Vector3.zero;
            base.characterMotor.rootMotion += this.slipVector * (this.moveSpeedStat * this.speedCoefficient * Time.deltaTime) * Mathf.Cos(base.fixedAge / (this.duration * 1.3f) * 1.57079637f);

            if (base.isAuthority)
            {
                if (base.characterDirection)
                {
                    base.characterDirection.forward = this.cachedForward;
                }
            }

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public virtual void DampenVelocity()
        {
            base.characterMotor.velocity *= 0.4f;
        }

        public override void OnExit()
        {
            this.DampenVelocity();
            //base.PlayAnimation("FullBody, Override", "BufferEmpty");

            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}