using R2API;
using R2API.Utils;
using RoR2;
using System;

namespace PaladinMod.Achievements
{
    [R2APISubmoduleDependency(nameof(UnlockablesAPI))]

    public class SunlightSpearAchievement : ModdedUnlockableAndAchievement<CustomSpriteProvider>
    {
        public override String AchievementIdentifier { get; } = "PALADIN_LIGHTNINGSPEARUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "PALADIN_LIGHTNINGSPEARUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "PALADIN_LIGHTNINGSPEARUNLOCKABLE_PREREQ_ID";
        public override String AchievementNameToken { get; } = "PALADIN_LIGHTNINGSPEARUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "PALADIN_LIGHTNINGSPEARUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "PALADIN_LIGHTNINGSPEARUNLOCKABLE_UNLOCKABLE_NAME";
        protected override CustomSpriteProvider SpriteProvider { get; } = new CustomSpriteProvider("@Paladin:Assets/Paladin/Icons/texLightningSpearAchievement.png");

        public override int LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("RobPaladinBody");
        }

        private void Check(On.RoR2.EquipmentSlot.orig_Execute orig, EquipmentSlot self)
        {
            orig(self);

            if (self)
            {
                if (self.characterBody)
                {
                    if (self.characterBody.name == "PALADIN_NAME")
                    {
                        if (self.equipmentIndex == EquipmentIndex.Lightning)
                        {
                            if (base.meetsBodyRequirement)
                            {
                                base.Grant();
                            }
                        }
                    }
                }
            }
        }

        public override void OnInstall()
        {
            base.OnInstall();

            On.RoR2.EquipmentSlot.Execute += this.Check;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            On.RoR2.EquipmentSlot.Execute -= this.Check;
        }
    }
}