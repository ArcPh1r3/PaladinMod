using RoR2;
using UnityEngine.Networking;
using EntityStates;
using UnityEngine;

namespace PaladinMod.States
{
    public class SpawnState : BaseState
    {
        public static float duration = 3f;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();
            base.PlayAnimation("Body", "Spawn", "Spawn.playbackRate", SpawnState.duration);
            Util.PlaySound(EntityStates.ParentMonster.SpawnState.spawnSoundString, base.gameObject);

            if (NetworkServer.active) base.characterBody.AddBuff(BuffIndex.HiddenInvincibility);

            if (this.animator)
            {
                this.animator.SetFloat(AnimationParameters.aimWeight, 0f);
            }

            base.GetModelChildLocator().FindChild("SpawnEffect").gameObject.SetActive(true);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (this.animator) this.animator.SetBool("inCombat", true);

            if (base.fixedAge >= SpawnState.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (this.animator)
            {
                this.animator.SetFloat(AnimationParameters.aimWeight, 1f);
            }

            if (NetworkServer.active) base.characterBody.RemoveBuff(BuffIndex.HiddenInvincibility);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}
