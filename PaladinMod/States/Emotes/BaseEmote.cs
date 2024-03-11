using BepInEx.Configuration;
using EntityStates;
using PaladinMod.Misc;
using RoR2;
using UnityEngine;

namespace PaladinMod.States.Emotes
{
    public class BaseEmote : BaseState
    {
        public string soundString;
        public string animString;
        public float duration;
        public float animDuration;
        public bool normalizeModel;

        private uint activePlayID;
        private Animator animator;
        protected ChildLocator childLocator;
        //private CharacterCameraParams originalCameraParams;
        protected PaladinSwordController swordController;
        public LocalUser localUser;
        private CameraTargetParams.CameraParamsOverrideHandle handle;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();
            this.childLocator = base.GetModelChildLocator();
            this.swordController = base.GetComponent<PaladinSwordController>();
            FindLocalUser();
            
            base.characterBody.hideCrosshair = true;

            if (base.GetAimAnimator()) base.GetAimAnimator().enabled = false;
            this.animator.SetLayerWeight(animator.GetLayerIndex("AimPitch"), 0);
            this.animator.SetLayerWeight(animator.GetLayerIndex("AimYaw"), 0);

            if (this.animDuration == 0 && this.duration != 0) this.animDuration = this.duration;

            if (this.duration > 0) base.PlayAnimation("FullBody, Override", this.animString, "Emote.playbackRate", this.duration);
            else base.PlayAnimation("FullBody, Override", this.animString, "Emote.playbackRate", this.animDuration);

            this.activePlayID = Util.PlaySound(soundString, base.gameObject);

            if (this.normalizeModel)
            {
                if (base.modelLocator)
                {
                    base.modelLocator.normalizeToFloor = true;
                }
            }

            //this.originalCameraParams = base.cameraTargetParams.cameraParams;
            handle = Modules.CameraParams.OverridePaladinCameraParams(base.cameraTargetParams, PaladinCameraParams.EMOTE, 0.5f);
        }

        public override void OnExit()
        {
            base.OnExit();

            base.characterBody.hideCrosshair = false;
            //base.cameraTargetParams.cameraParams = this.originalCameraParams;
            //base.cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Standard);
            //Modules.CameraParams.OverridePaladinCameraParams(base.cameraTargetParams, PaladinCameraParams.DEFAULT);
            base.cameraTargetParams.RemoveParamsOverride(handle, 0.2f);

            if (base.GetAimAnimator()) base.GetAimAnimator().enabled = true;
            if (this.animator)
            {
                this.animator.SetLayerWeight(animator.GetLayerIndex("AimPitch"), 1);
                this.animator.SetLayerWeight(animator.GetLayerIndex("AimYaw"), 1);
            }

            if (this.normalizeModel)
            {
                if (base.modelLocator)
                {
                    base.modelLocator.normalizeToFloor = false;
                }
            }

            base.PlayAnimation("FullBody, Override", "BufferEmpty");
            if (this.activePlayID != 0) AkSoundEngine.StopPlayingID(this.activePlayID);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            bool flag = false;

            if (base.characterMotor)
            {
                if (!base.characterMotor.isGrounded) flag = true;
                if (base.characterMotor.velocity != Vector3.zero) flag = true;
            }

            if (base.inputBank)
            {
                if (base.inputBank.skill1.down) flag = true;
                if (base.inputBank.skill2.down) flag = true;
                if (base.inputBank.skill3.down) flag = true;
                if (base.inputBank.skill4.down) flag = true;
                if (base.inputBank.jump.down) flag = true;

                if (base.inputBank.moveVector != Vector3.zero) flag = true;
            }

            //emote cancels
            if (base.isAuthority && base.characterMotor.isGrounded)
            {
                if (CheckEmote<PraiseTheSun>(Modules.Config.praiseKeybind))
                    return;
                if (CheckEmote<Rest>(Modules.Config.restKeybind))
                    return;
                if (CheckEmote<PointDown>(Modules.Config.pointKeybind))
                    return;
                if (CheckEmote<TestPose>(Modules.Config.swordPoseKeybind))
                    return;
            }

            if (this.duration > 0 && base.fixedAge >= this.duration) flag = true;

            if (this.animator) this.animator.SetBool("inCombat", true);

            if (flag)
            {
                this.outer.SetNextStateToMain();
            }
        }

        private bool CheckEmote<T>(ConfigEntry<KeyCode> keybind) where T : EntityState, new() {
            if (Input.GetKeyDown(keybind.Value)) {

                FindLocalUser();

                if (localUser != null && !localUser.isUIFocused) {
                    outer.SetInterruptState(new T(), InterruptPriority.Any);
                    return true;
                }
            }
            return false;
        }

        private void FindLocalUser() {
            if (localUser == null) {
                if (base.characterBody) {
                    foreach (LocalUser lu in LocalUserManager.readOnlyLocalUsersList) {
                        if (lu.cachedBody == base.characterBody) {
                            this.localUser = lu;
                            break;
                        }
                    }
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }
    }

}