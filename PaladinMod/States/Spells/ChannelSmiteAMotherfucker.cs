namespace PaladinMod.States.Spell
{
    public class ChannelSmiteAMotherfucker : BaseChannelSpellStateTargeted
    {
        public override void OnEnter()
        {
            baseDuration = 0.2f;
            base.OnEnter();
        }
        protected override BaseCastChanneledSpellState GetNextState()
        {
            return new CastSmiteAMotherfucker();
        }
    }
}