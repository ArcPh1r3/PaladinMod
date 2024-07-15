using PaladinMod.Modules;
using RoR2;
using System;
using UnityEngine;
using R2API;

using RoR2.Achievements;

namespace PaladinMod.Achievements
{
    [RegisterAchievement(identifier, unlockableIdentifier, "PALADIN_UNLOCKABLE_ACHIEVEMENT_ID", null)]
    internal class PoisonAchievement : BaseAchievement
    {
        public const string identifier = "PALADIN_POISONUNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = "PALADIN_POISONUNLOCKABLE_REWARD_ID";

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("RobPaladinBody");
        }

        private void Check(On.RoR2.CharacterMaster.orig_OnInventoryChanged orig, CharacterMaster self)
        {
            if (self)
            {
                if (self.teamIndex == TeamIndex.Player)
                {
                    if (self.inventory)
                    {
                        if (self.GetBody() && self.GetBody().baseNameToken == "PALADIN_NAME")
                        {
                            if (self.inventory.GetItemCount(RoR2Content.Items.NovaOnHeal) > 0 || self.inventory.GetEquipmentIndex() == RoR2Content.Equipment.AffixPoison.equipmentIndex)
                            {
                                if (base.meetsBodyRequirement) base.Grant();
                            }
                        }
                    }
                }
            }

            orig(self);
        }

        public override void OnInstall()
        {
            base.OnInstall();

            On.RoR2.CharacterMaster.OnInventoryChanged += this.Check;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            On.RoR2.CharacterMaster.OnInventoryChanged -= this.Check;
        }
    }
}