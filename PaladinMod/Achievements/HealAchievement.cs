using R2API;
using R2API.Utils;
using RoR2;
using System;
using UnityEngine;

namespace PaladinMod.Achievements
{
    [R2APISubmoduleDependency(nameof(UnlockablesAPI))]

    public class HealAchievement : ModdedUnlockableAndAchievement<CustomSpriteProvider>
    {
        public override String AchievementIdentifier { get; } = "PALADIN_HEALUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "PALADIN_HEALUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "PALADIN_HEALUNLOCKABLE_PREREQ_ID";
        public override String AchievementNameToken { get; } = "PALADIN_HEALUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "PALADIN_HEALUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "PALADIN_HEALUNLOCKABLE_UNLOCKABLE_NAME";
        protected override CustomSpriteProvider SpriteProvider { get; } = new CustomSpriteProvider("@Paladin:Assets/Paladin/Icons/texHealAchievement.png");

        public override int LookUpRequiredBodyIndex()
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