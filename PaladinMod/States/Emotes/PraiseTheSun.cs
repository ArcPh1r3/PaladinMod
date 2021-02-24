namespace PaladinMod.States.Emotes
{
    public class PraiseTheSun : BaseEmote
    {
        public override void OnEnter()
        {
            this.animString = "PraiseTheSun";
            this.duration = 3;
            base.OnEnter();

            if (base.swordController.skinName == "PALADINBODY_DRIP_SKIN_NAME")
            {
                this.outer.SetNextState(new Drip());
                return;
            }
        }
    }
}