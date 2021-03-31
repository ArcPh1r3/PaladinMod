using PaladinMod.Modules;
using RoR2;
using System;
using UnityEngine;

namespace PaladinMod.Achievements
{
    internal class ClayAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = "PALADIN_CLAYUNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = "PALADIN_CLAYUNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = "PALADIN_CLAYUNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = "PALADIN_UNLOCKABLE_REWARD_ID";
        public override string UnlockableNameToken { get; } = "PALADIN_CLAYUNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = "PALADIN_CLAYUNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texClayAchievement");

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString("PALADIN_CLAYUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("PALADIN_CLAYUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString("PALADIN_CLAYUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("PALADIN_CLAYUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));

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
                            if (self.inventory.GetItemCount(RoR2Content.Items.SiphonOnLowHealth) > 0)
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