using System;
using System.Collections.Generic;
using BepInEx;
using R2API;
using R2API.Utils;
using EntityStates;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;
using KinematicCharacterController;
using System.Runtime.CompilerServices;

namespace PaladinMod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, "Paladin", "1.0.10")]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "SurvivorAPI",
        "LoadoutAPI",
        "BuffAPI",
        "LanguageAPI",
        "SoundAPI",
        "EffectAPI",
        "UnlockablesAPI",
        "ResourcesAPI"
    })]

    public class PaladinPlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.rob.Paladin";

        public static PaladinPlugin instance;

        public static GameObject characterPrefab;
        public static GameObject characterDisplay;

        public GameObject doppelganger;

        public static Material commandoMat;

        public static event Action awake;
        public static event Action start;

        public static readonly Color characterColor = Color.red;

        public SkillLocator skillLocator;

        public static SkillDef scepterHealDef;
        public static SkillDef scepterTorporDef;
        public static SkillDef scepterWarcryDef;

        public PaladinPlugin()
        {
            awake += PaladinPlugin_Load;
            start += PaladinPlugin_LoadStart;
        }

        private void PaladinPlugin_Load()
        {
            instance = this;

            Modules.Assets.PopulateAssets();
            Modules.Config.ReadConfig();

            CreatePrefab();
            CreateDisplayPrefab();
            RegisterCharacter();
            Modules.Skins.RegisterSkins();
            Modules.Buffs.RegisterBuffs();
            Modules.Projectiles.RegisterProjectiles();
            Modules.ItemDisplays.RegisterDisplays();
            Modules.Effects.RegisterEffects();
            Modules.Unlockables.RegisterUnlockables();
            Modules.Tokens.AddTokens();

            CreateDoppelganger();

            //scepter stuff
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.ThinkInvisible.ClassicItems"))
            {
                ScepterSkillSetup();
                ScepterSetup();
            }

            Hook();
        }

        private void PaladinPlugin_LoadStart()
        {
        }

        public void Awake()
        {
            Action awake = PaladinPlugin.awake;
            if (awake == null)
            {
                return;
            }
            awake();
        }

        public void Start()
        {
            Action start = PaladinPlugin.start;
            if (start == null)
            {
                return;
            }
            start();
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void ScepterSetup()
        {
            ThinkInvisible.ClassicItems.Scepter_V2.instance.RegisterScepterSkill(scepterHealDef, "RobPaladinBody", SkillSlot.Special, 0);
            ThinkInvisible.ClassicItems.Scepter_V2.instance.RegisterScepterSkill(scepterTorporDef, "RobPaladinBody", SkillSlot.Special, 1);
            ThinkInvisible.ClassicItems.Scepter_V2.instance.RegisterScepterSkill(scepterWarcryDef, "RobPaladinBody", SkillSlot.Special, 2);
        }

        private void Hook()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.CharacterModel.UpdateOverlays += CharacterModel_UpdateOverlays;
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            if (self)
            {
                if (self.HasBuff(Modules.Buffs.warcryBuff))
                {
                    float damageBuff = StaticValues.warcryDamageMultiplier * self.damage;
                    self.damage += damageBuff;
                    self.armor += StaticValues.warcryArmorBuff;
                }

                if (self.HasBuff(Modules.Buffs.scepterWarcryBuff))
                {
                    float damageBuff = StaticValues.scepterWarcryDamageMultiplier * self.damage;
                    self.damage += damageBuff;
                    self.armor += StaticValues.scepterWarcryArmorBuff;
                }

                if (self.HasBuff(Modules.Buffs.torporDebuff))
                {
                    self.moveSpeed *= (1 - StaticValues.torporSlowAmount);
                    self.attackSpeed *= (1 - StaticValues.torporSlowAmount);
                }

                if (self.HasBuff(Modules.Buffs.scepterTorporDebuff))
                {
                    self.moveSpeed *= (1 - StaticValues.scepterTorporSlowAmount);
                    self.attackSpeed *= (1 - StaticValues.scepterTorporSlowAmount);
                }
            }
        }

        private void CharacterModel_UpdateOverlays(On.RoR2.CharacterModel.orig_UpdateOverlays orig, CharacterModel self)
        {
            orig(self);

            if (self)
            {
                if (self.body && self.body.HasBuff(Modules.Buffs.torporDebuff))
                {
                    var torporController = self.body.GetComponent<Misc.PaladinTorporTracker>();
                    if (!torporController) torporController = self.body.gameObject.AddComponent<Misc.PaladinTorporTracker>();
                    else return;

                    torporController.Body = self.body;
                    TemporaryOverlay overlay = self.gameObject.AddComponent<RoR2.TemporaryOverlay>();
                    overlay.duration = float.PositiveInfinity;
                    overlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    overlay.animateShaderAlpha = true;
                    overlay.destroyComponentOnEnd = true;
                    overlay.originalMaterial = Resources.Load<Material>("Materials/matDoppelganger");
                    overlay.AddToCharacerModel(self);
                    torporController.Overlay = overlay;
                }
            }
        }

        private static GameObject CreateModel(GameObject main, int index)
        {
            Destroy(main.transform.Find("ModelBase").gameObject);
            Destroy(main.transform.Find("CameraPivot").gameObject);
            Destroy(main.transform.Find("AimOrigin").gameObject);

            GameObject model = null;

            if (index == 0) model = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("mdlPaladin");
            else if (index == 1) model = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("PaladinDisplay");

            return model;
        }

        private static void CreateDisplayPrefab()
        {
            //spaghetti incoming, brace yourself
            GameObject tempDisplay = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody"), "PaladinDisplay");

            GameObject model = CreateModel(tempDisplay, 1);

            GameObject gameObject = new GameObject("ModelBase");
            gameObject.transform.parent = tempDisplay.transform;
            gameObject.transform.localPosition = new Vector3(0f, -0.92f, 0f);
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);

            GameObject gameObject2 = new GameObject("CameraPivot");
            gameObject2.transform.parent = gameObject.transform;
            gameObject2.transform.localPosition = new Vector3(0f, 1.6f, 0f);
            gameObject2.transform.localRotation = Quaternion.identity;
            gameObject2.transform.localScale = Vector3.one;

            GameObject gameObject3 = new GameObject("AimOrigin");
            gameObject3.transform.parent = gameObject.transform;
            gameObject3.transform.localPosition = new Vector3(0f, 1.8f, 0f);
            gameObject3.transform.localRotation = Quaternion.identity;
            gameObject3.transform.localScale = Vector3.one;

            Transform transform = model.transform;
            transform.parent = gameObject.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            ModelLocator modelLocator = tempDisplay.GetComponent<ModelLocator>();
            modelLocator.modelTransform = transform;
            modelLocator.modelBaseTransform = gameObject.transform;

            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            CharacterModel characterModel = model.AddComponent<CharacterModel>();
            characterModel.body = null;
            characterModel.baseRendererInfos = new CharacterModel.RendererInfo[]
            {
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = childLocator.FindChild("SwordModel").GetComponent<SkinnedMeshRenderer>().material,
                    renderer = childLocator.FindChild("SwordModel").GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                },
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = childLocator.FindChild("Model").GetComponent<SkinnedMeshRenderer>().material,
                    renderer = childLocator.FindChild("Model").GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                }
            };
            characterModel.autoPopulateLightInfos = true;
            characterModel.invisibilityCount = 0;
            characterModel.temporaryOverlays = new List<TemporaryOverlay>();

            //replace shader
            characterModel.baseRendererInfos[0].defaultMaterial = Modules.Skins.CreateMaterial("matPaladin", StaticValues.maxSwordGlow, Color.white, 0);
            characterModel.baseRendererInfos[1].defaultMaterial = Modules.Skins.CreateMaterial("matPaladin", 10, Color.white, 0);

            characterModel.SetFieldValue("mainSkinnedMeshRenderer", characterModel.baseRendererInfos[0].renderer.gameObject.GetComponent<SkinnedMeshRenderer>());

            characterDisplay = PrefabAPI.InstantiateClone(tempDisplay.GetComponent<ModelLocator>().modelBaseTransform.gameObject, "PaladinDisplay", true);

            characterDisplay.AddComponent<Misc.MenuSound>();
        }

        private static void CreatePrefab()
        {
            characterPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody"), "RobPaladinBody");

            characterPrefab.GetComponent<NetworkIdentity>().localPlayerAuthority = true;

            GameObject model = CreateModel(characterPrefab, 0);

            GameObject gameObject = new GameObject("ModelBase");
            gameObject.transform.parent = characterPrefab.transform;
            gameObject.transform.localPosition = new Vector3(0f, -0.92f, 0f);
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);

            GameObject gameObject2 = new GameObject("CameraPivot");
            gameObject2.transform.parent = gameObject.transform;
            gameObject2.transform.localPosition = new Vector3(0f, 1.6f, 0f);
            gameObject2.transform.localRotation = Quaternion.identity;
            gameObject2.transform.localScale = Vector3.one;

            GameObject gameObject3 = new GameObject("AimOrigin");
            gameObject3.transform.parent = gameObject.transform;
            gameObject3.transform.localPosition = new Vector3(0f, 2.2f, 0f);
            gameObject3.transform.localRotation = Quaternion.identity;
            gameObject3.transform.localScale = Vector3.one;

            Transform transform = model.transform;
            transform.parent = gameObject.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            CharacterDirection characterDirection = characterPrefab.GetComponent<CharacterDirection>();
            characterDirection.moveVector = Vector3.zero;
            characterDirection.targetTransform = gameObject.transform;
            characterDirection.overrideAnimatorForwardTransform = null;
            characterDirection.rootMotionAccumulator = null;
            characterDirection.modelAnimator = model.GetComponentInChildren<Animator>();
            characterDirection.driveFromRootRotation = false;
            characterDirection.turnSpeed = 720f;

            CharacterBody bodyComponent = characterPrefab.GetComponent<CharacterBody>();
            bodyComponent.bodyIndex = -1;
            bodyComponent.name = "RobPaladinBody";
            bodyComponent.baseNameToken = "PALADIN_NAME";
            bodyComponent.subtitleNameToken = "PALADIN_SUBTITLE";
            bodyComponent.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;
            bodyComponent.rootMotionInMainState = false;
            bodyComponent.mainRootSpeed = 0;
            bodyComponent.baseMaxHealth = 160;
            bodyComponent.levelMaxHealth = 64;
            bodyComponent.baseRegen = 1.5f;
            bodyComponent.levelRegen = 0.5f;
            bodyComponent.baseMaxShield = 0;
            bodyComponent.levelMaxShield = 0;
            bodyComponent.baseMoveSpeed = 7;
            bodyComponent.levelMoveSpeed = 0;
            bodyComponent.baseAcceleration = 80;
            bodyComponent.baseJumpPower = 15;
            bodyComponent.levelJumpPower = 0;
            bodyComponent.baseDamage = StaticValues.baseDamage;
            bodyComponent.levelDamage = StaticValues.baseDamagePerLevel;
            bodyComponent.baseAttackSpeed = 1;
            bodyComponent.levelAttackSpeed = 0;
            bodyComponent.baseCrit = 1;
            bodyComponent.levelCrit = 0;
            bodyComponent.baseArmor = 10;
            bodyComponent.levelArmor = StaticValues.armorPerLevel;
            bodyComponent.baseJumpCount = 1;
            bodyComponent.sprintingSpeedMultiplier = 1.45f;
            bodyComponent.wasLucky = false;
            bodyComponent.hideCrosshair = false;
            bodyComponent.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/SimpleDotCrosshair");
            bodyComponent.aimOriginTransform = gameObject3.transform;
            bodyComponent.hullClassification = HullClassification.Human;
            bodyComponent.portraitIcon = Modules.Assets.charPortrait;
            bodyComponent.isChampion = false;
            bodyComponent.currentVehicle = null;
            bodyComponent.skinIndex = 0U;
            bodyComponent.preferredPodPrefab = null;

            LoadoutAPI.AddSkill(typeof(States.PaladinMain));
            LoadoutAPI.AddSkill(typeof(States.SpawnState));
            LoadoutAPI.AddSkill(typeof(States.Emotes.BaseEmote));
            LoadoutAPI.AddSkill(typeof(States.Emotes.PraiseTheSun));
            LoadoutAPI.AddSkill(typeof(States.Emotes.PointDown));
            LoadoutAPI.AddSkill(typeof(States.Emotes.Rest));
            LoadoutAPI.AddSkill(typeof(States.Emotes.Drip));

            var stateMachine = bodyComponent.GetComponent<EntityStateMachine>();
            stateMachine.mainStateType = new SerializableEntityStateType(typeof(States.PaladinMain));
            stateMachine.initialStateType = new SerializableEntityStateType(typeof(States.SpawnState));

            CharacterMotor characterMotor = characterPrefab.GetComponent<CharacterMotor>();
            characterMotor.walkSpeedPenaltyCoefficient = 1f;
            characterMotor.characterDirection = characterDirection;
            characterMotor.muteWalkMotion = false;
            characterMotor.mass = 100f;
            characterMotor.airControl = 0.25f;
            characterMotor.disableAirControlUntilCollision = false;
            characterMotor.generateParametersOnAwake = true;

            CameraTargetParams cameraTargetParams = characterPrefab.GetComponent<CameraTargetParams>();
            cameraTargetParams.cameraParams = Resources.Load<GameObject>("Prefabs/CharacterBodies/MercBody").GetComponent<CameraTargetParams>().cameraParams;
            cameraTargetParams.cameraPivotTransform = null;
            cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;
            cameraTargetParams.recoil = Vector2.zero;
            cameraTargetParams.idealLocalCameraPos = Vector3.zero;
            cameraTargetParams.dontRaycastToPivot = false;

            ModelLocator modelLocator = characterPrefab.GetComponent<ModelLocator>();
            modelLocator.modelTransform = transform;
            modelLocator.modelBaseTransform = gameObject.transform;

            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            CharacterModel characterModel = model.AddComponent<CharacterModel>();
            characterModel.body = bodyComponent;
            characterModel.baseRendererInfos = new CharacterModel.RendererInfo[]
            {
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = childLocator.FindChild("SwordModel").GetComponent<SkinnedMeshRenderer>().material,
                    renderer = childLocator.FindChild("SwordModel").GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                },
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = childLocator.FindChild("Model").GetComponent<SkinnedMeshRenderer>().material,
                    renderer = childLocator.FindChild("Model").GetComponent<SkinnedMeshRenderer>(),
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                }
            };
            characterModel.autoPopulateLightInfos = true;
            characterModel.invisibilityCount = 0;
            characterModel.temporaryOverlays = new List<TemporaryOverlay>();

            //replace shader
            characterModel.baseRendererInfos[0].defaultMaterial = Modules.Skins.CreateMaterial("matPaladin", StaticValues.maxSwordGlow, Color.white, 0);
            characterModel.baseRendererInfos[1].defaultMaterial = Modules.Skins.CreateMaterial("matPaladin", 10, Color.white, 0);

            characterModel.SetFieldValue("mainSkinnedMeshRenderer", characterModel.baseRendererInfos[1].renderer.gameObject.GetComponent<SkinnedMeshRenderer>());

            TeamComponent teamComponent = null;
            if (characterPrefab.GetComponent<TeamComponent>() != null) teamComponent = characterPrefab.GetComponent<TeamComponent>();
            else teamComponent = characterPrefab.GetComponent<TeamComponent>();
            teamComponent.hideAllyCardDisplay = false;
            teamComponent.teamIndex = TeamIndex.None;

            HealthComponent healthComponent = characterPrefab.GetComponent<HealthComponent>();
            healthComponent.shield = 0f;
            healthComponent.barrier = 0f;
            healthComponent.magnetiCharge = 0f;
            healthComponent.body = null;
            healthComponent.dontShowHealthbar = false;
            healthComponent.globalDeathEventChanceCoefficient = 1f;

            characterPrefab.GetComponent<Interactor>().maxInteractionDistance = 3f;
            characterPrefab.GetComponent<InteractionDriver>().highlightInteractor = true;

            CharacterDeathBehavior characterDeathBehavior = characterPrefab.GetComponent<CharacterDeathBehavior>();
            characterDeathBehavior.deathStateMachine = characterPrefab.GetComponent<EntityStateMachine>();
            //characterDeathBehavior.deathState = new SerializableEntityStateType(typeof(GenericCharacterDeath));

            SfxLocator sfxLocator = characterPrefab.GetComponent<SfxLocator>();
            //sfxLocator.deathSound = Sounds.DeathSound;
            sfxLocator.barkSound = "";
            sfxLocator.openSound = "";
            sfxLocator.landingSound = "Play_char_land";
            sfxLocator.fallDamageSound = "Play_char_land_fall_damage";
            sfxLocator.aliveLoopStart = "";
            sfxLocator.aliveLoopStop = "";

            Rigidbody rigidbody = characterPrefab.GetComponent<Rigidbody>();
            rigidbody.mass = 100f;
            rigidbody.drag = 0f;
            rigidbody.angularDrag = 0f;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            rigidbody.interpolation = RigidbodyInterpolation.None;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.constraints = RigidbodyConstraints.None;

            CapsuleCollider capsuleCollider = characterPrefab.GetComponent<CapsuleCollider>();
            capsuleCollider.isTrigger = false;
            capsuleCollider.material = null;
            capsuleCollider.center = new Vector3(0f, 0f, 0f);
            capsuleCollider.radius = 0.5f;
            capsuleCollider.height = 1.82f;
            capsuleCollider.direction = 1;

            KinematicCharacterMotor kinematicCharacterMotor = characterPrefab.GetComponent<KinematicCharacterMotor>();
            kinematicCharacterMotor.CharacterController = characterMotor;
            kinematicCharacterMotor.Capsule = capsuleCollider;
            kinematicCharacterMotor.Rigidbody = rigidbody;

            kinematicCharacterMotor.DetectDiscreteCollisions = false;
            kinematicCharacterMotor.GroundDetectionExtraDistance = 0f;
            kinematicCharacterMotor.MaxStepHeight = 0.2f;
            kinematicCharacterMotor.MinRequiredStepDepth = 0.1f;
            kinematicCharacterMotor.MaxStableSlopeAngle = 55f;
            kinematicCharacterMotor.MaxStableDistanceFromLedge = 0.5f;
            kinematicCharacterMotor.PreventSnappingOnLedges = false;
            kinematicCharacterMotor.MaxStableDenivelationAngle = 55f;
            kinematicCharacterMotor.RigidbodyInteractionType = RigidbodyInteractionType.None;
            kinematicCharacterMotor.PreserveAttachedRigidbodyMomentum = true;
            kinematicCharacterMotor.HasPlanarConstraint = false;
            kinematicCharacterMotor.PlanarConstraintAxis = Vector3.up;
            kinematicCharacterMotor.StepHandling = StepHandlingMethod.None;
            kinematicCharacterMotor.LedgeHandling = true;
            kinematicCharacterMotor.InteractiveRigidbodyHandling = true;
            kinematicCharacterMotor.SafeMovement = false;

            HurtBoxGroup hurtBoxGroup = model.AddComponent<HurtBoxGroup>();

            HurtBox mainHurtbox = childLocator.FindChild("MainHurtbox").gameObject.AddComponent<HurtBox>();
            mainHurtbox.gameObject.layer = LayerIndex.entityPrecise.intVal;
            mainHurtbox.healthComponent = healthComponent;
            mainHurtbox.isBullseye = true;
            mainHurtbox.damageModifier = HurtBox.DamageModifier.Normal;
            mainHurtbox.hurtBoxGroup = hurtBoxGroup;
            mainHurtbox.indexInGroup = 0;

            hurtBoxGroup.hurtBoxes = new HurtBox[]
            {
                mainHurtbox
            };

            hurtBoxGroup.mainHurtBox = mainHurtbox;
            hurtBoxGroup.bullseyeCount = 1;

            Modules.Helpers.CreateHitbox(model, childLocator.FindChild("SwordHitbox"), "Sword");
            Modules.Helpers.CreateHitbox(model, childLocator.FindChild("LeapHitbox"), "LeapStrike");
            Modules.Helpers.CreateHitbox(model, childLocator.FindChild("SpinSlashHitbox"), "SpinSlash");
            Modules.Helpers.CreateHitbox(model, childLocator.FindChild("SpinSlashLargeHitbox"), "SpinSlashLarge");

            FootstepHandler footstepHandler = model.AddComponent<FootstepHandler>();
            footstepHandler.baseFootstepString = "Play_player_footstep";
            footstepHandler.sprintFootstepOverrideString = "";
            footstepHandler.enableFootstepDust = true;
            footstepHandler.footstepDustPrefab = Resources.Load<GameObject>("Prefabs/GenericFootstepDust");

            RagdollController ragdollController = model.GetComponent<RagdollController>();

            PhysicMaterial physicMat = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<RagdollController>().bones[1].GetComponent<Collider>().material;

            foreach (Transform i in ragdollController.bones)
            {
                if (i)
                {
                    i.gameObject.layer = LayerIndex.ragdoll.intVal;
                    Collider j = i.GetComponent<Collider>();
                    if (j)
                    {
                        j.material = physicMat;
                        j.sharedMaterial = physicMat;
                    }
                }
            }

            AimAnimator aimAnimator = model.AddComponent<AimAnimator>();
            aimAnimator.directionComponent = characterDirection;
            aimAnimator.pitchRangeMax = 60f;
            aimAnimator.pitchRangeMin = -60f;
            aimAnimator.yawRangeMin = -80f;
            aimAnimator.yawRangeMax = 80f;
            aimAnimator.pitchGiveupRange = 30f;
            aimAnimator.yawGiveupRange = 10f;
            aimAnimator.giveupDuration = 3f;
            aimAnimator.inputBank = characterPrefab.GetComponent<InputBankTest>();

            characterPrefab.AddComponent<Misc.PaladinSwordController>();
        }

        private void RegisterCharacter()
        {
            characterDisplay.AddComponent<NetworkIdentity>();

            string unlockString = "PALADIN_UNLOCKABLE_REWARD_ID";
            if (Modules.Config.forceUnlock.Value) unlockString = "";

            SurvivorDef survivorDef = new SurvivorDef
            {
                name = "PALADIN_NAME",
                unlockableName = unlockString,
                descriptionToken = "PALADIN_DESCRIPTION",
                primaryColor = characterColor,
                bodyPrefab = characterPrefab,
                displayPrefab = characterDisplay,
                outroFlavorToken = "PALADIN_OUTRO_FLAVOR"
            };

            SurvivorAPI.AddSurvivor(survivorDef);

            SkillSetup();

            BodyCatalog.getAdditionalEntries += delegate (List<GameObject> list)
            {
                list.Add(characterPrefab);
            };

            characterPrefab.tag = "Player";
        }

        private void CreateDoppelganger()
        {
            doppelganger = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterMasters/MercMonsterMaster"), "PaladinMonsterMaster");

            MasterCatalog.getAdditionalEntries += delegate (List<GameObject> list)
            {
                list.Add(doppelganger);
            };

            CharacterMaster component = doppelganger.GetComponent<CharacterMaster>();
            component.bodyPrefab = characterPrefab;
        }

        private void SkillSetup()
        {
            foreach (GenericSkill obj in characterPrefab.GetComponentsInChildren<GenericSkill>())
            {
                BaseUnityPlugin.DestroyImmediate(obj);
            }

            skillLocator = characterPrefab.GetComponent<SkillLocator>();

            PassiveSetup();
            PrimarySetup();
            SecondarySetup();
            UtilitySetup();
            SpecialSetup();
        }

        private void PassiveSetup()
        {
            skillLocator.passiveSkill.enabled = true;
            skillLocator.passiveSkill.skillNameToken = "PALADIN_PASSIVE_NAME";
            skillLocator.passiveSkill.skillDescriptionToken = "PALADIN_PASSIVE_DESCRIPTION";
            skillLocator.passiveSkill.icon = Modules.Assets.iconP;
        }

        private void PrimarySetup()
        {
            LoadoutAPI.AddSkill(typeof(States.Slash));

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.Slash));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Modules.Assets.icon1;
            mySkillDef.skillDescriptionToken = "PALADIN_PRIMARY_SLASH_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_PRIMARY_SLASH_NAME";
            mySkillDef.skillNameToken = "PALADIN_PRIMARY_SLASH_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_SWORDBEAM"
            };

            LoadoutAPI.AddSkillDef(mySkillDef);

            skillLocator.primary = characterPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamily);
            skillLocator.primary.SetFieldValue("_skillFamily", newFamily);
            SkillFamily skillFamily = skillLocator.primary.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }

        private void SecondarySetup()
        {
            LoadoutAPI.AddSkill(typeof(States.SpinSlashEntry));
            LoadoutAPI.AddSkill(typeof(States.GroundSweep));
            LoadoutAPI.AddSkill(typeof(States.AirSlam));
            LoadoutAPI.AddSkill(typeof(States.AirSlamAlt));

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.SpinSlashEntry));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 6f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Modules.Assets.icon2;
            mySkillDef.skillDescriptionToken = "PALADIN_SECONDARY_SPINSLASH_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_SECONDARY_SPINSLASH_NAME";
            mySkillDef.skillNameToken = "PALADIN_SECONDARY_SPINSLASH_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_STUNNING",
            };

            LoadoutAPI.AddSkillDef(mySkillDef);

            skillLocator.secondary = characterPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamily);
            skillLocator.secondary.SetFieldValue("_skillFamily", newFamily);
            SkillFamily skillFamily = skillLocator.secondary.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            LoadoutAPI.AddSkill(typeof(States.ChargeLightningSpear));
            LoadoutAPI.AddSkill(typeof(States.ThrowLightningSpear));

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.ChargeLightningSpear));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 6f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Modules.Assets.icon2b;
            mySkillDef.skillDescriptionToken = "PALADIN_SECONDARY_LIGHTNING_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_SECONDARY_LIGHTNING_NAME";
            mySkillDef.skillNameToken = "PALADIN_SECONDARY_LIGHTNING_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_SHOCKING",
                "KEYWORD_AGILE"
            };

            LoadoutAPI.AddSkillDef(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",//PALADIN_LIGHTNINGSPEARUNLOCKABLE_REWARD_ID
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            LoadoutAPI.AddSkill(typeof(States.LunarShards));

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.LunarShards));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = StaticValues.lunarShardMaxStock;
            mySkillDef.baseRechargeInterval = 0.75f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Modules.Assets.icon2c;
            mySkillDef.skillDescriptionToken = "PALADIN_SECONDARY_LUNARSHARD_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_SECONDARY_LUNARSHARD_NAME";
            mySkillDef.skillNameToken = "PALADIN_SECONDARY_LUNARSHARD_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_AGILE"
            };

            LoadoutAPI.AddSkillDef(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "PALADIN_LUNARSHARDUNLOCKABLE_REWARD_ID",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }

        private void UtilitySetup()
        {
            //LoadoutAPI.AddSkill(typeof(States.Dash.DashState));
            //LoadoutAPI.AddSkill(typeof(States.Dash.AirDash));
            //LoadoutAPI.AddSkill(typeof(States.Dash.GroundDash));

            //LoadoutAPI.AddSkill(typeof(States.Quickstep.BaseQuickstepState));
            //LoadoutAPI.AddSkill(typeof(States.Quickstep.QuickstepEntry));
            //LoadoutAPI.AddSkill(typeof(States.Quickstep.QuickstepForward));
            //LoadoutAPI.AddSkill(typeof(States.Quickstep.QuickstepBack));
            //LoadoutAPI.AddSkill(typeof(States.Quickstep.QuickstepLeft));
            //LoadoutAPI.AddSkill(typeof(States.Quickstep.QuickstepRight));

            LoadoutAPI.AddSkill(typeof(States.Quickstep.QuickstepSimple));

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.Quickstep.QuickstepSimple));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 2;
            mySkillDef.baseRechargeInterval = 10f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = false;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = false;
            mySkillDef.forceSprintDuringState = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Modules.Assets.icon3;
            mySkillDef.skillDescriptionToken = "PALADIN_UTILITY_DASH_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_UTILITY_DASH_NAME";
            mySkillDef.skillNameToken = "PALADIN_UTILITY_DASH_NAME";
            //mySkillDef.keywordTokens = new string[] {
            //    "KEYWORD_SHOCKING"
            //};

            LoadoutAPI.AddSkillDef(mySkillDef);

            skillLocator.utility = characterPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamily);
            skillLocator.utility.SetFieldValue("_skillFamily", newFamily);
            SkillFamily skillFamily = skillLocator.utility.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            /*LoadoutAPI.AddSkill(typeof(States.ChargeBolt));
            LoadoutAPI.AddSkill(typeof(States.ThrowBolt));

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.ChargeBolt));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 3;
            mySkillDef.baseRechargeInterval = 5;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Modules.Assets.icon3b;
            mySkillDef.skillDescriptionToken = "PALADIN_UTILITY_LIGHTNINGBOLT_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_UTILITY_LIGHTNINGBOLT_NAME";
            mySkillDef.skillNameToken = "PALADIN_UTILITY_LIGHTNINGBOLT_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_SHOCKING"
            };

            LoadoutAPI.AddSkillDef(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };*/

            LoadoutAPI.AddSkill(typeof(States.AimHeal));
            LoadoutAPI.AddSkill(typeof(States.CastHeal));

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.AimHeal));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 8;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Modules.Assets.icon3b;
            mySkillDef.skillDescriptionToken = "PALADIN_UTILITY_HEAL_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_UTILITY_HEAL_NAME";
            mySkillDef.skillNameToken = "PALADIN_UTILITY_HEAL_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "PALADIN_HEALUNLOCKABLE_REWARD_ID",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }

        private void SpecialSetup()
        {
            //LoadoutAPI.AddSkill(typeof(States.AimHealZone));
            //LoadoutAPI.AddSkill(typeof(States.CastHealZone));
            LoadoutAPI.AddSkill(typeof(States.Spell.ChannelHealZone));
            LoadoutAPI.AddSkill(typeof(States.Spell.CastChanneledHealZone));

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.Spell.ChannelHealZone));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 18f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = true;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Modules.Assets.icon4;
            mySkillDef.skillDescriptionToken = "PALADIN_SPECIAL_HEALZONE_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_SPECIAL_HEALZONE_NAME";
            mySkillDef.skillNameToken = "PALADIN_SPECIAL_HEALZONE_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            skillLocator.special = characterPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamily);
            skillLocator.special.SetFieldValue("_skillFamily", newFamily);
            SkillFamily skillFamily = skillLocator.special.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            //LoadoutAPI.AddSkill(typeof(States.AimTorpor));
            //LoadoutAPI.AddSkill(typeof(States.CastTorpor));
            LoadoutAPI.AddSkill(typeof(States.Spell.ChannelTorpor));
            LoadoutAPI.AddSkill(typeof(States.Spell.CastChanneledTorpor));

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.Spell.ChannelTorpor));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 18f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = true;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Modules.Assets.icon4b;
            mySkillDef.skillDescriptionToken = "PALADIN_SPECIAL_TORPOR_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_SPECIAL_TORPOR_NAME";
            mySkillDef.skillNameToken = "PALADIN_SPECIAL_TORPOR_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_TORPOR"
            };

            LoadoutAPI.AddSkillDef(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "PALADIN_TORPORUNLOCKABLE_REWARD_ID",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            LoadoutAPI.AddSkill(typeof(States.Spell.ChannelWarcry));
            LoadoutAPI.AddSkill(typeof(States.Spell.CastChanneledWarcry));

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.Spell.ChannelWarcry));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 18f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = true;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Modules.Assets.icon4c;
            mySkillDef.skillDescriptionToken = "PALADIN_SPECIAL_WARCRY_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_SPECIAL_WARCRY_NAME";
            mySkillDef.skillNameToken = "PALADIN_SPECIAL_WARCRY_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }

        private void ScepterSkillSetup()
        {
            LoadoutAPI.AddSkill(typeof(States.Spell.ScepterChannelHealZone));
            LoadoutAPI.AddSkill(typeof(States.Spell.ScepterCastHealZone));

            scepterHealDef = ScriptableObject.CreateInstance<SkillDef>();
            scepterHealDef.activationState = new SerializableEntityStateType(typeof(States.Spell.ScepterChannelHealZone));
            scepterHealDef.activationStateMachineName = "Weapon";
            scepterHealDef.baseMaxStock = 1;
            scepterHealDef.baseRechargeInterval = 18f;
            scepterHealDef.beginSkillCooldownOnSkillEnd = true;
            scepterHealDef.canceledFromSprinting = true;
            scepterHealDef.fullRestockOnAssign = true;
            scepterHealDef.interruptPriority = InterruptPriority.Skill;
            scepterHealDef.isBullets = false;
            scepterHealDef.isCombatSkill = true;
            scepterHealDef.mustKeyPress = false;
            scepterHealDef.noSprint = true;
            scepterHealDef.rechargeStock = 1;
            scepterHealDef.requiredStock = 1;
            scepterHealDef.shootDelay = 0.5f;
            scepterHealDef.stockToConsume = 1;
            scepterHealDef.icon = Modules.Assets.icon4S;
            scepterHealDef.skillDescriptionToken = "PALADIN_SPECIAL_SCEPTERHEALZONE_DESCRIPTION";
            scepterHealDef.skillName = "PALADIN_SPECIAL_SCEPTERHEALZONE_NAME";
            scepterHealDef.skillNameToken = "PALADIN_SPECIAL_SCEPTERHEALZONE_NAME";

            LoadoutAPI.AddSkillDef(scepterHealDef);

            LoadoutAPI.AddSkill(typeof(States.Spell.ScepterChannelTorpor));
            LoadoutAPI.AddSkill(typeof(States.Spell.ScepterCastTorpor));

            scepterTorporDef = ScriptableObject.CreateInstance<SkillDef>();
            scepterTorporDef.activationState = new SerializableEntityStateType(typeof(States.Spell.ScepterChannelTorpor));
            scepterTorporDef.activationStateMachineName = "Weapon";
            scepterTorporDef.baseMaxStock = 1;
            scepterTorporDef.baseRechargeInterval = 18f;
            scepterTorporDef.beginSkillCooldownOnSkillEnd = true;
            scepterTorporDef.canceledFromSprinting = true;
            scepterTorporDef.fullRestockOnAssign = true;
            scepterTorporDef.interruptPriority = InterruptPriority.Skill;
            scepterTorporDef.isBullets = false;
            scepterTorporDef.isCombatSkill = true;
            scepterTorporDef.mustKeyPress = false;
            scepterTorporDef.noSprint = true;
            scepterTorporDef.rechargeStock = 1;
            scepterTorporDef.requiredStock = 1;
            scepterTorporDef.shootDelay = 0.5f;
            scepterTorporDef.stockToConsume = 1;
            scepterTorporDef.icon = Modules.Assets.icon4bS;
            scepterTorporDef.skillDescriptionToken = "PALADIN_SPECIAL_SCEPTERTORPOR_DESCRIPTION";
            scepterTorporDef.skillName = "PALADIN_SPECIAL_SCEPTERTORPOR_NAME";
            scepterTorporDef.skillNameToken = "PALADIN_SPECIAL_SCEPTERTORPOR_NAME";
            scepterTorporDef.keywordTokens = new string[] {
                "KEYWORD_TORPOR"
            };

            LoadoutAPI.AddSkillDef(scepterTorporDef);

            LoadoutAPI.AddSkill(typeof(States.Spell.ScepterChannelWarcry));
            LoadoutAPI.AddSkill(typeof(States.Spell.ScepterCastWarcry));

            scepterWarcryDef = ScriptableObject.CreateInstance<SkillDef>();
            scepterWarcryDef.activationState = new SerializableEntityStateType(typeof(States.Spell.ScepterChannelWarcry));
            scepterWarcryDef.activationStateMachineName = "Weapon";
            scepterWarcryDef.baseMaxStock = 1;
            scepterWarcryDef.baseRechargeInterval = 18f;
            scepterWarcryDef.beginSkillCooldownOnSkillEnd = true;
            scepterWarcryDef.canceledFromSprinting = true;
            scepterWarcryDef.fullRestockOnAssign = true;
            scepterWarcryDef.interruptPriority = InterruptPriority.Skill;
            scepterWarcryDef.isBullets = false;
            scepterWarcryDef.isCombatSkill = true;
            scepterWarcryDef.mustKeyPress = false;
            scepterWarcryDef.noSprint = true;
            scepterWarcryDef.rechargeStock = 1;
            scepterWarcryDef.requiredStock = 1;
            scepterWarcryDef.shootDelay = 0.5f;
            scepterWarcryDef.stockToConsume = 1;
            scepterWarcryDef.icon = Modules.Assets.icon4cS;
            scepterWarcryDef.skillDescriptionToken = "PALADIN_SPECIAL_SCEPTERWARCRY_DESCRIPTION";
            scepterWarcryDef.skillName = "PALADIN_SPECIAL_SCEPTERWARCRY_NAME";
            scepterWarcryDef.skillNameToken = "PALADIN_SPECIAL_SCEPTERWARCRY_NAME";

            LoadoutAPI.AddSkillDef(scepterWarcryDef);
        }
    }
}