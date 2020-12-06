using R2API;

namespace PaladinMod.Modules
{
    public static class Unlockables
    {
        public static void RegisterUnlockables()
        {
            UnlockablesAPI.AddUnlockable<Achievements.PaladinUnlockAchievement>(true);
            UnlockablesAPI.AddUnlockable<Achievements.MasteryAchievement>(true);
            UnlockablesAPI.AddUnlockable<Achievements.PoisonAchievement>(true);
        }
    }
}
