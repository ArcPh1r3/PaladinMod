namespace PaladinMod.States.Emotes
{
    public class PraiseTheSun : BaseEmote
    {
        public override void OnEnter()
        {
            this.animString = "PraiseTheSun";
            this.duration = 4;
            //this.soundString = EnforcerPlugin.Sounds.DefaultDance;
            base.OnEnter();
        }
    }
}
