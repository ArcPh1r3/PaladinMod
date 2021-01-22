using R2API;
using RoR2;
using UnityEngine.Networking;

namespace PaladinMod.Modules
{
    public static class Survivors
    {
        public static void RegisterSurvivors()
        {
            Prefabs.paladinDisplayPrefab.AddComponent<NetworkIdentity>();

            string unlockString = "PALADIN_UNLOCKABLE_REWARD_ID";
            if (Config.forceUnlock.Value) unlockString = "";

            SurvivorDef survivorDef = new SurvivorDef
            {
                name = "PALADIN_NAME",
                unlockableName = unlockString,
                descriptionToken = "PALADIN_DESCRIPTION",
                primaryColor = PaladinPlugin.characterColor,
                bodyPrefab = Prefabs.paladinPrefab,
                displayPrefab = Prefabs.paladinDisplayPrefab,
                outroFlavorToken = "PALADIN_OUTRO_FLAVOR"
            };

            SurvivorAPI.AddSurvivor(survivorDef);


            /*SurvivorDef tempDef = new SurvivorDef
            {
                name = "LUNAR_KNIGHT_BODY_NAME",
                unlockableName = "",
                descriptionToken = "LUNAR_KNIGHT_BODY_DESCRIPTION",
                primaryColor = PaladinPlugin.characterColor,
                bodyPrefab = Prefabs.lunarKnightPrefab,
                displayPrefab = Prefabs.paladinDisplayPrefab,
                outroFlavorToken = "LUNAR_KNIGHT_BODY_OUTRO_FLAVOR"
            };

            SurvivorAPI.AddSurvivor(tempDef);*/
        }
    }
}