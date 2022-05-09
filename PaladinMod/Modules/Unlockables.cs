using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using RoR2.Achievements;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace PaladinMod.Modules
{
    internal static class Unlockables {
        internal static UnlockableDef paladinUnlockDef;

        internal static UnlockableDef paladinMasterySkinDef;
        internal static UnlockableDef paladinGrandMasterySkinDef;
        internal static UnlockableDef paladinPoisonSkinDef;
        internal static UnlockableDef paladinClaySkinDef;

        internal static UnlockableDef paladinLunarShardSkillDef;
        internal static UnlockableDef paladinHealSkillDefDef;
        internal static UnlockableDef paladinTorporSkillDefDef;
        internal static UnlockableDef paladinCruelSunSkillDefDef;

        public static void RegisterUnlockables() {                                                                           //cringe
            paladinUnlockDef = UnlockableAPI.AddUnlockable<Achievements.PaladinUnlockAchievement>();

            paladinLunarShardSkillDef = UnlockableAPI.AddUnlockable<Achievements.LunarShardAchievement>();
            paladinHealSkillDefDef = UnlockableAPI.AddUnlockable<Achievements.HealAchievement>();
            paladinTorporSkillDefDef = UnlockableAPI.AddUnlockable<Achievements.TorporAchievement>();
            paladinCruelSunSkillDefDef = UnlockableAPI.AddUnlockable<Achievements.CruelSunAchievement>();

            paladinMasterySkinDef = UnlockableAPI.AddUnlockable<Achievements.MasteryAchievement>();
            paladinPoisonSkinDef = UnlockableAPI.AddUnlockable<Achievements.PoisonAchievement>();
            paladinClaySkinDef = UnlockableAPI.AddUnlockable<Achievements.ClayAchievement>();

            paladinGrandMasterySkinDef = UnlockableAPI.AddUnlockable<Achievements.GrandMasteryAchievement>();
        }
    }
}