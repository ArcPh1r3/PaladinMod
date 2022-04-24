using PaladinMod.Modules;
using RoR2;
using System;
using UnityEngine;
using R2API;

namespace PaladinMod.Achievements
{
    internal class SunlightSpearAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = "PALADIN_LIGHTNINGSPEARUNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = "PALADIN_LIGHTNINGSPEARUNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = "PALADIN_LIGHTNINGSPEARUNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = "PALADIN_UNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableNameToken { get; } = "PALADIN_LIGHTNINGSPEARUNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = "PALADIN_LIGHTNINGSPEARUNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texLightningSpearAchievement");

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString("PALADIN_LIGHTNINGSPEARUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("PALADIN_LIGHTNINGSPEARUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString("PALADIN_LIGHTNINGSPEARUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("PALADIN_LIGHTNINGSPEARUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));

        public override BodyIndex LookUpRequiredBodyIndex()
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
                        if (self.equipmentIndex == RoR2Content.Equipment.Lightning.equipmentIndex)
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