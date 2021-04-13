using PaladinMod.Modules;
using RoR2;
using System;
using UnityEngine;

namespace PaladinMod.Achievements
{
    internal class CruelSunAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = "PALADIN_CRUELSUNUNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = "PALADIN_CRUELSUNUNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = "PALADIN_CRUELSUNUNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = "PALADIN_UNLOCKABLE_REWARD_ID";
        public override string UnlockableNameToken { get; } = "PALADIN_CRUELSUNUNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = "PALADIN_CRUELSUNUNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texCruelSunAchievement");

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString("PALADIN_CRUELSUNUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("PALADIN_CRUELSUNUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString("PALADIN_CRUELSUNUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("PALADIN_CRUELSUNUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("RobPaladinBody");
        }

        private void Check(Run run)
        {
            if (base.meetsBodyRequirement) base.Grant();
        }

        public override void OnInstall()
        {
            base.OnInstall();

            States.PaladinMain.onSunSurvived += this.Check;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            States.PaladinMain.onSunSurvived += this.Check;
        }
    }
}