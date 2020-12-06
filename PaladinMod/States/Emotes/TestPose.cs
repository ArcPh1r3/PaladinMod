namespace PaladinMod.States.Emotes
{
    public class TestPose : BaseEmote
    {
        public override void OnEnter()
        {
            this.animString = "TestPose";
            this.animDuration = 2f;
            base.OnEnter();
        }
    }
}
