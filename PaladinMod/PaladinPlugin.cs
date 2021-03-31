using System;
using System.Collections.Generic;
using BepInEx;
using R2API;
using R2API.Utils;
using EntityStates;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace PaladinMod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.DestroyedClone.AncientScepter", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.KomradeSpectre.Aetherium", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.Sivelos.SivsItems", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.K1454.SupplyDrop", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.TeamMoonstorm.Starstorm2", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, "Paladin", "1.4.2")]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
    })]

    public class PaladinPlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.rob.Paladin";

        public static PaladinPlugin instance;

        public static GameObject characterPrefab;

        public GameObject doppelganger;

        // clone this material to make our own with proper shader/properties
        public static Material commandoMat;

        public static event Action awake;
        public static event Action start;

        public static readonly Color characterColor = new Color(0.7176f, 0.098039f, 0.098039f);

        // for scepter upgrades
        public static SkillDef scepterHealDef;
        public static SkillDef scepterTorporDef;
        public static SkillDef scepterWarcryDef;

        // for modded item display rules
        public static bool aetheriumInstalled = false;
        public static bool sivsItemsInstalled = false;
        public static bool supplyDropInstalled = false;

        // ss2 compat
        public static bool starstormInstalled = false;

        // eh
        public static uint claySkinIndex = 3;

        public PaladinPlugin()
        {
            awake += PaladinPlugin_Load;
            start += PaladinPlugin_LoadStart;
        }

        private void PaladinPlugin_Load()
        {
            instance = this;

            // load assets and read config
            Modules.Assets.PopulateAssets();
            Modules.Config.ReadConfig();

            // modded item displays
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.KomradeSpectre.Aetherium")) aetheriumInstalled = true;
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Sivelos.SivsItems")) sivsItemsInstalled = true;
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.K1454.SupplyDrop")) supplyDropInstalled = true;

            // ss2
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.TeamMoonstorm.Starstorm2"))
            {
                starstormInstalled = true;
                claySkinIndex++;
            }

            Modules.Unlockables.RegisterUnlockables(); // add unlockables
            Modules.States.RegisterStates(); // register states

            Modules.Prefabs.CreatePrefabs(); // create body and display prefabs
            characterPrefab = Modules.Prefabs.paladinPrefab; // cache this for other mods to use it
            Modules.ItemDisplays.InitializeItemDisplays(); // add item displays(pain)
            Modules.Skills.SetupSkills(Modules.Prefabs.paladinPrefab);
            Modules.Skills.SetupSkills(Modules.Prefabs.lunarKnightPrefab);
            Modules.Survivors.RegisterSurvivors(); // register them into the body catalog
            Modules.Skins.RegisterSkins(); // add skins
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Effects.RegisterEffects(); // add and register custom effects
            Modules.Tokens.AddTokens(); // register name tokens

            CreateDoppelganger(); // artifact of vengeance

            //scepter upgrades
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter"))
            {
                //ScepterSkillSetup();
                //ScepterSetup();
            }

            new Modules.ContentPacks().CreateContentPack();

            Hook();
        }

        private void PaladinPlugin_LoadStart()
        {
            Modules.Projectiles.LateSetup();
            Modules.ItemDisplays.SetItemDisplays();
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
            //ThinkInvisible.ClassicItems.Scepter_V2.instance.RegisterScepterSkill(scepterHealDef, "RobPaladinBody", SkillSlot.Special, 0);
            //ThinkInvisible.ClassicItems.Scepter_V2.instance.RegisterScepterSkill(scepterTorporDef, "RobPaladinBody", SkillSlot.Special, 1);
            //ThinkInvisible.ClassicItems.Scepter_V2.instance.RegisterScepterSkill(scepterWarcryDef, "RobPaladinBody", SkillSlot.Special, 2);
        }

        private void Hook()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.CharacterModel.UpdateOverlays += CharacterModel_UpdateOverlays;
            On.RoR2.CharacterMaster.OnInventoryChanged += CharacterMaster_OnInventoryChanged;
            //On.RoR2.SceneDirector.Start += SceneDirector_Start;
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
                    self.attackSpeed += StaticValues.warcryAttackSpeedBuff;
                }

                if (self.HasBuff(Modules.Buffs.scepterWarcryBuff))
                {
                    float damageBuff = StaticValues.scepterWarcryDamageMultiplier * self.damage;
                    self.damage += damageBuff;
                    self.attackSpeed += StaticValues.scepterWarcryAttackSpeedBuff;
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

        private void CharacterMaster_OnInventoryChanged(On.RoR2.CharacterMaster.orig_OnInventoryChanged orig, CharacterMaster self)
        {
            orig(self);

            if (self.hasBody)
            {
                if (self.GetBody().baseNameToken == "PALADIN_NAME")
                {
                    Misc.PaladinSwordController swordComponent = self.GetBody().GetComponent<Misc.PaladinSwordController>();
                    if (swordComponent)
                    {
                        swordComponent.CheckInventory();
                    }
                }
            }
        }

        private void SceneDirector_Start(On.RoR2.SceneDirector.orig_Start orig, SceneDirector self)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "arena")
            {
                //check if any paladin has beads of fealty
                bool hasBeads = false;
                for (int i = 0; i < CharacterMaster.readOnlyInstancesList.Count; i++)
                {
                    if (CharacterMaster.readOnlyInstancesList[i].bodyPrefab.GetComponent<CharacterBody>().baseNameToken == "PALADIN_NAME" &&  CharacterMaster.readOnlyInstancesList[i].inventory.GetItemCount(RoR2Content.Items.LunarTrinket) > 0)
                    {
                        hasBeads = true;
                        break;
                    }
                }
                
                if (hasBeads)
                {
                    //destroy environment
                    GameObject.Destroy(GameObject.Find("HOLDER: Nullifier Props").gameObject);
                    GameObject.Destroy(GameObject.Find("HOLDER: Misc Props").gameObject);
                    GameObject.Destroy(GameObject.Find("HOLDER: Terrain").gameObject);
                    GameObject.Destroy(GameObject.Find("HOLDER: Ruins").gameObject);
                    GameObject.Destroy(GameObject.Find("HOLDER: Small Trees").gameObject);
                    GameObject.Destroy(GameObject.Find("HOLDER: Artifact Formula").gameObject);
                    GameObject.Destroy(GameObject.Find("HOLDER: Rocks").gameObject);
                    //GameObject.Destroy(GameObject.Find("FOLIAGE: Skybox Kelp Trees").gameObject);
                    GameObject.Destroy(GameObject.Find("ArenaBoulderFix").gameObject);
                    GameObject.Destroy(GameObject.Find("PortalArena").gameObject);
                    GameObject.Find("ArenaMissionController").transform.Find("PPSick").gameObject.SetActive(false);

                    GameObject.Destroy(GameObject.Find("ArenaMissionController").transform.Find("NullSafeZone (1)").gameObject);
                    GameObject.Destroy(GameObject.Find("ArenaMissionController").transform.Find("NullSafeZone (2)").gameObject);
                    GameObject.Destroy(GameObject.Find("ArenaMissionController").transform.Find("NullSafeZone (3)").gameObject);
                    GameObject.Destroy(GameObject.Find("ArenaMissionController").transform.Find("NullSafeZone (4)").gameObject);
                    GameObject.Destroy(GameObject.Find("ArenaMissionController").transform.Find("NullSafeZone (5)").gameObject);
                    GameObject.Destroy(GameObject.Find("ArenaMissionController").transform.Find("NullSafeZone (6)").gameObject);
                    GameObject.Destroy(GameObject.Find("ArenaMissionController").transform.Find("NullSafeZone (7)").gameObject);
                    GameObject.Destroy(GameObject.Find("ArenaMissionController").transform.Find("NullSafeZone (8)").gameObject);
                    GameObject.Destroy(GameObject.Find("ArenaMissionController").transform.Find("NullSafeZone (9)").gameObject);

                    GameObject.Find("HOLDER: Ruins and Gameplay Zones").SetActive(true);
                    GameObject.Find("HOLDER: Ruins and Gameplay Zones").transform.Find("ShrineCombat").gameObject.SetActive(true);
                    GameObject.Find("HOLDER: Ruins and Gameplay Zones").transform.Find("WPPlatform2").gameObject.SetActive(true);
                    GameObject.Find("HOLDER: Ruins and Gameplay Zones").transform.Find("WPPlatform2").Find("WPPlatform2Barrier").gameObject.SetActive(true);
                    GameObject.Find("HOLDER: Ruins and Gameplay Zones").transform.Find("WPPlatform2").Find("WPPlatform2Barrier (1)").gameObject.SetActive(true);
                    GameObject.Find("HOLDER: Ruins and Gameplay Zones").transform.Find("WPPlatform2").Find("WPPlatform2Barrier (2)").gameObject.SetActive(true);
                    GameObject.Find("HOLDER: Ruins and Gameplay Zones").transform.Find("WPPlatform2").Find("WPPlatform2Barrier (3)").gameObject.SetActive(true);
                    GameObject.Find("HOLDER: Ruins and Gameplay Zones").transform.Find("WPPlatform2").Find("WPPlatform2Barrier (4)").gameObject.SetActive(true);
                    GameObject.Find("HOLDER: Ruins and Gameplay Zones").transform.Find("WPPlatform2").Find("WPPlatform2Barrier (5)").gameObject.SetActive(true);
                    GameObject.Find("HOLDER: Ruins and Gameplay Zones").transform.Find("WPPlatform2").Find("WPPlatform2Barrier (6)").gameObject.SetActive(true);
                    GameObject.Find("HOLDER: Ruins and Gameplay Zones").transform.Find("WPPlatform2").Find("WPPlatform2Barrier (7)").gameObject.SetActive(true);
                    GameObject.Find("HOLDER: Ruins and Gameplay Zones").transform.Find("WPPlatform2").Find("ArenaBarrier").Find("ArenaBarrierCenter").gameObject.SetActive(true);
                    GameObject.Find("HOLDER: Ruins and Gameplay Zones").transform.Find("WPPlatform2").Find("ArenaBarrier (1)").Find("ArenaBarrierCenter").gameObject.SetActive(true);
                    GameObject.Find("HOLDER: Ruins and Gameplay Zones").transform.Find("WPPlatform2").Find("ArenaBarrier (2)").Find("ArenaBarrierCenter").gameObject.SetActive(true);
                    GameObject.Find("HOLDER: Ruins and Gameplay Zones").transform.Find("WPPlatform2").Find("ArenaBarrier (3)").Find("ArenaBarrierCenter").gameObject.SetActive(true);

                    GameObject.Find("HOLDER: Ruins and Gameplay Zones").transform.Find("ShrineCombat").localPosition = new Vector3(-84, 112, -6);
                    GameObject.Find("HOLDER: Ruins and Gameplay Zones").transform.Find("WPPlatform2").localPosition = new Vector3(-84, 84, -6);

                    GameObject.Find("HOLDER: Lighting, Effects, Wind, etc").transform.Find("ArenaSkybox").Find("CameraRelative").Find("Dust").gameObject.SetActive(false);
                    GameObject.Find("HOLDER: Lighting, Effects, Wind, etc").transform.Find("ArenaSkybox").Find("CameraRelative").Find("Weather (Locked Position/Rotation").Find("Embers").gameObject.SetActive(true);
                    GameObject.Find("HOLDER: Lighting, Effects, Wind, etc").transform.Find("ArenaSkybox").Find("CameraRelative").Find("Weather (Locked Position/Rotation").Find("GiantDust?").gameObject.SetActive(true);

                    foreach (SpawnPoint i in FindObjectsOfType<SpawnPoint>())
                    {
                        i.transform.position = new Vector3(-84, 103, 32);
                    };

                    GameObject.Find("ArenaMissionController").GetComponent<ArenaMissionController>().enabled = false;
                }
            }

            orig(self);
        }

        private void CreateDoppelganger()
        {
            doppelganger = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterMasters/MercMonsterMaster"), "PaladinMonsterMaster");
            doppelganger.GetComponent<CharacterMaster>().bodyPrefab = Modules.Prefabs.paladinPrefab;

            Modules.Prefabs.masterPrefabs.Add(doppelganger);
        }

        private void ScepterSkillSetup()
        {
            Modules.States.AddSkill(typeof(States.Spell.ScepterChannelHealZone));
            Modules.States.AddSkill(typeof(States.Spell.ScepterCastHealZone));

            scepterHealDef = ScriptableObject.CreateInstance<SkillDef>();
            scepterHealDef.activationState = new SerializableEntityStateType(typeof(States.Spell.ScepterChannelHealZone));
            scepterHealDef.activationStateMachineName = "Weapon";
            scepterHealDef.baseMaxStock = 1;
            scepterHealDef.baseRechargeInterval = 18f;
            scepterHealDef.beginSkillCooldownOnSkillEnd = true;
            scepterHealDef.canceledFromSprinting = true;
            scepterHealDef.fullRestockOnAssign = true;
            scepterHealDef.interruptPriority = InterruptPriority.Skill;
            scepterHealDef.resetCooldownTimerOnUse = false;
            scepterHealDef.isCombatSkill = true;
            scepterHealDef.mustKeyPress = false;
            scepterHealDef.canceledFromSprinting = true;
            scepterHealDef.rechargeStock = 1;
            scepterHealDef.requiredStock = 1;
            scepterHealDef.stockToConsume = 1;
            scepterHealDef.icon = Modules.Assets.icon4S;
            scepterHealDef.skillDescriptionToken = "PALADIN_SPECIAL_SCEPTERHEALZONE_DESCRIPTION";
            scepterHealDef.skillName = "PALADIN_SPECIAL_SCEPTERHEALZONE_NAME";
            scepterHealDef.skillNameToken = "PALADIN_SPECIAL_SCEPTERHEALZONE_NAME";

            Modules.Skills.skillDefs.Add(scepterHealDef);

            Modules.States.AddSkill(typeof(States.Spell.ScepterChannelTorpor));
            Modules.States.AddSkill(typeof(States.Spell.ScepterCastTorpor));

            scepterTorporDef = ScriptableObject.CreateInstance<SkillDef>();
            scepterTorporDef.activationState = new SerializableEntityStateType(typeof(States.Spell.ScepterChannelTorpor));
            scepterTorporDef.activationStateMachineName = "Weapon";
            scepterTorporDef.baseMaxStock = 1;
            scepterTorporDef.baseRechargeInterval = 18f;
            scepterTorporDef.beginSkillCooldownOnSkillEnd = true;
            scepterTorporDef.canceledFromSprinting = true;
            scepterTorporDef.fullRestockOnAssign = true;
            scepterTorporDef.interruptPriority = InterruptPriority.Skill;
            scepterTorporDef.resetCooldownTimerOnUse = false;
            scepterTorporDef.isCombatSkill = true;
            scepterTorporDef.mustKeyPress = false;
            scepterTorporDef.cancelSprintingOnActivation = true;
            scepterTorporDef.rechargeStock = 1;
            scepterTorporDef.requiredStock = 1;
            scepterTorporDef.stockToConsume = 1;
            scepterTorporDef.icon = Modules.Assets.icon4bS;
            scepterTorporDef.skillDescriptionToken = "PALADIN_SPECIAL_SCEPTERTORPOR_DESCRIPTION";
            scepterTorporDef.skillName = "PALADIN_SPECIAL_SCEPTERTORPOR_NAME";
            scepterTorporDef.skillNameToken = "PALADIN_SPECIAL_SCEPTERTORPOR_NAME";
            scepterTorporDef.keywordTokens = new string[] {
                "KEYWORD_TORPOR"
            };

            Modules.Skills.skillDefs.Add(scepterTorporDef);

            Modules.States.AddSkill(typeof(States.Spell.ScepterChannelWarcry));
            Modules.States.AddSkill(typeof(States.Spell.ScepterCastWarcry));

            scepterWarcryDef = ScriptableObject.CreateInstance<SkillDef>();
            scepterWarcryDef.activationState = new SerializableEntityStateType(typeof(States.Spell.ScepterChannelWarcry));
            scepterWarcryDef.activationStateMachineName = "Weapon";
            scepterWarcryDef.baseMaxStock = 1;
            scepterWarcryDef.baseRechargeInterval = 18f;
            scepterWarcryDef.beginSkillCooldownOnSkillEnd = true;
            scepterWarcryDef.canceledFromSprinting = true;
            scepterWarcryDef.fullRestockOnAssign = true;
            scepterWarcryDef.interruptPriority = InterruptPriority.Skill;
            scepterWarcryDef.resetCooldownTimerOnUse = false;
            scepterWarcryDef.isCombatSkill = true;
            scepterWarcryDef.mustKeyPress = false;
            scepterWarcryDef.cancelSprintingOnActivation = true;
            scepterWarcryDef.rechargeStock = 1;
            scepterWarcryDef.requiredStock = 1;
            scepterWarcryDef.stockToConsume = 1;
            scepterWarcryDef.icon = Modules.Assets.icon4cS;
            scepterWarcryDef.skillDescriptionToken = "PALADIN_SPECIAL_SCEPTERWARCRY_DESCRIPTION";
            scepterWarcryDef.skillName = "PALADIN_SPECIAL_SCEPTERWARCRY_NAME";
            scepterWarcryDef.skillNameToken = "PALADIN_SPECIAL_SCEPTERWARCRY_NAME";

            Modules.Skills.skillDefs.Add(scepterWarcryDef);
        }
    }
}