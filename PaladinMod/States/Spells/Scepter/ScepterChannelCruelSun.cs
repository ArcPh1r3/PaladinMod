using PaladinMod.Misc;
using UnityEngine;

namespace PaladinMod.States.Spell
{
    public class ScepterChannelCruelSun : ChannelCruelSun
    {
        protected override BaseCastChanneledSpellState GetNextState()
        {
            return new ScepterCastCruelSun();
        }
    }
}