using PaladinMod.Misc;

namespace PaladinMod.States.Emotes
{
    public class PraiseTheSun : BaseEmote
    {
        public override void OnEnter()
        {
            this.animString = "PraiseTheSun";
            this.duration = 3;

            if (base.GetComponent<PaladinSwordController>().skinName == "PALADINBODY_DRIP_SKIN_NAME")
            {
                this.animString = "DripPose";
                this.duration = 0f;
                this.animDuration = 0.75f;
                this.soundString = Modules.Sounds.Drip;
            }

            base.OnEnter();
        }
    }
}