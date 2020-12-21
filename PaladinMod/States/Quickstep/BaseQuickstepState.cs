using System;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.States.Quickstep
{
    public class BaseQuickstepState : BaseState
    {
        public static float baseDuration = 0.3f;
        protected Vector3 slideVector;
        protected Quaternion slideRotation;

        public override void OnEnter()
        {
            base.OnEnter();
            Util.PlaySound(EntityStates.BrotherMonster.BaseSlideState.soundString, base.gameObject);
            base.characterBody.isSprinting = true;

            if (base.isAuthority)
            {
                base.healthComponent.AddBarrierAuthority(StaticValues.dashBarrierAmount * base.healthComponent.fullBarrier);

                base.characterMotor.velocity *= 0.1f;
            }

            if (EntityStates.BrotherMonster.BaseSlideState.slideEffectPrefab && base.characterBody)
            {
                Vector3 position = base.characterBody.corePosition;
                Quaternion rotation = Quaternion.identity;
                Transform transform = base.FindModelChild("Base");

                if (transform)
                {
                    position = transform.position;
                }

                if (base.characterDirection)
                {
                    rotation = Util.QuaternionSafeLookRotation(this.slideRotation * base.characterDirection.forward, Vector3.up);
                }

                //EffectManager.SimpleEffect(EntityStates.BrotherMonster.BaseSlideState.slideEffectPrefab, position, rotation, false);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterBody.isSprinting = true;

            if (base.isAuthority)
            {
                Vector3 a = Vector3.zero;
                if (base.inputBank && base.characterDirection)
                {
                    a = base.characterDirection.forward;
                }

                if (base.characterMotor)
                {
                    float num = EntityStates.BrotherMonster.BaseSlideState.speedCoefficientCurve.Evaluate(base.fixedAge / BaseQuickstepState.baseDuration);
                    base.characterMotor.rootMotion += 0.6f * (this.slideRotation * (num * this.moveSpeedStat * a * Time.fixedDeltaTime));

                    base.characterMotor.velocity.y = 0f;
                }

                if (base.fixedAge >= BaseQuickstepState.baseDuration)
                {
                    this.outer.SetNextStateToMain();
                }
            }
        }

        public override void OnExit()
        {
            if (!this.outer.destroying)
            {
                this.PlayImpactAnimation();
            }

            base.PlayAnimation("FullBody, Override", "BufferEmpty");

            base.OnExit();
        }

        private void PlayImpactAnimation()
        {
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
