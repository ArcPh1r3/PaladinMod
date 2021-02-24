using R2API;
using R2API.Utils;
using RoR2;
using System;

namespace PaladinMod.Achievements
{
    [R2APISubmoduleDependency(nameof(UnlockablesAPI))]

    public class GrandMasteryAchievement : ModdedUnlockableAndAchievement<CustomSpriteProvider>
    {
        public override String AchievementIdentifier { get; } = "PALADIN_TYPHOONUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "PALADIN_TYPHOONUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "PALADIN_TYPHOONUNLOCKABLE_PREREQ_ID";
        public override String AchievementNameToken { get; } = "PALADIN_TYPHOONUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "PALADIN_TYPHOONUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "PALADIN_TYPHOONUNLOCKABLE_UNLOCKABLE_NAME";
        protected override CustomSpriteProvider SpriteProvider { get; } = new CustomSpriteProvider("@Paladin:Assets/Paladin/Icons/texMasteryAchievement.png");

        public override int LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("RobPaladinBody");
        }

        public void ClearCheck(Run run, RunReport runReport)
        {
            if (run is null) return;
            if (runReport is null) return;

            if (!runReport.gameEnding) return;

            if (runReport.gameEnding.isWin)
            {
                DifficultyDef difficultyDef = DifficultyCatalog.GetDifficultyDef(runReport.ruleBook.FindDifficulty());

                if (difficultyDef != null && difficultyDef.nameToken == "DIFFICULTY_TYPHOON_NAME")
                {
                    if (base.meetsBodyRequirement)
                    {
                        base.Grant();
                    }
                }
            }
        }

        public override void OnInstall()
        {
            base.OnInstall();

            Run.onClientGameOverGlobal += this.ClearCheck;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            Run.onClientGameOverGlobal -= this.ClearCheck;
        }
    }
}