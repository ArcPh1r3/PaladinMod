namespace PaladinMod.States.Emotes
{
    public class PointDown : BaseEmote
    {
        public override void OnEnter()
        {
            this.animString = "PointDown";
            //this.soundString = EnforcerPlugin.Sounds.DefaultDance;
            this.animDuration = 2f;
            base.OnEnter();
        }
    }
}
