namespace PaladinMod.Achievements
{
    internal class GrandMasteryAchievement : BaseMasteryUnlockable
    {
        public override string AchievementTokenPrefix => "PALADIN_TYPHOON";
        public override string PrerequisiteIdentifier => "PALADIN_UNLOCKABLE_ACHIEVEMENT_ID";
        public override string AchievementSpriteName => "texGrandMasteryAchievement";

        public override string RequiredCharacterBody => "RobPaladinBody";

        public override float RequiredDifficultyCoefficient => 3.5f;
    }
}