using UnityEngine;

namespace PaladinMod.States.Quickstep
{
    public class QuickstepBack : BaseQuickstepState
    {
        public override void OnEnter()
        {
            this.slideRotation = Quaternion.AngleAxis(-180f, Vector3.up);
            base.OnEnter();
            base.PlayAnimation("FullBody, Override", "DashForward", "Whirlwind.playbackRate", BaseQuickstepState.baseDuration);
        }
    }
}
