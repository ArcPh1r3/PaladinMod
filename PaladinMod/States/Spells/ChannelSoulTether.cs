using RoR2;

namespace PaladinMod.States.Spell
{
    public class ChannelSoulTether : BaseChannelSpellStateTargeted
    {
        public override void OnEnter()
        {
            baseDuration = 0.3f;
            base.OnEnter();
        }
        protected override BaseCastChanneledSpellState GetNextState()
        {
            return new CastSoulTether();
        }
        protected override bool VerifyValidTarget(HurtBox hurtBox)
        {
            //target both teams
            return true;
        }
    }
}