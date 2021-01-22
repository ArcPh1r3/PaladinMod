namespace PaladinMod.States.Emotes
{
    public class PointDown : BaseEmote
    {
        public override void OnEnter()
        {
            this.animString = "PointDown";
            this.duration = 2.5f;
            base.OnEnter();
        }
    }
}