using R2API;
using RoR2;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PaladinMod.Modules
{
    public static class ItemDisplays
    {
        public static ItemDisplayRuleSet itemDisplayRuleSet;
        public static List<ItemDisplayRuleSet.NamedRuleGroup> itemRules;
        public static List<ItemDisplayRuleSet.NamedRuleGroup> equipmentRules;

        public static GameObject capacitorPrefab;

        private static Dictionary<string, GameObject> itemDisplayPrefabs = new Dictionary<string, GameObject>();

        public static void RegisterDisplays()
        {
            PopulateDisplays();

            GameObject bodyPrefab = Prefabs.paladinPrefab;

            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            itemDisplayRuleSet = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();

            itemRules = new List<ItemDisplayRuleSet.NamedRuleGroup>();
            equipmentRules = new List<ItemDisplayRuleSet.NamedRuleGroup>();

            //gotta add these to avoid bugs
            equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("Jetpack", "DisplayBugWings", "Chest", new Vector3(0, 0.001f, 0), new Vector3(0, 0, 0), new Vector3(0.002f, 0.002f, 0.002f)));
            equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("GoldGat", "DisplayGoldGat", "Chest", new Vector3(0.002f, 0.005f, -0.002f), new Vector3(0, 90, 290), new Vector3(0.002f, 0.002f, 0.002f)));
            equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("BFG", "DisplayBFG", "Chest", new Vector3(-0.001f, 0.002f, 0), new Vector3(330, 0, 45), new Vector3(0.004f, 0.004f, 0.004f)));

            if (Config.itemDisplays.Value)
            {
                #region Display Rules

                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("CritGlasses", "DisplayGlasses", "Head", new Vector3(0, 0.0012f, 0.0015f), new Vector3(0, 0, 90), new Vector3(0.0025f, 0.002f, 0.0025f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Syringe", "DisplaySyringeCluster", "Chest", new Vector3(-0.001f, 0.003f, 0), new Vector3(25, 315, 0), new Vector3(0.002f, 0.002f, 0.002f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("NearbyDamageBonus", "DisplayDiamond", "Sword", new Vector3(0, -0.001f, 0), new Vector3(0, 0, 0), new Vector3(0.001f, 0.001f, 0.001f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ArmorReductionOnHit", "DisplayWarhammer", "HandL", new Vector3(-0.0005f, 0.001f, 0.006f), new Vector3(0, 0, 90), new Vector3(0.0035f, 0.0035f, 0.0035f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SecondarySkillMagazine", "DisplayDoubleMag", "Sword", new Vector3(0.0008f, 0.001f, -0.0008f), new Vector3(15, 325, 0), new Vector3(0.001f, 0.001f, 0.001f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Bear", "DisplayBear", "Chest", new Vector3(-0.002f, -0.0025f, 0), new Vector3(0, 270, 0), new Vector3(0.0025f, 0.0025f, 0.0025f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SprintOutOfCombat", "DisplayWhip", "Pelvis", new Vector3(0.003f, 0, 0), new Vector3(0, 200, 335), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("PersonalShield", "DisplayShieldGenerator", "Chest", new Vector3(-0.0015f, 0, 0.0015f), new Vector3(45, 270, 90), new Vector3(0.002f, 0.002f, 0.002f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("RegenOnKill", "DisplaySteakCurved", "Head", new Vector3(0, 0.0022f, 0.0015f), new Vector3(335, 0, 180), new Vector3(0.001f, 0.001f, 0.001f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("FireballsOnHit", "DisplayFireballsOnHit", "Sword", new Vector3(0.003f, 0.016f, -0.003f), new Vector3(0, 120, 0), new Vector3(0.002f, 0.002f, 0.002f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Hoof", "DisplayHoof", "CalfR", new Vector3(0, 0.0034f, -0.0006f), new Vector3(80, 0, 0), new Vector3(0.0012f, 0.0012f, 0.001f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("WardOnLevel", "DisplayWarbanner", "Pelvis", new Vector3(0, 0.001f, -0.0012f), new Vector3(0, 0, 90), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BarrierOnOverHeal", "DisplayAegis", "ElbowL", new Vector3(0.0008f, 0, 0), new Vector3(90, 270, 0), new Vector3(0.0035f, 0.0035f, 0.0035f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("WarCryOnMultiKill", "DisplayPauldron", "ShoulderL", new Vector3(0.0015f, 0.0005f, 0), new Vector3(60, 90, 0), new Vector3(0.01f, 0.01f, 0.01f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SprintArmor", "DisplayBuckler", "ElbowR", new Vector3(0, 0.0025f, 0), new Vector3(0, 270, 90), new Vector3(0.003f, 0.003f, 0.003f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("IceRing", "DisplayIceRing", "Sword", new Vector3(0, 0.0018f, 0), new Vector3(270, 90, 0), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("FireRing", "DisplayFireRing", "Sword", new Vector3(0, 0.002f, 0), new Vector3(270, 90, 0), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Behemoth", "DisplayBehemoth", "Sword", new Vector3(-0.002f, 0.008f, 0), new Vector3(0, 280, 0), new Vector3(0.001f, 0.001f, 0.001f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Missile", "DisplayMissileLauncher", "Chest", new Vector3(0.0025f, 0.0055f, 0), new Vector3(0, 0, 335), new Vector3(0.001f, 0.001f, 0.001f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Dagger", "DisplayDagger", "Chest", new Vector3(0, 0.002f, 0), new Vector3(270, 45, 0), new Vector3(0.01f, 0.01f, 0.01f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ChainLightning", "DisplayUkulele", "Sword", new Vector3(0.00035f, 0.014f, 0.00035f), new Vector3(0, 40, 180), new Vector3(0.01f, 0.01f, 0.01f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("GhostOnKill", "DisplayMask", "Head", new Vector3(0, 0.001f, 0.0012f), new Vector3(0, 0, 0), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Mushroom", "DisplayMushroom", "Chest", new Vector3(0.0012f, 0.003f, 0), new Vector3(45, 90, 0), new Vector3(0.001f, 0.001f, 0.001f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("AttackSpeedOnCrit", "DisplayWolfPelt", "Head", new Vector3(0, 0.0024f, 0), new Vector3(340, 0, 0), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BleedOnHit", "DisplayTriTip", "HandL", new Vector3(-0.0004f, 0.001f, -0.0005f), new Vector3(0, 180, 0), new Vector3(0.004f, 0.004f, 0.004f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("HealOnCrit", "DisplayScythe", "Chest", new Vector3(0, 0.002f, -0.002f), new Vector3(270, 0, 0), new Vector3(0.002f, 0.002f, 0.002f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("HealWhileSafe", "DisplaySnail", "Chest", new Vector3(0, 0, 0), new Vector3(270, 180, 0), new Vector3(0.0015f, 0.0015f, 0.0015f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Clover", "DisplayClover", "Head", new Vector3(-0.0008f, 0.003f, -0.0006f), new Vector3(270, 90, 0), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("GoldOnHit", "DisplayBoneCrown", "Head", new Vector3(0, 0.002f, 0.0002f), new Vector3(10, 0, 0), new Vector3(0.01f, 0.01f, 0.01f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("JumpBoost", "DisplayWaxBird", "Head", new Vector3(0, -0.0036f, 0), new Vector3(0, 0, 0), new Vector3(0.01f, 0.01f, 0.01f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ArmorPlate", "DisplayRepulsionArmorPlate", "CalfL", new Vector3(-0.0004f, 0.0024f, 0.0004f), new Vector3(90, 0, 0), new Vector3(0.004f, 0.004f, 0.004f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Feather", "DisplayFeather", "ElbowL", new Vector3(0, 0.0004f, 0.0006f), new Vector3(90, 90, 0), new Vector3(0.00035f, 0.00035f, 0.00035f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Crowbar", "DisplayCrowbar", "Sword", new Vector3(0.00035f, 0.012f, 0.00035f), new Vector3(0, 110, 0), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ExecuteLowHealthElite", "DisplayGuillotine", "ThighR", new Vector3(0.0015f, 0, -0.0012f), new Vector3(60, 315, 0), new Vector3(0.0025f, 0.0025f, 0.0025f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("EquipmentMagazine", "DisplayBattery", "Chest", new Vector3(0.001f, 0, 0.0025f), new Vector3(0, 270, 45), new Vector3(0.003f, 0.003f, 0.003f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Infusion", "DisplayInfusion", "Pelvis", new Vector3(-0.002f, 0.0015f, 0.001f), new Vector3(0, 300, 0), new Vector3(0.0075f, 0.0075f, 0.0075f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Medkit", "DisplayMedkit", "Pelvis", new Vector3(0, 0.0015f, -0.0008f), new Vector3(90, 0, 0), new Vector3(0.01f, 0.01f, 0.01f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Bandolier", "DisplayBandolier", "Chest", new Vector3(0, 0, 0), new Vector3(330, 270, 90), new Vector3(0.008f, 0.008f, 0.008f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BounceNearby", "DisplayHook", "Chest", new Vector3(0, 0.003f, -0.0012f), new Vector3(0, 0, 0), new Vector3(0.002f, 0.002f, 0.002f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("StunChanceOnHit", "DisplayStunGrenade", "CalfR", new Vector3(0, 0.002f, -0.0016f), new Vector3(90, 0, 0), new Vector3(0.01f, 0.01f, 0.01f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("IgniteOnKill", "DisplayGasoline", "ThighR", new Vector3(0.0015f, 0.002f, -0.002f), new Vector3(90, 60, 0), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Firework", "DisplayFirework", "Pelvis", new Vector3(-0.0015f, 0.002f, -0.001f), new Vector3(270, 5, 0), new Vector3(0.004f, 0.004f, 0.004f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("LunarDagger", "DisplayLunarDagger", "Chest", new Vector3(-0.002f, -0.002f, -0.0018f), new Vector3(45, 90, 270), new Vector3(0.0075f, 0.0075f, 0.0075f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Knurl", "DisplayKnurl", "Chest", new Vector3(-0.003f, 0.0015f, 0), new Vector3(90, 0, 0), new Vector3(0.00125f, 0.00125f, 0.00125f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BeetleGland", "DisplayBeetleGland", "Chest", new Vector3(0.0025f, 0.002f, -0.001f), new Vector3(0, 270, 0), new Vector3(0.0015f, 0.0015f, 0.0015f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SprintBonus", "DisplaySoda", "Pelvis", new Vector3(0.002f, 0.001f, 0), new Vector3(270, 90, 0), new Vector3(0.004f, 0.004f, 0.004f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("StickyBomb", "DisplayStickyBomb", "Pelvis", new Vector3(0.0012f, 0.002f, -0.0014f), new Vector3(345, 15, 0), new Vector3(0.002f, 0.002f, 0.002f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("TreasureCache", "DisplayKey", "Pelvis", new Vector3(0, 0.0008f, -0.0012f), new Vector3(90, 0, 0), new Vector3(0.01f, 0.01f, 0.01f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BossDamageBonus", "DisplayAPRound", "Pelvis", new Vector3(-0.0012f, 0, -0.001f), new Vector3(90, 45, 0), new Vector3(0.008f, 0.008f, 0.008f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ExtraLife", "DisplayHippo", "Chest", new Vector3(-0.002f, 0, -0.001f), new Vector3(0, 220, 0), new Vector3(0.0025f, 0.0025f, 0.0025f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("KillEliteFrenzy", "DisplayBrainstalk", "Head", new Vector3(0, 0.002f, 0.0005f), new Vector3(0, 0, 0), new Vector3(0.002f, 0.002f, 0.002f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("RepeatHeal", "DisplayCorpseFlower", "Chest", new Vector3(0.0012f, 0.003f, 0), new Vector3(0, 25, 300), new Vector3(0.004f, 0.004f, 0.004f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("AutoCastEquipment", "DisplayFossil", "Pelvis", new Vector3(0.002f, 0.002f, 0.0012f), new Vector3(0, 315, 0), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("TitanGoldDuringTP", "DisplayGoldHeart", "Chest", new Vector3(-0.002f, 0, 0.0012f), new Vector3(0, 235, 0), new Vector3(0.0025f, 0.0025f, 0.0025f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SprintWisp", "DisplayBrokenMask", "ShoulderL", new Vector3(0.0015f, 0, 0), new Vector3(0, 90, 180), new Vector3(0.002f, 0.002f, 0.002f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BarrierOnKill", "DisplayBrooch", "Chest", new Vector3(0, 0.002f, 0.0012f), new Vector3(90, 0, 0), new Vector3(0.01f, 0.01f, 0.01f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("TPHealingNova", "DisplayGlowFlower", "Chest", new Vector3(0.0012f, 0.002f, 0.0018f), new Vector3(340, 30, 0), new Vector3(0.004f, 0.004f, 0.004f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("LunarUtilityReplacement", "DisplayBirdFoot", "Head", new Vector3(0, 0.002f, -0.0012f), new Vector3(0, 270, 0), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Thorns", "DisplayRazorwireLeft", "Sword", new Vector3(0, 0.006f, -0.001f), new Vector3(270, 300, 0), new Vector3(0.006f, 0.009f, 0.012f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("LunarPrimaryReplacement", "DisplayBirdEye", "Head", new Vector3(0, 0.001f, 0.0012f), new Vector3(270, 0, 0), new Vector3(0.003f, 0.003f, 0.003f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("NovaOnLowHealth", "DisplayJellyGuts", "Chest", new Vector3(0, 0.003f, -0.001f), new Vector3(310, 0, 0), new Vector3(0.001f, 0.001f, 0.001f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("LunarTrinket", "DisplayBeads", "ElbowL", new Vector3(0, 0.0008f, 0), new Vector3(0, 90, 90), new Vector3(0.01f, 0.01f, 0.01f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Plant", "DisplayInterstellarDeskPlant", "Chest", new Vector3(-0.001388083f, 0.003360658f, 0.0002904454f), new Vector3(285, 278, 170), new Vector3(0.001f, 0.001f, 0.001f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("DeathMark", "DisplayDeathMark", "HandL", new Vector3(0, 0, 0), new Vector3(90, 270, 0), new Vector3(0.0004f, 0.0004f, 0.0004f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("CooldownOnCrit", "DisplaySkull", "Chest", new Vector3(0, 0.0012f, 0.0024f), new Vector3(270, 0, 0), new Vector3(0.0025f, 0.0025f, 0.0025f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("UtilitySkillMagazine", "DisplayAfterburnerShoulderRing", "ShoulderL", new Vector3(-0.0014f, 0, 0), new Vector3(0, 0, 90), new Vector3(0.01f, 0.01f, 0.01f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ExplodeOnDeath", "DisplayWilloWisp", "Pelvis", new Vector3(-0.002f, 0.0012f, 0), new Vector3(0, 0, 0), new Vector3(0.0005f, 0.0005f, 0.0005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Seed", "DisplaySeed", "ElbowR", new Vector3(0, 0, -0.0004f), new Vector3(270, 0, 0), new Vector3(0.0005f, 0.0005f, 0.0005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Phasing", "DisplayStealthkit", "CalfL", new Vector3(0, 0.002f, -0.0012f), new Vector3(90, 0, 0), new Vector3(0.004f, 0.005f, 0.005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ShockNearby", "DisplayTeslaCoil", "Chest", new Vector3(0, 0.0025f, -0.0015f), new Vector3(290, 0, 0), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("AlienHead", "DisplayAlienHead", "Sword", new Vector3(0, 0, 0), new Vector3(315, 0, 0), new Vector3(0.01f, 0.01f, 0.01f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("HeadHunter", "DisplaySkullCrown", "Head", new Vector3(0, 0.003f, 0), new Vector3(0, 0, 0), new Vector3(0.005f, 0.002f, 0.002f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("EnergizedOnEquipmentUse", "DisplayWarHorn", "Pelvis", new Vector3(-0.0028f, 0.002f, 0), new Vector3(0, 190, 270), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Tooth", "DisplayToothMeshLarge", "Chest", new Vector3(0, 0.003f, 0), new Vector3(290, 0, 0), new Vector3(0.08f, 0.08f, 0.08f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Pearl", "DisplayPearl", "HandL", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0.002f, 0.002f, 0.002f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ShinyPearl", "DisplayShinyPearl", "HandR", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0.002f, 0.002f, 0.002f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BonusGoldPackOnKill", "DisplayTome", "ThighL", new Vector3(-0.0008f, 0.0012f, -0.0024f), new Vector3(20, 200, 0), new Vector3(0.0008f, 0.0008f, 0.0008f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Squid", "DisplaySquidTurret", "Stomach", new Vector3(0, 0.001f, -0.0012f), new Vector3(270, 0, 0), new Vector3(0.001f, 0.001f, 0.001f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("LaserTurbine", "DisplayLaserTurbine", "ThighR", new Vector3(0.012f, 0.0032f, 0), new Vector3(0, 90, 0), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Incubator", "DisplayAncestralIncubator", "Chest", new Vector3(0, 0, 0), new Vector3(330, 0, 0), new Vector3(0.0006f, 0.0006f, 0.0006f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SiphonOnLowHealth", "DisplaySiphonOnLowHealth", "Pelvis", new Vector3(0.0006f, 0, -0.0006f), new Vector3(0, 315, 0), new Vector3(0.001f, 0.001f, 0.001f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BleedOnHitAndExplode", "DisplayBleedOnHitAndExplode", "ThighR", new Vector3(0, 0.0025f, 0.002f), new Vector3(0, 0, 0), new Vector3(0.001f, 0.001f, 0.001f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("MonstersOnShrineUse", "DisplayMonstersOnShrineUse", "ThighL", new Vector3(0, 0.003f, -0.0024f), new Vector3(90, 90, 0), new Vector3(0.001f, 0.001f, 0.001f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("RandomDamageZone", "DisplayRandomDamageZone", "HandR", new Vector3(-0.001f, 0.0006f, -0.0005f), new Vector3(0, 90, 270), new Vector3(0.0008f, 0.0006f, 0.0008f)));

                equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("QuestVolatileBattery", "DisplayBatteryArray", "Chest", new Vector3(0, 0, -0.0028f), new Vector3(0, 0, 0), new Vector3(0.003f, 0.003f, 0.003f)));
                equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("CommandMissile", "DisplayMissileRack", "Chest", new Vector3(0, 0.002f, 0), new Vector3(90, 180, 0), new Vector3(0.006f, 0.006f, 0.006f)));
                equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("Fruit", "DisplayFruit", "Chest", new Vector3(0, -0.005f, 0.004f), new Vector3(0, 150, 0), new Vector3(0.005f, 0.005f, 0.005f)));
                equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("AffixWhite", "DisplayEliteIceCrown", "Head", new Vector3(0, 0.003f, 0), new Vector3(270, 0, 0), new Vector3(0.0003f, 0.0003f, 0.0003f)));
                equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("AffixPoison", "DisplayEliteUrchinCrown", "Head", new Vector3(0, 0.0025f, 0), new Vector3(270, 0, 0), new Vector3(0.0005f, 0.0005f, 0.0005f)));
                equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("AffixHaunted", "DisplayEliteStealthCrown", "Head", new Vector3(0, 0.002f, 0), new Vector3(270, 0, 0), new Vector3(0.0008f, 0.0008f, 0.0008f)));
                equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("CritOnUse", "DisplayNeuralImplant", "Head", new Vector3(0, 0.0012f, 0.002f), new Vector3(0, 0, 0), new Vector3(0.004f, 0.004f, 0.004f)));
                equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("DroneBackup", "DisplayRadio", "Pelvis", new Vector3(0.002f, 0.0016f, 0), new Vector3(0, 90, 0), new Vector3(0.01f, 0.01f, 0.01f)));
                equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("Lightning", capacitorPrefab, "ShoulderL", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0.01f, 0.01f, 0.01f)));
                equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("BurnNearby", "DisplayPotion", "Pelvis", new Vector3(0.0025f, 0.0012f, 0), new Vector3(0, 0, 330), new Vector3(0.0005f, 0.0005f, 0.0005f)));
                equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("CrippleWard", "DisplayEffigy", "Pelvis", new Vector3(0.0025f, 0.0012f, 0), new Vector3(0, 270, 0), new Vector3(0.004f, 0.004f, 0.004f)));
                equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("GainArmor", "DisplayElephantFigure", "CalfR", new Vector3(0, 0.003f, 0.0012f), new Vector3(90, 0, 0), new Vector3(0.008f, 0.008f, 0.008f)));
                equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("Recycle", "DisplayRecycler", "Chest", new Vector3(0, 0.002f, -0.002f), new Vector3(0, 90, 0), new Vector3(0.001f, 0.001f, 0.001f)));
                equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("FireBallDash", "DisplayEgg", "Pelvis", new Vector3(0.0025f, 0.0012f, 0), new Vector3(270, 0, 0), new Vector3(0.003f, 0.003f, 0.003f)));
                equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("Cleanse", "DisplayWaterPack", "Chest", new Vector3(0, 0, -0.0018f), new Vector3(0, 180, 0), new Vector3(0.0014f, 0.0014f, 0.0014f)));
                equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("Tonic", "DisplayTonic", "Pelvis", new Vector3(0.0025f, 0.0012f, 0), new Vector3(0, 90, 0), new Vector3(0.003f, 0.003f, 0.003f)));
                equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("Gateway", "DisplayVase", "Pelvis", new Vector3(0.0025f, 0.0012f, 0), new Vector3(0, 0, 0), new Vector3(0.003f, 0.003f, 0.003f)));
                equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("Scanner", "DisplayScanner", "Pelvis", new Vector3(0.0025f, 0.0012f, 0), new Vector3(270, 90, 0), new Vector3(0.0015f, 0.0015f, 0.0015f)));
                equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("DeathProjectile", "DisplayDeathProjectile", "Pelvis", new Vector3(-0.0024f, 0, -0.001f), new Vector3(0, 240, 0), new Vector3(0.0015f, 0.0015f, 0.0015f)));
                equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("LifestealOnHit", "DisplayLifestealOnHit", "Head", new Vector3(-0.0015f, 0.004f, 0), new Vector3(45, 90, 0), new Vector3(0.001f, 0.001f, 0.001f)));
                equipmentRules.Add(ItemDisplays.CreateGenericDisplayRule("TeamWarCry", "DisplayTeamWarCry", "Pelvis", new Vector3(0, 0, 0.003f), new Vector3(0, 0, 0), new Vector3(0.0015f, 0.0015f, 0.0015f)));

                itemRules.Add(ItemDisplays.CreateFollowerDisplayRule("Icicle", "DisplayFrostRelic", new Vector3(0.013f, 0.01f, -0.006f), new Vector3(90, 0, 0), new Vector3(2, 2, 2)));
                itemRules.Add(ItemDisplays.CreateFollowerDisplayRule("Talisman", "DisplayTalisman", new Vector3(-0.013f, 0.01f, -0.006f), new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
                itemRules.Add(ItemDisplays.CreateFollowerDisplayRule("FocusConvergence", "DisplayFocusedConvergence", new Vector3(-0.01f, 0.005f, -0.01f), new Vector3(0, 0, 0), new Vector3(0.2f, 0.2f, 0.2f)));

                equipmentRules.Add(ItemDisplays.CreateFollowerDisplayRule("Saw", "DisplaySawmerang", new Vector3(0, 0.01f, -0.015f), new Vector3(90, 0, 0), new Vector3(0.25f, 0.25f, 0.25f)));
                equipmentRules.Add(ItemDisplays.CreateFollowerDisplayRule("Meteor", "DisplayMeteor", new Vector3(0, 0.01f, -0.015f), new Vector3(90, 0, 0), new Vector3(1, 1, 1)));
                equipmentRules.Add(ItemDisplays.CreateFollowerDisplayRule("Blackhole", "DisplayGravCube", new Vector3(0, 0.01f, -0.015f), new Vector3(90, 0, 0), new Vector3(1, 1, 1)));

                //weird rules here
                #region weirdshit
                itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "IncreaseHealing",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAntler"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.002f, 0.0005f),
                            localAngles = new Vector3(0, 90, 0),
                            localScale = new Vector3(0.005f, 0.005f, 0.005f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAntler"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.002f, 0.0005f),
                            localAngles = new Vector3(0, 90, 0),
                            localScale = new Vector3(0.005f, 0.005f, -0.005f),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "AffixRed",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteHorn"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.002f, 0),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.001f, 0.001f, 0.001f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteHorn"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.002f, 0),
                            localAngles = new Vector3(0, 180, 0),
                            localScale = new Vector3(0.001f, 0.001f, -0.001f),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "AffixBlue",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.002f, 0.002f),
                            localAngles = new Vector3(315, 0, 0),
                            localScale = new Vector3(0.005f, 0.005f, 0.005f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.002f, 0),
                            localAngles = new Vector3(290, 0, 0),
                            localScale = new Vector3(0.0025f, 0.0025f, 0.0025f),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "ShieldOnly",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldBug"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.002f, 0.001f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.004f, 0.004f, 0.004f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldBug"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.002f, 0.001f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(-0.004f, 0.004f, 0.004f),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });


                itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "FallBoots",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGravBoots"),
                            childName = "FootR",
                            localPos = new Vector3(0, 0, -0.0006f),
                            localAngles = new Vector3(90, 0, 0),
                            localScale = new Vector3(0.0025f, 0.0025f, 0.0025f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGravBoots"),
                            childName = "FootL",
                            localPos = new Vector3(0, 0, -0.0006f),
                            localAngles = new Vector3(90, 0, 0),
                            localScale = new Vector3(0.0025f, 0.0025f, 0.0025f),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
                {
                    name = "NovaOnHeal",
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDevilHorns"),
                            childName = "Head",
                            localPos = new Vector3(0, 0, 0),
                            localAngles = new Vector3(0, 0, 20),
                            localScale = new Vector3(0.01f, 0.01f, 0.01f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDevilHorns"),
                            childName = "Head",
                            localPos = new Vector3(0, 0, 0),
                            localAngles = new Vector3(0, 0, 340),
                            localScale = new Vector3(-0.01f, 0.01f, 0.01f),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });
                #endregion

                #endregion

                RegisterModdedDisplays();
            }

            ItemDisplayRuleSet.NamedRuleGroup[] item = itemRules.ToArray();
            ItemDisplayRuleSet.NamedRuleGroup[] equip = equipmentRules.ToArray();
            itemDisplayRuleSet.namedItemRuleGroups = item;
            itemDisplayRuleSet.namedEquipmentRuleGroups = equip;

            characterModel.itemDisplayRuleSet = itemDisplayRuleSet;

            Prefabs.lunarKnightPrefab.GetComponentInChildren<CharacterModel>().itemDisplayRuleSet = itemDisplayRuleSet;
        }

        public static void RegisterModdedDisplays()
        {
            #region Aetherium
            if (PaladinPlugin.aetheriumInstalled)
            {
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ITEM_ACCURSED_POTION", ItemDisplays.LoadAetheriumDisplay("AccursedPotion"), "Pelvis", new Vector3(0.002f, 0.002f, 0.0012f), new Vector3(0, 0, 0), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ITEM_VOIDHEART", ItemDisplays.LoadAetheriumDisplay("VoidHeart"), "Chest", new Vector3(0, 0.005f, 0), new Vector3(0, 0, 0), new Vector3(0.004f, 0.004f, 0.004f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ITEM_SHARK_TEETH", ItemDisplays.LoadAetheriumDisplay("SharkTeeth"), "CalfL", new Vector3(0, 0.0012f, 0), new Vector3(0, 0, 310), new Vector3(0.005f, 0.005f, 0.003f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ITEM_BLOOD_SOAKED_SHIELD", ItemDisplays.LoadAetheriumDisplay("BloodSoakedShield"), "ElbowL", new Vector3(0.0012f, 0.002f, 0), new Vector3(0, 90, 0), new Vector3(0.003f, 0.003f, 0.003f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ITEM_FEATHERED_PLUME", ItemDisplays.LoadAetheriumDisplay("FeatheredPlume"), "Head", new Vector3(0, 0.0025f, 0), new Vector3(0, 270, 0), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ITEM_SHIELDING_CORE", ItemDisplays.LoadAetheriumDisplay("ShieldingCore"), "Chest", new Vector3(0, 0.001f, -0.002f), new Vector3(0, 180, 0), new Vector3(0.002f, 0.002f, 0.002f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ITEM_UNSTABLE_DESIGN", ItemDisplays.LoadAetheriumDisplay("UnstableDesign"), "Chest", new Vector3(0, -0.0012f, -0.0012f), new Vector3(0, 45, 0), new Vector3(0.01f, 0.01f, 0.01f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ITEM_WEIGHTED_ANKLET", ItemDisplays.LoadAetheriumDisplay("WeightedAnklet"), "CalfR", new Vector3(0, 0.001f, 0), new Vector3(0, 0, 0), new Vector3(0.003f, 0.003f, 0.003f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ITEM_BLASTER_SWORD", ItemDisplays.LoadAetheriumDisplay("BlasterSword"), "HandL", new Vector3(-0.0004f, 0.001f, 0.0055f), new Vector3(0, 90, 270), new Vector3(0.001f, 0.001f, 0.001f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ITEM_WITCHES_RING", ItemDisplays.LoadAetheriumDisplay("WitchesRing"), "Sword", new Vector3(0, 0, 0), new Vector3(0, 180, 0), new Vector3(0.0015f, 0.0015f, 0.0015f)));

                itemRules.Add(ItemDisplays.CreateFollowerDisplayRule("ITEM_ALIEN_MAGNET", ItemDisplays.LoadAetheriumDisplay("AlienMagnet"), new Vector3(0.008f, 0.002f, -0.004f), new Vector3(0, 0, 0), new Vector3(0.001f, 0.001f, 0.001f)));
                itemRules.Add(ItemDisplays.CreateFollowerDisplayRule("ITEM_INSPIRING_DRONE", ItemDisplays.LoadAetheriumDisplay("InspiringDrone"), new Vector3(-0.014f, 0.006f, -0.014f), new Vector3(0, 0, 0), new Vector3(0.0015f, 0.0015f, 0.0015f)));

                equipmentRules.Add(ItemDisplays.CreateFollowerDisplayRule("EQUIPMENT_JAR_OF_RESHAPING", ItemDisplays.LoadAetheriumDisplay("JarOfReshaping"), new Vector3(0.008f, 0.015f, 0), new Vector3(0, 270, 270), new Vector3(0.001f, 0.001f, 0.001f)));
            }
            #endregion
            #region SivsItems
            if (PaladinPlugin.sivsItemsInstalled)
            {
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BeetlePlush", ItemDisplays.LoadSivDisplay("BeetlePlush"), "Chest", new Vector3(0, 0.004f, -0.002f), new Vector3(0, 0, 0), new Vector3(0.01f, 0.01f, 0.01f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("BisonShield", ItemDisplays.LoadSivDisplay("BisonShield"), "ElbowR", new Vector3(-0.001f, 0.002f, 0), new Vector3(0, 270, 0), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("FlameGland", ItemDisplays.LoadSivDisplay("FlameGland"), "ElbowL", new Vector3(0, 0.0028f, 0), new Vector3(0, 0, 180), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Geode", ItemDisplays.LoadSivDisplay("Geode"), "CalfR", new Vector3(0, 0.0028f, -0.001f), new Vector3(0, 270, 270), new Vector3(0.003f, 0.003f, 0.003f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Tarbine", ItemDisplays.LoadSivDisplay("Tarbine"), "ElbowL", new Vector3(0, 0.004f, 0), new Vector3(0, 0, 0), new Vector3(0.006f, 0.005f, 0.006f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("Tentacle", ItemDisplays.LoadSivDisplay("Tentacle"), "Head", new Vector3(0, 0.002f, -0.0012f), new Vector3(0, 0, 270), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ImpEye", ItemDisplays.LoadSivDisplay("ImpEye"), "Head", new Vector3(0, 0.0011f, 0.0012f), new Vector3(0, 0, 0), new Vector3(0.005f, 0.005f, 0.005f)));

                itemRules.Add(ItemDisplays.CreateFollowerDisplayRule("NullSeed", ItemDisplays.LoadSivDisplay("NullSeed"), new Vector3(0.008f, 0.005f, 0), new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
            }
            #endregion
            #region SupplyDrop
            if (PaladinPlugin.supplyDropInstalled)
            {
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SUPPDRPElectroPlankton", ItemDisplays.LoadSupplyDropDisplay("ElectroPlankton"), "Chest", new Vector3(0, 0, -0.003f), new Vector3(90, 90, 0), new Vector3(0.001f, 0.001f, 0.001f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SUPPDRPHardenedBoneFragments", ItemDisplays.LoadSupplyDropDisplay("HardenedBoneFragments"), "Chest", new Vector3(-0.002f, 0.0035f, 0), new Vector3(0, 270, 0), new Vector3(0.015f, 0.015f, 0.015f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SUPPDRPQSGen", ItemDisplays.LoadSupplyDropDisplay("QSGen"), "ElbowL", new Vector3(0, 0.002f, 0), new Vector3(0, 0, 270), new Vector3(0.001f, 0.001f, 0.001f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SUPPDRPSalvagedWires", ItemDisplays.LoadSupplyDropDisplay("SalvagedWires"), "Pelvis", new Vector3(0, 0, -0.002f), new Vector3(50, 300, 0), new Vector3(0.005f, 0.005f, 0.005f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SUPPDRPShellPlating", ItemDisplays.LoadSupplyDropDisplay("ShellPlating"), "Pelvis", new Vector3(0, 0.002f, 0.003f), new Vector3(30, 0, 0), new Vector3(0.0015f, 0.0015f, 0.0015f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SUPPDRPPlagueHat", ItemDisplays.LoadSupplyDropDisplay("PlagueHat"), "Head", new Vector3(-7.683411E-09f, 0.002843072f, 0.0008528258f), new Vector3(333.9211f, 180, -2.266285E-13f), new Vector3(0.001f, 0.001f, 0.001f)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("SUPPDRPPlagueMask", ItemDisplays.LoadSupplyDropDisplay("PlagueMask"), "Head", new Vector3(-2.677552E-09f, 0.001063528f, 0.002444532f), new Vector3(0, 180, 0), new Vector3(0.0015f, 0.0015f, 0.0015f)));

                itemRules.Add(ItemDisplays.CreateFollowerDisplayRule("SUPPDRPBloodBook", ItemDisplays.LoadSupplyDropDisplay("BloodBook"), new Vector3(-0.008f, 0.005f, -0.008f), new Vector3(0, 0, 0), new Vector3(0.08f, 0.08f, 0.08f)));
            }
            #endregion
            #region Starstorm2
            if (PaladinPlugin.starstormInstalled)
            {
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ITEM_FORK_NAME", "DisplayMissileRack", "Chest", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ITEM_EXPOVERTIME_NAME", "DisplayMissileRack", "Chest", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ITEM_TREMATODE_NAME", "DisplayMissileRack", "Chest", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ITEM_COIN_NAME", "DisplayMissileRack", "Chest", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ITEM_FIREONEQUIPMENTUSE_NAME", "DisplayMissileRack", "Chest", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ITEM_DRONEONELITEKILL_NAME", "DisplayMissileRack", "Chest", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ITEM_SOULONKILL_NAME", "DisplayMissileRack", "Chest", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ITEM_BLOODTESTER_NAME", "DisplayMissileRack", "Chest", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ITEM_SPRINTHEAL_NAME", "DisplayMissileRack", "Chest", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0)));
                itemRules.Add(ItemDisplays.CreateGenericDisplayRule("ITEM_COFFEE_NAME", "DisplayMissileRack", "Chest", new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0)));
            }
            #endregion

            ItemDisplayRuleSet.NamedRuleGroup[] item = itemRules.ToArray();
            ItemDisplayRuleSet.NamedRuleGroup[] equip = equipmentRules.ToArray();
            itemDisplayRuleSet.namedItemRuleGroups = item;
            itemDisplayRuleSet.namedEquipmentRuleGroups = equip;
        }

        public static ItemDisplayRuleSet.NamedRuleGroup CreateGenericDisplayRule(string itemName, string prefabName, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet.NamedRuleGroup displayRule = new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = itemName,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = childName,
                            followerPrefab = ItemDisplays.LoadDisplay(prefabName),
                            limbMask = LimbFlags.None,
                            localPos = position,
                            localAngles = rotation,
                            localScale = scale
                        }
                    }
                }
            };

            return displayRule;
        }
        public static ItemDisplayRuleSet.NamedRuleGroup CreateGenericDisplayRule(string itemName, GameObject itemPrefab, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet.NamedRuleGroup displayRule = new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = itemName,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = childName,
                            followerPrefab = itemPrefab,
                            limbMask = LimbFlags.None,
                            localPos = position,
                            localAngles = rotation,
                            localScale = scale
                        }
                    }
                }
            };

            return displayRule;
        }

        public static ItemDisplayRuleSet.NamedRuleGroup CreateFollowerDisplayRule(string itemName, string prefabName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet.NamedRuleGroup displayRule = new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = itemName,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = "Base",
                            followerPrefab = ItemDisplays.LoadDisplay(prefabName),
                            limbMask = LimbFlags.None,
                            localPos = position,
                            localAngles = rotation,
                            localScale = scale
                        }
                    }
                }
            };

            return displayRule;
        }
        public static ItemDisplayRuleSet.NamedRuleGroup CreateFollowerDisplayRule(string itemName, GameObject itemPrefab, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet.NamedRuleGroup displayRule = new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = itemName,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = "Base",
                            followerPrefab = itemPrefab,
                            limbMask = LimbFlags.None,
                            localPos = position,
                            localAngles = rotation,
                            localScale = scale
                        }
                    }
                }
            };

            return displayRule;
        }

        private static void PopulateDisplays()
        {
            ItemDisplayRuleSet itemDisplayRuleSet = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet;

            capacitorPrefab = PrefabAPI.InstantiateClone(itemDisplayRuleSet.FindEquipmentDisplayRuleGroup("Lightning").rules[0].followerPrefab, "DisplayPaladinLightning", true);
            capacitorPrefab.AddComponent<UnityEngine.Networking.NetworkIdentity>();

            var limbMatcher = capacitorPrefab.GetComponent<LimbMatcher>();

            limbMatcher.limbPairs[0].targetChildLimb = "ShoulderL";
            limbMatcher.limbPairs[1].targetChildLimb = "ElbowL";
            limbMatcher.limbPairs[2].targetChildLimb = "HandL";

            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            ItemDisplayRuleSet.NamedRuleGroup[] array = typeof(ItemDisplayRuleSet).GetField("namedItemRuleGroups", bindingAttr).GetValue(itemDisplayRuleSet) as ItemDisplayRuleSet.NamedRuleGroup[];
            ItemDisplayRuleSet.NamedRuleGroup[] array2 = typeof(ItemDisplayRuleSet).GetField("namedEquipmentRuleGroups", bindingAttr).GetValue(itemDisplayRuleSet) as ItemDisplayRuleSet.NamedRuleGroup[];
            ItemDisplayRuleSet.NamedRuleGroup[] array3 = array;

            for (int i = 0; i < array3.Length; i++)
            {
                ItemDisplayRule[] rules = array3[i].displayRuleGroup.rules;
                for (int j = 0; j < rules.Length; j++)
                {
                    GameObject followerPrefab = rules[j].followerPrefab;
                    if (!(followerPrefab == null))
                    {
                        string name = followerPrefab.name;
                        string key = (name != null) ? name.ToLower() : null;
                        if (!itemDisplayPrefabs.ContainsKey(key))
                        {
                            itemDisplayPrefabs[key] = followerPrefab;
                        }
                    }
                }
            }

            array3 = array2;
            for (int i = 0; i < array3.Length; i++)
            {
                ItemDisplayRule[] rules = array3[i].displayRuleGroup.rules;
                for (int j = 0; j < rules.Length; j++)
                {
                    GameObject followerPrefab2 = rules[j].followerPrefab;
                    if (!(followerPrefab2 == null))
                    {
                        string name2 = followerPrefab2.name;
                        string key2 = (name2 != null) ? name2.ToLower() : null;
                        if (!itemDisplayPrefabs.ContainsKey(key2))
                        {
                            itemDisplayPrefabs[key2] = followerPrefab2;
                        }
                    }
                }
            }
        }

        public static GameObject LoadDisplay(string name)
        {
            if (itemDisplayPrefabs.ContainsKey(name.ToLower()))
            {
                if (itemDisplayPrefabs[name.ToLower()]) return itemDisplayPrefabs[name.ToLower()];
            }
            return null;
        }

        
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static GameObject LoadAetheriumDisplay(string name)
        {
            switch (name)
            {
                case "AccursedPotion":
                    return Aetherium.Items.AccursedPotion.ItemBodyModelPrefab;
                case "AlienMagnet":
                    return Aetherium.Items.AlienMagnet.ItemFollowerPrefab;
                case "BlasterSword":
                    return Aetherium.Items.BlasterSword.ItemBodyModelPrefab;
                case "BloodSoakedShield":
                    return Aetherium.Items.BloodSoakedShield.ItemBodyModelPrefab;
                case "FeatheredPlume":
                    return Aetherium.Items.FeatheredPlume.ItemBodyModelPrefab;
                case "InspiringDrone":
                    return Aetherium.Items.InspiringDrone.ItemFollowerPrefab;
                case "SharkTeeth":
                    return Aetherium.Items.SharkTeeth.ItemBodyModelPrefab;
                case "ShieldingCore":
                    return Aetherium.Items.ShieldingCore.ItemBodyModelPrefab;
                case "UnstableDesign":
                    return Aetherium.Items.UnstableDesign.ItemBodyModelPrefab;
                case "VoidHeart":
                    return Aetherium.Items.Voidheart.ItemBodyModelPrefab;
                case "WeightedAnklet":
                    return Aetherium.Items.WeightedAnklet.ItemBodyModelPrefab;
                case "WitchesRing":
                    return Aetherium.Items.WitchesRing.ItemBodyModelPrefab;
                case "JarOfReshaping":
                    return Aetherium.Equipment.JarOfReshaping.ItemBodyModelPrefab;
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static GameObject LoadSivDisplay(string name)
        {
            switch (name)
            {
                case "BeetlePlush":
                    return SivsItemsRoR2.BeetlePlush.displayPrefab;
                case "BisonShield":
                    return SivsItemsRoR2.BisonShield.displayPrefab;
                case "FlameGland":
                    return SivsItemsRoR2.FlameGland.displayPrefab;
                case "Geode":
                    return SivsItemsRoR2.Geode.displayPrefab;
                case "ImpEye":
                    return SivsItemsRoR2.ImpEye.displayPrefab;
                case "NullSeed":
                    return SivsItemsRoR2.NullSeed.displayPrefab;
                case "Tarbine":
                    return SivsItemsRoR2.Tarbine.displayPrefab;
                case "Tentacle":
                    return SivsItemsRoR2.Tentacle.displayPrefab;
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static GameObject LoadSupplyDropDisplay(string name)
        {
            switch (name)
            {
                case "BloodBook":
                    return SupplyDrop.Items.BloodBook.ItemBodyModelPrefab;
                case "ElectroPlankton":
                    return SupplyDrop.Items.ElectroPlankton.ItemBodyModelPrefab;
                case "HardenedBoneFragments":
                    return SupplyDrop.Items.HardenedBoneFragments.ItemBodyModelPrefab;
                case "PlagueHat":
                    return SupplyDrop.Items.PlagueHat.ItemBodyModelPrefab;
                case "PlagueMask":
                    return SupplyDrop.Items.PlagueMask.ItemBodyModelPrefab;
                case "QSGen":
                    return SupplyDrop.Items.QSGen.ItemBodyModelPrefab;
                case "SalvagedWires":
                    return SupplyDrop.Items.SalvagedWires.ItemBodyModelPrefab;
                case "ShellPlating":
                    return SupplyDrop.Items.ShellPlating.ItemBodyModelPrefab;
                case "UnassumingTie":
                    return SupplyDrop.Items.UnassumingTie.ItemBodyModelPrefab;
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static GameObject LoadStarstormDisplay(string name)
        {
            // make your fucking items public reeee
            /*switch (name)
            {
                case "Fork":
                    return Starstorm2.Cores.Items.
            }*/
            return null;
        }
    }
}