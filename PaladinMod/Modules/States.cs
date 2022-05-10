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

        internal static void AddState(Type t)
        {
            entityStates.Add(t);
        }

        public static void RegisterStates()
        {
            AddState(typeof(PaladinMain));
            AddState(typeof(SpawnState));
            AddState(typeof(BaseEmote));
            AddState(typeof(PraiseTheSun));
            AddState(typeof(PointDown));
            AddState(typeof(Rest));
            AddState(typeof(Drip));

            AddState(typeof(Slash));

            AddState(typeof(SpinSlashEntry));
            AddState(typeof(GroundSweep));
            AddState(typeof(GroundSweepAlt));
            AddState(typeof(AirSlam));
            AddState(typeof(AirSlamAlt));

            AddState(typeof(ChargeLightningSpear));
            AddState(typeof(ThrowLightningSpear));

            AddState(typeof(LunarShards));

            AddState(typeof(QuickstepSimple));

            //AddState(typeof(AimHeal));
            //AddState(typeof(CastHeal));
            AddState(typeof(ChannelSmallHeal));
            AddState(typeof(CastSmallHeal));

            AddState(typeof(ChannelHealZone));
            AddState(typeof(CastChanneledHealZone));
            AddState(typeof(ScepterChannelHealZone));
            AddState(typeof(ScepterCastHealZone));

            AddState(typeof(ChannelTorpor));
            AddState(typeof(CastChanneledTorpor));
            AddState(typeof(ScepterChannelTorpor));
            AddState(typeof(ScepterCastTorpor));

            AddState(typeof(ChannelWarcry));
            AddState(typeof(CastChanneledWarcry));
            AddState(typeof(ScepterChannelWarcry));
            AddState(typeof(ScepterCastWarcry));

            AddState(typeof(ChannelCruelSun));
            AddState(typeof(CastCruelSun));
            AddState(typeof(ScepterChannelCruelSun));
            AddState(typeof(ScepterCastCruelSun));
            AddState(typeof(PaladinMod.States.Sun.PaladinSunBase));
            AddState(typeof(PaladinMod.States.Sun.PaladinSunSpawn));
            AddState(typeof(PaladinMod.States.Sun.PaladinSunMain));
            AddState(typeof(PaladinMod.States.Sun.PaladinSunDeath));

            //Legacy Cruel Sun support
            AddState(typeof(ChannelCruelSunOld));
            AddState(typeof(CastCruelSunOld));
            AddState(typeof(ScepterChannelCruelSunOld));
            AddState(typeof(ScepterCastCruelSunOld));

            AddState(typeof(RageEnter));
            AddState(typeof(RageEnterOut));
            AddState(typeof(RageExit));

            AddState(typeof(FlashStep));

            AddState(typeof(MaceSlam));
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