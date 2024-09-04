using MonoMod.RuntimeDetour;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PaladinMod.Modules
{
    internal class MemeCompat
    {
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void Init()
        {
            On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
        }

        private static void SurvivorCatalog_Init(On.RoR2.SurvivorCatalog.orig_Init orig)
        {
            orig();
            GameObject skele = Modules.Asset.mainAssetBundle.LoadAsset<GameObject>("Paladin_Meme");
            EmotesAPI.CustomEmotesAPI.ImportArmature(Prefabs.paladinPrefab, skele, true);
        }
    }
}
