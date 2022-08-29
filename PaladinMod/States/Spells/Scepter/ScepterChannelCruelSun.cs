using PaladinMod.Misc;
using UnityEngine;

namespace PaladinMod.States.Spell
{
    public class ScepterChannelCruelSun : ChannelCruelSun
    {
        protected override string chargeEffectChild => "ScepterCruelSunChannelEffect";

        protected override BaseCastChanneledSpellState GetNextState()
        {
            return new ScepterCastCruelSun();
        }
    }
}