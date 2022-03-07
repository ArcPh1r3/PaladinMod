using EntityStates;
using PaladinMod.Misc;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.States.Rage
{
    public class RageEnter : BaseSkillState
    {
        public static float baseDuration = 1.5f;

        private float duration;
        private Vector3 storedPosition;
        private Animator modelAnimator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = RageEnter.baseDuration;// / this.attackSpeedStat;
            this.modelAnimator = base.GetModelAnimator();
            base.characterBody.hideCrosshair = true;

            if (this.modelAnimator)
            {
                this.modelAnimator.SetBool("isMoving", false);
                this.modelAnimator.SetBool("isSprinting", false);
            }

            if (NetworkServer.active) base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);

            foreach (EntityStateMachine i in base.gameObject.GetComponents<EntityStateMachine>())
            {
                if (i)
                {
                    if (i.customName == "Weapon")
                    {
                        i.SetNextStateToMain();
                    }
                    if (i.customName == "Slide")
                    {
                        i.SetNextStateToMain();
                    }
                }
            }

            PaladinRageController rageComponent = base.gameObject.GetComponent<PaladinRageController>();
            if (rageComponent) rageComponent.SpendRage(100f);

            //EffectManager.SimpleMuzzleFlash(Modules.Assets.frenzyChargeEffect, base.gameObject, "Chest", false);
            base.PlayAnimation("Gesture, Override", "BufferEmpty");
            base.PlayAnimation("FullBody, Override", "RageEnter", "Rage.playbackRate", this.duration * 2.75f);
            Util.PlaySound("HenryFrenzyCharge", base.gameObject);

            //todo fix with new camera params system
            Modules.CameraParams.OverridePaladinCameraParams(base.cameraTargetParams, PaladinCameraParams.RAGE_ENTER);

            this.storedPosition = base.transform.position;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.transform.position = this.storedPosition;
            base.characterBody.isSprinting = false;
            if (base.characterMotor) base.characterMotor.velocity = Vector3.zero;

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextState(new RageEnterOut());
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}