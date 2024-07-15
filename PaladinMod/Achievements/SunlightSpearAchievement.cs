using PaladinMod.Modules;
using RoR2;
using System;
using UnityEngine;
using R2API;

using RoR2.Achievements;

namespace PaladinMod.Achievements
{
    //[RegisterAchievement(identifier, unlockableIdentifier, "PALADIN_UNLOCKABLE_ACHIEVEMENT_ID", null)]
    internal class SunlightSpearAchievement : BaseAchievement
    {
        public const string identifier = "PALADIN_LIGHTNINGSPEARUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = "PALADIN_LIGHTNINGSPEARUNLOCKABLE_REWARD_ID";
        //"texLightningSpearAchievement"
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