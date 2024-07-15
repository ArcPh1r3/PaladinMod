using PaladinMod.Modules;
using RoR2;
using System;
using UnityEngine;
using R2API;
using RoR2.Achievements;

namespace PaladinMod.Achievements
{
    [RegisterAchievement(identifier, unlockableIdentifier, "PALADIN_UNLOCKABLE_ACHIEVEMENT_ID", null)]
    internal class HealAchievement : BaseAchievement
    {
        public const string identifier = "PALADIN_HEALUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = "PALADIN_HEALUNLOCKABLE_REWARD_ID";

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