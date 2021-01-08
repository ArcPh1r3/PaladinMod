using EntityStates;
using RoR2;
using UnityEngine;

namespace PaladinMod.States.Quickstep
{
    public class QuickstepSimple : BaseState
    {
        private Vector3 slipVector = Vector3.zero;
        public float duration = 0.25f;
        public float speedCoefficient = 5.5f;

        public override void OnEnter()
        {
            base.OnEnter();
            this.slipVector = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;

            base.PlayCrossfade("FullBody, Override", "DashForward", 0.05f);
            base.PlayAnimation("Gesture, Override", "BufferEmpty");

            Util.PlaySound(EntityStates.BrotherMonster.BaseSlideState.soundString, base.gameObject);

            if (base.isAuthority)
            {
                base.healthComponent.AddBarrierAuthority(StaticValues.dashBarrierAmount * base.healthComponent.fullBarrier);

                //EffectData effectData = new EffectData();
                //effectData.rotation = Util.QuaternionSafeLookRotation(this.slipVector);
                //effectData.origin = base.gameObject.GetComponent<CharacterBody>().corePosition;

                //EffectManager.SpawnEffect(Modules.Assets.dashFX, effectData, false);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterMotor.velocity = Vector3.zero;
            base.characterMotor.rootMotion = this.slipVector * (this.moveSpeedStat * this.speedCoefficient * Time.fixedDeltaTime) * Mathf.Cos(base.fixedAge / (this.duration * 1.3f) * 1.57079637f);

            if (base.isAuthority)
            {
                if (base.characterDirection)
                {
                    base.characterDirection.forward = this.slipVector;
                }
            }

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.characterMotor.velocity *= 0.1f;
            base.PlayAnimation("FullBody, Override", "BufferEmpty");

            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
