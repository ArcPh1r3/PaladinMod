using RoR2;

namespace PaladinMod.States
{
    public abstract class BaseCastChanneledSpellStateTargeted : BaseCastChanneledSpellState
    {
        public HurtBox trackingTarget;
    }
}