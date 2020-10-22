using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.States
{
    public class DashForward : BaseSkillState
    {
        public static float baseDuration = 0.25f;
        public static float initialSpeedCoefficient = 16f;
        public static float finalSpeedCoefficient = 0.5f;

        private float dashSpeed;
        private Vector3 forwardDirection;
        private Vector3 previousPosition;
        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = DashForward.baseDuration;

            Util.PlayScaledSound(EntityStates.Croco.Leap.leapSoundString, base.gameObject, 1.75f);
            base.PlayAnimation("FullBody, Override", "DashForward", "Whirlwind.playbackRate", this.duration);

            if (base.isAuthority && base.inputBank && base.characterDirection)
            {
                this.forwardDirection = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
            }

            this.RecalculateSpeed();

            if (base.characterMotor && base.characterDirection)
            {
                base.characterMotor.velocity.y *= 0.2f;
                base.characterMotor.velocity = this.forwardDirection * this.dashSpeed;
            }

            if (NetworkServer.active) base.characterBody.AddBuff(BuffIndex.HiddenInvincibility);

            Vector3 b = base.characterMotor ? base.characterMotor.velocity : Vector3.zero;
            this.previousPosition = base.transform.position - b;
        }

        private void RecalculateSpeed()
        {
            this.dashSpeed = (2 + (0.5f * this.moveSpeedStat)) * Mathf.Lerp(DashForward.initialSpeedCoefficient, DashForward.finalSpeedCoefficient, base.fixedAge / this.duration);
        }

        public override void OnExit()
        {
            if (base.characterMotor) base.characterMotor.disableAirControlUntilCollision = false;

            if (base.cameraTargetParams)
            {
                base.cameraTargetParams.fovOverride = -1f;
            }

            if (NetworkServer.active) base.characterBody.RemoveBuff(BuffIndex.HiddenInvincibility);

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            base.characterBody.isSprinting = false;

            if (base.fixedAge >= this.duration)
            {
                this.outer.SetNextState(new SpinningSlash());
                return;
            }

            this.RecalculateSpeed();

            if (base.cameraTargetParams)
            {
                base.cameraTargetParams.fovOverride = Mathf.Lerp(EntityStates.Commando.DodgeState.dodgeFOV, 60f, base.fixedAge / this.duration);
            }

            if (base.isAuthority)
            {
                Vector3 normalized = (base.transform.position - this.previousPosition).normalized;

                if (base.characterDirection)
                {
                    if (normalized != Vector3.zero)
                    {
                        Vector3 vector = normalized * this.dashSpeed;
                        float d = Mathf.Max(Vector3.Dot(vector, this.forwardDirection), 0f);
                        vector = this.forwardDirection * d;
                        vector.y = base.characterMotor.velocity.y;
                        base.characterMotor.velocity = vector;
                    }

                    base.characterDirection.forward = this.forwardDirection;
                }

                this.previousPosition = base.transform.position;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.forwardDirection);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.forwardDirection = reader.ReadVector3();
        }
    }

}
