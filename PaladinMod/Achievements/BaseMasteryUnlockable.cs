using PaladinMod.Modules;
using RoR2;
using System;
using UnityEngine;
using R2API;
using RoR2.Achievements;

namespace PaladinMod.Achievements
{
    public abstract class BaseMasteryUnlockable : BaseAchievement
    {
        public abstract string RequiredCharacterBody { get; }
        public abstract float RequiredDifficultyCoefficient { get; }

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(RequiredCharacterBody);
        }

        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            Run.onClientGameOverGlobal += this.OnClientGameOverGlobal;
        }

        public override void OnBodyRequirementBroken()
        {
            Run.onClientGameOverGlobal -= this.OnClientGameOverGlobal;
            base.OnBodyRequirementBroken();
        }

        private void OnClientGameOverGlobal(Run run, RunReport runReport)
        {
            if (!runReport.gameEnding)
            {
                return;
            }
            if (runReport.gameEnding.isWin)
            {
                DifficultyIndex difficultyIndex = runReport.ruleBook.FindDifficulty();
                DifficultyDef difficultyDef = DifficultyCatalog.GetDifficultyDef(difficultyIndex);
                if (difficultyDef != null)
                {

                    bool isDifficulty = difficultyDef.countsAsHardMode && difficultyDef.scalingValue >= RequiredDifficultyCoefficient;
                    bool isInferno = difficultyDef.nameToken == "INFERNO_NAME";
                    bool isEclipse = difficultyIndex >= DifficultyIndex.Eclipse1 && difficultyIndex <= DifficultyIndex.Eclipse8;

                    if (isDifficulty || isInferno || isEclipse)
                    {
                        base.Grant();
                    }
                }
            }
        }
    }
}