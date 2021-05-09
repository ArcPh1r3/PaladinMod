using PaladinMod.States;
using PaladinMod.States.Emotes;
using PaladinMod.States.Quickstep;
using PaladinMod.States.Rage;
using PaladinMod.States.Spell;
using PaladinMod.States.LunarKnight;
using System;
using System.Collections.Generic;

namespace PaladinMod.Modules
{
    public static class States
    {
        internal static List<Type> entityStates = new List<Type>();

        internal static void AddSkill(Type t)
        {
            entityStates.Add(t);
        }

        public static void RegisterStates()
        {
            AddSkill(typeof(PaladinMain));
            AddSkill(typeof(SpawnState));
            AddSkill(typeof(BaseEmote));
            AddSkill(typeof(PraiseTheSun));
            AddSkill(typeof(PointDown));
            AddSkill(typeof(Rest));
            AddSkill(typeof(Drip));

            AddSkill(typeof(Slash));

            AddSkill(typeof(SpinSlashEntry));
            AddSkill(typeof(GroundSweep));
            AddSkill(typeof(GroundSweepAlt));
            AddSkill(typeof(AirSlam));
            AddSkill(typeof(AirSlamAlt));

            AddSkill(typeof(ChargeLightningSpear));
            AddSkill(typeof(ThrowLightningSpear));

            AddSkill(typeof(LunarShards));

            AddSkill(typeof(QuickstepSimple));

            //AddSkill(typeof(AimHeal));
            //AddSkill(typeof(CastHeal));
            AddSkill(typeof(ChannelSmallHeal));
            AddSkill(typeof(CastSmallHeal));

            AddSkill(typeof(ChannelHealZone));
            AddSkill(typeof(CastChanneledHealZone));
            AddSkill(typeof(ScepterChannelHealZone));
            AddSkill(typeof(ScepterCastHealZone));

            AddSkill(typeof(ChannelTorpor));
            AddSkill(typeof(CastChanneledTorpor));
            AddSkill(typeof(ScepterChannelTorpor));
            AddSkill(typeof(ScepterCastTorpor));

            AddSkill(typeof(ChannelWarcry));
            AddSkill(typeof(CastChanneledWarcry));
            AddSkill(typeof(ScepterChannelWarcry));
            AddSkill(typeof(ScepterCastWarcry));

            AddSkill(typeof(ChannelCruelSun));
            AddSkill(typeof(CastCruelSun));
            AddSkill(typeof(ScepterChannelCruelSun));
            AddSkill(typeof(ScepterCastCruelSun));

            AddSkill(typeof(RageEnter));
            AddSkill(typeof(RageEnterOut));
            AddSkill(typeof(RageExit));

            AddSkill(typeof(FlashStep));

            AddSkill(typeof(MaceSlam));
        }
    }
}