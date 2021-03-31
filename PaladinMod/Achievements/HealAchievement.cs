using PaladinMod.Modules;
using RoR2;
using System;
using UnityEngine;

namespace PaladinMod.Achievements
{
    internal class HealAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = "PALADIN_HEALUNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = "PALADIN_HEALUNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = "PALADIN_HEALUNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = "PALADIN_UNLOCKABLE_REWARD_ID";
        public override string UnlockableNameToken { get; } = "PALADIN_HEALUNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = "PALADIN_HEALUNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texHealAchievement");

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString("PALADIN_HEALUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("PALADIN_HEALUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString("PALADIN_HEALUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("PALADIN_HEALUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("RobPaladinBody");
        }

        private void Check(On.RoR2.HealingFollowerController.orig_AssignNewTarget orig, HealingFollowerController self, GameObject target)
        {
            if (self)
            {
                if (target != self.ownerBodyObject)
                {
                    if (base.meetsBodyRequirement) base.Grant();
                }
            }

            orig(self, target);
        }

        public override void OnInstall()
        {
            base.OnInstall();

            On.RoR2.HealingFollowerController.AssignNewTarget += this.Check;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            On.RoR2.HealingFollowerController.AssignNewTarget -= this.Check;
        }
    }
}