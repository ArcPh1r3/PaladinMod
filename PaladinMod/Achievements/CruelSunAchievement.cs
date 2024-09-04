using PaladinMod.Modules;
using RoR2;
using System;
using UnityEngine;
using R2API;

using RoR2.Achievements;

namespace PaladinMod.Achievements
{
    [RegisterAchievement(identifier, unlockableIdentifier, "PALADIN_UNLOCKABLE_ACHIEVEMENT_ID", 5, null)]
    internal class CruelSunAchievement : BaseAchievement
    {
        public const string identifier = "PALADIN_CRUELSUNUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = "PALADIN_CRUELSUNUNLOCKABLE_REWARD_ID";

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