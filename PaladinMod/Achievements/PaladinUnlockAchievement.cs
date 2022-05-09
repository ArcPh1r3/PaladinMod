using PaladinMod.Modules;
using RoR2;
using System;
using UnityEngine;
using R2API;

namespace PaladinMod.Achievements
{
    internal class PaladinUnlockAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = "PALADIN_UNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = "PALADIN_UNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = "PALADIN_UNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = "";
        public override string UnlockableNameToken { get; } = "PALADIN_UNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = "PALADIN_UNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPaladinAchievement");

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString("PALADIN_UNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("PALADIN_UNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString("PALADIN_UNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("PALADIN_UNLOCKABLE_ACHIEVEMENT_DESC")
                            }));

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("RobPaladinBody");
        }

        private void Check(On.RoR2.SceneDirector.orig_Start orig, SceneDirector self)
        {
            if (self)
            {
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "limbo")
                {
                    base.Grant();
                }
            }

            orig(self);
        }

        public override void OnInstall()
        {
            base.OnInstall();

            On.RoR2.SceneDirector.Start += this.Check;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            On.RoR2.SceneDirector.Start -= this.Check;
        }
    }
}