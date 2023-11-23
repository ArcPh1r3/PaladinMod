using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinMod.Modules
{
    public static class Prefabs
    {
        public static GameObject paladinPrefab;
        public static GameObject paladinDisplayPrefab;

        public static GameObject nemPaladinPrefab;
        public static GameObject nemPaladinDisplayPrefab;

        public static GameObject lunarKnightPrefab;

        private static PhysicMaterial ragdollMaterial;
        
        internal static List<SurvivorDef> survivorDefinitions = new List<SurvivorDef>();
        internal static List<GameObject> bodyPrefabs = new List<GameObject>();
        internal static List<GameObject> masterPrefabs = new List<GameObject>();
        internal static List<GameObject> projectilePrefabs = new List<GameObject>();

        public static void CreatePrefabs()
        {
            CreatePaladin();
            CreateLunarKnight();
            //CreateNemesisPaladin();
        }

        private static void CreatePaladin()
        {
            paladinPrefab = CreatePrefab("RobPaladinBody", "mdlPaladin", new BodyInfo
            {
                armor = 20f,
                armorGrowth = 0f,//StaticValues.armorPerLevel,
                bodyName = "RobPaladinBody",
                bodyNameToken = "PALADIN_NAME",
                characterPortrait = Assets.charPortrait,
                crosshair = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/SimpleDotCrosshair"),
                damage = StaticValues.baseDamage,
                healthGrowth = 44.4f,
                healthRegen = 1.5f,
                jumpCount = 1,
                maxHealth = 148f,
                subtitleNameToken = "PALADIN_SUBTITLE",
                bodyColor = PaladinPlugin.characterColor
            });

            CustomRendererInfo[] customRendererInfos = new CustomRendererInfo[]
            {
                new CustomRendererInfo
                {
                    childName = "SwordModel",
                    material = Modules.Skins.CreateMaterial("matPaladinSword", StaticValues.maxSwordGlow, Color.white, 1f)
                },
                new CustomRendererInfo
                {
                    childName = "CapeModel",
                    material = Modules.Skins.CreateMaterial("matPaladinGMOld"),
                    ignoreOverlays = true,
                },
                new CustomRendererInfo
                {
                    childName = "MeshCreepyArmsLeft",
                    material = Modules.Skins.CreateMaterial("matPaladinNkuhana", 3, Color.white)
                },
                new CustomRendererInfo
                {
                    childName = "MeshCreepyArmsRight",
                    material = Modules.Skins.CreateMaterial("matPaladinNkuhana", 3, Color.white)
                },
                new CustomRendererInfo
                {
                    childName = "CrownCrystal0",
                    material = Modules.Skins.CreateMaterial("matPaladinGMSword")
                },                
                new CustomRendererInfo
                {
                    childName = "Model",
                    material = Modules.Skins.CreateMaterial("matPaladin", 10, Color.white, 0.25f)
                },
            };

            SetupCharacterModel(paladinPrefab, customRendererInfos, customRendererInfos.Length - 1);

            paladinPrefab.AddComponent<Misc.PaladinSwordController>();
            paladinPrefab.AddComponent<Misc.PaladinRageController>();
            paladinPrefab.GetComponent<ModelLocator>().modelTransform.gameObject.AddComponent<Misc.PaladinAnimationEvents>();

            //VR 
            paladinPrefab.AddComponent<Misc.PaladinVRController>();

            ChildLocator childLocator = paladinPrefab.GetComponentInChildren<ChildLocator>();
            Material eyeTrailMat = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherBody").GetComponentInChildren<CharacterModel>().baseRendererInfos[4].defaultMaterial;
            childLocator.FindChild("EyeTrail").gameObject.GetComponentInChildren<TrailRenderer>().material = eyeTrailMat;

            paladinPrefab.GetComponent<CharacterBody>().sprintingSpeedMultiplier = 1.6f;

            paladinPrefab.GetComponent<CameraTargetParams>().cameraParams = Modules.CameraParams.CreateCameraParamsWithData(PaladinCameraParams.DEFAULT);

            paladinDisplayPrefab = CreateDisplayPrefab("PaladinDisplay", paladinPrefab);
            paladinDisplayPrefab.AddComponent<Misc.MenuSound>();

            // lightning vfx
            #region VFX
            childLocator.FindChild("SpearChargeEffect").Find("ChargeSphere").GetComponent<ParticleSystemRenderer>().material = Modules.Assets.matLoaderLightningSphere;
            childLocator.FindChild("SpearChargeEffect").Find("ChargeSphere").Find("Lightning").GetComponent<ParticleSystemRenderer>().trailMaterial = Modules.Assets.matYellowLightningLong;
            childLocator.FindChild("SpearChargeEffect").Find("ChargeSphere").Find("Spear").GetComponent<ParticleSystemRenderer>().trailMaterial = Modules.Assets.matYellowLightningLong;
            childLocator.FindChild("SpearChargeEffect").Find("ChargeSphere").Find("Spear").Find("SpearLightning").GetComponent<ParticleSystemRenderer>().trailMaterial = Modules.Assets.matYellowLightningLong;

            childLocator.FindChild("SwordLightningEffect").Find("Lightning").GetComponent<ParticleSystemRenderer>().trailMaterial = Modules.Assets.matYellowLightningLong;
            #endregion

            // create hitboxes

            GameObject model = paladinPrefab.GetComponent<ModelLocator>().modelTransform.gameObject;

            Modules.Helpers.CreateHitbox(model, childLocator.FindChild("SwordHitbox"), "Sword");
            Modules.Helpers.CreateHitbox(model, childLocator.FindChild("LeapHitbox"), "LeapStrike");
            Modules.Helpers.CreateHitbox(model, childLocator.FindChild("SpinSlashHitbox"), "SpinSlash");
            Modules.Helpers.CreateHitbox(model, childLocator.FindChild("SpinSlashLargeHitbox"), "SpinSlashLarge");
        }

        private static void CreateNemesisPaladin()
        {
            nemPaladinPrefab = CreatePrefab("NemesisbPaladinBody", "mdlNemPaladin", new BodyInfo
            {
                armor = 10f,
                armorGrowth = StaticValues.armorPerLevel,
                bodyName = "NemesisPaladinBody",
                bodyNameToken = "NEMPALADIN_NAME",
                characterPortrait = Assets.mainAssetBundle.LoadAsset<Texture>("texNemPaladinPlayerIcon"),
                crosshair = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/SimpleDotCrosshair"),
                damage = StaticValues.baseDamage,
                healthGrowth = 64,
                healthRegen = 1.5f,
                jumpCount = 1,
                maxHealth = 160f,
                subtitleNameToken = "NEMPALADIN_SUBTITLE",
                bodyColor = Color.blue
            });

            Material featherMat = Modules.Skins.CreateMaterial("matNemPaladinFeather");
            featherMat.EnableKeyword("CUTOUT");

            Material clothMat = Modules.Skins.CreateMaterial("matNemPaladinCloth", 0, Color.black, 1f);
            clothMat.EnableKeyword("CUTOUT");

            SetupCharacterModel(nemPaladinPrefab, new CustomRendererInfo[]
            {
                new CustomRendererInfo
                {
                    childName = "ClothModel",
                    material = clothMat
                },
                new CustomRendererInfo
                {
                    childName = "CrystalModel",
                    material = Modules.Skins.CreateMaterial("matNemPaladin", 15f)
                },
                new CustomRendererInfo
                {
                    childName = "HairModel",
                    material = featherMat
                },
                new CustomRendererInfo
                {
                    childName = "FeatherModel",
                    material = featherMat
                },
                new CustomRendererInfo
                {
                    childName = "Model",
                    material = Modules.Skins.CreateMaterial("matNemPaladin", 15f)
                }
            }, 4);

            nemPaladinPrefab.GetComponent<CharacterBody>().sprintingSpeedMultiplier = 1.6f;

            nemPaladinPrefab.GetComponent<CameraTargetParams>().cameraParams = Modules.CameraParams.CreateCameraParamsWithData(PaladinCameraParams.DEFAULT);

            nemPaladinDisplayPrefab = CreateDisplayPrefab("NemPaladinDisplay", nemPaladinPrefab);

            // create hitboxes

            GameObject model = nemPaladinPrefab.GetComponent<ModelLocator>().modelTransform.gameObject;
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            Modules.Helpers.CreateHitbox(model, childLocator.FindChild("PunchHitbox"), "Punch");
        }

        private static void CreateLunarKnight()
        {
            lunarKnightPrefab = CreatePrefab("LunarKnightBody", "mdlLunarKnight", new BodyInfo
            {
                armor = 20f,
                bodyName = "LunarKnightBody",
                bodyNameToken = "LUNAR_KNIGHT_BODY_NAME",
                characterPortrait = Assets.charPortrait,
                crosshair = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Crosshair/SimpleDotCrosshair"),
                damage = 12f,
                healthGrowth = 144,
                healthRegen = 0f,
                jumpCount = 1,
                maxHealth = 480f,
                subtitleNameToken = "LUNAR_KNIGHT_BODY_SUBTITLE"
            });

            SetupCharacterModel(lunarKnightPrefab, new CustomRendererInfo[]
            {
                new CustomRendererInfo
                {
                    childName = "SwordModel",
                    material = Modules.Skins.CreateMaterial("matLunarKnight")
                },
                new CustomRendererInfo
                {
                    childName = "Model",
                    material = Modules.Skins.CreateMaterial("matLunarKnight")
                }
            }, 1);

            lunarKnightPrefab.AddComponent<Misc.PaladinSwordController>();

            // create hitboxes

            GameObject model = lunarKnightPrefab.GetComponent<ModelLocator>().modelTransform.gameObject;
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            Modules.Helpers.CreateHitbox(model, childLocator.FindChild("SwordHitbox"), "Sword");
            Modules.Helpers.CreateHitbox(model, childLocator.FindChild("LeapHitbox"), "LeapStrike");
            Modules.Helpers.CreateHitbox(model, childLocator.FindChild("SpinSlashHitbox"), "SpinSlash");
            Modules.Helpers.CreateHitbox(model, childLocator.FindChild("SpinSlashLargeHitbox"), "SpinSlashLarge");
        }

        public static GameObject CreateDisplayPrefab(string modelName, GameObject prefab)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody"), modelName + "Prefab");

            GameObject model = CreateModel(newPrefab, modelName);
            Transform modelBaseTransform = SetupModel(newPrefab, model.transform);

            model.AddComponent<CharacterModel>().baseRendererInfos = prefab.GetComponentInChildren<CharacterModel>().baseRendererInfos;

            return model.gameObject;
        }

        public static void SetupCharacterModel(GameObject prefab, CustomRendererInfo[] rendererInfo, int mainRendererIndex)
        {
            CharacterModel characterModel = prefab.GetComponent<ModelLocator>().modelTransform.gameObject.AddComponent<CharacterModel>();
            ChildLocator childLocator = characterModel.GetComponent<ChildLocator>();

            characterModel.body = prefab.GetComponent<CharacterBody>();

            List<CharacterModel.RendererInfo> rendererInfos = new List<CharacterModel.RendererInfo>();

            for (int i = 0; i < rendererInfo.Length; i++)
            {
                Renderer renderer = childLocator.FindChild(rendererInfo[i].childName)?.GetComponent<SkinnedMeshRenderer>();

                if (renderer == null)
                {
                    renderer = childLocator.FindChild(rendererInfo[i].childName)?.GetComponent<MeshRenderer>();
                }
                if (renderer == null)
                {
                    Debug.LogError("no renderer found for " + rendererInfo[i].childName);
                }

                rendererInfos.Add(new CharacterModel.RendererInfo
                {
                    renderer = renderer,
                    defaultMaterial = rendererInfo[i].material,
                    ignoreOverlays = rendererInfo[i].ignoreOverlays,
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On
                });
            }

            characterModel.baseRendererInfos = rendererInfos.ToArray();

            characterModel.autoPopulateLightInfos = true;
            characterModel.invisibilityCount = 0;
            characterModel.temporaryOverlays = new List<TemporaryOverlay>();

            characterModel.mainSkinnedMeshRenderer = characterModel.baseRendererInfos[mainRendererIndex].renderer.GetComponent<SkinnedMeshRenderer>();
        }

        public static GameObject CreatePrefab(string bodyName, string modelName, BodyInfo bodyInfo)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody"), bodyName);

            GameObject model = CreateModel(newPrefab, modelName);
            Transform modelBaseTransform = SetupModel(newPrefab, model.transform);

            #region CharacterBody
            CharacterBody bodyComponent = newPrefab.GetComponent<CharacterBody>();

            bodyComponent.name = bodyInfo.bodyName;
            bodyComponent.baseNameToken = bodyInfo.bodyNameToken;
            bodyComponent.subtitleNameToken = bodyInfo.subtitleNameToken;
            bodyComponent.portraitIcon = bodyInfo.characterPortrait;
            bodyComponent._defaultCrosshairPrefab = bodyInfo.crosshair;
            bodyComponent.bodyColor = bodyInfo.bodyColor;

            bodyComponent.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;
            bodyComponent.rootMotionInMainState = false;

            bodyComponent.baseMaxHealth = bodyInfo.maxHealth;
            bodyComponent.levelMaxHealth = bodyInfo.healthGrowth;

            bodyComponent.baseRegen = bodyInfo.healthRegen;
            bodyComponent.levelRegen = bodyComponent.baseRegen * 0.2f;

            bodyComponent.baseMaxShield = bodyInfo.shield;
            bodyComponent.levelMaxShield = bodyInfo.shieldGrowth;

            bodyComponent.baseMoveSpeed = bodyInfo.moveSpeed;
            bodyComponent.levelMoveSpeed = bodyInfo.moveSpeedGrowth;

            bodyComponent.baseAcceleration = bodyInfo.acceleration;

            bodyComponent.baseJumpPower = bodyInfo.jumpPower;
            bodyComponent.levelJumpPower = bodyInfo.jumpPowerGrowth;

            bodyComponent.baseDamage = bodyInfo.damage;
            bodyComponent.levelDamage = bodyComponent.baseDamage * 0.2f;

            bodyComponent.baseAttackSpeed = bodyInfo.attackSpeed;
            bodyComponent.levelAttackSpeed = bodyInfo.attackSpeedGrowth;

            bodyComponent.baseArmor = bodyInfo.armor;
            bodyComponent.levelArmor = bodyInfo.armorGrowth;

            bodyComponent.baseCrit = bodyInfo.crit;
            bodyComponent.levelCrit = bodyInfo.critGrowth;

            bodyComponent.baseJumpCount = bodyInfo.jumpCount;

            bodyComponent.sprintingSpeedMultiplier = 1.45f;

            bodyComponent.hideCrosshair = false;
            bodyComponent.aimOriginTransform = modelBaseTransform.Find("AimOrigin");
            bodyComponent.hullClassification = HullClassification.Human;

            bodyComponent.preferredPodPrefab = bodyInfo.podPrefab;

            bodyComponent.isChampion = false;
            #endregion

            SetupCharacterDirection(newPrefab, modelBaseTransform, model.transform);
            SetupCameraTargetParams(newPrefab);
            SetupModelLocator(newPrefab, modelBaseTransform, model.transform);
            SetupRigidbody(newPrefab);
            SetupCapsuleCollider(newPrefab);
            SetupMainHurtbox(newPrefab, model);
            SetupFootstepController(model);
            SetupRagdoll(model);
            SetupAimAnimator(newPrefab, model);

            bodyPrefabs.Add(newPrefab);

            return newPrefab;
        }

        private static Transform SetupModel(GameObject prefab, Transform modelTransform)
        {
            GameObject modelBase = new GameObject("ModelBase");
            modelBase.transform.parent = prefab.transform;
            modelBase.transform.localPosition = new Vector3(0f, -0.92f, 0f);
            modelBase.transform.localRotation = Quaternion.identity;
            modelBase.transform.localScale = new Vector3(1f, 1f, 1f);

            GameObject cameraPivot = new GameObject("CameraPivot");
            cameraPivot.transform.parent = modelBase.transform;
            cameraPivot.transform.localPosition = new Vector3(0f, 1.6f, 0f);
            cameraPivot.transform.localRotation = Quaternion.identity;
            cameraPivot.transform.localScale = Vector3.one;

            GameObject aimOrigin = new GameObject("AimOrigin");
            aimOrigin.transform.parent = modelBase.transform;
            aimOrigin.transform.localPosition = new Vector3(0f, 2.2f, 0f);
            aimOrigin.transform.localRotation = Quaternion.identity;
            aimOrigin.transform.localScale = Vector3.one;
            prefab.GetComponent<CharacterBody>().aimOriginTransform = aimOrigin.transform;

            modelTransform.parent = modelBase.transform;
            modelTransform.localPosition = Vector3.zero;
            modelTransform.localRotation = Quaternion.identity;

            return modelBase.transform;
        }

        private static GameObject CreateModel(GameObject main, string modelName)
        {
            PaladinPlugin.DestroyImmediate(main.transform.Find("ModelBase").gameObject);
            PaladinPlugin.DestroyImmediate(main.transform.Find("CameraPivot").gameObject);
            PaladinPlugin.DestroyImmediate(main.transform.Find("AimOrigin").gameObject);

            return Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(modelName);
        }

        private static void SetupCharacterDirection(GameObject prefab, Transform modelBaseTransform, Transform modelTransform)
        {
            CharacterDirection characterDirection = prefab.GetComponent<CharacterDirection>();
            characterDirection.targetTransform = modelBaseTransform;
            characterDirection.overrideAnimatorForwardTransform = null;
            characterDirection.rootMotionAccumulator = null;
            characterDirection.modelAnimator = modelTransform.GetComponent<Animator>();
            characterDirection.driveFromRootRotation = false;
            characterDirection.turnSpeed = 720f;
        }

        private static void SetupCameraTargetParams(GameObject prefab)
        {
            CameraTargetParams cameraTargetParams = prefab.GetComponent<CameraTargetParams>();
            cameraTargetParams.cameraParams = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ToolBotBody").GetComponent<CameraTargetParams>().cameraParams;
            cameraTargetParams.cameraPivotTransform = prefab.transform.Find("ModelBase").Find("CameraPivot");
            //cameraTargetParams.currentCameraParamsData.default = CameraTargetParams.AimType.Standard;
            cameraTargetParams.recoil = Vector2.zero;
        }

        private static void SetupModelLocator(GameObject prefab, Transform modelBaseTransform, Transform modelTransform)
        {
            ModelLocator modelLocator = prefab.GetComponent<ModelLocator>();
            modelLocator.modelTransform = modelTransform;
            modelLocator.modelBaseTransform = modelBaseTransform;
        }

        private static void SetupRigidbody(GameObject prefab)
        {
            Rigidbody rigidbody = prefab.GetComponent<Rigidbody>();
            rigidbody.mass = 100f;
        }

        private static void SetupCapsuleCollider(GameObject prefab)
        {
            CapsuleCollider capsuleCollider = prefab.GetComponent<CapsuleCollider>();
            capsuleCollider.center = new Vector3(0f, 0f, 0f);
            capsuleCollider.radius = 0.5f;
            capsuleCollider.height = 1.82f;
            capsuleCollider.direction = 1;
        }

        private static void SetupMainHurtbox(GameObject prefab, GameObject model)
        {
            HurtBoxGroup hurtBoxGroup = model.AddComponent<HurtBoxGroup>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            HurtBox mainHurtbox = childLocator.FindChild("MainHurtbox").gameObject.AddComponent<HurtBox>();
            mainHurtbox.gameObject.layer = LayerIndex.entityPrecise.intVal;
            mainHurtbox.healthComponent = prefab.GetComponent<HealthComponent>();
            mainHurtbox.isBullseye = true;
            mainHurtbox.damageModifier = HurtBox.DamageModifier.Normal;
            mainHurtbox.hurtBoxGroup = hurtBoxGroup;
            mainHurtbox.indexInGroup = 0;
            mainHurtbox.isSniperTarget = true;

            hurtBoxGroup.hurtBoxes = new HurtBox[]
            {
                mainHurtbox
            };

            hurtBoxGroup.mainHurtBox = mainHurtbox;
            hurtBoxGroup.bullseyeCount = 1;
        }

        private static void SetupFootstepController(GameObject model)
        {
            FootstepHandler footstepHandler = model.AddComponent<FootstepHandler>();
            footstepHandler.baseFootstepString = "Play_player_footstep";
            footstepHandler.sprintFootstepOverrideString = "";
            footstepHandler.enableFootstepDust = true;
            footstepHandler.footstepDustPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/GenericFootstepDust");
        }

        private static void SetupRagdoll(GameObject model)
        {
            RagdollController ragdollController = model.GetComponent<RagdollController>();

            if (!ragdollController) return;

            if (ragdollMaterial == null) ragdollMaterial = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<RagdollController>().bones[1].GetComponent<Collider>().material;

            foreach (Transform i in ragdollController.bones)
            {
                if (i)
                {
                    i.gameObject.layer = LayerIndex.ragdoll.intVal;
                    Collider j = i.GetComponent<Collider>();
                    if (j)
                    {
                        j.material = ragdollMaterial;
                        j.sharedMaterial = ragdollMaterial;
                    }
                }
            }
        }

        private static void SetupAimAnimator(GameObject prefab, GameObject model)
        {
            AimAnimator aimAnimator = model.AddComponent<AimAnimator>();
            aimAnimator.directionComponent = prefab.GetComponent<CharacterDirection>();
            aimAnimator.pitchRangeMax = 60f;
            aimAnimator.pitchRangeMin = -60f;
            aimAnimator.yawRangeMin = -80f;
            aimAnimator.yawRangeMax = 80f;
            aimAnimator.pitchGiveupRange = 30f;
            aimAnimator.yawGiveupRange = 10f;
            aimAnimator.giveupDuration = 3f;
            aimAnimator.inputBank = prefab.GetComponent<InputBankTest>();
        }
    }
}