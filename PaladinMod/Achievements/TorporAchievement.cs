using R2API;
using R2API.Utils;
using RoR2;
using System;

namespace PaladinMod.Achievements
{
    [R2APISubmoduleDependency(nameof(UnlockablesAPI))]

    public class TorporAchievement : ModdedUnlockableAndAchievement<CustomSpriteProvider>
    {
        public override String AchievementIdentifier { get; } = "PALADIN_TORPORUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "PALADIN_TORPORUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "PALADIN_TORPORUNLOCKABLE_PREREQ_ID";
        public override String AchievementNameToken { get; } = "PALADIN_TORPORUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "PALADIN_TORPORUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "PALADIN_TORPORUNLOCKABLE_UNLOCKABLE_NAME";
        protected override CustomSpriteProvider SpriteProvider { get; } = new CustomSpriteProvider("@Paladin:Assets/Paladin/Icons/texTorporAchievement.png");

        public override int LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("RobPaladinBody");
        }

        private void Check(On.RoR2.CharacterBody.orig_AddBuff orig, CharacterBody self, BuffIndex buff)
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

            On.RoR2.CharacterBody.AddBuff += this.Check;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            On.RoR2.CharacterBody.AddBuff -= this.Check;
        }
    }
}