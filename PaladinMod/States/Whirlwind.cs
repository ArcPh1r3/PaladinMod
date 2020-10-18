namespace PaladinMod.States
{
    public class WhirlwindGround : WhirlwindBase
    {
        protected override void PlayAnim()
        {
            base.PlayAnimation("FullBody, Override", "AirSlam", "Whirlwind.playbackRate", this.duration);
        }
    }
}
