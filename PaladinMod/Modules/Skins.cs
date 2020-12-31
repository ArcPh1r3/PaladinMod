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

        public static Material CreateMaterial(string materialName, float emission, Color emissionColor, float normalStrength)
        {
            if (!PaladinPlugin.commandoMat) PaladinPlugin.commandoMat = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;

            Material mat = UnityEngine.Object.Instantiate<Material>(PaladinPlugin.commandoMat);
            Material tempMat = Assets.mainAssetBundle.LoadAsset<Material>(materialName);
            if (!tempMat)
            {
                return PaladinPlugin.commandoMat;
            }

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
            GameObject bodyPrefab = PaladinPlugin.characterPrefab;

            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = Reflection.GetFieldValue<SkinnedMeshRenderer>(characterModel, "mainSkinnedMeshRenderer");

            Material commandoMat = PaladinPlugin.commandoMat;

            List<SkinDef> skinDefs = new List<SkinDef>();

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

            skinDefs.Add(defaultSkin);
            #endregion
            
            #region MasterySkin
            CharacterModel.RendererInfo[] masteryRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(masteryRendererInfos, 0);

            masteryRendererInfos[0].defaultMaterial = CreateMaterial("matPaladinLunar", StaticValues.maxSwordGlow, Color.white, 0);
            masteryRendererInfos[1].defaultMaterial = CreateMaterial("matPaladinLunar", 5, Color.white, 0);

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

            skinDefs.Add(masterySkin);
            #endregion
            
            #region PoisonSkin
            CharacterModel.RendererInfo[] poisonRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(poisonRendererInfos, 0);

            poisonRendererInfos[0].defaultMaterial = CreateMaterial("matPaladinNkuhana", StaticValues.maxSwordGlow, Color.white, 0);
            poisonRendererInfos[1].defaultMaterial = CreateMaterial("matPaladinNkuhana", 3, Color.white, 0);

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

            skinDefs.Add(poisonSkin);
            #endregion
            /*
            #region HunterSkin
            CharacterModel.RendererInfo[] hunterRendererInfos = new CharacterModel.RendererInfo[defaultRenderers.Length];
            defaultRenderers.CopyTo(hunterRendererInfos, 0);

            bodyMat = CreateMaterial("matHunterBody", 10, Color.white, 0);
            clothMat = CreateMaterial("matPaladinPoisonCloth", 0, Color.white, 0);
            swordMat = CreateMaterial("matHunterSword", StaticValues.maxSwordGlow, Color.white, 0);

            hunterRendererInfos[0].defaultMaterial = bodyMat;
            for (int i = 1; i < 6; i++)
            {
                hunterRendererInfos[i].defaultMaterial = clothMat;
            }
            hunterRendererInfos[6].defaultMaterial = swordMat;
            hunterRendererInfos[7].defaultMaterial = clothMat;

            SkinDef hunterSkin = CreateSkinDef("PALADINBODY_HUNTER_SKIN_NAME", Assets.mainAssetBundle.LoadAsset<Sprite>("texPoisonAchievement"), hunterRendererInfos, mainRenderer, model, "");
            hunterSkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = defaultRenderers[5].renderer.gameObject,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = defaultRenderers[7].renderer.gameObject,
                    shouldActivate = false
                }
            };
            hunterSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.hunterMesh,
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Assets.defaultSwordMesh,
                    renderer = defaultRenderers[6].renderer
                }
            };

            skinDefs.Add(hunterSkin);
            #endregion
    */

            skinController.skins = skinDefs.ToArray();
        }
    }
}
