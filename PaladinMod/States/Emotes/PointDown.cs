namespace PaladinMod.States.Emotes
{
    public class PointDown : BaseEmote
    {
        public override void OnEnter()
        {
            this.animString = "PointDown";
            this.animDuration = 2f;
            base.OnEnter();
        }
    }
}
