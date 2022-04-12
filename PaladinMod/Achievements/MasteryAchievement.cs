namespace PaladinMod.Achievements
{

    internal class MasteryAchievement : BaseMasteryUnlockable
    {
        public override string AchievementTokenPrefix => "PALADIN_MASTERY";
        public override string PrerequisiteIdentifier => "PALADIN_UNLOCKABLE_ACHIEVEMENT_ID";
        public override string AchievementSpriteName => "texMasteryAchievement";

        public override string RequiredCharacterBody => "RobPaladinBody";

        public override float RequiredDifficultyCoefficient => 3f;
    }
}