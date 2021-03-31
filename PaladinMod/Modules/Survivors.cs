using EntityStates;
using PaladinMod.States;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.Modules
{
    public static class Survivors
    {
        public static void RegisterSurvivors()
        {
            Prefabs.paladinDisplayPrefab.AddComponent<NetworkIdentity>();

            UnlockableDef unlockDef = Modules.Unlockables.paladinUnlockDef;
            if (Config.forceUnlock.Value) unlockDef = null;

            SurvivorDef survivorDef = ScriptableObject.CreateInstance<SurvivorDef>();
            survivorDef.bodyPrefab = Prefabs.paladinPrefab;
            survivorDef.displayPrefab = Prefabs.paladinDisplayPrefab;
            survivorDef.primaryColor = PaladinPlugin.characterColor;
            survivorDef.displayNameToken = "PALADIN_NAME";
            survivorDef.descriptionToken = "PALADIN_DESCRIPTION";
            survivorDef.outroFlavorToken = "PALADIN_OUTRO_FLAVOR";
            survivorDef.mainEndingEscapeFailureFlavorToken = "PALADIN_OUTRO_FAILURE";
            survivorDef.desiredSortPosition = 50f;
            survivorDef.unlockableDef = unlockDef;

            Modules.Prefabs.survivorDefinitions.Add(survivorDef);

            EntityStateMachine paladinStateMachine = Prefabs.paladinPrefab.GetComponent<EntityStateMachine>();
            paladinStateMachine.mainStateType = new SerializableEntityStateType(typeof(PaladinMain));
            paladinStateMachine.initialStateType = new SerializableEntityStateType(typeof(SpawnState));

            EntityStateMachine lunarKnightStateMachine = Prefabs.lunarKnightPrefab.GetComponent<EntityStateMachine>();
            lunarKnightStateMachine.mainStateType = new SerializableEntityStateType(typeof(PaladinMain));
            lunarKnightStateMachine.initialStateType = new SerializableEntityStateType(typeof(SpawnState));
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