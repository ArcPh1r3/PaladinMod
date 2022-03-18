using EntityStates;
using PaladinMod.Misc;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PaladinMod.Modules
{
    public static class Skills
    {
        internal static SkillDef berserkOutSkillDef;
        //internal static SkillDef berserkSlashSkillDef;
        //internal static SkillDef berserkSpinSlashSkillDef;
        internal static SkillDef berserkDashSkillDef;

        private static SkillLocator skillLocator;

        internal static List<SkillFamily> skillFamilies = new List<SkillFamily>();
        internal static List<SkillDef> skillDefs = new List<SkillDef>();

        public static void SetupSkills(GameObject bodyPrefab)
        {
            foreach (GenericSkill obj in bodyPrefab.GetComponentsInChildren<GenericSkill>())
            {
                PaladinPlugin.DestroyImmediate(obj);
            }

            skillLocator = bodyPrefab.GetComponent<SkillLocator>();

            PassiveSetup();
            PrimarySetup(bodyPrefab);
            SecondarySetup(bodyPrefab);
            UtilitySetup(bodyPrefab);
            SpecialSetup(bodyPrefab);
        }

        private static void PassiveSetup()
        {
            skillLocator.passiveSkill.enabled = true;
            skillLocator.passiveSkill.skillNameToken = "PALADIN_PASSIVE_NAME";
            skillLocator.passiveSkill.skillDescriptionToken = "PALADIN_PASSIVE_DESCRIPTION";
            skillLocator.passiveSkill.icon = Assets.iconP;
        }

        private static void PrimarySetup(GameObject bodyPrefab)
        {
            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Slash));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon1;
            mySkillDef.skillDescriptionToken = "PALADIN_PRIMARY_SLASH_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_PRIMARY_SLASH_NAME";
            mySkillDef.skillNameToken = "PALADIN_PRIMARY_SLASH_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_SWORDBEAM"
            };

            if (bodyPrefab.name == "LunarKnightBody")
            {
                mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.LunarKnight.MaceSlam));
                mySkillDef.activationStateMachineName = "Body";
            }

            if (bodyPrefab.name == "NemesisPaladinBody")
            {
                mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Nemesis.PunchCombo));
            }

            skillDefs.Add(mySkillDef);

            skillLocator.primary = bodyPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            skillFamilies.Add(newFamily);
            skillLocator.primary._skillFamily = newFamily;
            SkillFamily skillFamily = skillLocator.primary.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }

        private static void SecondarySetup(GameObject bodyPrefab)
        {
            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.SpinSlashEntry));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 6f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon2;
            mySkillDef.skillDescriptionToken = "PALADIN_SECONDARY_SPINSLASH_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_SECONDARY_SPINSLASH_NAME";
            mySkillDef.skillNameToken = "PALADIN_SECONDARY_SPINSLASH_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_STUNNING",
            };

            skillDefs.Add(mySkillDef);

            skillLocator.secondary = bodyPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            skillFamilies.Add(newFamily);
            skillLocator.secondary._skillFamily = newFamily;
            SkillFamily skillFamily = skillLocator.secondary.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.ChargeLightningSpear));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 8f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.cancelSprintingOnActivation = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon2b;
            mySkillDef.skillDescriptionToken = "PALADIN_SECONDARY_LIGHTNING_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_SECONDARY_LIGHTNING_NAME";
            mySkillDef.skillNameToken = "PALADIN_SECONDARY_LIGHTNING_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_SHOCKING",
                "KEYWORD_AGILE"
            };

            skillDefs.Add(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.LunarShards));
            mySkillDef.activationStateMachineName = "Slide";
            mySkillDef.baseMaxStock = StaticValues.lunarShardMaxStock;
            mySkillDef.baseRechargeInterval = 1.5f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.resetCooldownTimerOnUse = true;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.cancelSprintingOnActivation = false;
            mySkillDef.rechargeStock = 12;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon2c;
            mySkillDef.skillDescriptionToken = "PALADIN_SECONDARY_LUNARSHARD_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_SECONDARY_LUNARSHARD_NAME";
            mySkillDef.skillNameToken = "PALADIN_SECONDARY_LUNARSHARD_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_AGILE"
            };

            skillDefs.Add(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableDef = Modules.Unlockables.paladinLunarShardSkillDef,
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }

        private static void UtilitySetup(GameObject bodyPrefab)
        {
            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Quickstep.QuickstepSimple));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 2;
            mySkillDef.baseRechargeInterval = 10f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = false;
            mySkillDef.mustKeyPress = false;
            mySkillDef.cancelSprintingOnActivation = false;
            mySkillDef.forceSprintDuringState = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon3;
            mySkillDef.skillDescriptionToken = "PALADIN_UTILITY_DASH_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_UTILITY_DASH_NAME";
            mySkillDef.skillNameToken = "PALADIN_UTILITY_DASH_NAME";

            skillDefs.Add(mySkillDef);

            skillLocator.utility = bodyPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            skillFamilies.Add(newFamily);
            skillLocator.utility._skillFamily = newFamily;
            SkillFamily skillFamily = skillLocator.utility.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Spell.ChannelSmallHeal));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 8;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon3b;
            mySkillDef.skillDescriptionToken = "PALADIN_UTILITY_HEAL_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_UTILITY_HEAL_NAME";
            mySkillDef.skillNameToken = "PALADIN_UTILITY_HEAL_NAME";

            skillDefs.Add(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableDef = Modules.Unlockables.paladinHealSkillDefDef,
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }

        private static void SpecialSetup(GameObject bodyPrefab)
        {
            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Spell.ChannelHealZone));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 18f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon4;
            mySkillDef.skillDescriptionToken = "PALADIN_SPECIAL_HEALZONE_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_SPECIAL_HEALZONE_NAME";
            mySkillDef.skillNameToken = "PALADIN_SPECIAL_HEALZONE_NAME";

            skillDefs.Add(mySkillDef);

            skillLocator.special = bodyPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            skillFamilies.Add(newFamily);
            skillLocator.special._skillFamily = newFamily;
            SkillFamily skillFamily = skillLocator.special.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Spell.ChannelTorpor));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 18f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon4b;
            mySkillDef.skillDescriptionToken = "PALADIN_SPECIAL_TORPOR_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_SPECIAL_TORPOR_NAME";
            mySkillDef.skillNameToken = "PALADIN_SPECIAL_TORPOR_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_TORPOR"
            };

            skillDefs.Add(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableDef = Modules.Unlockables.paladinTorporSkillDefDef,
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Spell.ChannelWarcry));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 18f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon4c;
            mySkillDef.skillDescriptionToken = "PALADIN_SPECIAL_WARCRY_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_SPECIAL_WARCRY_NAME";
            mySkillDef.skillNameToken = "PALADIN_SPECIAL_WARCRY_NAME";

            skillDefs.Add(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Spell.ChannelCruelSun));
            if (Config.legacyCruelSun.Value) mySkillDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Spell.ChannelCruelSunOld));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 20f;
            if (Config.legacyCruelSun.Value) mySkillDef.baseRechargeInterval = 40f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = true;
            if (Config.legacyCruelSun.Value) mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.resetCooldownTimerOnUse = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = true;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Assets.icon4d;
            mySkillDef.skillDescriptionToken = "PALADIN_SPECIAL_SUN_DESCRIPTION";
            mySkillDef.skillName = "PALADIN_SPECIAL_SUN_NAME";
            mySkillDef.skillNameToken = "PALADIN_SPECIAL_SUN_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_OVERHEAT"
            };
            if (Config.legacyCruelSun.Value) mySkillDef.keywordTokens = null;

            skillDefs.Add(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableDef = Modules.Unlockables.paladinCruelSunSkillDefDef,
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            //pride flare testing; see PaladinPlugin.cs for real settings (why tho???)
            /*
            SkillDef scepterCruelSunDef = ScriptableObject.CreateInstance<SkillDef>();
            scepterCruelSunDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Spell.ScepterChannelCruelSun));
            if (Modules.Config.legacyCruelSun.Value) scepterCruelSunDef.activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Spell.ScepterChannelCruelSunOld));
            scepterCruelSunDef.activationStateMachineName = "Weapon";
            scepterCruelSunDef.baseMaxStock = 1;
            scepterCruelSunDef.baseRechargeInterval = 20f;
            if (Modules.Config.legacyCruelSun.Value) scepterCruelSunDef.baseRechargeInterval = 40f;
            scepterCruelSunDef.beginSkillCooldownOnSkillEnd = true;
            scepterCruelSunDef.canceledFromSprinting = true;
            if (Modules.Config.legacyCruelSun.Value) scepterCruelSunDef.canceledFromSprinting = false;
            scepterCruelSunDef.fullRestockOnAssign = true;
            scepterCruelSunDef.interruptPriority = InterruptPriority.Skill;
            scepterCruelSunDef.resetCooldownTimerOnUse = false;
            scepterCruelSunDef.isCombatSkill = true;
            scepterCruelSunDef.mustKeyPress = true;
            scepterCruelSunDef.cancelSprintingOnActivation = true;
            scepterCruelSunDef.rechargeStock = 1;
            scepterCruelSunDef.requiredStock = 1;
            scepterCruelSunDef.stockToConsume = 1;
            scepterCruelSunDef.icon = Modules.Assets.icon4dS;
            scepterCruelSunDef.skillDescriptionToken = "PALADIN_SPECIAL_SCEPSUN_DESCRIPTION";
            scepterCruelSunDef.skillName = "PALADIN_SPECIAL_SCEPSUN_NAME";
            scepterCruelSunDef.skillNameToken = "PALADIN_SPECIAL_SCEPSUN_NAME";
            scepterCruelSunDef.keywordTokens = new string[] {
                "KEYWORD_OVERHEAT"
            };
            if (Config.legacyCruelSun.Value) scepterCruelSunDef.keywordTokens = null;

            skillDefs.Add(scepterCruelSunDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = scepterCruelSunDef,
                unlockableDef = Modules.Unlockables.paladinCruelSunSkillDefDef,
                viewableNode = new ViewablesCatalog.Node(scepterCruelSunDef.skillNameToken, false, null)
            };
            */
            //end testing

            SkillDef berserkSkillDef = CreateRageSkillDef(new SkillDefInfo
            {
                skillName = "PALADIN_SPECIAL_BERSERK_NAME",
                skillNameToken = "PALADIN_SPECIAL_BERSERK_NAME",
                skillDescriptionToken = "PALADIN_SPECIAL_BERSERK_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("BerserkIcon"),
                activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Rage.RageEnter)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 100,
                requiredStock = 0,
                stockToConsume = 0
            });

            berserkOutSkillDef = CreateSkillDef(new SkillDefInfo
            {
                skillName = "PALADIN_SPECIAL_BERSERK_NAME",
                skillNameToken = "PALADIN_SPECIAL_BERSERK_NAME",
                skillDescriptionToken = "PALADIN_SPECIAL_BERSERK_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("BerserkIcon"),
                activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Rage.RageExit)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 5f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            berserkDashSkillDef = CreateSkillDef(new SkillDefInfo
            {
                skillName = "PALADIN_UTILITY_BLINK_NAME",
                skillNameToken = "PALADIN_UTILITY_BLINK_NAME",
                skillDescriptionToken = "PALADIN_UTILITY_BLINK_DESCRIPTION",
                skillIcon = Assets.icon3,
                activationState = new SerializableEntityStateType(typeof(PaladinMod.States.Quickstep.FlashStep)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 3,
                baseRechargeInterval = 4f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            //if (Modules.Config.cursed.Value) AddSpecialSkill(bodyPrefab, berserkSkillDef);
        }

        internal static SkillDef CreateSkillDef(SkillDefInfo skillDefInfo)
        {
            SkillDef skillDef = ScriptableObject.CreateInstance<SkillDef>();

            skillDef.skillName = skillDefInfo.skillName;
            skillDef.skillNameToken = skillDefInfo.skillNameToken;
            skillDef.skillDescriptionToken = skillDefInfo.skillDescriptionToken;
            skillDef.icon = skillDefInfo.skillIcon;

            skillDef.activationState = skillDefInfo.activationState;
            skillDef.activationStateMachineName = skillDefInfo.activationStateMachineName;
            skillDef.baseMaxStock = skillDefInfo.baseMaxStock;
            skillDef.baseRechargeInterval = skillDefInfo.baseRechargeInterval;
            skillDef.beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
            skillDef.canceledFromSprinting = skillDefInfo.canceledFromSprinting;
            skillDef.forceSprintDuringState = skillDefInfo.forceSprintDuringState;
            skillDef.fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
            skillDef.interruptPriority = skillDefInfo.interruptPriority;
            skillDef.resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
            skillDef.isCombatSkill = skillDefInfo.isCombatSkill;
            skillDef.mustKeyPress = skillDefInfo.mustKeyPress;
            skillDef.cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
            skillDef.rechargeStock = skillDefInfo.rechargeStock;
            skillDef.requiredStock = skillDefInfo.requiredStock;
            skillDef.stockToConsume = skillDefInfo.stockToConsume;

            skillDef.keywordTokens = skillDefInfo.keywordTokens;

            skillDefs.Add(skillDef);

            return skillDef;
        }

        internal static SkillDef CreateRageSkillDef(SkillDefInfo skillDefInfo)
        {
            PaladinRageSkillDef skillDef = ScriptableObject.CreateInstance<PaladinRageSkillDef>();

            skillDef.skillName = skillDefInfo.skillName;
            skillDef.skillNameToken = skillDefInfo.skillNameToken;
            skillDef.skillDescriptionToken = skillDefInfo.skillDescriptionToken;
            skillDef.icon = skillDefInfo.skillIcon;

            skillDef.activationState = skillDefInfo.activationState;
            skillDef.activationStateMachineName = skillDefInfo.activationStateMachineName;
            skillDef.baseMaxStock = skillDefInfo.baseMaxStock;
            skillDef.baseRechargeInterval = skillDefInfo.baseRechargeInterval;
            skillDef.beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
            skillDef.canceledFromSprinting = skillDefInfo.canceledFromSprinting;
            skillDef.forceSprintDuringState = skillDefInfo.forceSprintDuringState;
            skillDef.fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
            skillDef.interruptPriority = skillDefInfo.interruptPriority;
            skillDef.resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
            skillDef.isCombatSkill = skillDefInfo.isCombatSkill;
            skillDef.mustKeyPress = skillDefInfo.mustKeyPress;
            skillDef.cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
            skillDef.rechargeStock = skillDefInfo.rechargeStock;
            skillDef.requiredStock = skillDefInfo.requiredStock;
            skillDef.stockToConsume = skillDefInfo.stockToConsume;

            skillDef.keywordTokens = skillDefInfo.keywordTokens;

            skillDefs.Add(skillDef);

            return skillDef;
        }

        internal static void AddPrimarySkill(GameObject targetPrefab, SkillDef skillDef)
        {
            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily = skillLocator.primary.skillFamily;

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };
        }

        internal static void AddSecondarySkill(GameObject targetPrefab, SkillDef skillDef)
        {
            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily = skillLocator.secondary.skillFamily;

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };
        }

        internal static void AddSecondarySkills(GameObject targetPrefab, params SkillDef[] skillDefs)
        {
            foreach (SkillDef i in skillDefs)
            {
                AddSecondarySkill(targetPrefab, i);
            }
        }

        internal static void AddUtilitySkill(GameObject targetPrefab, SkillDef skillDef)
        {
            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily = skillLocator.utility.skillFamily;

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };
        }

        internal static void AddUtilitySkills(GameObject targetPrefab, params SkillDef[] skillDefs)
        {
            foreach (SkillDef i in skillDefs)
            {
                AddUtilitySkill(targetPrefab, i);
            }
        }

        internal static void AddSpecialSkill(GameObject targetPrefab, SkillDef skillDef)
        {
            SkillLocator skillLocator = targetPrefab.GetComponent<SkillLocator>();

            SkillFamily skillFamily = skillLocator.special.skillFamily;

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
            };
        }

        internal static void AddSpecialSkills(GameObject targetPrefab, params SkillDef[] skillDefs)
        {
            foreach (SkillDef i in skillDefs)
            {
                AddSpecialSkill(targetPrefab, i);
            }
        }
    }
}

internal class SkillDefInfo
{
    public string skillName;
    public string skillNameToken;
    public string skillDescriptionToken;
    public Sprite skillIcon;

    public SerializableEntityStateType activationState;
    public string activationStateMachineName;
    public int baseMaxStock;
    public float baseRechargeInterval;
    public bool beginSkillCooldownOnSkillEnd;
    public bool canceledFromSprinting;
    public bool forceSprintDuringState;
    public bool fullRestockOnAssign;
    public InterruptPriority interruptPriority;
    public bool resetCooldownTimerOnUse;
    public bool isCombatSkill;
    public bool mustKeyPress;
    public bool cancelSprintingOnActivation;
    public int rechargeStock;
    public int requiredStock;
    public int stockToConsume;

    public string[] keywordTokens;
}