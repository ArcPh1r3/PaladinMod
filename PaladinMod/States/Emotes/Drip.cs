namespace PaladinMod.States.Emotes
{
    public class Drip : BaseEmote
    {
        public override void OnEnter()
        {
            this.animString = "DripPose";
            this.animDuration = 0.75f;
            this.soundString = Modules.Sounds.Drip;
            base.OnEnter();
        }
    }
}
