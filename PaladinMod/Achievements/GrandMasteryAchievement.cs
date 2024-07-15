using RoR2;

namespace PaladinMod.Achievements
{
    [RegisterAchievement(identifier, unlockableIdentifier, "PALADIN_UNLOCKABLE_ACHIEVEMENT_ID", null)]
    internal class GrandMasteryAchievement : BaseMasteryUnlockable
    {
        public const string identifier = AchievementTokenPrefix + "UNLOCKABLE_ACHIEVEMENT_ID";
        public const string unlockableIdentifier = AchievementTokenPrefix + "UNLOCKABLE_REWARD_ID";

        public const string AchievementTokenPrefix = "PALADIN_TYPHOON";

        public override string RequiredCharacterBody => "RobPaladinBody";

        public override float RequiredDifficultyCoefficient => 3.5f;
    }
}