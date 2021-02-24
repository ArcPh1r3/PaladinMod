using System;
using UnityEngine;
using R2API;
using RoR2;
using R2API.Utils;
using System.Collections.Generic;

namespace PaladinMod.Modules
{
    public static class Skins
    {
        public static SkinDef CreateSkinDef(string skinName, Sprite skinIcon, CharacterModel.RendererInfo[] rendererInfos, SkinnedMeshRenderer mainRenderer, GameObject root, string unlockName)
        {
            LoadoutAPI.SkinDefInfo skinDefInfo = new LoadoutAPI.SkinDefInfo
            {
                BaseSkins = Array.Empty<SkinDef>(),
                GameObjectActivations = new SkinDef.GameObjectActivation[0],
                Icon = skinIcon,
                MeshReplacements = new SkinDef.MeshReplacement[0],
                MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0],
                Name = skinName,
                NameToken = skinName,
                ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0],
                RendererInfos = rendererInfos,
                RootObject = root,
                UnlockableName = unlockName
            };

            SkinDef skin = LoadoutAPI.CreateNewSkinDef(skinDefInfo);

            return skin;
        }

        public static SkinDef CreateSkinDef(string skinName, Sprite skinIcon, CharacterModel.RendererInfo[] rendererInfos, SkinnedMeshRenderer mainRenderer, GameObject root, string unlockName, Mesh skinMesh)
        {
            LoadoutAPI.SkinDefInfo skinDefInfo = new LoadoutAPI.SkinDefInfo
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
                UnlockableName = unlockName
            };

            SkinDef skin = LoadoutAPI.CreateNewSkinDef(skinDefInfo);

            return skin;
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

            List<SkinDef> skinDefs = new List<SkinDef>();

            GameObject cape = childLocator.FindChild("Cape").gameObject;

            SkinDef.GameObjectActivation[] defaultActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = cape,
                    shouldActivate = false
                }
            };

            #region DefaultSkin
            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;
            SkinDef defaultSkin = CreateSkinDef("PALADINBODY_DEFAULT_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texMainSkin"), defaultRenderers, mainRenderer, model, "");
            defaultSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.defaultMesh,
                    renderer = defaultRenderers[1].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.defaultSwordMesh,
                    renderer = defaultRenderers[0].renderer
                }
            };

            defaultSkin.gameObjectActivations = defaultActivations;

            skinDefs.Add(defaultSkin);
            #endregion
            
            #region MasterySkin
            CharacterModel.RendererInfo[] masteryRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(masteryRendererInfos, 0);

            masteryRendererInfos[0].defaultMaterial = CreateMaterial("matPaladinLunar", StaticValues.maxSwordGlow, Color.white);
            masteryRendererInfos[1].defaultMaterial = CreateMaterial("matPaladinLunar", 5, Color.white);

            SkinDef masterySkin = CreateSkinDef("PALADINBODY_LUNAR_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievement"), masteryRendererInfos, mainRenderer, model, "PALADIN_MASTERYUNLOCKABLE_REWARD_ID");
            masterySkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.lunarMesh,
                    renderer = defaultRenderers[1].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.lunarSwordMesh,
                    renderer = defaultRenderers[0].renderer
                }
            };

            masterySkin.gameObjectActivations = defaultActivations;

            skinDefs.Add(masterySkin);
            #endregion

            #region GrandMasterySkin
            CharacterModel.RendererInfo[] grandMasteryRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(grandMasteryRendererInfos, 0);

            grandMasteryRendererInfos[0].defaultMaterial = CreateMaterial("matPaladinGMSword", StaticValues.maxSwordGlow, Color.white);
            grandMasteryRendererInfos[1].defaultMaterial = CreateMaterial("matPaladinGM", 15, Color.white);

            SkinDef grandMasterySkin = CreateSkinDef("PALADINBODY_TYPHOON_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievement"), grandMasteryRendererInfos, mainRenderer, model, "PALADIN_TYPHOONUNLOCKABLE_REWARD_ID");
            grandMasterySkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.gmMesh,
                    renderer = defaultRenderers[1].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.gmSwordMesh,
                    renderer = defaultRenderers[0].renderer
                }
            };

            grandMasterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = cape,
                    shouldActivate = true
                }
            };

            skinDefs.Add(grandMasterySkin);
            #endregion

            #region PoisonSkin
            CharacterModel.RendererInfo[] poisonRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(poisonRendererInfos, 0);

            poisonRendererInfos[0].defaultMaterial = CreateMaterial("matPaladinNkuhana", StaticValues.maxSwordGlow, Color.white);
            poisonRendererInfos[1].defaultMaterial = CreateMaterial("matPaladinNkuhana", 3, Color.white);

            SkinDef poisonSkin = CreateSkinDef("PALADINBODY_POISON_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texPoisonAchievement"), poisonRendererInfos, mainRenderer, model, "PALADIN_POISONUNLOCKABLE_REWARD_ID");
            poisonSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.poisonMesh,
                    renderer = defaultRenderers[1].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.poisonSwordMesh,
                    renderer = defaultRenderers[0].renderer
                }
            };

            poisonSkin.gameObjectActivations = defaultActivations;

            skinDefs.Add(poisonSkin);
            #endregion

            #region ClaySkin
            CharacterModel.RendererInfo[] clayRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(clayRendererInfos, 0);

            clayRendererInfos[0].defaultMaterial = CreateMaterial("matClayPaladin", StaticValues.maxSwordGlow, Color.white);
            clayRendererInfos[1].defaultMaterial = CreateMaterial("matClayPaladin");

            SkinDef claySkin = CreateSkinDef("PALADINBODY_CLAY_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texClayAchievement"), clayRendererInfos, mainRenderer, model, "PALADIN_CLAYUNLOCKABLE_REWARD_ID");
            claySkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.clayMesh,
                    renderer = defaultRenderers[1].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.claySwordMesh,
                    renderer = defaultRenderers[0].renderer
                }
            };

            claySkin.gameObjectActivations = defaultActivations;

            skinDefs.Add(claySkin);
            #endregion

            #region DripSkin
            CharacterModel.RendererInfo[] dripRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(dripRendererInfos, 0);

            dripRendererInfos[0].defaultMaterial = CreateMaterial("matPaladinDrip", StaticValues.maxSwordGlow, Color.white);
            dripRendererInfos[1].defaultMaterial = CreateMaterial("matPaladinDrip", 3, Color.white);

            SkinDef dripSkin = CreateSkinDef("PALADINBODY_DRIP_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texDripAchievement"), dripRendererInfos, mainRenderer, model, "PALADIN_dripUNLOCKABLE_REWARD_ID");
            dripSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.dripMesh,
                    renderer = defaultRenderers[1].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.batMesh,
                    renderer = defaultRenderers[0].renderer
                }
            };

            dripSkin.gameObjectActivations = defaultActivations;

            if (Modules.Config.cursed.Value) skinDefs.Add(dripSkin);
            #endregion

            #region MinecraftSkin
            CharacterModel.RendererInfo[] minecraftRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(minecraftRendererInfos, 0);

            minecraftRendererInfos[0].defaultMaterial = CreateMaterial("matMinecraftSword", StaticValues.maxSwordGlow, Color.white);
            minecraftRendererInfos[1].defaultMaterial = CreateMaterial("matMinecraftPaladin", 3, Color.white);

            SkinDef minecraftSkin = CreateSkinDef("PALADINBODY_MINECRAFT_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texMinecraftSkin"), minecraftRendererInfos, mainRenderer, model, "");
            minecraftSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.minecraftMesh,
                    renderer = defaultRenderers[1].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.minecraftSwordMesh,
                    renderer = defaultRenderers[0].renderer
                }
            };

            minecraftSkin.gameObjectActivations = defaultActivations;

            if (Modules.Config.cursed.Value) skinDefs.Add(minecraftSkin);
            #endregion

            skinController.skins = skinDefs.ToArray();
        }
    }
}
