using PaladinMod.States;
using PaladinMod.States.Emotes;
using PaladinMod.States.Quickstep;
using PaladinMod.States.Rage;
using PaladinMod.States.Spell;
using PaladinMod.States.LunarKnight;

using System;
using System.Collections.Generic;

using EntityStates;
using MonoMod.RuntimeDetour;
using RoR2;
using System.Reflection;

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

            //"AddSkill" should really be called "AddState" or something
            AddSkill(typeof(PaladinMod.States.Sun.PaladinSunBase));
            AddSkill(typeof(PaladinMod.States.Sun.PaladinSunSpawn));
            AddSkill(typeof(PaladinMod.States.Sun.PaladinSunMain));
            AddSkill(typeof(PaladinMod.States.Sun.PaladinSunDeath));
        }


        private static Hook set_stateTypeHook;
        private static Hook set_typeNameHook;
        private static readonly BindingFlags allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
        private delegate void set_stateTypeDelegate(ref SerializableEntityStateType self, Type value);
        private delegate void set_typeNameDelegate(ref SerializableEntityStateType self, String value);

        internal static void FixStates() {
            Type type = typeof(SerializableEntityStateType);
            HookConfig cfg = default;
            cfg.Priority = Int32.MinValue;
            set_stateTypeHook = new Hook(type.GetMethod("set_stateType", allFlags), new set_stateTypeDelegate(SetStateTypeHook), cfg);
            set_typeNameHook = new Hook(type.GetMethod("set_typeName", allFlags), new set_typeNameDelegate(SetTypeName), cfg);
        }

        private static void SetStateTypeHook(ref this SerializableEntityStateType self, Type value) {
            self._typeName = value.AssemblyQualifiedName;
        }

        private static void SetTypeName(ref this SerializableEntityStateType self, String value) {
            Type t = GetTypeFromName(value);
            if (t != null) {
                self.SetStateTypeHook(t);
            }
        }

        private static Type GetTypeFromName(String name) {
            Type[] types = EntityStateCatalog.stateIndexToType;
            return Type.GetType(name);
        }
    }
}