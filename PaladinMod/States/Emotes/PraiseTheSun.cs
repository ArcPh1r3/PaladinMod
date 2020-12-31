namespace PaladinMod.States.Emotes
{
    public class PraiseTheSun : BaseEmote
    {
        public override void OnEnter()
        {
            this.animString = "PraiseTheSun";
            this.duration = 3;
            base.OnEnter();
        }
    }
}
