using System;
using UnityEngine;
using R2API;
using RoR2;
using R2API.Utils;

namespace PaladinMod.Modules
{
    public static class Skins
    {
        public static void RegisterSkins()
        {
            GameObject bodyPrefab = PaladinPlugin.characterPrefab;

            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = Reflection.GetFieldValue<SkinnedMeshRenderer>(characterModel, "mainSkinnedMeshRenderer");

            GameObject cloth1 = childLocator.FindChild("Cloth1").gameObject;
            GameObject cloth2 = childLocator.FindChild("Cloth2").gameObject;
            GameObject cloth3 = childLocator.FindChild("Cloth3").gameObject;
            GameObject cloth4 = childLocator.FindChild("Cloth4").gameObject;
            GameObject cloth5 = childLocator.FindChild("Cloth5").gameObject;

            Material commandoMat = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[0].defaultMaterial;

            LoadoutAPI.SkinDefInfo skinDefInfo = default(LoadoutAPI.SkinDefInfo);
            skinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            skinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            skinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            skinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = cloth1,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = cloth2,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = cloth3,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = cloth4,
                    shouldActivate = true
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = cloth5,
                    shouldActivate = true
                }
            };

            skinDefInfo.Icon = Assets.mainAssetBundle.LoadAsset<Sprite>("texMainSkin");
            skinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = mainRenderer.sharedMesh
                }
            };
            skinDefInfo.Name = "PALADINBODY_DEFAULT_SKIN_NAME";
            skinDefInfo.NameToken = "PALADINBODY_DEFAULT_SKIN_NAME";
            skinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            skinDefInfo.RootObject = model;
            skinDefInfo.UnlockableName = "";

            CharacterModel.RendererInfo[] rendererInfos = skinDefInfo.RendererInfos;
            CharacterModel.RendererInfo[] array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            Material material = null;

            for (int i = 1; i < 6; i++)
            {
                material = array[i].defaultMaterial;

                if (material)
                {
                    material = UnityEngine.Object.Instantiate<Material>(commandoMat);
                    material.SetColor("_Color", Modules.Assets.mainAssetBundle.LoadAsset<Material>("matPaladinCloth").GetColor("_Color"));
                    material.SetTexture("_MainTex", Modules.Assets.mainAssetBundle.LoadAsset<Material>("matPaladinCloth").GetTexture("_MainTex"));
                    material.SetColor("_EmColor", Color.white);
                    material.SetFloat("_EmPower", 0);
                    material.SetTexture("_EmTex", Modules.Assets.mainAssetBundle.LoadAsset<Material>("matPaladinCloth").GetTexture("_EmissionMap"));
                    material.SetFloat("_NormalStrength", 0f);

                    array[i].defaultMaterial = material;
                }
            }

            skinDefInfo.RendererInfos = array;

            SkinDef defaultSkin = LoadoutAPI.CreateNewSkinDef(skinDefInfo);

            LoadoutAPI.SkinDefInfo lunarSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            lunarSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            lunarSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            lunarSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            lunarSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = cloth1,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = cloth2,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = cloth3,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = cloth4,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = cloth5,
                    shouldActivate = false
                }
            };

            lunarSkinDefInfo.Icon = Assets.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievement");
            lunarSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = mainRenderer.sharedMesh
                }
            };
            lunarSkinDefInfo.Name = "PALADINBODY_LUNAR_SKIN_NAME";
            lunarSkinDefInfo.NameToken = "PALADINBODY_LUNAR_SKIN_NAME";
            lunarSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            lunarSkinDefInfo.RootObject = model;
            lunarSkinDefInfo.UnlockableName = "PALADIN_MASTERYUNLOCKABLE_REWARD_ID";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            material = array[0].defaultMaterial;

            material = UnityEngine.Object.Instantiate<Material>(commandoMat);
            material.SetTexture("_MainTex", Assets.mainAssetBundle.LoadAsset<Material>("matPaladinLunarBody").GetTexture("_MainTex"));
            material.SetColor("_EmColor", Color.white);
            material.SetFloat("_EmPower", 10);
            material.SetTexture("_EmTex", Assets.mainAssetBundle.LoadAsset<Material>("matPaladinLunarBody").GetTexture("_EmissionMap"));

            array[0].defaultMaterial = material;

            for (int i = 1; i < 6; i ++)
            {
                material = array[i].defaultMaterial;

                if (material)
                {
                    material = UnityEngine.Object.Instantiate<Material>(commandoMat);
                    Assets.mainAssetBundle.LoadAsset<Material>("matPaladinLunarCloth").GetTexture("_MainTex");
                    material.SetColor("_Color", Color.white);
                    material.SetFloat("_EmPower", 0);

                    array[i].defaultMaterial = material;
                }
            }

            material = array[6].defaultMaterial;

            material = UnityEngine.Object.Instantiate<Material>(commandoMat);
            material.SetTexture("_MainTex", Assets.mainAssetBundle.LoadAsset<Material>("matPaladinLunarSword").GetTexture("_MainTex"));
            material.SetColor("_EmColor", Color.white);
            material.SetFloat("_EmPower", StaticValues.maxSwordGlow);
            material.SetTexture("_EmTex", Assets.mainAssetBundle.LoadAsset<Material>("matPaladinLunarSword").GetTexture("_EmissionMap"));

            array[6].defaultMaterial = material;

            lunarSkinDefInfo.RendererInfos = array;

            SkinDef lunarSkin = LoadoutAPI.CreateNewSkinDef(lunarSkinDefInfo);

            LoadoutAPI.SkinDefInfo poisonSkinDefInfo = default(LoadoutAPI.SkinDefInfo);
            poisonSkinDefInfo.BaseSkins = Array.Empty<SkinDef>();
            poisonSkinDefInfo.MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0];
            poisonSkinDefInfo.ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0];

            poisonSkinDefInfo.GameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = cloth1,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = cloth2,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = cloth3,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = cloth4,
                    shouldActivate = false
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = cloth5,
                    shouldActivate = false
                }
            };

            poisonSkinDefInfo.Icon = Assets.mainAssetBundle.LoadAsset<Sprite>("texPoisonAchievement");
            poisonSkinDefInfo.MeshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    renderer = mainRenderer,
                    mesh = mainRenderer.sharedMesh
                }
            };
            poisonSkinDefInfo.Name = "PALADINBODY_POISON_SKIN_NAME";
            poisonSkinDefInfo.NameToken = "PALADINBODY_POISON_SKIN_NAME";
            poisonSkinDefInfo.RendererInfos = characterModel.baseRendererInfos;
            poisonSkinDefInfo.RootObject = model;
            poisonSkinDefInfo.UnlockableName = "PALADIN_POISONUNLOCKABLE_REWARD_ID";

            rendererInfos = skinDefInfo.RendererInfos;
            array = new CharacterModel.RendererInfo[rendererInfos.Length];
            rendererInfos.CopyTo(array, 0);

            material = array[0].defaultMaterial;

            material = UnityEngine.Object.Instantiate<Material>(commandoMat);
            material.SetTexture("_MainTex", Assets.mainAssetBundle.LoadAsset<Material>("matPaladinPoisonBody").GetTexture("_MainTex"));
            material.SetColor("_EmColor", Color.white);
            material.SetFloat("_EmPower", 5);
            material.SetTexture("_EmTex", Assets.mainAssetBundle.LoadAsset<Material>("matPaladinPoisonBody").GetTexture("_EmissionMap"));

            array[0].defaultMaterial = material;

            for (int i = 1; i < 6; i++)
            {
                material = array[i].defaultMaterial;

                if (material)
                {
                    material = UnityEngine.Object.Instantiate<Material>(commandoMat);
                    Assets.mainAssetBundle.LoadAsset<Material>("matPaladinPoisonCloth").GetTexture("_MainTex");
                    material.SetColor("_Color", Color.white);
                    material.SetFloat("_EmPower", 0);

                    array[i].defaultMaterial = material;
                }
            }

            material = array[6].defaultMaterial;

            material = UnityEngine.Object.Instantiate<Material>(commandoMat);
            material.SetTexture("_MainTex", Assets.mainAssetBundle.LoadAsset<Material>("matPaladinPoisonSword").GetTexture("_MainTex"));
            material.SetColor("_EmColor", Color.white);
            material.SetFloat("_EmPower", StaticValues.maxSwordGlow);
            material.SetTexture("_EmTex", Assets.mainAssetBundle.LoadAsset<Material>("matPaladinPoisonSword").GetTexture("_EmissionMap"));

            array[6].defaultMaterial = material;

            poisonSkinDefInfo.RendererInfos = array;

            SkinDef poisonSkin = LoadoutAPI.CreateNewSkinDef(poisonSkinDefInfo);


            skinController.skins = new SkinDef[]
            {
                defaultSkin,
                lunarSkin,
                poisonSkin
            };
        }
    }
}
