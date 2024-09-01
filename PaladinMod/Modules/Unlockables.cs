using Mono.Cecil.Cil;
using MonoMod.Cil;
using PaladinMod.Achievements;
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

        internal static List<UnlockableDef> paladinUnlockableDefs = new List<UnlockableDef>();

        public static void RegisterUnlockables() {                                                                           //cringe //no longer cringe also idk why this is all the way out here
                                                                                                                                           //ok it's a little cringe I could reduce this copypaste
            paladinUnlockDef = CreateAndAddUnlockbleDef(
                PaladinUnlockAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(PaladinUnlockAchievement.identifier),
                Modules.Asset.mainAssetBundle.LoadAsset<Sprite>("texPaladinAchievement"));

            paladinLunarShardSkillDef = CreateAndAddUnlockbleDef(
                LunarShardAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(LunarShardAchievement.identifier),
                Modules.Asset.mainAssetBundle.LoadAsset<Sprite>("texLunarShardAchievement"));

            paladinHealSkillDefDef = CreateAndAddUnlockbleDef(
                HealAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(HealAchievement.identifier),
                Modules.Asset.mainAssetBundle.LoadAsset<Sprite>("texHealAchievement"));

            paladinTorporSkillDefDef = CreateAndAddUnlockbleDef(
                TorporAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(TorporAchievement.identifier),
                Modules.Asset.mainAssetBundle.LoadAsset<Sprite>("texTorporAchievement"));

            paladinCruelSunSkillDefDef = CreateAndAddUnlockbleDef(
                CruelSunAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(CruelSunAchievement.identifier),
                Modules.Asset.mainAssetBundle.LoadAsset<Sprite>("texCruelSunAchievement"));

            paladinMasterySkinDef = CreateAndAddUnlockbleDef(
                MasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(MasteryAchievement.identifier),
                Modules.Asset.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievement"));

            paladinGrandMasterySkinDef = CreateAndAddUnlockbleDef(
                GrandMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(GrandMasteryAchievement.identifier),
                Modules.Asset.mainAssetBundle.LoadAsset<Sprite>("texGrandMasteryAchievement"));

            paladinPoisonSkinDef = CreateAndAddUnlockbleDef(
                PoisonAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(PoisonAchievement.identifier),
                Modules.Asset.mainAssetBundle.LoadAsset<Sprite>("texPoisonAchievement"));

            paladinClaySkinDef = CreateAndAddUnlockbleDef(
                ClayAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(ClayAchievement.identifier),
                Modules.Asset.mainAssetBundle.LoadAsset<Sprite>("texClayAchievement"));
        }

        public static void AddUnlockableDef(UnlockableDef unlockableDef)
        {
            paladinUnlockableDefs.Add(unlockableDef);
        }
        internal static UnlockableDef CreateAndAddUnlockbleDef(string identifier, string nameToken, Sprite achievementIcon)
        {
            UnlockableDef unlockableDef = ScriptableObject.CreateInstance<UnlockableDef>();
            unlockableDef.cachedName = identifier;
            unlockableDef.nameToken = nameToken;
            unlockableDef.achievementIcon = achievementIcon;

            AddUnlockableDef(unlockableDef);

            return unlockableDef;
        }
    }
}