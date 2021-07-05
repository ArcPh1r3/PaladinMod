using System;
using UnityEngine;
using RoR2;
using System.Collections.Generic;
using System.Linq;

namespace PaladinMod.Modules
{
    public static class Skins
    {
        static List<GameObject> allGameObjectActivations = new List<GameObject>();

        static List<Material> cachedMaterials = new List<Material>();

        /// <summary>
        /// create an array of all gameobjects that are activated/deactivated by skins, then for each skin pass in the specific objects that will be active
        /// </summary>
        /// <param name="allObjects">array of all gameobjects that are activated/deactivated by skins</param>
        /// <param name="activatedObjects">specific objects that will be active</param>
        /// <returns></returns>
        private static SkinDef.GameObjectActivation[] getActivations(params GameObject[] activatedObjects) {

            List<SkinDef.GameObjectActivation> GameObjectActivations = new List<SkinDef.GameObjectActivation>();

            for (int i = 0; i < allGameObjectActivations.Count; i++) {

                bool activate = activatedObjects.Contains(allGameObjectActivations[i]);

                GameObjectActivations.Add(new SkinDef.GameObjectActivation {
                    gameObject = allGameObjectActivations[i],
                    shouldActivate = activate
                });
            }

            return GameObjectActivations.ToArray();
        }


        public static SkinDef CreateSkinDef(string skinName, Sprite skinIcon, CharacterModel.RendererInfo[] rendererInfos, SkinnedMeshRenderer mainRenderer, GameObject root)
        {
            return CreateSkinDef(skinName, skinIcon, rendererInfos, mainRenderer, root, null);
        }

        public static SkinDef CreateSkinDef(string skinName, Sprite skinIcon, CharacterModel.RendererInfo[] rendererInfos, SkinnedMeshRenderer mainRenderer, GameObject root, UnlockableDef unlockableDef)
        {
            return CreateSkinDef(skinName, skinIcon, rendererInfos, mainRenderer, root, unlockableDef, null);
        }

        public static SkinDef CreateSkinDef(string skinName, Sprite skinIcon, CharacterModel.RendererInfo[] rendererInfos, SkinnedMeshRenderer mainRenderer, GameObject root, UnlockableDef unlockableDef, Mesh skinMesh)
        {
            SkinDefInfo skinDefInfo = new SkinDefInfo
            {
                BaseSkins = Array.Empty<SkinDef>(),
                GameObjectActivations = new SkinDef.GameObjectActivation[0],
                Icon = skinIcon,
                MeshReplacements = new SkinDef.MeshReplacement[]
                {
                    new SkinDef.MeshReplacement
                    {
                        renderer = mainRenderer,
                        mesh = skinMesh
                    }
                },
                MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0],
                Name = skinName,
                NameToken = skinName,
                ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0],
                RendererInfos = rendererInfos,
                RootObject = root,
                UnlockableDef = unlockableDef 
            };

            On.RoR2.SkinDef.Awake += DoNothing;

            SkinDef skinDef = ScriptableObject.CreateInstance<RoR2.SkinDef>();
            skinDef.baseSkins = skinDefInfo.BaseSkins;
            skinDef.icon = skinDefInfo.Icon;
            skinDef.unlockableDef = skinDefInfo.UnlockableDef;
            skinDef.rootObject = skinDefInfo.RootObject;
            skinDef.rendererInfos = skinDefInfo.RendererInfos;
            skinDef.gameObjectActivations = skinDefInfo.GameObjectActivations;
            skinDef.meshReplacements = skinDefInfo.MeshReplacements;
            skinDef.projectileGhostReplacements = skinDefInfo.ProjectileGhostReplacements;
            skinDef.minionSkinReplacements = skinDefInfo.MinionSkinReplacements;
            skinDef.nameToken = skinDefInfo.NameToken;
            skinDef.name = skinDefInfo.Name;

            On.RoR2.SkinDef.Awake -= DoNothing;

            return skinDef;
        }

        private static void DoNothing(On.RoR2.SkinDef.orig_Awake orig, RoR2.SkinDef self)
        {
        }

        internal struct SkinDefInfo
        {
            internal SkinDef[] BaseSkins;
            internal Sprite Icon;
            internal string NameToken;
            internal UnlockableDef UnlockableDef;
            internal GameObject RootObject;
            internal CharacterModel.RendererInfo[] RendererInfos;
            internal SkinDef.MeshReplacement[] MeshReplacements;
            internal SkinDef.GameObjectActivation[] GameObjectActivations;
            internal SkinDef.ProjectileGhostReplacement[] ProjectileGhostReplacements;
            internal SkinDef.MinionSkinReplacement[] MinionSkinReplacements;
            internal string Name;
        }

        public static Material CreateMaterial(string materialName)
        {
            return CreateMaterial(materialName, 0);
        }

        public static Material CreateMaterial(string materialName, float emission)
        {
            return CreateMaterial(materialName, emission, Color.black);
        }

        public static Material CreateMaterial(string materialName, float emission, Color emissionColor)
        {
            return CreateMaterial(materialName, emission, emissionColor, 0);
        }

        public static Material CreateMaterial(string materialName, float emission, Color emissionColor, float normalStrength)
        {
            if (!PaladinPlugin.commandoMat) PaladinPlugin.commandoMat = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;
            
            Material cachedMaterial = cachedMaterials.Find((item) => { return item.name == materialName; });
            if (cachedMaterial) {
                return cachedMaterial;
            }

            Material mat = UnityEngine.Object.Instantiate<Material>(PaladinPlugin.commandoMat);
            Material tempMat = Assets.mainAssetBundle.LoadAsset<Material>(materialName);
            if (!tempMat)
            {
                return PaladinPlugin.commandoMat;
            }

            mat.name = materialName;
            mat.SetColor("_Color", tempMat.GetColor("_Color"));
            mat.SetTexture("_MainTex", tempMat.GetTexture("_MainTex"));
            mat.SetColor("_EmColor", emissionColor);
            mat.SetFloat("_EmPower", emission);
            mat.SetTexture("_EmTex", tempMat.GetTexture("_EmissionMap"));
            mat.SetFloat("_NormalStrength", normalStrength);

            cachedMaterials.Add(mat);
            return mat;
        }

        public static void RegisterSkins()
        {
            GameObject bodyPrefab = Prefabs.paladinPrefab;

            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            GameObject displayPrefab = Prefabs.paladinDisplayPrefab;
            ChildLocator displayChildLocator = displayPrefab.GetComponent<ChildLocator>(); 
            CharacterSelectSurvivorPreviewDisplayController CSSPreviewController = displayPrefab.GetComponent<CharacterSelectSurvivorPreviewDisplayController>();
            CSSPreviewController.bodyPrefab = bodyPrefab;

            List<SkinDef> skinDefs = new List<SkinDef>();

            GameObject cape = childLocator.FindChild("Cape").gameObject;
            GameObject armLeft = childLocator.FindChild("CreepyArmsLeft").gameObject;
            GameObject armRight = childLocator.FindChild("CreepyArmsRight").gameObject;
            GameObject fuckingCrystalCrown = childLocator.FindChild("FuckMe").gameObject;

            allGameObjectActivations.Add(cape);
            allGameObjectActivations.Add(armLeft);
            allGameObjectActivations.Add(armRight);
            allGameObjectActivations.Add(fuckingCrystalCrown);

            SkinDef.GameObjectActivation[] defaultActivations = getActivations();
            SkinDef.GameObjectActivation[] capeActivations = getActivations(cape);
            SkinDef.GameObjectActivation[] nkuhanaActivations = getActivations(armLeft, armRight);
            SkinDef.GameObjectActivation[] GMActivations = getActivations(cape, fuckingCrystalCrown);

            #region DefaultSkin
            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;
            SkinDef defaultSkin = CreateSkinDef("PALADINBODY_DEFAULT_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texMainSkin"), defaultRenderers, mainRenderer, model);
            defaultSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.defaultMesh,
                    renderer = defaultRenderers[4].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.defaultSwordMesh,
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.defaultCapeMesh,
                    renderer = defaultRenderers[1].renderer
                },
            }; 

            if (Modules.Config.cape.Value)
            {
                defaultSkin.gameObjectActivations = capeActivations;
                childLocator.FindChild("Cape").gameObject.SetActive(true);
            }
            else defaultSkin.gameObjectActivations = defaultActivations;

            skinDefs.Add(defaultSkin);

            CSSPreviewController.skinChangeResponses[0].triggerSkin = defaultSkin;
            #endregion

            #region MasterySkin(lunar)
            CharacterModel.RendererInfo[] masteryRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(masteryRendererInfos, 0);

            // add the passive effect
            GameObject lunarPassiveEffect = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/CharacterBodies/BrotherBody").GetComponentInChildren<ChildLocator>().FindChild("Phase3HammerFX").gameObject);
            lunarPassiveEffect.transform.parent = childLocator.FindChild("SwordActiveEffectLunar");
            lunarPassiveEffect.transform.localScale = Vector3.one * 0.0002f;
            lunarPassiveEffect.transform.rotation = Quaternion.Euler(new Vector3(45, 90, 0));
            lunarPassiveEffect.transform.localPosition = new Vector3(0, 0, -0.003f);
            lunarPassiveEffect.gameObject.SetActive(true);

            lunarPassiveEffect.transform.Find("Amb_Fire_Ps, Left").localScale = Vector3.one * 0.6f;
            lunarPassiveEffect.transform.Find("Amb_Fire_Ps, Right").localScale = Vector3.one * 0.6f;
            lunarPassiveEffect.transform.Find("Core, Light").localScale = Vector3.one * 0.1f;
            lunarPassiveEffect.transform.Find("Blocks, Spinny").localScale = Vector3.one * 0.4f;
            lunarPassiveEffect.transform.Find("Sparks").localScale = Vector3.one * 0.4f;

            lunarPassiveEffect = GameObject.Instantiate(lunarPassiveEffect);
            lunarPassiveEffect.transform.parent = displayChildLocator.FindChild("SwordActiveEffectLunar");
            lunarPassiveEffect.transform.localScale = Vector3.one * 0.0002f;
            lunarPassiveEffect.transform.rotation = Quaternion.Euler(new Vector3(45, 90, 0));
            lunarPassiveEffect.transform.localPosition = new Vector3(0, 0, -0.003f);
            lunarPassiveEffect.gameObject.SetActive(true);

            Material lunarSwordMat = CreateMaterial("matLunarSword", 0, Color.black, 1f);
            lunarSwordMat.EnableKeyword("FORCE_SPEC");
            lunarSwordMat.EnableKeyword("FRESNEL_EMISSION");
            lunarSwordMat.SetFloat("_SpecularStrength", 1f);

            masteryRendererInfos[0].defaultMaterial = lunarSwordMat;
            //masteryRendererInfos[1].defaultMaterial = CreateMaterial("matLunarCape");
            masteryRendererInfos[2].defaultMaterial = CreateMaterial("matLunarPaladin", 5, Color.white, 1f);

            SkinDef masterySkin = CreateSkinDef("PALADINBODY_LUNAR_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievement"), masteryRendererInfos, mainRenderer, model, Modules.Unlockables.paladinMasterySkinDef);
            masterySkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.lunarMesh,
                    renderer = defaultRenderers[4].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.lunarSwordMesh,
                    renderer = defaultRenderers[0].renderer
                }
            };

            masterySkin.gameObjectActivations = defaultActivations;

            skinDefs.Add(masterySkin);

            CSSPreviewController.skinChangeResponses[1].triggerSkin = masterySkin;
            #endregion

            #region GrandMasterySkin
            CharacterModel.RendererInfo[] grandMasteryRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(grandMasteryRendererInfos, 0);

            grandMasteryRendererInfos[0].defaultMaterial = CreateMaterial("matPaladinGMSword", StaticValues.maxSwordGlow, Color.white);
            grandMasteryRendererInfos[1].defaultMaterial = CreateMaterial("matPaladinGM");
            grandMasteryRendererInfos[4].defaultMaterial = CreateMaterial("matPaladinGM", 15, Color.white, 1f);

            SkinDef grandMasterySkin = CreateSkinDef("PALADINBODY_TYPHOON_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texGrandMasteryAchievement"), grandMasteryRendererInfos, mainRenderer, model, Modules.Unlockables.paladinGrandMasterySkinDef);
            grandMasterySkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.gmMesh,
                    renderer = defaultRenderers[4].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.gmSwordMesh,
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.gmCapeMesh,
                    renderer = defaultRenderers[1].renderer
                }
            };

            grandMasterySkin.gameObjectActivations = GMActivations; 

            //if (PaladinPlugin.starstormInstalled) 
                skinDefs.Add(grandMasterySkin);

            CSSPreviewController.skinChangeResponses[2].triggerSkin = grandMasterySkin; 

            #endregion

            #region PoisonSkin
            CharacterModel.RendererInfo[] poisonRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(poisonRendererInfos, 0);

            poisonRendererInfos[0].defaultMaterial = CreateMaterial("matPaladinNkuhanaScythe", StaticValues.maxSwordGlow, Color.white);
            poisonRendererInfos[2].defaultMaterial = CreateMaterial("matPaladinNkuhana", 3, Color.white);
            poisonRendererInfos[3].defaultMaterial = CreateMaterial("matPaladinNkuhana", 3, Color.white);
            poisonRendererInfos[4].defaultMaterial = CreateMaterial("matPaladinNkuhana", 3, Color.white);

            SkinDef poisonSkin = CreateSkinDef("PALADINBODY_POISON_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texPoisonAchievement"), poisonRendererInfos, mainRenderer, model, Modules.Unlockables.paladinPoisonSkinDef);
            poisonSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.poisonMesh,
                    renderer = defaultRenderers[4].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.poisonSwordMesh,
                    renderer = defaultRenderers[0].renderer
                }
                //if arms are only in this skin they don't need to be mesh replaced, just activated
            };

            poisonSkin.gameObjectActivations = nkuhanaActivations;

            skinDefs.Add(poisonSkin);

            CSSPreviewController.skinChangeResponses[3].triggerSkin = poisonSkin;
            #endregion

            #region ClaySkin
            CharacterModel.RendererInfo[] clayRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(clayRendererInfos, 0);

            clayRendererInfos[0].defaultMaterial = CreateMaterial("matClayPaladin", StaticValues.maxSwordGlow, Color.white);
            clayRendererInfos[4].defaultMaterial = CreateMaterial("matClayPaladin");

            SkinDef claySkin = CreateSkinDef("PALADINBODY_CLAY_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texClayAchievement"), clayRendererInfos, mainRenderer, model, Modules.Unlockables.paladinClaySkinDef);
            claySkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.clayMesh,
                    renderer = defaultRenderers[4].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.claySwordMesh,
                    renderer = defaultRenderers[0].renderer
                }
            };

            claySkin.gameObjectActivations = defaultActivations;

            skinDefs.Add(claySkin);

            CSSPreviewController.skinChangeResponses[4].triggerSkin = claySkin;
            #endregion

            #region SpecterSkin
            CharacterModel.RendererInfo[] specterRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(specterRendererInfos, 0);

            specterRendererInfos[0].defaultMaterial = CreateMaterial("matPaladinSpecterScythe", StaticValues.maxSwordGlow, Color.white);
            specterRendererInfos[1].defaultMaterial = CreateMaterial("matPaladinGMOld"); //HACK
            specterRendererInfos[4].defaultMaterial = CreateMaterial("matPaladinSpecter");

            SkinDef specterSkin = CreateSkinDef("PALADINBODY_SPECTER_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texSpecterSkin"), specterRendererInfos, mainRenderer, model, Modules.Unlockables.paladinPoisonSkinDef);
            specterSkin.meshReplacements = new SkinDef.MeshReplacement[]  
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.specterMesh,
                    renderer = defaultRenderers[4].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.poisonSwordMesh,
                    renderer = defaultRenderers[0].renderer 
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.defaultCapeMesh,
                    renderer = defaultRenderers[1].renderer
                }
            };

            specterSkin.gameObjectActivations = capeActivations;

            if (Modules.Config.cursed.Value) skinDefs.Add(specterSkin);

            CSSPreviewController.skinChangeResponses[5].triggerSkin = specterSkin;

            #endregion

            #region DripSkin
            CharacterModel.RendererInfo[] dripRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(dripRendererInfos, 0);

            dripRendererInfos[0].defaultMaterial = CreateMaterial("matPaladinDrip", StaticValues.maxSwordGlow, Color.white);
            dripRendererInfos[4].defaultMaterial = CreateMaterial("matPaladinDrip", 3, Color.white);

            SkinDef dripSkin = CreateSkinDef("PALADINBODY_DRIP_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texDripAchievement"), dripRendererInfos, mainRenderer, model);
            dripSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.dripMesh,
                    renderer = defaultRenderers[4].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.batMesh,
                    renderer = defaultRenderers[0].renderer
                }
            };

            dripSkin.gameObjectActivations = defaultActivations;

            if (Modules.Config.cursed.Value) skinDefs.Add(dripSkin);

            CSSPreviewController.skinChangeResponses[6].triggerSkin = dripSkin;
            #endregion

            #region MinecraftSkin
            CharacterModel.RendererInfo[] minecraftRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(minecraftRendererInfos, 0);

            minecraftRendererInfos[0].defaultMaterial = CreateMaterial("matMinecraftSword", StaticValues.maxSwordGlow, Color.white);
            minecraftRendererInfos[4].defaultMaterial = CreateMaterial("matMinecraftPaladin", 3, Color.white);

            SkinDef minecraftSkin = CreateSkinDef("PALADINBODY_MINECRAFT_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texMinecraftSkin"), minecraftRendererInfos, mainRenderer, model);
            minecraftSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.minecraftMesh,
                    renderer = defaultRenderers[4].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.minecraftSwordMesh,
                    renderer = defaultRenderers[0].renderer
                }
            };

            minecraftSkin.gameObjectActivations = defaultActivations;

            if (Modules.Config.cursed.Value) skinDefs.Add(minecraftSkin);

            CSSPreviewController.skinChangeResponses[7].triggerSkin = minecraftSkin;
            #endregion

            skinController.skins = skinDefs.ToArray();

            InitializeNemSkins();
        }

        private static void InitializeNemSkins()
        {
            if (Prefabs.nemPaladinPrefab == null) return;

            GameObject bodyPrefab = Prefabs.nemPaladinPrefab;

            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            List<SkinDef> skinDefs = new List<SkinDef>();

            #region DefaultSkin
            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;
            SkinDef defaultSkin = CreateSkinDef("PALADINBODY_DEFAULT_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texMainSkin"), defaultRenderers, mainRenderer, model);
            defaultSkin.meshReplacements = new SkinDef.MeshReplacement[0];

            skinDefs.Add(defaultSkin);
            #endregion

            skinController.skins = skinDefs.ToArray();
        }
    }
}
