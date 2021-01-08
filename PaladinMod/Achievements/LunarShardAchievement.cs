using R2API;
using R2API.Utils;
using RoR2;
using System;

namespace PaladinMod.Achievements
{
    [R2APISubmoduleDependency(nameof(UnlockablesAPI))]

    public class LunarShardAchievement : ModdedUnlockableAndAchievement<CustomSpriteProvider>
    {
        public override String AchievementIdentifier { get; } = "PALADIN_LUNARSHARDUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "PALADIN_LUNARSHARDUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "PALADIN_LUNARSHARDUNLOCKABLE_PREREQ_ID";
        public override String AchievementNameToken { get; } = "PALADIN_LUNARSHARDUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "PALADIN_LUNARSHARDUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "PALADIN_LUNARSHARDUNLOCKABLE_UNLOCKABLE_NAME";
        protected override CustomSpriteProvider SpriteProvider { get; } = new CustomSpriteProvider("@Paladin:Assets/Paladin/Icons/texLunarShardAchievement.png");

        public override int LookUpRequiredBodyIndex()
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
                            if (self.inventory.GetTotalItemCountOfTier(ItemTier.Lunar) >= 8)
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