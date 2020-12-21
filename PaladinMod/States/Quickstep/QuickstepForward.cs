using UnityEngine;

namespace PaladinMod.States.Quickstep
{
    public class QuickstepForward : BaseQuickstepState
    {
        public override void OnEnter()
        {
            this.slideRotation = Quaternion.identity;
            base.OnEnter();
            base.PlayAnimation("FullBody, Override", "DashForward", "Whirlwind.playbackRate", BaseQuickstepState.baseDuration);
            base.PlayCrossfade("Body", "Run", 0.05f);
        }
    }
}
