using PaladinMod.Modules;
using RoR2;
using System;
using UnityEngine;
using R2API;
using RoR2.Achievements;

namespace PaladinMod.Achievements
{
    [RegisterAchievement(identifier, unlockableIdentifier, null, null)]
    public class PaladinUnlockAchievement : BaseAchievement
    {
        public const string identifier = "PALADIN_UNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = "PALADIN_UNLOCKABLE_REWARD_ID";

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