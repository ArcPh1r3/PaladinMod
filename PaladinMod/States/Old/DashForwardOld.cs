using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.States
{
    public class DashForwardOld : BaseSkillState
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
            base.characterBody.isSprinting = true;
            this.duration = DashForwardOld.baseDuration;

            Util.PlayAttackSpeedSound(EntityStates.Croco.Leap.leapSoundString, base.gameObject, 1.75f);
            base.PlayAnimation("FullBody, Override", "DashForward", "Whirlwind.playbackRate", this.duration);

            if (base.isAuthority && base.inputBank && base.characterDirection)
            {
                this.forwardDirection = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
            }

            this.RecalculateSpeed();

            if (base.characterMotor && base.characterDirection)
            {
                base.characterMotor.velocity = this.forwardDirection * this.dashSpeed;
                base.characterMotor.velocity.y = 0f;
            }

            Transform modelTransform = base.GetModelTransform();
            if (modelTransform)
            {
                TemporaryOverlay temporaryOverlay = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay.duration = 0.75f;
                temporaryOverlay.animateShaderAlpha = true;
                temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlay.destroyComponentOnEnd = true;
                temporaryOverlay.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matDoppelganger");
                temporaryOverlay.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
            }

            if (NetworkServer.active) base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);

            Vector3 b = base.characterMotor ? base.characterMotor.velocity : Vector3.zero;
            this.previousPosition = base.transform.position - b;
        }

        private void RecalculateSpeed()
        {
            this.dashSpeed = (2 + (0.5f * this.moveSpeedStat)) * Mathf.Lerp(DashForwardOld.initialSpeedCoefficient, DashForwardOld.finalSpeedCoefficient, base.fixedAge / this.duration);
        }

        public override void OnExit()
        {
            base.characterBody.isSprinting = false;

            if (base.characterMotor)
            {
                base.characterMotor.disableAirControlUntilCollision = false;
                base.characterMotor.velocity *= 0.1f;
            }

            if (base.cameraTargetParams)
            {
                base.cameraTargetParams.fovOverride = -1f;
            }

            if (NetworkServer.active) base.characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            base.characterBody.isSprinting = true;

            if (base.fixedAge >= this.duration)
            {
                this.outer.SetNextState(new SpinningSlashOld());
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
