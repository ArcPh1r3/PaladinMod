using EntityStates;

namespace PaladinMod.States.Emotes
{
    public class PraiseTheSun : BaseEmote
    {
        public override void OnEnter()
        {
            if (base.characterBody.skinIndex == 3)
            {
                this.outer.SetNextState(new Drip());
                return;
            }

            this.animString = "PraiseTheSun";
            this.duration = 3;
            base.OnEnter();
        }
    }
}
