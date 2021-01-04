using RoR2;
using UnityEngine.Networking;
using EntityStates;

namespace PaladinMod.States
{
    public class SpawnState : BaseState
    {
        public static float duration = 2.5f;

        public override void OnEnter()
        {
            base.OnEnter();
            base.PlayAnimation("Body", "Spawn");
            Util.PlaySound(EntityStates.ParentMonster.SpawnState.spawnSoundString, base.gameObject);

            if (NetworkServer.active) base.characterBody.AddBuff(BuffIndex.HiddenInvincibility);

            base.GetModelChildLocator().FindChild("SpawnEffect").gameObject.SetActive(true);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= SpawnState.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (NetworkServer.active) base.characterBody.RemoveBuff(BuffIndex.HiddenInvincibility);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}
