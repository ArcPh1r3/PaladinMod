using PaladinMod.Modules;
using RoR2;
using System;
using UnityEngine;
using R2API;

using RoR2.Achievements;

namespace PaladinMod.Achievements
{
    [RegisterAchievement(identifier, unlockableIdentifier, "PALADIN_UNLOCKABLE_ACHIEVEMENT_ID", 3, null)]
    internal class TorporAchievement : BaseAchievement
    {
        public const string identifier = "PALADIN_TORPORUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = "PALADIN_TORPORUNLOCKABLE_REWARD_ID";

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("RobPaladinBody");
        }

        private void Check(On.RoR2.CharacterBody.orig_AddBuff_BuffIndex orig, CharacterBody self, BuffIndex buff)
        {
            if (self)
            {
                if (self.activeBuffsList != null)
                {
                    if (self.activeBuffsListCount >= 4)
                    {
                        if (base.meetsBodyRequirement)
                        {
                            base.Grant();
                        }
                    }
                }
            }

            orig(self, buff);
        }

        private void Check(On.RoR2.CharacterBody.orig_AddBuff_BuffDef orig, CharacterBody self, BuffDef buff)
        {
            if (self)
            {
                if (self.activeBuffsList != null)
                {
                    if (self.activeBuffsListCount >= 4)
                    {
                        if (base.meetsBodyRequirement)
                        {
                            base.Grant();
                        }
                    }
                }
            }

            orig(self, buff);
        }

        public override void OnInstall()
        {
            base.OnInstall();

            On.RoR2.CharacterBody.AddBuff_BuffDef += this.Check;
            On.RoR2.CharacterBody.AddBuff_BuffIndex += this.Check;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            On.RoR2.CharacterBody.AddBuff_BuffDef -= this.Check;
            On.RoR2.CharacterBody.AddBuff_BuffIndex -= this.Check;
        }
    }
}