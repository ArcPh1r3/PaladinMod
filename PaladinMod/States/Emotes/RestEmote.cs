namespace PaladinMod.States.Emotes
{
    public class Rest : BaseEmote
    {
        public override void OnEnter()
        {
            this.animString = "Rest";
            this.animDuration = 1.75f;
            this.normalizeModel = true;
            base.OnEnter();
        }
    }
}
