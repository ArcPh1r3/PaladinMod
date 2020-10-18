using System.Reflection;
using R2API;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using RoR2;

namespace PaladinMod.Modules
{
    public static class Assets
    {
        public static AssetBundle mainAssetBundle;

        public static Texture charPortrait;

        public static Sprite iconP;
        public static Sprite icon1;
        public static Sprite icon2;
        public static Sprite icon3;
        public static Sprite icon4;

        public static GameObject lightningSpear;
        public static GameObject swordBeam;

        public static GameObject lightningHitFX;
        public static GameObject lightningImpactFX;

        public static void PopulateAssets()
        {
            if (mainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PaladinMod.paladin"))
                {
                    mainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                    var provider = new AssetBundleResourcesProvider("@Paladin", mainAssetBundle);
                    ResourcesAPI.AddProvider(provider);
                }
            }

            /*using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("MinerV2.MinerBank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }*/

            charPortrait = mainAssetBundle.LoadAsset<Sprite>("texPaladinIcon").texture;

            iconP = mainAssetBundle.LoadAsset<Sprite>("SlashIcon");
            icon1 = mainAssetBundle.LoadAsset<Sprite>("SlashIcon");
            icon2 = mainAssetBundle.LoadAsset<Sprite>("SlashIcon");
            icon3 = mainAssetBundle.LoadAsset<Sprite>("SlashIcon");
            icon4 = mainAssetBundle.LoadAsset<Sprite>("SlashIcon");

            lightningSpear = mainAssetBundle.LoadAsset<GameObject>("LightningSpear");
            swordBeam = mainAssetBundle.LoadAsset<GameObject>("SwordBeam");

            lightningHitFX = mainAssetBundle.LoadAsset<GameObject>("LightningHitFX");
            lightningImpactFX = mainAssetBundle.LoadAsset<GameObject>("LightningImpact");

            lightningHitFX.AddComponent<DestroyOnTimer>().duration = 8;
            lightningHitFX.AddComponent<NetworkIdentity>();
            lightningHitFX.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            var effect = lightningHitFX.AddComponent<EffectComponent>();
            effect.applyScale = false;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = true;
            effect.positionAtReferencedTransform = true;
            effect.soundName = "Play_mage_R_lightningBlast";

            lightningImpactFX.AddComponent<DestroyOnTimer>().duration = 8;
            lightningImpactFX.AddComponent<NetworkIdentity>();
            lightningImpactFX.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            effect = lightningImpactFX.AddComponent<EffectComponent>();
            effect.applyScale = false;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = true;
            effect.positionAtReferencedTransform = true;
            effect.soundName = "";

            EffectAPI.AddEffect(lightningHitFX);
            EffectAPI.AddEffect(lightningImpactFX);
        }
    }
}
