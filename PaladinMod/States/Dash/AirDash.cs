using EntityStates;
using PaladinMod.Misc;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.States.Dash
{
    public class AirDash : BaseState
    {
        public static float duration = 0.3f;
        public static float speedCoefficient = 5f;

        private float stopwatch;
        private Transform modelTransform;
        private Vector3 dashVector = Vector3.zero;
        private CharacterModel characterModel;
        private PaladinSwordController swordController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.swordController = base.GetComponent<PaladinSwordController>();
            //Util.PlaySound(this.beginSoundString, base.gameObject);
            this.modelTransform = base.GetModelTransform();

            if (this.swordController) this.swordController.attacking = true;

            if (this.modelTransform)
            {
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
            }

            Util.PlayAttackSpeedSound(EntityStates.Croco.Leap.leapSoundString, base.gameObject, 1.75f);
            base.PlayAnimation("FullBody, Override", "DashForward", "Whirlwind.playbackRate", AirDash.duration);

            if (NetworkServer.active) base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);

            this.dashVector = this.GetDashVector();
            //this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject));
        }
        
        protected virtual Vector3 GetDashVector()
        {
            return base.inputBank.aimDirection;
        }

        private void CreateBlinkEffect(Vector3 origin)
        {
            EffectData effectData = new EffectData();
            effectData.rotation = Util.QuaternionSafeLookRotation(this.dashVector);
            effectData.origin = origin;
            EffectManager.SpawnEffect(EntityStates.Huntress.BlinkState.blinkPrefab, effectData, false);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterBody.isSprinting = true;
            this.stopwatch += Time.deltaTime;

            if (base.characterMotor && base.characterDirection)
            {
                base.characterMotor.velocity = Vector3.zero;
                base.characterMotor.rootMotion += this.dashVector * (this.moveSpeedStat * AirDash.speedCoefficient * Time.deltaTime);
                base.characterDirection.forward = this.dashVector;
            }
            
            if (this.stopwatch >= AirDash.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            if (!this.outer.destroying)
            {
                //Util.PlaySound(this.endSoundString, base.gameObject);
                //this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject));

                if (this.modelTransform)
                {
                    TemporaryOverlayInstance overlay = TemporaryOverlayManager.AddOverlay(this.modelTransform.gameObject);
                    overlay.duration = 0.6f;
                    overlay.animateShaderAlpha = true;
                    overlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    overlay.destroyComponentOnEnd = true;
                    overlay.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashBright");
                    overlay.AddToCharacterModel(this.modelTransform.GetComponent<CharacterModel>());
                }
            }

            if (this.swordController) this.swordController.attacking = false;

            if (NetworkServer.active)
            {
                base.characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
            }

            if (base.isAuthority)
            {
                base.healthComponent.AddBarrierAuthority(StaticValues.dashBarrierAmount * base.healthComponent.fullBarrier);
            }

            base.PlayAnimation("FullBody, Override", "BufferEmpty");

            base.OnExit();
        }
    }
}
