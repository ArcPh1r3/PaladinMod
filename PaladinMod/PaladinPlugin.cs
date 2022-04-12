using BepInEx;
using R2API;
using R2API.Utils;
using EntityStates;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using RoR2.UI;
using UnityEngine.UI;
using PaladinMod.Misc;
using RoR2.Orbs;
using System.Collections.Generic;
using BepInEx.Logging;

namespace PaladinMod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.DestroyedClone.AncientScepter", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.KomradeSpectre.Aetherium", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.Sivelos.SivsItems", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.K1454.SupplyDrop", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.TeamMoonstorm.Starstorm2", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
    })]                                  //pushed out a version 69 so we gotta put a 7 here my b
    [BepInPlugin(MODUID, "Paladin", "1.5.78")]

    public class PaladinPlugin : BaseUnityPlugin
    {
        //keeping id the same so it versions over previous paladin
        public const string MODUID = "com.rob.Paladin";
         
        public static PaladinPlugin instance;

        public static ManualLogSource logger;

        public static GameObject characterPrefab;

        public GameObject doppelganger;

        // clone this material to make our own with proper shader/properties
        public static Material commandoMat;

        public static readonly Color characterColor = new Color(0.7176f, 0.098039f, 0.098039f); 

        // for scepter upgrades
        public static SkillDef scepterHealDef;
        public static SkillDef scepterTorporDef;
        public static SkillDef scepterWarcryDef;
        public static SkillDef scepterCruelSunDef; 

        // for modded item display rules
        public static bool aetheriumInstalled = false;
        public static bool sivsItemsInstalled = false;
        public static bool supplyDropInstalled = false;
        public static bool ancientScepterInstalled = false;

        // ss2 compat
        public static bool starstormInstalled = false;

        // eh
        public static uint claySkinIndex = 4;

        public void Start() {
            Logger.LogInfo("[Initializing Paladin]");
            //Modules.States.FixStates();
            instance = this;
            logger = base.Logger;

            // load assets and read config
            Modules.Assets.PopulateAssets();
            Modules.Config.ReadConfig();
            Modules.CameraParams.InitializeParams(); // create camera params for our character to use

            // modded item displays
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.KomradeSpectre.Aetherium")) aetheriumInstalled = true;
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Sivelos.SivsItems")) sivsItemsInstalled = true;
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.K1454.SupplyDrop")) supplyDropInstalled = true;

            // ss2
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.TeamMoonstorm.Starstorm2"))
            {
                starstormInstalled = true;
            }

            Modules.Unlockables.RegisterUnlockables(); // add unlockables
            Modules.States.RegisterStates(); // register states

            Modules.Prefabs.CreatePrefabs(); // create body and display prefabs
            characterPrefab = Modules.Prefabs.paladinPrefab; // cache this for other mods to use it
            Modules.ItemDisplays.InitializeItemDisplays(); // add item displays(pain)
            Modules.Skills.SetupSkills(Modules.Prefabs.paladinPrefab);
            Modules.Skills.SetupSkills(Modules.Prefabs.lunarKnightPrefab);
            //Modules.Skills.SetupSkills(Modules.Prefabs.nemPaladinPrefab);
            Modules.Survivors.RegisterSurvivors(); // register them into the body catalog
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Skins.RegisterSkins(); // add skins
            Modules.Effects.RegisterEffects(); // add and register custom effects
            Modules.Tokens.AddTokens(); // register name tokens

            CreateDoppelganger(); // artifact of vengeance

            //scepter upgrades
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter"))
            {
                ancientScepterInstalled = true;
                ScepterSkillSetup();
                ScepterSetup();
            }

            new Modules.ContentPacks().Initialize();

            Hook();

            RoR2.ContentManagement.ContentManager.onContentPacksAssigned += LateSetup;
            RoR2.RoR2Application.onLoad += LateSetupItemDisplays;
            On.RoR2.EntityStateCatalog.Init += EntityStateCatalog_Init;
            Logger.LogInfo("[Initialized]");
        }

        private void LateSetupItemDisplays() {
            Modules.ItemDisplays.SetItemDisplays();
        }

        private void LateSetup(HG.ReadOnlyArray<RoR2.ContentManagement.ReadOnlyContentPack> obj)
        {
            Modules.Projectiles.LateSetup();
        }

        private void EntityStateCatalog_Init(On.RoR2.EntityStateCatalog.orig_Init orig)
        {
            orig();

            // this is some godawful code but i'm not sure how else to do this.

            Material circleMat = UnityEngine.Object.Instantiate(EntityStates.GolemMonster.ChargeLaser.effectPrefab.transform.Find("Particles").Find("Glow").GetComponent<ParticleSystemRenderer>().material);

            ChildLocator childLocator = Modules.Prefabs.paladinPrefab.GetComponentInChildren<ChildLocator>();

            childLocator.FindChild("HealChannelEffect").Find("Orbs").Find("MagicCircle").GetComponent<ParticleSystemRenderer>().material = circleMat;
            childLocator.FindChild("HealChannelEffect").Find("Orbs").Find("MagicCircle").Find("BigCircle").GetComponent<ParticleSystemRenderer>().material = circleMat;
            childLocator.FindChild("HealChannelEffect").Find("Orbs").Find("MagicCircle").Find("MagicCircle2").GetComponent<ParticleSystemRenderer>().material = circleMat;
            childLocator.FindChild("HealChannelEffect").Find("Orbs").Find("MagicCircle").Find("MagicCircle2").Find("BigCircle2").GetComponent<ParticleSystemRenderer>().material = circleMat;

            childLocator.FindChild("ScepterHealChannelEffect").Find("Orbs").Find("MagicCircle").GetComponent<ParticleSystemRenderer>().material = circleMat;
            childLocator.FindChild("ScepterHealChannelEffect").Find("Orbs").Find("MagicCircle").Find("BigCircle").GetComponent<ParticleSystemRenderer>().material = circleMat;
            childLocator.FindChild("ScepterHealChannelEffect").Find("Orbs").Find("MagicCircle").Find("MagicCircle2").GetComponent<ParticleSystemRenderer>().material = circleMat;
            childLocator.FindChild("ScepterHealChannelEffect").Find("Orbs").Find("MagicCircle").Find("MagicCircle2").Find("BigCircle2").GetComponent<ParticleSystemRenderer>().material = circleMat;

            childLocator.FindChild("TorporChannelEffect").Find("Lightning").Find("MagicCircle").GetComponent<ParticleSystemRenderer>().material = circleMat;
            childLocator.FindChild("TorporChannelEffect").Find("Lightning").Find("BigCircle").GetComponent<ParticleSystemRenderer>().material = circleMat;
            childLocator.FindChild("TorporChannelEffect").Find("Lightning").Find("MagicCircle2").GetComponent<ParticleSystemRenderer>().material = circleMat;
            childLocator.FindChild("TorporChannelEffect").Find("Lightning").Find("MagicCircle2").Find("BigCircle2").GetComponent<ParticleSystemRenderer>().material = circleMat;

            childLocator.FindChild("ScepterTorporChannelEffect").Find("Orbs").Find("MagicCircle").GetComponent<ParticleSystemRenderer>().material = circleMat;
            childLocator.FindChild("ScepterTorporChannelEffect").Find("Orbs").Find("MagicCircle").Find("BigCircle").GetComponent<ParticleSystemRenderer>().material = circleMat;
            childLocator.FindChild("ScepterTorporChannelEffect").Find("Orbs").Find("MagicCircle").Find("MagicCircle2").GetComponent<ParticleSystemRenderer>().material = circleMat;
            childLocator.FindChild("ScepterTorporChannelEffect").Find("Orbs").Find("MagicCircle").Find("MagicCircle2").Find("BigCircle2").GetComponent<ParticleSystemRenderer>().material = circleMat;

            childLocator.FindChild("WarcryChannelEffect").Find("Orbs").Find("MagicCircle").GetComponent<ParticleSystemRenderer>().material = circleMat;
            childLocator.FindChild("WarcryChannelEffect").Find("Orbs").Find("MagicCircle").Find("BigCircle").GetComponent<ParticleSystemRenderer>().material = circleMat;
            childLocator.FindChild("WarcryChannelEffect").Find("Orbs").Find("MagicCircle").Find("MagicCircle2").GetComponent<ParticleSystemRenderer>().material = circleMat;
            childLocator.FindChild("WarcryChannelEffect").Find("Orbs").Find("MagicCircle").Find("MagicCircle2").Find("BigCircle2").GetComponent<ParticleSystemRenderer>().material = circleMat;

            childLocator.FindChild("ScepterWarcryChannelEffect").Find("Orbs").Find("MagicCircle").GetComponent<ParticleSystemRenderer>().material = circleMat;
            childLocator.FindChild("ScepterWarcryChannelEffect").Find("Orbs").Find("MagicCircle").Find("BigCircle").GetComponent<ParticleSystemRenderer>().material = circleMat;
            childLocator.FindChild("ScepterWarcryChannelEffect").Find("Orbs").Find("MagicCircle").Find("MagicCircle2").GetComponent<ParticleSystemRenderer>().material = circleMat;
            childLocator.FindChild("ScepterWarcryChannelEffect").Find("Orbs").Find("MagicCircle").Find("MagicCircle2").Find("BigCircle2").GetComponent<ParticleSystemRenderer>().material = circleMat;

            //childLocator.FindChild("SpawnEffect").Find("MagicCircle").GetComponent<ParticleSystemRenderer>().material = circleMat;
            childLocator.FindChild("SpawnEffect").Find("MagicCircle").Find("BigCircle").GetComponent<ParticleSystemRenderer>().material = circleMat;
            childLocator.FindChild("SpawnEffect").Find("MagicCircle").Find("BigCircle").Find("BigCircle2").GetComponent<ParticleSystemRenderer>().material = circleMat;
        }


        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void ScepterSetup()
        {
            AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(scepterHealDef, "RobPaladinBody", SkillSlot.Special, 0);
            AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(scepterTorporDef, "RobPaladinBody", SkillSlot.Special, 1);
            AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(scepterWarcryDef, "RobPaladinBody", SkillSlot.Special, 2);
            AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(scepterCruelSunDef, "RobPaladinBody", SkillSlot.Special, 3);
        }

        private void Hook()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.CharacterModel.UpdateOverlays += CharacterModel_UpdateOverlays;
            On.RoR2.CharacterMaster.OnInventoryChanged += CharacterMaster_OnInventoryChanged;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            //On.RoR2.SceneDirector.Start += SceneDirector_Start;
            On.EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle.OnEnter += PlayVisionsAnimation;
            On.EntityStates.GlobalSkills.LunarNeedle.ChargeLunarSecondary.PlayChargeAnimation += PlayChargeLunarAnimation;
            On.EntityStates.GlobalSkills.LunarNeedle.ThrowLunarSecondary.PlayThrowAnimation += PlayThrowLunarAnimation;
            On.EntityStates.GlobalSkills.LunarDetonator.Detonate.OnEnter += PlayRuinAnimation;

            On.RoR2.CharacterSpeech.BrotherSpeechDriver.DoInitialSightResponse += BrotherSpeechDriver_DoInitialSightResponse;
            On.RoR2.CharacterSpeech.BrotherSpeechDriver.OnBodyKill += BrotherSpeechDriver_OnBodyKill;

            On.RoR2.UI.HUD.Awake += HUDAwake;
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);

            if (damageInfo.attacker && damageInfo.procCoefficient >= 1f)
            {
                CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
                if (attackerBody)
                {
                    if (attackerBody.HasBuff(Modules.Buffs.overchargeBuff))
                    {
                        float damageCoefficient = 0.75f;
                        float damageValue = Util.OnHitProcDamage(damageInfo.damage, attackerBody.damage, damageCoefficient);

                        LightningOrb lightningOrb = new LightningOrb();
                        lightningOrb.origin = damageInfo.position;
                        lightningOrb.damageValue = damageValue;
                        lightningOrb.isCrit = damageInfo.crit;
                        lightningOrb.bouncesRemaining = 3;
                        lightningOrb.teamIndex = attackerBody.teamComponent.teamIndex;
                        lightningOrb.attacker = damageInfo.attacker;
                        lightningOrb.bouncedObjects = new List<HealthComponent>
                            {
                                victim.GetComponent<HealthComponent>()
                            };
                        lightningOrb.procChainMask = damageInfo.procChainMask;
                        lightningOrb.procChainMask.AddProc(ProcType.ChainLightning);
                        lightningOrb.procCoefficient = 0f;
                        lightningOrb.lightningType = LightningOrb.LightningType.Loader;
                        lightningOrb.damageColorIndex = DamageColorIndex.Default;
                        lightningOrb.range += 6f;
                        HurtBox hurtBox = lightningOrb.PickNextTarget(damageInfo.position);
                        if (hurtBox)
                        {
                            lightningOrb.target = hurtBox;
                            OrbManager.instance.AddOrb(lightningOrb);
                        }
                    }
                }
            }
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            bool isHealing = false;

            if (self)
            {
                if (damageInfo.attacker)
                {
                    CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
                    if (attackerBody)
                    {
                        if (attackerBody.baseNameToken == "NEMPALADIN_NAME")
                        {
                            if (damageInfo.damageType.HasFlag(DamageType.BlightOnHit))
                            {
                                damageInfo.damageType = DamageType.Generic;
                                isHealing = true;
                            }
                        }
                    }
                }
            }

            orig(self, damageInfo);

            if (isHealing && !damageInfo.rejected)
            {
                damageInfo.attacker.GetComponent<HealthComponent>().Heal(damageInfo.damage * 0.2f, default(ProcChainMask));
            }
        }

        private void BrotherSpeechDriver_OnBodyKill(On.RoR2.CharacterSpeech.BrotherSpeechDriver.orig_OnBodyKill orig, RoR2.CharacterSpeech.BrotherSpeechDriver self, DamageReport damageReport)
        {
            if (damageReport.victimBody)
            {
                if (damageReport.victimBodyIndex == BodyCatalog.FindBodyIndex(Modules.Prefabs.paladinPrefab))
                {
                    RoR2.CharacterSpeech.CharacterSpeechController.SpeechInfo[] responsePool = new RoR2.CharacterSpeech.CharacterSpeechController.SpeechInfo[]
                    {
                    new RoR2.CharacterSpeech.CharacterSpeechController.SpeechInfo
                    {
                        duration = 1f,
                        maxWait = 4f,
                        mustPlay = true,
                        priority = 0f,
                        token = "BROTHER_KILL_PALADIN_1"
                    },
                    new RoR2.CharacterSpeech.CharacterSpeechController.SpeechInfo
                    {
                        duration = 1f,
                        maxWait = 4f,
                        mustPlay = true,
                        priority = 0f,
                        token = "BROTHER_KILL_PALADIN_2"
                    },
                    new RoR2.CharacterSpeech.CharacterSpeechController.SpeechInfo
                    {
                        duration = 1f,
                        maxWait = 4f,
                        mustPlay = true,
                        priority = 0f,
                        token = "BROTHER_KILL_PALADIN_3"
                    }
                    };

                    self.SendReponseFromPool(responsePool);
                }
            }

            orig(self, damageReport);
        }

        private void BrotherSpeechDriver_DoInitialSightResponse(On.RoR2.CharacterSpeech.BrotherSpeechDriver.orig_DoInitialSightResponse orig, RoR2.CharacterSpeech.BrotherSpeechDriver self)
        {
            bool isThereAPaladin = false;

            ReadOnlyCollection<CharacterBody> characterBodies = CharacterBody.readOnlyInstancesList;
            for (int i = 0; i < characterBodies.Count; i++)
            {
                BodyIndex bodyIndex = characterBodies[i].bodyIndex;
                isThereAPaladin |= (bodyIndex == BodyCatalog.FindBodyIndex(Modules.Prefabs.paladinPrefab));
            }

            if (isThereAPaladin)
            {
                RoR2.CharacterSpeech.CharacterSpeechController.SpeechInfo[] responsePool = new RoR2.CharacterSpeech.CharacterSpeechController.SpeechInfo[]
                {
                    new RoR2.CharacterSpeech.CharacterSpeechController.SpeechInfo
                    {
                        duration = 1f,
                        maxWait = 4f,
                        mustPlay = true,
                        priority = 0f,
                        token = "BROTHER_SEE_PALADIN_1"
                    },
                    new RoR2.CharacterSpeech.CharacterSpeechController.SpeechInfo
                    {
                        duration = 1f,
                        maxWait = 4f,
                        mustPlay = true,
                        priority = 0f,
                        token = "BROTHER_SEE_PALADIN_2"
                    },
                    new RoR2.CharacterSpeech.CharacterSpeechController.SpeechInfo
                    {
                        duration = 1f,
                        maxWait = 4f,
                        mustPlay = true,
                        priority = 0f,
                        token = "BROTHER_SEE_PALADIN_3"
                    }
                };

                self.SendReponseFromPool(responsePool);
            }

            orig(self);
        }

        private void PlayVisionsAnimation(On.EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle.orig_OnEnter orig, EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle self)
        {
            orig(self);

            if (self.characterBody.baseNameToken == "PALADIN_NAME")
            {
                self.PlayAnimation("Gesture, Override", "FireVisions", "Visions.playbackRate", self.duration  * 2.5f);
            }
        }

        private void PlayChargeLunarAnimation(On.EntityStates.GlobalSkills.LunarNeedle.ChargeLunarSecondary.orig_PlayChargeAnimation orig, EntityStates.GlobalSkills.LunarNeedle.ChargeLunarSecondary self)
        {
            orig(self);

            if (self.characterBody.baseNameToken == "PALADIN_NAME")
            {
                self.PlayAnimation("Gesture, Override", "ChargeLunarSecondary", "LunarSecondary.playbackRate", self.duration * 0.5f);
            }
        }

        private void PlayThrowLunarAnimation(On.EntityStates.GlobalSkills.LunarNeedle.ThrowLunarSecondary.orig_PlayThrowAnimation orig, EntityStates.GlobalSkills.LunarNeedle.ThrowLunarSecondary self)
        {
            orig(self);

            if (self.characterBody.baseNameToken == "PALADIN_NAME")
            {
                self.PlayAnimation("Gesture, Override", "ThrowLunarSecondary", "LunarSecondary.playbackRate", self.duration);
            }
        }

        private void PlayRuinAnimation(On.EntityStates.GlobalSkills.LunarDetonator.Detonate.orig_OnEnter orig, EntityStates.GlobalSkills.LunarDetonator.Detonate self)
        {
            orig(self);

            if (self.characterBody.baseNameToken == "PALADIN_NAME")
            {
                self.PlayAnimation("Gesture, Override", "CastRuin", "Ruin.playbackRate", self.duration * 0.5f);
                Util.PlaySound("PaladinFingerSnap", self.gameObject);
                self.StartAimMode(self.duration + 0.5f);

                EffectManager.SimpleMuzzleFlash(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/MuzzleFlashes/MuzzleflashLunarNeedle"), self.gameObject, "HandL", false);
            }
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

                if (self.HasBuff(Modules.Buffs.rageBuff))
                {
                    self.armor += 100f;
                    self.damage *= 1.5f;
                    self.moveSpeed += 6f;

                    PaladinRageController rageComponent = self.GetComponent<PaladinRageController>();
                    if (rageComponent)
                    {
                        float regenAmount = rageComponent.currentRegen;
                        self.regen += regenAmount;
                    }
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
                    var torporController = self.body.GetComponent<PaladinTorporTracker>();
                    if (!torporController) torporController = self.body.gameObject.AddComponent<PaladinTorporTracker>();
                    else return;

                    torporController.Body = self.body;
                    TemporaryOverlay overlay = self.gameObject.AddComponent<TemporaryOverlay>();
                    overlay.duration = float.PositiveInfinity;
                    overlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    overlay.animateShaderAlpha = true;
                    overlay.destroyComponentOnEnd = true;
                    overlay.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matDoppelganger");
                    overlay.AddToCharacerModel(self);
                    torporController.Overlay = overlay;
                }

                if (self.body && self.body.HasBuff(Modules.Buffs.rageBuff))
                {
                    var rageTracker = self.body.GetComponent<PaladinRageTracker>();
                    if (!rageTracker) rageTracker = self.body.gameObject.AddComponent<PaladinRageTracker>();
                    else return;

                    rageTracker.Body = self.body;
                    TemporaryOverlay overlay = self.gameObject.AddComponent<TemporaryOverlay>();
                    overlay.duration = float.PositiveInfinity;
                    overlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    overlay.animateShaderAlpha = true;
                    overlay.destroyComponentOnEnd = true;
                    overlay.originalMaterial = RoR2.LegacyResourcesAPI.Load<Material>("Materials/matMercEvisTarget");
                    overlay.AddToCharacerModel(self);
                    rageTracker.Overlay = overlay;
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

        #region HUD
        internal static void HUDAwake(On.RoR2.UI.HUD.orig_Awake orig, HUD self)
        {
            orig(self);

            Misc.RageHUD rageHud = self.gameObject.AddComponent<Misc.RageHUD>();

            GameObject rageGauge = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("RageGauge"), self.transform.Find("MainContainer").Find("MainUIArea").Find("SpringCanvas").Find("BottomLeftCluster"));
            rageGauge.GetComponent<RectTransform>().localPosition = Vector3.zero;
            rageGauge.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -147.8f);
            rageGauge.GetComponent<RectTransform>().localScale = new Vector3(0.72f, 0.15f, 1f);
            rageHud.rageGauge = rageGauge;
            rageHud.rageFill = rageGauge.transform.Find("GaugeFill").gameObject.GetComponent<Image>();
        }
        #endregion

        private void CreateDoppelganger()
        {
            doppelganger = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterMasters/MercMonsterMaster"), "PaladinMonsterMaster");
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
            scepterHealDef.canceledFromSprinting = false;
            scepterHealDef.fullRestockOnAssign = true;
            scepterHealDef.interruptPriority = InterruptPriority.Skill;
            scepterHealDef.resetCooldownTimerOnUse = false;
            scepterHealDef.isCombatSkill = true;
            scepterHealDef.mustKeyPress = false;
            scepterHealDef.cancelSprintingOnActivation = true;
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

            scepterCruelSunDef = ScriptableObject.CreateInstance<SkillDef>();
            scepterCruelSunDef.activationState = new SerializableEntityStateType(typeof(States.Spell.ScepterChannelCruelSun));
            scepterCruelSunDef.activationStateMachineName = "Weapon";
            scepterCruelSunDef.baseMaxStock = 1;
            scepterCruelSunDef.baseRechargeInterval = 40f;
            scepterCruelSunDef.beginSkillCooldownOnSkillEnd = true;
            scepterCruelSunDef.canceledFromSprinting = false;
            scepterCruelSunDef.fullRestockOnAssign = true;
            scepterCruelSunDef.interruptPriority = InterruptPriority.Skill;
            scepterCruelSunDef.resetCooldownTimerOnUse = false;
            scepterCruelSunDef.isCombatSkill = true;
            scepterCruelSunDef.mustKeyPress = false;
            scepterCruelSunDef.cancelSprintingOnActivation = true;
            scepterCruelSunDef.rechargeStock = 1;
            scepterCruelSunDef.requiredStock = 1;
            scepterCruelSunDef.stockToConsume = 1;
            scepterCruelSunDef.icon = Modules.Assets.icon4dS;
            scepterCruelSunDef.skillDescriptionToken = "PALADIN_SPECIAL_SCEPSUN_DESCRIPTION";
            scepterCruelSunDef.skillName = "PALADIN_SPECIAL_SCEPSUN_NAME";
            scepterCruelSunDef.skillNameToken = "PALADIN_SPECIAL_SCEPSUN_NAME";

            Modules.Skills.skillDefs.Add(scepterCruelSunDef);
        }
    }
}