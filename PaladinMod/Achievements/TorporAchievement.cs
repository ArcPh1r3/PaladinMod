using PaladinMod.Modules;
using RoR2;
using System;
using UnityEngine;

namespace PaladinMod.Achievements
{
    internal class TorporAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = "PALADIN_TORPORUNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = "PALADIN_TORPORUNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = "PALADIN_TORPORUNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = "PALADIN_UNLOCKABLE_REWARD_ID";
        public override string UnlockableNameToken { get; } = "PALADIN_TORPORUNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = "PALADIN_TORPORUNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texTorporAchievement");

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
                            {
                                Language.GetString("PALADIN_TORPORUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("PALADIN_TORPORUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));
        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
                            {
                                Language.GetString("PALADIN_TORPORUNLOCKABLE_ACHIEVEMENT_NAME"),
                                Language.GetString("PALADIN_TORPORUNLOCKABLE_ACHIEVEMENT_DESC")
                            }));

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