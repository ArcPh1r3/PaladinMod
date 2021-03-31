using EntityStates;
using R2API;
using RoR2;
using PaladinMod.States;
using PaladinMod.States.Dash;
using PaladinMod.States.Emotes;
using PaladinMod.States.Quickstep;
using PaladinMod.States.Spell;
using PaladinMod.States.LunarKnight;
using System;
using System.Reflection;
using System.Collections.Generic;
using MonoMod.RuntimeDetour;

namespace PaladinMod.Modules
{
    public static class States
    {
        internal static List<Type> entityStates = new List<Type>();

        private static Hook set_stateTypeHook;
        private static Hook set_typeNameHook;
        private static readonly BindingFlags allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
        private delegate void set_stateTypeDelegate(ref SerializableEntityStateType self, Type value);
        private delegate void set_typeNameDelegate(ref SerializableEntityStateType self, String value);

        internal static void AddSkill(Type t)
        {
            entityStates.Add(t);
        }

        public static void RegisterStates()
        {
            Type type = typeof(SerializableEntityStateType);
            HookConfig cfg = default;
            cfg.Priority = Int32.MinValue;
            set_stateTypeHook = new Hook(type.GetMethod("set_stateType", allFlags), new set_stateTypeDelegate(SetStateTypeHook), cfg);
            set_typeNameHook = new Hook(type.GetMethod("set_typeName", allFlags), new set_typeNameDelegate(SetTypeName), cfg);

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

            AddSkill(typeof(AimHeal));
            AddSkill(typeof(CastHeal));

            AddSkill(typeof(ChannelHealZone));
            AddSkill(typeof(CastChanneledHealZone));

            AddSkill(typeof(ChannelTorpor));
            AddSkill(typeof(CastChanneledTorpor));

            AddSkill(typeof(ChannelWarcry));
            AddSkill(typeof(CastChanneledWarcry));

            AddSkill(typeof(MaceSlam));
        }

        private static void SetStateTypeHook(ref this SerializableEntityStateType self, Type value)
        {
            self._typeName = value.AssemblyQualifiedName;
        }

        private static void SetTypeName(ref this SerializableEntityStateType self, String value)
        {
            Type t = GetTypeFromName(value);
            if (t != null)
            {
                self.SetStateTypeHook(t);
            }
        }

        private static Type GetTypeFromName(String name)
        {
            Type[] types = EntityStateCatalog.stateIndexToType;
            return Type.GetType(name);
        }
    }
}