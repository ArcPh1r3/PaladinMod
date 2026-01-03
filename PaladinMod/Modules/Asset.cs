using System.Reflection;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using R2API;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.AddressableAssets;
using RoR2.Audio;
using EntityStates;
using Path = System.IO.Path;
using System.Runtime.ExceptionServices;

namespace PaladinMod.Modules
{
    public static class Asset
    {
        //the bundle to load assets from
        public static AssetBundle mainAssetBundle;
        public static AssetBundle VRAssetBundle = null;

        //character portrait
        public static Texture charPortrait;

        //heheha
        public static Material blessingMat;

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
        public static Sprite icon4d;
        public static Sprite icon4dS;

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
        public static GameObject paladinSunPrefab;
        public static GameObject paladinSunSpawnPrefab;
        public static GameObject paladinScepterSunPrefab;

        //particle effects

        public static GameObject swordSwing;
        public static GameObject swordSwingEmpowered;
        public static GameObject spinningSlashFX;
        public static GameObject spinningSlashEmpoweredFX;

        public static GameObject swordSwingGreen;
        public static GameObject spinningSlashFXGreen;
        public static GameObject spinningSlashEmpoweredFXGreen;

        public static GameObject swordSwingYellow;
        public static GameObject spinningSlashFXYellow;
        public static GameObject spinningSlashEmpoweredFXYellow;

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

        public static GameObject swordSwingWhite;

        public static GameObject swordSwingBat;

        public static GameObject hitFX;
        public static GameObject hitFXGreen;
        public static GameObject hitFXYellow;
        public static GameObject hitFXRed;
        public static GameObject hitFXClay;
        public static GameObject hitFXPurple;
        public static GameObject hitFXBlack;
        public static GameObject hitFXBlunt;

        public static List<GameObject> hitEffects = new List<GameObject> ();
        public static List<GameObject> swordSwingEffects = new List<GameObject>();
        public static List<GameObject> spinSlashEffects = new List<GameObject>();
        public static List<GameObject> spinSlashEmpoweredEffects = new List<GameObject>();


        public static GameObject lightningHitFX;
        public static GameObject lightningImpactFX;
        public static GameObject altLightningImpactFX;

        public static GameObject dashFX;
        public static GameObject airSlamFX;
        public static GameObject airSlamBoostedFX;

        public static GameObject torporVoidFX;

        #region Mesh
        //skin meshes
        public static Mesh defaultMesh;
        public static Mesh defaultSwordMesh;
        public static Mesh defaultCapeMesh;
        public static Mesh lunarMesh;
        public static Mesh lunarSwordMesh;
        public static Mesh lunarKnightMesh;
        public static Mesh lunarKnightSwordMesh;
        public static Mesh lunarKnightCapeMesh;
        public static Mesh gmMesh;
        public static Mesh gmSwordMesh;
        public static Mesh gmCapeMesh;
        public static Mesh gmLegacyMesh;
        public static Mesh gmLegacySwordMesh;
        public static Mesh poisonMesh;
        public static Mesh poisonSwordMesh;
        public static Mesh poisonLegacyMesh;
        public static Mesh poisonLegacySwordMesh;
        public static Mesh poisonLegacyCapeMesh;
        public static Mesh specterMesh;
        public static Mesh specterSwordMesh;
        //public static Mesh hunterMesh;
        public static Mesh dripMesh;
        public static Mesh batMesh;
        public static Mesh clayMesh;
        public static Mesh claySwordMesh;
        public static Mesh minecraftMesh;
        public static Mesh minecraftSwordMesh;
        #endregion

        #region Shields
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
        #endregion

        #region Sounds
        // networked hit sounds
        internal static NetworkSoundEventDef swordHitSoundEventS;
        internal static NetworkSoundEventDef swordHitSoundEventM;
        internal static NetworkSoundEventDef swordHitSoundEventL;
        internal static NetworkSoundEventDef batHitSoundEventS;
        internal static NetworkSoundEventDef batHitSoundEventM;
        internal static NetworkSoundEventDef batHitSoundEventL;
        #endregion

        #region Materials
        // vfx materials
        // this is pointless after addressables but idc
        internal static Material supplyDropMat;
        internal static Material airStrikeMat;
        internal static Material crippleSphereMat;
        internal static Material areaIndicatorMat;
        internal static Material matMeteorIndicator;

        internal static Material matBlueLightningLong;
        internal static Material matYellowLightningLong;
        internal static Material matJellyfishLightning;
        internal static Material matJellyfishLightningLarge;
        internal static Material matMageMatrixDirectionalLightning;
        internal static Material matDistortion;
        internal static Material matMercSwipe;
        internal static Material matLoaderLightningSphere;
        #endregion

        #region PostProcessing
        internal static PostProcessProfile grandParentPP;
        #endregion


        #region VRAssets
        public static GameObject vrPaladinDominantHand;
        public static GameObject vrPaladinNonDominantHand;

        public static void loadVRBundle()
        {
            try
            {
                VRAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(PaladinPlugin.instance.Info.Location), "AssetBundles", "paladinvr"));

                vrPaladinDominantHand = VRAssetBundle.LoadAsset<GameObject>("PaladinDominantHand");
                vrPaladinNonDominantHand = VRAssetBundle.LoadAsset<GameObject>("PaladinNonDominantHand");  
            }
            catch (System.Exception e) { 
                PaladinPlugin.logger.LogError($"Error loading paladinvr bundle\n" + e);
            } 
        }
        #endregion

        internal static List<EffectDef> effectDefs = new List<EffectDef>();
        internal static List<NetworkSoundEventDef> networkSoundEventDefs = new List<NetworkSoundEventDef>();

        public static Material capeMat;

        public static void LoadAssetBundle()
        {
            try
            {
                mainAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(PaladinPlugin.instance.Info.Location), "AssetBundles", "paladin"));
            }
            catch (System.Exception e)
            {
                PaladinPlugin.logger.LogError($"Error loading paladin bundle\n" + e);
            }
        }

        public static void PopulateAssets() {

            LoadAssetBundle();

            GatherMaterials();

            PopulateIcons();

            PopulateProjectileGhosts();

            PopulateSpellEffects();

            PopulateSwordEffects();

            PopulateMiscEffects();

            PopulateMeshes();
            //capeMat = Skins.CreateMaterial("matPaladinGMOld", 1, Color.white);
            //was this being used anywhere?

            altLightningImpactFX = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/LightningStrikeImpact"), "PaladinLightningStrikeImpact", true);
            PaladinPlugin.Destroy(altLightningImpactFX.transform.Find("LightningRibbon").gameObject);
            PaladinPlugin.Destroy(altLightningImpactFX.transform.Find("Ring").gameObject);
            altLightningImpactFX.transform.Find("PostProcess").GetComponent<PostProcessVolume>().sharedProfile = grandParentPP;
            altLightningImpactFX.transform.Find("Sphere").GetComponent<ParticleSystemRenderer>().material = matLoaderLightningSphere;
            Light light = altLightningImpactFX.GetComponentInChildren<Light>();
            light.intensity = 80f;
            light.color = Color.yellow;

            altLightningImpactFX.AddComponent<NetworkIdentity>();

            AddEffect(altLightningImpactFX, altLightningImpactFX.GetComponent<EffectComponent>().soundName);

            //clone mithrix's dash effect and resize it for my dash
            dashFX = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/BrotherDashEffect"), "PaladinDashEffect", false);
            dashFX.GetComponent<EffectComponent>().applyScale = true;
            dashFX.transform.localScale *= 0.35f;

            dashFX.transform.Find("Dash").GetComponent<ParticleSystemRenderer>().material = Addressables.LoadAssetAsync<Material>("RoR2/Base/Huntress/matHuntressSwipe.mat").WaitForCompletion();
            dashFX.transform.Find("Dash").transform.localPosition = new Vector3(0f, 0f, -8f);
            dashFX.transform.Find("Donut").transform.localScale = Vector3.one * 0.35f;
            dashFX.transform.Find("Donut, Distortion").transform.localScale = Vector3.one * 0.35f;

            AddEffect(dashFX);

            airSlamFX = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPaths.RoR2_Base_BeetleGuard.BeetleGuardGroundSlam_prefab).WaitForCompletion().InstantiateClone("PaladinAirSlamImpact", false);

            airSlamFX.transform.Find("ParticleInitial/Debris, 3D").localScale *= 0.75f;
            airSlamFX.transform.Find("ParticleInitial/Debris").localScale *= 0.75f;
            airSlamFX.transform.Find("ParticleInitial/DustRing").localScale = new Vector3(0.75f, 0.75f, 2f);
            airSlamFX.transform.Find("ParticleInitial/Decal").localScale *= 0.75f;

            airSlamFX.transform.Find("ParticleInitial/Spikes, Large").gameObject.SetActive(false);
            airSlamFX.transform.Find("ParticleInitial/Spikes, Small").gameObject.SetActive(false);
            airSlamFX.transform.Find("ParticleInitial/Flash").gameObject.SetActive(false);
            airSlamFX.transform.Find("ParticleInitial/Dust").gameObject.SetActive(false);

            airSlamFX.GetComponentInChildren<AnimateShaderAlpha>().timeMax = 5f;
            airSlamFX.GetComponent<ShakeEmitter>().wave.amplitude *= 0.3f;

            PaladinPlugin.Destroy(airSlamFX.GetComponent<DestroyOnParticleEnd>());
            airSlamFX.AddComponent<DestroyOnTimer>().duration = 5f;

            AddEffect(airSlamFX);
            airSlamBoostedFX = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPaths.RoR2_Base_BeetleGuard.BeetleGuardGroundSlam_prefab).WaitForCompletion().InstantiateClone("PaladinAirSlamImpactBoosted", false);

            airSlamBoostedFX.transform.Find("ParticleInitial/Debris, 3D").localScale *= 0.85f;
            airSlamBoostedFX.transform.Find("ParticleInitial/Debris").localScale *= 0.85f;
            airSlamBoostedFX.transform.Find("ParticleInitial/DustRing").localScale = new Vector3(0.5f, 0.5f, 3f);
            airSlamBoostedFX.transform.Find("ParticleInitial/Decal").localScale *= 0.85f;

            airSlamBoostedFX.transform.Find("ParticleInitial/DustRing").gameObject.GetComponent<ParticleSystemRenderer>().material = Addressables.LoadAssetAsync<Material>("RoR2/Base/LunarGolem/matLunarGolemExplosion.mat").WaitForCompletion();
            airSlamFX.transform.Find("ParticleInitial/Dust").gameObject.GetComponent<ParticleSystemRenderer>().material = Addressables.LoadAssetAsync<Material>("RoR2/Base/LunarGolem/matLunarGolemSpawnDustLG.mat").WaitForCompletion();

            airSlamBoostedFX.transform.Find("ParticleInitial/Spikes, Large").gameObject.SetActive(false);
            airSlamBoostedFX.transform.Find("ParticleInitial/Spikes, Small").gameObject.SetActive(false);
            //airSlamFX.transform.Find("ParticleInitial/Flash").gameObject.SetActive(false);
            //airSlamFX.transform.Find("ParticleInitial/Dust").gameObject.SetActive(false);

            airSlamBoostedFX.GetComponentInChildren<AnimateShaderAlpha>().timeMax = 5f;
            airSlamBoostedFX.GetComponent<ShakeEmitter>().wave.amplitude *= 0.3f;

            PaladinPlugin.Destroy(airSlamBoostedFX.GetComponent<DestroyOnParticleEnd>());
            airSlamBoostedFX.AddComponent<DestroyOnTimer>().duration = 5f;

            AddEffect(airSlamBoostedFX);

            swordHitSoundEventS = CreateNetworkSoundEventDef(Modules.Sounds.HitS);
            swordHitSoundEventM = CreateNetworkSoundEventDef(Modules.Sounds.HitM);
            swordHitSoundEventL = CreateNetworkSoundEventDef(Modules.Sounds.HitL);

            batHitSoundEventS = CreateNetworkSoundEventDef(Modules.Sounds.HitBluntS);
            batHitSoundEventM = CreateNetworkSoundEventDef(Modules.Sounds.HitBluntM);
            batHitSoundEventL = CreateNetworkSoundEventDef(Modules.Sounds.HitBluntL);
        }

        private static void PopulateSwordEffects()
        {

            #region SwordEffects
            swordSwing = Asset.LoadSwingNotEffectFuck("PaladinSwing", "");
            Material distortionMat = Addressables.LoadAssetAsync<Material>("RoR2/Base/Croco/matCrocoSlashDistortion.mat").WaitForCompletion();

            Material ogSwingMat = swordSwing.transform.Find("SwingTrail").GetComponent<ParticleSystemRenderer>().material;
            Material swingMat = Material.Instantiate(Addressables.LoadAssetAsync<Material>("RoR2/Base/Brother/matBrotherSwing.mat").WaitForCompletion());

            swingMat.SetTexture("_MainTex", ogSwingMat.GetTexture("_MainTex"));
            swingMat.SetTexture("_Cloud1Tex", Addressables.LoadAssetAsync<Texture>("RoR2/Base/Common/texCloudWaterFoam1.jpg").WaitForCompletion());
            swingMat.SetFloat("_Boost", 2f);
            swingMat.SetFloat("_AlphaBoost", 3f);
            swingMat.SetFloat("_AlphaBias", 0.06053269f);

            swordSwing.transform.Find("SwingTrail").localPosition = new Vector3(0f, 0f, -0.5f);
            swordSwing.transform.Find("SwingTrail").localRotation = Quaternion.Euler(new Vector3(270f, 25f, 0f));
            swordSwing.transform.Find("SwingTrail").localScale = new Vector3(1.35f, 1.5f, 1.4f);
            swordSwing.transform.Find("SwingTrail").GetComponent<ParticleSystemRenderer>().material = swingMat;
            swordSwing.transform.Find("SwingTrail").Find("SwingTrail2").GetComponent<ParticleSystemRenderer>().material = distortionMat;

            swordSwingEmpowered = PrefabAPI.InstantiateClone(swordSwing, "PaladinSwingEmpowered", false);

            Material swingMat2 = Material.Instantiate(Addressables.LoadAssetAsync<Material>("RoR2/Base/Brother/matBrotherSwing.mat").WaitForCompletion());
            swingMat2.SetTexture("_MainTex", ogSwingMat.GetTexture("_MainTex"));
            swingMat2.SetTexture("_Cloud1Tex", Addressables.LoadAssetAsync<Texture>("RoR2/Base/Common/texCloudWaterFoam1.jpg").WaitForCompletion());
            swingMat2.SetFloat("_Boost", 8f);
            swingMat2.SetFloat("_AlphaBoost", 3f);
            swingMat2.SetFloat("_AlphaBias", 0.06053269f);

            swordSwingEmpowered.transform.Find("SwingTrail").GetComponent<ParticleSystemRenderer>().material = swingMat2;

            //AddEffect(swordSwingEmpowered);

            spinningSlashFX = Asset.LoadEffect("SpinSlashEffect", "");
            spinningSlashEmpoweredFX = Asset.LoadEffect("EmpSpinSlashEffect", "");
            spinningSlashEmpoweredFX.transform.Find("pog").GetComponent<ParticleSystemRenderer>().material = Addressables.LoadAssetAsync<Material>("RoR2/Base/Lemurian/matLizardBiteTrail.mat").WaitForCompletion();
            spinningSlashEmpoweredFX.transform.Find("pog").Find("champ").GetComponent<ParticleSystemRenderer>().material = Addressables.LoadAssetAsync<Material>("RoR2/Base/Loader/matLoaderLightningLarge.mat").WaitForCompletion();
            spinningSlashEmpoweredFX.transform.Find("pog").Find("Distortion").GetComponent<ParticleSystemRenderer>().material = matDistortion;

            //hitFX = Assets.LoadEffect("ImpactPaladinSwing", "");

            hitFX = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Merc/OmniImpactVFXSlashMerc.prefab").WaitForCompletion(), "PaladinImpactVFX", false);

            hitFX.transform.Find("Flash, Hard").localScale = new Vector3(4f, 4f, 4f);
            hitFX.transform.Find("Dash, Bright").localScale = new Vector3(5f, 3f, 3f);
            hitFX.transform.Find("ScaledSmokeRing, Mesh").localScale = new Vector3(4f, 4f, 4f);
            hitFX.transform.Find("ScaledSmokeRing, Mesh").GetComponent<ParticleSystemRenderer>().material = Addressables.LoadAssetAsync<Material>("RoR2/Junk/LunarWisp/matLunarWispBombClouds.mat").WaitForCompletion();
            hitFX.transform.Find("Impact Slash").localScale = new Vector3(2f, 2f, 2f);
            hitFX.transform.Find("Scaled Hitspark 4, Directional (Random Color)").localScale = new Vector3(2f, 2f, 2f);
            hitFX.transform.Find("Scaled Hitspark 3, Radial (Random Color)").localScale = new Vector3(2f, 2f, 2f);
            hitFX.transform.Find("Scaled Hitspark 2 (Random Color)").localScale = new Vector3(2f, 2f, 2f);

            AddEffect(hitFX);

            swordSwingGreen = Asset.LoadSwingNotEffectFuck("PaladinSwingGreen", "");
            spinningSlashFXGreen = Asset.LoadEffect("SpinSlashEffectGreen", "");
            spinningSlashEmpoweredFXGreen = Asset.LoadEffect("EmpSpinSlashEffectGreen", "");
            hitFXGreen = Asset.LoadEffect("ImpactPaladinSwingGreen", "");

            swordSwingYellow = Asset.LoadSwingNotEffectFuck("PaladinSwingYellow", "");
            spinningSlashFXYellow = Asset.LoadEffect("SpinSlashEffectYellow", "");
            spinningSlashEmpoweredFXYellow = Asset.LoadEffect("EmpSpinSlashEffectYellow", "");
            hitFXYellow = Asset.LoadEffect("ImpactPaladinSwingYellow", "");

            swordSwingWhite = Asset.LoadSwingNotEffectFuck("PaladinSwingWhite", "");
            hitFXBlunt = Asset.LoadEffect("ImpactPaladinSwingBlunt", "");

            swordSwingBat = Asset.LoadSwingNotEffectFuck("PaladinSwingBat", "");

            swordSwingRed = Asset.LoadSwingNotEffectFuck("PaladinSwingRed", "");
            spinningSlashFXRed = Asset.LoadEffect("SpinSlashEffectRed", "");
            spinningSlashEmpoweredFXRed = Asset.LoadEffect("EmpSpinSlashEffectRed", "");
            hitFXRed = Asset.LoadEffect("ImpactPaladinSwingRed", "");

            swordSwingClay = Asset.LoadSwingNotEffectFuck("PaladinSwingClay", "");
            spinningSlashFXClay = Asset.LoadEffect("SpinSlashEffectClay", "");
            spinningSlashEmpoweredFXClay = Asset.LoadEffect("EmpSpinSlashEffectClay", "");
            hitFXClay = Asset.LoadEffect("ImpactPaladinSwingClay", "");

            swordSwingPurple = Asset.LoadSwingNotEffectFuck("PaladinSwingPurple", "");
            spinningSlashFXPurple = Asset.LoadEffect("SpinSlashEffectPurple", "");
            spinningSlashEmpoweredFXPurple = Asset.LoadEffect("EmpSpinSlashEffectPurple", "");
            hitFXPurple = Asset.LoadEffect("ImpactPaladinSwingPurple", "");

            swordSwingFlame = Asset.LoadSwingNotEffectFuck("PaladinSwingFlame", "");
            spinningSlashFXFlame = Asset.LoadEffect("SpinSlashEffectFlame", "");
            spinningSlashEmpoweredFXFlame = Asset.LoadEffect("EmpSpinSlashEffectFlame", "");

            swordSwingBlack = Asset.LoadSwingNotEffectFuck("PaladinSwingBlack", "");
            spinningSlashFXBlack = Asset.LoadEffect("SpinSlashEffectBlack", "");
            spinningSlashEmpoweredFXBlack = Asset.LoadEffect("EmpSpinSlashEffectBlack", "");
            hitFXBlack = Asset.LoadEffect("ImpactPaladinSwingBlack", "");

            //for custom skin effect stuff, in order of enum
            hitEffects.Add(hitFX);
            hitEffects.Add(hitFXGreen);
            hitEffects.Add(hitFXYellow);
            hitEffects.Add(hitFXRed);
            hitEffects.Add(hitFXClay);
            hitEffects.Add(hitFXPurple);
            hitEffects.Add(hitFXBlack);
            hitEffects.Add(hitFXBlunt);

            swordSwingEffects.Add(swordSwing);
            swordSwingEffects.Add(swordSwingEmpowered);
            swordSwingEffects.Add(swordSwingGreen);
            swordSwingEffects.Add(swordSwingYellow);
            swordSwingEffects.Add(swordSwingRed);
            swordSwingEffects.Add(swordSwingClay);
            swordSwingEffects.Add(swordSwingPurple);
            swordSwingEffects.Add(swordSwingFlame);
            swordSwingEffects.Add(swordSwingBlack);
            swordSwingEffects.Add(swordSwingWhite);
            swordSwingEffects.Add(swordSwingBat);

            spinSlashEffects.Add(spinningSlashFX);
            spinSlashEffects.Add(spinningSlashFXGreen);
            spinSlashEffects.Add(spinningSlashFXYellow);
            spinSlashEffects.Add(spinningSlashFXRed);
            spinSlashEffects.Add(spinningSlashFXClay);
            spinSlashEffects.Add(spinningSlashFXPurple);
            spinSlashEffects.Add(spinningSlashFXBlack);
            spinSlashEffects.Add(spinningSlashFXFlame);

            spinSlashEmpoweredEffects.Add(spinningSlashEmpoweredFX);
            spinSlashEmpoweredEffects.Add(spinningSlashEmpoweredFXGreen);
            spinSlashEmpoweredEffects.Add(spinningSlashEmpoweredFXYellow);
            spinSlashEmpoweredEffects.Add(spinningSlashEmpoweredFXRed);
            spinSlashEmpoweredEffects.Add(spinningSlashEmpoweredFXClay);
            spinSlashEmpoweredEffects.Add(spinningSlashEmpoweredFXPurple);
            spinSlashEmpoweredEffects.Add(spinningSlashEmpoweredFXBlack);
            spinSlashEmpoweredEffects.Add(spinningSlashEmpoweredFXFlame);

            #endregion
        }

        private static void PopulateMiscEffects() {

            #region MiscEffects
            lightningHitFX = Asset.LoadEffect("LightningHitFX", "");
            lightningImpactFX = Asset.LoadEffect("LightningImpact", "Play_mage_R_lightningBlast");
            torporVoidFX = Asset.LoadEffect("TorporVoidFX", "RoR2_nullifier_attack1_explode_02");
            #endregion
        }

        private static void PopulateMeshes() {

            #region Meshes
            defaultMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinDefault");
            defaultSwordMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinDefault_Sword");
            defaultCapeMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinDefault_Cape");
            lunarMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinBrother");
            lunarSwordMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinBrother_Sword");
            lunarKnightMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinLunarKnight");
            lunarKnightSwordMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinLunarKnight_Sword");
            lunarKnightCapeMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinLunarKnight_Cape");
            gmMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinGM");
            gmSwordMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinGM_Sword");
            gmCapeMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinGM_Cape");
            gmLegacyMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinGMLegacy");
            gmLegacySwordMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinGMLegacy_Sword");
            poisonMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinNkuhana");
            poisonSwordMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinNkuhana_Sword");
            poisonLegacyMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinNkuhanaLegacy");
            poisonLegacySwordMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinNkuhanaLegacy_Sword");
            poisonLegacyCapeMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinNkuhanaLegacy_Cape");
            specterMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinSpecter");
            specterSwordMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinSpecter_Sword");
            //hunterMesh = mainAssetBundle.LoadAsset<Mesh>("HunterMesh");
            dripMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinDrip");
            batMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinDrip_Sword");
            clayMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinAphelian");
            claySwordMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinAphelian_Sword");
            minecraftMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinMinecraft");
            minecraftSwordMesh = mainAssetBundle.LoadAsset<Mesh>("meshPaladinMinecraft_Sword");
            #endregion
        }

        private static void PopulateProjectileGhosts() {

            #region ProjectileGhosts
            lightningSpear = mainAssetBundle.LoadAsset<GameObject>("LightningSpear");
            swordBeam = mainAssetBundle.LoadAsset<GameObject>("SwordBeam");
            
            swordBeamGhost = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Merc/EvisProjectileGhost.prefab").WaitForCompletion().InstantiateClone("PaladinSwordBeamGhost", false);
            swordBeamGhost.transform.Find("Base").GetComponent<ParticleSystemRenderer>().material = Addressables.LoadAssetAsync<Material>("RoR2/Base/LunarWisp/matLunarWispBombTrail.mat").WaitForCompletion();
            swordBeamGhost.transform.Find("Base").localScale = new Vector3(2f, 1f, 4f);

            /*var h = swordBeamGhost.transform.Find("Base").gameObject.AddComponent<ObjectScaleCurve>();
            h.timeMax = 0.3f;
            h.baseScale = new Vector3(2f, 1f, 4f);
            h.curveZ = new AnimationCurve
            {
                keys = new Keyframe[]
                {
                    new Keyframe(0f, 0f),
                    new Keyframe(0.2f, 1.1f),
                    new Keyframe(0.6f, 1f),
                    new Keyframe(1f, 0f)
                }
            };
            h.curveX = new AnimationCurve
            {
                keys = new Keyframe[]
                {
                    new Keyframe(0f, 1f),
                    new Keyframe(1f, 1f)
                }
            };
            h.curveY = new AnimationCurve
            {
                keys = new Keyframe[]
                {
                    new Keyframe(0f, 1f),
                    new Keyframe(1f, 1f)
                }
            };*/

            tornadoEffect = mainAssetBundle.LoadAsset<GameObject>("PaladinTornadoEffect");
            tornadoEffect.AddComponent<ProjectileGhostController>();
            #endregion
        }

        private static void PopulateSpellEffects() {

            #region SpellEffects
            healEffectPrefab = mainAssetBundle.LoadAsset<GameObject>("HealEffect");
            healZoneEffectPrefab = mainAssetBundle.LoadAsset<GameObject>("HealZoneEffect");
            torporEffectPrefab = mainAssetBundle.LoadAsset<GameObject>("TorporEffect");
            warcryEffectPrefab = mainAssetBundle.LoadAsset<GameObject>("HealZoneEffect").InstantiateClone("WarcryEffect", false);

            GameObject engiShieldObj = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/EngiBubbleShield");

            Material shieldFillMat = UnityEngine.Object.Instantiate<Material>(engiShieldObj.transform.Find("Collision").Find("ActiveVisual").GetComponent<MeshRenderer>().material);
            Material shieldOuterMat = UnityEngine.Object.Instantiate<Material>(engiShieldObj.transform.Find("Collision").Find("ActiveVisual").Find("Edge").GetComponent<MeshRenderer>().material);

            GameObject voidExplosionObj = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/NullifierDeathExplosion");//DeathExplosion
            Material voidMat = voidExplosionObj.transform.Find("AreaIndicator").GetComponent<ParticleSystemRenderer>().material;
            torporMat = voidMat;

            GameObject healNovaObj = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/TPHealNovaEffect");
            Material healMat = healNovaObj.transform.Find("AreaIndicator").GetComponent<ParticleSystemRenderer>().material;

            healEffectPrefab.GetComponentInChildren<ParticleSystemRenderer>().material = healMat;

            healZoneEffectPrefab.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = shieldOuterMat;
            healZoneEffectPrefab.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = healMat;

            torporEffectPrefab.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = voidMat;

            warcryEffectPrefab.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = shieldOuterMat;
            warcryEffectPrefab.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", Color.red);
            warcryEffectPrefab.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = RoR2.LegacyResourcesAPI.Load<Material>("materials/matFullCrit");
            //
            //GameObject warbannerEffect = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/WarbannerWard").InstantiateClone("x", true);
            #endregion

            #region CruelSun
            //Clone the existing Grandparent Sun prefab and modify it for our own use.
            paladinSunPrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Grandparent/GrandParentSun.prefab").WaitForCompletion(), "PaladinSun", true);

            //Transfering over some data we need from the old script; buff definitions, SFX definitions
            paladinSunPrefab.AddComponent<PaladinSunController>();
            GrandParentSunController baseSunScript = paladinSunPrefab.GetComponent<GrandParentSunController>();
            PaladinSunController paladinSunScript = paladinSunPrefab.GetComponent<PaladinSunController>();
            paladinSunScript.buffApplyEffect = baseSunScript.buffApplyEffect;
            paladinSunScript.buffDef = baseSunScript.buffDef;
            paladinSunScript.activeLoopDef = baseSunScript.activeLoopDef;
            paladinSunScript.damageLoopDef = baseSunScript.damageLoopDef;
            paladinSunScript.stopSoundName = baseSunScript.stopSoundName;
            Object.DestroyImmediate(baseSunScript); //VERY important to remove this once we're done transfering data, since we now have our own controller.

            //Simple script for syncing positions
            paladinSunPrefab.AddComponent<PaladinSunNetworkController>();

            //EntityStateMachine that can go die in the actual sun. reset the NetworkStateMachine value just in case
            Object.DestroyImmediate(paladinSunPrefab.GetComponent<EntityStateMachine>());
            paladinSunPrefab.AddComponent<EntityStateMachine>();
            EntityStateMachine esmPaladin = paladinSunPrefab.GetComponent<EntityStateMachine>();
            esmPaladin.name = "Body";
            esmPaladin.initialStateType = new SerializableEntityStateType(typeof(PaladinMod.States.Sun.PaladinSunSpawn));
            paladinSunPrefab.GetComponent<NetworkStateMachine>().stateMachines[0] = esmPaladin;

            //VFX - Use StaticValues.cruelSunVfxSize to control the scale, changing anything here will cause it not to align with gameplay logic anymore.
            paladinSunPrefab.transform.localScale = Vector3.one * StaticValues.cruelSunVfxSize;
            paladinSunPrefab.transform.Find("VfxRoot/LightSpinner/LightSpinner/Point Light").GetComponent<Light>().intensity *= StaticValues.cruelSunVfxSize;
            paladinSunPrefab.transform.Find("VfxRoot/LightSpinner/LightSpinner/Point Light").GetComponent<Light>().range = 200 * StaticValues.cruelSunVfxSize;
            paladinSunPrefab.transform.Find("VfxRoot/Mesh/SunMesh").transform.localScale = Vector3.one * StaticValues.cruelSunVfxCenterSize;
            paladinSunPrefab.transform.Find("VfxRoot/Mesh/AreaIndicator").transform.localScale = Vector3.one * 180;

            //Removing some distracting effects that don't work well here (imo).
            Object.DestroyImmediate(paladinSunPrefab.transform.Find("VfxRoot/Mesh/SunMesh/MoonMesh").gameObject);
            //ParticleSystems need to have their modules referenced in a variable before we can assign anything to them. I have no fucking idea why.
            //Could destroy these instead of disabling them, but this framework might be useful later for tinkering with other particle settings.
            ParticleSystem psSparks = paladinSunPrefab.transform.Find("VfxRoot/Particles/Sparks").GetComponent<ParticleSystem>();
            var psSparks_emission = psSparks.emission;
            psSparks_emission.enabled = false;
            ParticleSystem psGoo = paladinSunPrefab.transform.Find("VfxRoot/Particles/Goo, Drip").GetComponent<ParticleSystem>();
            var psGoo_emission = psGoo.emission;
            psGoo_emission.enabled = false;
            #endregion

            #region CruelSunSpawn
            //Cruel Sun spawn explosion effect
            paladinSunSpawnPrefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Grandparent/GrandParentSunSpawn.prefab").WaitForCompletion(), "PaladinSunSpawn", false);
            paladinSunSpawnPrefab.transform.localScale = new Vector3(StaticValues.cruelSunVfxSize, StaticValues.cruelSunVfxSize, StaticValues.cruelSunVfxSize);
            paladinSunSpawnPrefab.transform.Find("Point Light").GetComponent<Light>().intensity *= StaticValues.cruelSunVfxLightIntensity;
            paladinSunSpawnPrefab.transform.Find("Point Light").GetComponent<Light>().range = 200 * StaticValues.cruelSunVfxSize;
            paladinSunSpawnPrefab.GetComponent<DestroyOnTimer>().duration = 1.5f;
            AddEffect(paladinSunSpawnPrefab, "Play_grandparent_attack3_sun_spawn");
            #endregion

            #region PrideFlare
            //Pride Flare base prefab, projectile is derived from this base
            paladinScepterSunPrefab = PrefabAPI.InstantiateClone(Asset.paladinSunPrefab, "PaladinScepterSun");
            //TODO: VFX changes here
            paladinScepterSunPrefab.transform.Find("VfxRoot/Mesh/SunMesh").transform.localScale = new Vector3(5f, 5f, 5f);
            paladinScepterSunPrefab.transform.Find("VfxRoot/Particles").transform.localScale = new Vector3(7.5f, 7.5f, 7.5f);
            paladinScepterSunPrefab.transform.Find("VfxRoot/LightSpinner/LightSpinner/Point Light").GetComponent<Light>().colorTemperature = 3000f;
            MeshRenderer pssMR = paladinScepterSunPrefab.transform.Find("VfxRoot/Mesh/SunMesh").GetComponent<MeshRenderer>();
            Material pssNewM = Object.Instantiate<Material>(pssMR.material);
            pssNewM.SetColor("_TintColor", new Color(0.33f,0.33f,0.33f));
            pssNewM.SetTexture("_RemapTex", Addressables.LoadAssetAsync<Texture>("RoR2/Base/Common/ColorRamps/texRampFogDebug.png").WaitForCompletion()); //RoR2/Base/Common/ColorRamps/texRampMageFire.png for alternate color
            pssMR.material = pssNewM;
            ParticleSystem.MainModule pssPS = paladinScepterSunPrefab.transform.Find("VfxRoot/Particles/SoftGlow, Backdrop").GetComponent<ParticleSystem>().main;
            //ParticleSystemRenderer pssPSRenderer = paladinScepterSunPrefab.transform.Find("VfxRoot/Particles/SoftGlow, Backdrop").GetComponent<ParticleSystemRenderer>();
            pssPS.startColor = new Color(0.7f, 0.7f, 0.6f, 0.7f);
            #endregion
        }

        private static void PopulateIcons() {

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
            icon4d = mainAssetBundle.LoadAsset<Sprite>("CruelSunIcon");
            icon4dS = mainAssetBundle.LoadAsset<Sprite>("ScepterCruelSunIcon");
            #endregion
        }

        private static void GatherMaterials()
        {
            // addressables exist now but do i care enough to rewrite this?
            grandParentPP = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GrandParentBody").GetComponentInChildren<PostProcessVolume>().sharedProfile;
            supplyDropMat = UnityEngine.Object.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/CaptainSupplyDrops/CaptainSupplyDrop, Healing").transform.Find("Inactive").Find("Sphere, Outer").GetComponent<MeshRenderer>().material);
            airStrikeMat = UnityEngine.Object.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/ProjectileGhosts/CaptainAirstrikeGhost1").transform.Find("Expander").Find("Sphere, Outer").GetComponent<MeshRenderer>().material);
            crippleSphereMat = UnityEngine.Object.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/TemporaryVisualEffects/CrippleEffect").transform.Find("Visual").GetChild(1).GetComponent<MeshRenderer>().material);
            areaIndicatorMat = UnityEngine.Object.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/SpiteBombDelayEffect").transform.Find("Nova Sphere").GetComponent<ParticleSystemRenderer>().material);
            matBlueLightningLong = UnityEngine.Object.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/LightningStrikeOrbEffect").transform.Find("Ring").GetComponent<ParticleSystemRenderer>().material);
            matJellyfishLightning = UnityEngine.Object.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/JellyfishNova").transform.Find("Lightning, Spark Center").GetComponent<ParticleSystemRenderer>().material);
            matJellyfishLightningLarge = UnityEngine.Object.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/VagrantCannonExplosion").transform.Find("Lightning, Radial").GetComponent<ParticleSystemRenderer>().material);
            matMageMatrixDirectionalLightning = UnityEngine.Object.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniImpactVFXLightningMage").transform.Find("Matrix, Directional").GetComponent<ParticleSystemRenderer>().material);
            matDistortion = UnityEngine.Object.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/LoaderGroundSlam").transform.Find("Sphere, Distortion").GetComponent<ParticleSystemRenderer>().material);
            matMercSwipe = UnityEngine.Object.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/EvisProjectile").GetComponent<ProjectileController>().ghostPrefab.transform.Find("Base").GetComponent<ParticleSystemRenderer>().material);
            matLoaderLightningSphere = UnityEngine.Object.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/LoaderGroundSlam").transform.Find("Sphere, Expanding").GetComponent<ParticleSystemRenderer>().material);
            matYellowLightningLong = UnityEngine.Object.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/LoaderPylon").transform.Find("loader pylon").Find("LoaderPylonArmature").Find("ROOT").Find("ActiveParticles").Find("Sparks, Trail").GetComponent<ParticleSystemRenderer>().trailMaterial);
            matMeteorIndicator = UnityEngine.Object.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/MeteorStrikePredictionEffect").transform.Find("GroundSlamIndicator").GetComponent<MeshRenderer>().material);

            // no, i don't.

            blessingMat = Material.Instantiate(Addressables.LoadAssetAsync<Material>("RoR2/DLC1/VoidMegaCrab/matVoidCrabMatterOverlay.mat").WaitForCompletion());
            blessingMat.SetColor("_TintColor", new Color(0f / 255f, 145f / 255f, 173f / 255f));
        }

        public static T LoadResources<T>(string assString) where T : UnityEngine.Object {

            T loadedAss = RoR2.LegacyResourcesAPI.Load<T>(assString);

            if (loadedAss == null) {
                Debug.LogError($"Null asset: {assString}.\nAttempt to load asset '{assString}' from assetbundles returned null");
            }

            return loadedAss;
        }

        internal static NetworkSoundEventDef CreateNetworkSoundEventDef(string eventName)
        {
            NetworkSoundEventDef networkSoundEventDef = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            networkSoundEventDef.akId = AkSoundEngine.GetIDFromString(eventName);
            networkSoundEventDef.eventName = eventName;

            networkSoundEventDefs.Add(networkSoundEventDef);

            return networkSoundEventDef;
        }

        public static GameObject ConvertAllRenderersToHopooShader(this GameObject rootObject) {
            if (!rootObject) return rootObject;

            foreach (MeshRenderer i in rootObject.GetComponentsInChildren<MeshRenderer>()) {
                if (i?.sharedMaterial != null) {
                    i.sharedMaterial.SetHotpooMaterial();
                }
            }

            foreach (SkinnedMeshRenderer i in rootObject.GetComponentsInChildren<SkinnedMeshRenderer>()) {
                if (i?.sharedMaterial != null) {
                    i.sharedMaterial.SetHotpooMaterial();
                }
            }

            return rootObject;
        }

        private static GameObject LoadSwingNotEffectFuck(string resourceName, string soundName)
        {
            GameObject newEffect = mainAssetBundle.LoadAsset<GameObject>(resourceName);
            newEffect.AddComponent<DestroyOnTimer>().duration = 12;
            return newEffect;
        }

        private static GameObject LoadEffect(string resourceName, string soundName)
        {
            GameObject newEffect = mainAssetBundle.LoadAsset<GameObject>(resourceName);

            newEffect.AddComponent<DestroyOnTimer>().duration = 12;
            newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            EffectComponent effect = newEffect.AddComponent<EffectComponent>();
            effect.applyScale = false;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = true;
            effect.positionAtReferencedTransform = true;
            effect.soundName = soundName;

            AddEffect(newEffect);

            return newEffect;
        }

        private static void AddEffect(GameObject effectPrefab)
        {
            AddEffect(effectPrefab, "");
        }

        private static void AddEffect(GameObject effectPrefab, string soundName)
        {
            EffectDef newEffectDef = new EffectDef();
            newEffectDef.prefab = effectPrefab;
            newEffectDef.prefabEffectComponent = effectPrefab.GetComponent<EffectComponent>();
            newEffectDef.prefabName = effectPrefab.name;
            newEffectDef.prefabVfxAttributes = effectPrefab.GetComponent<VFXAttributes>();
            newEffectDef.spawnSoundEventName = soundName;

            effectDefs.Add(newEffectDef);
        }
    }
}