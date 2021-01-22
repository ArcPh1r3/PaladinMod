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
        //the bundle to load assets from
        public static AssetBundle mainAssetBundle;

        //character portrait
        public static Texture charPortrait;

        //torpor material
        public static Material torporMat;

        //skill icons
        public static Sprite iconP;
        public static Sprite icon1;
        public static Sprite icon2;
        public static Sprite icon2b;
        public static Sprite icon2c;
        public static Sprite icon3;
        public static Sprite icon3b;
        public static Sprite icon4;
        public static Sprite icon4b;
        public static Sprite icon4c;
        public static Sprite icon4S;
        public static Sprite icon4bS;
        public static Sprite icon4cS;

        //projectile ghosts
        public static GameObject lightningSpear;
        public static GameObject swordBeam;
        public static GameObject swordBeamGhost;
        public static GameObject tornadoEffect;

        //spell effects
        public static GameObject healEffectPrefab;
        public static GameObject healZoneEffectPrefab;
        public static GameObject torporEffectPrefab;
        public static GameObject warcryEffectPrefab;

        //particle effects
        public static GameObject swordSwing;
        public static GameObject spinningSlashFX;
        public static GameObject spinningSlashEmpoweredFX;

        public static GameObject swordSwingGreen;
        public static GameObject spinningSlashFXGreen;
        public static GameObject spinningSlashEmpoweredFXGreen;

        public static GameObject swordSwingYellow;
        public static GameObject spinningSlashFXYellow;
        public static GameObject spinningSlashEmpoweredFXYellow;

        public static GameObject swordSwingWhite;

        public static GameObject swordSwingBat;

        public static GameObject swordSwingRed;
        public static GameObject spinningSlashFXRed;
        public static GameObject spinningSlashEmpoweredFXRed;

        public static GameObject swordSwingClay;
        public static GameObject spinningSlashFXClay;
        public static GameObject spinningSlashEmpoweredFXClay;

        public static GameObject swordSwingPurple;
        public static GameObject spinningSlashFXPurple;
        public static GameObject spinningSlashEmpoweredFXPurple;

        public static GameObject swordSwingFlame;
        public static GameObject spinningSlashFXFlame;
        public static GameObject spinningSlashEmpoweredFXFlame;

        public static GameObject swordSwingBlack;
        public static GameObject spinningSlashFXBlack;
        public static GameObject spinningSlashEmpoweredFXBlack;

        public static GameObject hitFX;
        public static GameObject hitFXGreen;
        public static GameObject hitFXYellow;
        public static GameObject hitFXBlunt;
        public static GameObject hitFXRed;
        public static GameObject hitFXClay;
        public static GameObject hitFXPurple;
        public static GameObject hitFXBlack;

        public static GameObject lightningHitFX;
        public static GameObject lightningImpactFX;
        public static GameObject altLightningImpactFX;

        public static GameObject dashFX;

        public static GameObject torporVoidFX;

        //skin meshes
        public static Mesh defaultMesh;
        public static Mesh defaultSwordMesh;
        public static Mesh lunarMesh;
        public static Mesh lunarSwordMesh;
        public static Mesh poisonMesh;
        public static Mesh poisonSwordMesh;
        //public static Mesh hunterMesh;
        public static Mesh dripMesh;
        public static Mesh batMesh;
        public static Mesh clayMesh;
        public static Mesh claySwordMesh;
        public static Mesh minecraftMesh;
        public static Mesh minecraftSwordMesh;

        //dark souls shields
        public static GameObject artoriasShield;
        public static GameObject blackKnightShield;
        public static GameObject giantShield;
        public static GameObject goldenShield;
        public static GameObject havelShield;
        public static GameObject pursuerShield;
        public static GameObject sunlightShield;
        public static GameObject hawkwoodShield;
        public static GameObject dragonHeadShield;
        public static GameObject watcherDagger;

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

            #region Icons
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
            icon4c = mainAssetBundle.LoadAsset<Sprite>("WarcryIcon");
            icon4S = mainAssetBundle.LoadAsset<Sprite>("ScepterHealZoneIcon");
            icon4bS = mainAssetBundle.LoadAsset<Sprite>("ScepterTorporIcon");
            icon4cS = mainAssetBundle.LoadAsset<Sprite>("ScepterWarcryIcon");
            #endregion

            #region ProjectileGhosts
            lightningSpear = mainAssetBundle.LoadAsset<GameObject>("LightningSpear");
            swordBeam = mainAssetBundle.LoadAsset<GameObject>("SwordBeam");
            swordBeamGhost = mainAssetBundle.LoadAsset<GameObject>("SwordBeamGhost");
            swordBeamGhost.AddComponent<ProjectileGhostController>();
            tornadoEffect = mainAssetBundle.LoadAsset<GameObject>("PaladinTornadoEffect");
            tornadoEffect.AddComponent<ProjectileGhostController>();
            #endregion

            #region SpellEffects
            healEffectPrefab = mainAssetBundle.LoadAsset<GameObject>("HealEffect");
            healZoneEffectPrefab = mainAssetBundle.LoadAsset<GameObject>("HealZoneEffect");
            torporEffectPrefab = mainAssetBundle.LoadAsset<GameObject>("TorporEffect");
            warcryEffectPrefab = mainAssetBundle.LoadAsset<GameObject>("HealZoneEffect").InstantiateClone("WarcryEffect", false);

            GameObject engiShieldObj = Resources.Load<GameObject>("Prefabs/Projectiles/EngiBubbleShield");

            Material shieldFillMat = UnityEngine.Object.Instantiate<Material>(engiShieldObj.transform.Find("Collision").Find("ActiveVisual").GetComponent<MeshRenderer>().material);
            Material shieldOuterMat = UnityEngine.Object.Instantiate<Material>(engiShieldObj.transform.Find("Collision").Find("ActiveVisual").Find("Edge").GetComponent<MeshRenderer>().material);

            GameObject voidExplosionObj = Resources.Load<GameObject>("Prefabs/Effects/NullifierDeathExplosion");//DeathExplosion
            Material voidMat = voidExplosionObj.transform.Find("AreaIndicator (1)").GetComponent<ParticleSystemRenderer>().material;
            torporMat = voidMat;

            GameObject healNovaObj = Resources.Load<GameObject>("Prefabs/Effects/TPHealNovaEffect");
            Material healMat = healNovaObj.transform.Find("AreaIndicator").GetComponent<ParticleSystemRenderer>().material;

            healZoneEffectPrefab.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = shieldOuterMat;
            healZoneEffectPrefab.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = healMat;

            torporEffectPrefab.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = voidMat;

            warcryEffectPrefab.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = shieldOuterMat;
            warcryEffectPrefab.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", Color.red);
            warcryEffectPrefab.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = Resources.Load<Material>("materials/matFullCrit");
            //
            //GameObject warbannerEffect = Resources.Load<GameObject>("Prefabs/NetworkedObjects/WarbannerWard").InstantiateClone("x", true);
            #endregion

            #region SwordEffects
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

            swordSwingWhite = Assets.LoadEffect("PaladinSwingWhite", "");
            hitFXBlunt = Assets.LoadEffect("ImpactPaladinSwingBlunt", "");

            swordSwingBat = Assets.LoadEffect("PaladinSwingBat", "");

            swordSwingRed = Assets.LoadEffect("PaladinSwingRed", "");
            spinningSlashFXRed = Assets.LoadEffect("SpinSlashEffectRed", "");
            spinningSlashEmpoweredFXRed = Assets.LoadEffect("EmpSpinSlashEffectRed", "");
            hitFXRed = Assets.LoadEffect("ImpactPaladinSwingRed", "");

            swordSwingClay = Assets.LoadEffect("PaladinSwingClay", "");
            spinningSlashFXClay = Assets.LoadEffect("SpinSlashEffectClay", "");
            spinningSlashEmpoweredFXClay = Assets.LoadEffect("EmpSpinSlashEffectClay", "");
            hitFXClay = Assets.LoadEffect("ImpactPaladinSwingClay", "");

            swordSwingPurple = Assets.LoadEffect("PaladinSwingPurple", "");
            spinningSlashFXPurple = Assets.LoadEffect("SpinSlashEffectPurple", "");
            spinningSlashEmpoweredFXPurple = Assets.LoadEffect("EmpSpinSlashEffectPurple", "");
            hitFXPurple = Assets.LoadEffect("ImpactPaladinSwingPurple", "");

            swordSwingFlame = Assets.LoadEffect("PaladinSwingFlame", "");
            spinningSlashFXFlame = Assets.LoadEffect("SpinSlashEffectFlame", "");
            spinningSlashEmpoweredFXFlame = Assets.LoadEffect("EmpSpinSlashEffectFlame", "");

            swordSwingBlack = Assets.LoadEffect("PaladinSwingBlack", "");
            spinningSlashFXBlack = Assets.LoadEffect("SpinSlashEffectBlack", "");
            spinningSlashEmpoweredFXBlack = Assets.LoadEffect("EmpSpinSlashEffectBlack", "");
            hitFXBlack = Assets.LoadEffect("ImpactPaladinSwingBlack", "");
            #endregion

            #region MiscEffects
            lightningHitFX = Assets.LoadEffect("LightningHitFX", "");
            lightningImpactFX = Assets.LoadEffect("LightningImpact", "Play_mage_R_lightningBlast");
            torporVoidFX = Assets.LoadEffect("TorporVoidFX", "RoR2_nullifier_attack1_explode_02");
            #endregion

            #region Meshes
            defaultMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladin");
            defaultSwordMesh = mainAssetBundle.LoadAsset<Mesh>("meshSword");
            lunarMesh = mainAssetBundle.LoadAsset<Mesh>("meshLunarPaladin");
            lunarSwordMesh = mainAssetBundle.LoadAsset<Mesh>("meshLunarSword");
            poisonMesh = mainAssetBundle.LoadAsset<Mesh>("meshNkuhanaPaladin");
            poisonSwordMesh = mainAssetBundle.LoadAsset<Mesh>("meshNkuhanaSword");
            //hunterMesh = mainAssetBundle.LoadAsset<Mesh>("HunterMesh");
            dripMesh = mainAssetBundle.LoadAsset<Mesh>("meshDripPaladin");
            batMesh = mainAssetBundle.LoadAsset<Mesh>("meshBat");
            clayMesh = mainAssetBundle.LoadAsset<Mesh>("meshClayPaladin");
            claySwordMesh = mainAssetBundle.LoadAsset<Mesh>("meshClaySword");
            minecraftMesh = mainAssetBundle.LoadAsset<Mesh>("meshMinecraftPaladin");
            minecraftSwordMesh = mainAssetBundle.LoadAsset<Mesh>("meshMinecraftSword");
            #endregion

            //weird shit to get the lightning effect looking how i want it
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

            //clone mithrix's dash effect and resize it for my dash
            dashFX = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Effects/BrotherDashEffect"), "PaladinDashEffect", true);
            dashFX.AddComponent<NetworkIdentity>();
            dashFX.transform.localScale *= 0.3f;

            EffectAPI.AddEffect(dashFX);

            InitCustomItems();
        }

        private static void InitCustomItems()
        {
            //create item display prefabs for all of the shields
            artoriasShield = CreateItemDisplay("DisplayArtoriasShield", "matArtyShield");
            blackKnightShield = CreateItemDisplay("DisplayBKShield", "matBlackKnightShield");
            giantShield = CreateItemDisplay("DisplayGiantShield", "matGiantShield");
            goldenShield = CreateItemDisplay("DisplayGoldenShield", "matGoldenShield");
            havelShield = CreateItemDisplay("DisplayHavelShield", "matHavelShield");
            pursuerShield = CreateItemDisplay("DisplayPursuerShield", "matPursuerShield");
            sunlightShield = CreateItemDisplay("DisplaySunlightShield", "matSunlightShield");
            watcherDagger = CreateItemDisplay("DisplayWatcherDagger", "matWatcherDagger");
        }

        private static GameObject CreateItemDisplay(string prefabName, string matName)
        {
            GameObject displayPrefab = mainAssetBundle.LoadAsset<GameObject>(prefabName);
            Material itemMat = Skins.CreateMaterial(matName, 0, Color.black, 0);
            MeshRenderer renderer = displayPrefab.GetComponent<MeshRenderer>();

            renderer.material = itemMat;
            displayPrefab.AddComponent<ItemDisplay>().rendererInfos = new CharacterModel.RendererInfo[]
            {
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = itemMat,
                    renderer = renderer,
                    ignoreOverlays = false,
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On
                }
            };

            return displayPrefab;
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