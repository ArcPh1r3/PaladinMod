using System.Reflection;
using R2API;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using RoR2;
using RoR2.Projectile;

namespace PaladinMod.Modules
{
    public static class Assets
    {
        public static AssetBundle mainAssetBundle;

        public static Texture charPortrait;

        public static Sprite iconP;
        public static Sprite icon1;
        public static Sprite icon2;
        public static Sprite icon2b;
        public static Sprite icon2c;
        public static Sprite icon3;
        public static Sprite icon3b;
        public static Sprite icon4;
        public static Sprite icon4b;

        public static GameObject lightningSpear;
        public static GameObject swordBeam;
        public static GameObject swordBeamGhost;
        public static GameObject tornadoEffect;

        public static GameObject healEffectPrefab;
        public static GameObject healZoneEffectPrefab;
        public static GameObject torporEffectPrefab;

        public static GameObject swordSwing;
        public static GameObject spinningSlashFX;
        public static GameObject spinningSlashEmpoweredFX;

        public static GameObject swordSwingGreen;
        public static GameObject spinningSlashFXGreen;
        public static GameObject spinningSlashEmpoweredFXGreen;

        public static GameObject swordSwingYellow;
        public static GameObject spinningSlashFXYellow;
        public static GameObject spinningSlashEmpoweredFXYellow;

        public static GameObject hitFX;
        public static GameObject hitFXGreen;
        public static GameObject hitFXYellow;

        public static GameObject lightningHitFX;
        public static GameObject lightningImpactFX;
        public static GameObject altLightningImpactFX;

        public static GameObject torporVoidFX;

        public static Mesh defaultMesh;
        public static Mesh defaultSwordMesh;
        public static Mesh lunarMesh;
        public static Mesh lunarSwordMesh;
        public static Mesh poisonMesh;
        public static Mesh poisonSwordMesh;
        //public static Mesh hunterMesh;

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

            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("PaladinMod.PaladinBank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }

            charPortrait = mainAssetBundle.LoadAsset<Sprite>("texPaladinIcon").texture;

            iconP = mainAssetBundle.LoadAsset<Sprite>("PassiveIcon");
            icon1 = mainAssetBundle.LoadAsset<Sprite>("SlashIcon");
            icon2 = mainAssetBundle.LoadAsset<Sprite>("SpinSlashIcon");
            icon2b = mainAssetBundle.LoadAsset<Sprite>("LightningSpearIcon");
            icon2c = mainAssetBundle.LoadAsset<Sprite>("LunarShardIcon");
            icon3 = mainAssetBundle.LoadAsset<Sprite>("DashIcon");
            icon3b = mainAssetBundle.LoadAsset<Sprite>("HealIcon");
            icon4 = mainAssetBundle.LoadAsset<Sprite>("HealZoneIcon");
            icon4b = mainAssetBundle.LoadAsset<Sprite>("TorporIcon");

            lightningSpear = mainAssetBundle.LoadAsset<GameObject>("LightningSpear");
            swordBeam = mainAssetBundle.LoadAsset<GameObject>("SwordBeam");
            swordBeamGhost = mainAssetBundle.LoadAsset<GameObject>("SwordBeamGhost");
            swordBeamGhost.AddComponent<ProjectileGhostController>();
            tornadoEffect = mainAssetBundle.LoadAsset<GameObject>("PaladinTornadoEffect");
            tornadoEffect.AddComponent<ProjectileGhostController>();

            healEffectPrefab = mainAssetBundle.LoadAsset<GameObject>("HealEffect");
            healZoneEffectPrefab = mainAssetBundle.LoadAsset<GameObject>("HealZoneEffect");
            torporEffectPrefab = mainAssetBundle.LoadAsset<GameObject>("TorporEffect");

            GameObject engiShieldObj = Resources.Load<GameObject>("Prefabs/Projectiles/EngiBubbleShield");

            Material shieldFillMat = UnityEngine.Object.Instantiate<Material>(engiShieldObj.transform.Find("Collision").Find("ActiveVisual").GetComponent<MeshRenderer>().material);
            Material shieldOuterMat = UnityEngine.Object.Instantiate<Material>(engiShieldObj.transform.Find("Collision").Find("ActiveVisual").Find("Edge").GetComponent<MeshRenderer>().material);

            shieldOuterMat.SetTexture("_EmTex", mainAssetBundle.LoadAsset<Texture>("texHealZone"));
            Material torporMat = UnityEngine.Object.Instantiate<Material>(shieldOuterMat);
            torporMat.SetTexture("_EmTex", mainAssetBundle.LoadAsset<Texture>("texTorpor"));

            healZoneEffectPrefab.GetComponentInChildren<ParticleSystemRenderer>().material = shieldOuterMat;
            torporEffectPrefab.GetComponentInChildren<ParticleSystemRenderer>().material = UnityEngine.Object.Instantiate<Material>(shieldOuterMat);
            torporEffectPrefab.GetComponentInChildren<ParticleSystemRenderer>().material.SetColor("_TintColor", Color.red);

            swordSwing = Assets.LoadEffect("PaladinSwing", "");
            spinningSlashFX = Assets.LoadEffect("SpinSlashEffect", "");
            spinningSlashEmpoweredFX = Assets.LoadEffect("EmpSpinSlashEffect", "");
            hitFX = Assets.LoadEffect("ImpactPaladinSwing", "");
            swordSwingGreen = Assets.LoadEffect("PaladinSwingGreen", "");
            spinningSlashFXGreen = Assets.LoadEffect("SpinSlashEffectGreen", "");
            spinningSlashEmpoweredFXGreen = Assets.LoadEffect("EmpSpinSlashEffectGreen", "");
            hitFXGreen = Assets.LoadEffect("ImpactPaladinSwingGreen", "");
            swordSwingYellow = Assets.LoadEffect("PaladinSwingYellow", "");
            spinningSlashFXYellow = Assets.LoadEffect("SpinSlashEffectYellow", "");
            spinningSlashEmpoweredFXYellow = Assets.LoadEffect("EmpSpinSlashEffectYellow", "");
            hitFXYellow = Assets.LoadEffect("ImpactPaladinSwingYellow", "");
            lightningHitFX = Assets.LoadEffect("LightningHitFX", "");
            lightningImpactFX = Assets.LoadEffect("LightningImpact", "Play_mage_R_lightningBlast");
            torporVoidFX = Assets.LoadEffect("TorporVoidFX", "RoR2_nullifier_attack1_explode_02");

            defaultMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladin");
            defaultSwordMesh = mainAssetBundle.LoadAsset<Mesh>("meshSword");
            lunarMesh = mainAssetBundle.LoadAsset<Mesh>("meshLunarPaladin");
            lunarSwordMesh = mainAssetBundle.LoadAsset<Mesh>("meshLunarSword");
            poisonMesh = mainAssetBundle.LoadAsset<Mesh>("meshNkuhanaPaladin");
            poisonSwordMesh = mainAssetBundle.LoadAsset<Mesh>("meshNkuhanaSword");
            //hunterMesh = mainAssetBundle.LoadAsset<Mesh>("HunterMesh");

            altLightningImpactFX = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/LightningStrikeImpact"), "PaladinLightningStrikeImpact", true);

            PaladinPlugin.Destroy(altLightningImpactFX.transform.Find("LightningRibbon").gameObject);

            foreach (ParticleSystemRenderer i in altLightningImpactFX.GetComponentsInChildren<ParticleSystemRenderer>())
            {
                if (i)
                {
                    i.material.SetColor("_TintColor", Color.yellow);
                }
            }

            altLightningImpactFX.AddComponent<NetworkIdentity>();

            EffectAPI.AddEffect(altLightningImpactFX);
        }

        private static GameObject LoadEffect(string resourceName, string soundName)
        {
            GameObject newEffect = mainAssetBundle.LoadAsset<GameObject>(resourceName);

            newEffect.AddComponent<DestroyOnTimer>().duration = 12;
            newEffect.AddComponent<NetworkIdentity>();
            newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            var effect = newEffect.AddComponent<EffectComponent>();
            effect.applyScale = false;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = true;
            effect.positionAtReferencedTransform = true;
            effect.soundName = soundName;

            EffectAPI.AddEffect(newEffect);

            return newEffect;
        }
    }
}
