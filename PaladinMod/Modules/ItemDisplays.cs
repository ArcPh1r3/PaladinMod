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
        internal static ItemDisplayRuleSet itemDisplayRuleSet;
        internal static List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules;

        private static Dictionary<string, GameObject> itemDisplayPrefabs = new Dictionary<string, GameObject>();

        internal static void InitializeItemDisplays()
        {
            PopulateDisplays();

            CharacterModel characterModel = Modules.Prefabs.paladinPrefab.GetComponentInChildren<CharacterModel>();

            itemDisplayRuleSet = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();
            itemDisplayRuleSet.name = "idrsPaladin";

            characterModel.itemDisplayRuleSet = itemDisplayRuleSet;
        }

        internal static void SetItemDisplays()
        {
            if (PaladinMod.PaladinPlugin.VRInstalled)
                return;

            itemDisplayRules = new List<ItemDisplayRuleSet.KeyAssetRuleGroup>();

            // add item displays here
            //  HIGHLY recommend using KingEnderBrine's ItemDisplayPlacementHelper mod for this

            #region Regular Item Displays
            //enderbrine custom copy thing
/*

                            childName = {childName},
                            localPos = {localPos},
                            localAngles = {localAngles},
                            localScale = {localScale}
*/

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup {
                keyAsset = RoR2Content.Equipment.Jetpack,
                displayRuleGroup = new DisplayRuleGroup {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBugWings"),
childName = "Chest",
localPos = new Vector3(-0.0008F, 0.1986F, -0.1308F),
localAngles = new Vector3(21.4993F, 358.6616F, 358.3334F),
localScale = new Vector3(0.2605F, 0.2605F, 0.2605F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.GoldGat,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGoldGat"),
childName = "Chest",
localPos = new Vector3(-0.2462F, 0.6317F, 0.0311F),
localAngles = new Vector3(329.078F, 83.1908F, 333.7404F),
localScale = new Vector3(0.15F, 0.15F, 0.15F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.BFG,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBFG"),
                            childName = "Chest",
                            localPos = new Vector3(-0.3665F, 0.39016F, -0.09635F),
                            localAngles = new Vector3(0F, 0F, 43.71795F),
                            localScale = new Vector3(0.41306F, 0.41306F, 0.41306F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });
                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.CritGlasses,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGlasses"),
                            childName = "Head",
                            localPos = new Vector3(0F, 0.15267F, 0.20987F),
                            localAngles = new Vector3(0F, 0F, 90F),
                            localScale = new Vector3(0.29887F, 0.31575F, 0.31575F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Syringe,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySyringeCluster"),
                            childName = "UpperArmL",
localPos = new Vector3(0.09334F, -0.01443F, -0.15909F),
localAngles = new Vector3(16.62452F, 48.5012F, 268.5267F),
localScale = new Vector3(0.15463F, 0.16198F, 0.15463F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Behemoth,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBehemoth"),
childName = "LowerArmL",
localPos = new Vector3(0F, 0.3364F, -0.2083F),
localAngles = new Vector3(0F, 180F, 0F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Missile,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMissileLauncher"),
                            childName = "Chest",
                            localPos = new Vector3(0.42197F, 0.61158F, -0.53733F),
                            localAngles = new Vector3(310.2395F, 10.78981F, 335.808F),
                            localScale = new Vector3(0.12615F, 0.12615F, 0.12615F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Dagger,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDagger"),
childName = "Chest",
localPos = new Vector3(0.1386F, 0.4594F, 0.1186F),
localAngles = new Vector3(334.8839F, 31.5284F, 34.6784F),
localScale = new Vector3(1.2428F, 1.2428F, 1.2299F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Hoof,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayHoof"),
                            childName = "CalfL",
                            localPos = new Vector3(-0.03586F, 0.49177F, -0.04191F),
                            localAngles = new Vector3(80.42696F, 8.01645F, 347.4096F),
                            localScale = new Vector3(0.17124F, 0.16339F, 0.14453F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.ChainLightning,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayUkulele"),
                            childName = "Chest",
                            localPos = new Vector3(0.0788F, 0.4095F, -0.19088F),
                            localAngles = new Vector3(323.0142F, 180.056F, 89.31227F),
                            localScale = new Vector3(0.98885F, 0.98885F, 0.98885F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.GhostOnKill,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMask"),
                            childName = "Head",
                            localPos = new Vector3(0.00017F, 0.19785F, 0.19316F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.75096F, 0.75096F, 0.75096F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Mushroom,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMushroom"),
                            childName = "UpperArmR",
                            localPos = new Vector3(-0.03466F, 0.36978F, -0.114F),
                            localAngles = new Vector3(359.4572F, 277.4478F, 79.3168F),
                            localScale = new Vector3(0.08371F, 0.08371F, 0.08371F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.AttackSpeedOnCrit,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWolfPelt"),
childName = "HandR",
localPos = new Vector3(0.0297F, 0.1028F, -0.0096F),
localAngles = new Vector3(286.8817F, 107.8454F, 0F),
localScale = new Vector3(0.3814F, 0.3814F, 0.3814F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.BleedOnHit,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTriTip"),
                            childName = "HandL",
                            localPos = new Vector3(-0.07315F, 0.11133F, -0.15195F),
                            localAngles = new Vector3(11.41095F, 180F, 180F),
                            localScale = new Vector3(0.76274F, 0.76274F, 0.79862F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.WardOnLevel,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarbanner"),
                            childName = "Stomach",
                            localPos = new Vector3(0.01435F, 0.17578F, -0.23544F),
                            localAngles = new Vector3(315.682F, 177.5153F, 272.9106F),
                            localScale = new Vector3(0.53229F, 0.53229F, 0.53229F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.HealOnCrit,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayScythe"),
                            childName = "HandL",
                            localPos = new Vector3(-0.06617F, 0.2783F, 0.8635F),
                            localAngles = new Vector3(345.3667F, 0.23077F, 295.8703F),
                            localScale = new Vector3(0.71734F, 0.6443F, 0.86362F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.HealWhileSafe,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySnail"),
                            childName = "UpperArmL",
                            localPos = new Vector3(0.2047F, 0.24823F, 0.04136F),
                            localAngles = new Vector3(66.66012F, 31.80719F, 293.0543F),
                            localScale = new Vector3(0.10311F, 0.1079F, 0.1079F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Clover,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayClover"),
                            childName = "Sword",
                            localPos = new Vector3(0.00111F, 0.1156F, -0.05202F),
                            localAngles = new Vector3(85.61921F, 0.0001F, 179.4897F),
                            localScale = new Vector3(0.3406F, 0.3406F, 0.3406F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.BarrierOnOverHeal,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAegis"),
                            childName = "LowerArmL",
                            localPos = new Vector3(0.09811F, 0.15264F, -0.04067F),
                            localAngles = new Vector3(83.36375F, 102.634F, 171.2473F),
                            localScale = new Vector3(0.4326F, 0.4326F, 0.4326F),
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.GoldOnHit,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBoneCrown"),
childName = "Head",
localPos = new Vector3(0F, 0.2717F, 0.0252F),
localAngles = new Vector3(4.7434F, 0F, 0F),
localScale = new Vector3(1.4258F, 1.4258F, 1.4258F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.WarCryOnMultiKill,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayPauldron"),
childName = "UpperArmL",
localPos = new Vector3(0.1796F, 0.1229F, 0.0515F),
localAngles = new Vector3(67.4722F, 127.3766F, 33.8939F),
localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.SprintArmor,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBuckler"),
childName = "LowerArmR",
localPos = new Vector3(-0.0838F, 0.285F, 0.0144F),
localAngles = new Vector3(0F, 275.0831F, 0F),
localScale = new Vector3(0.3351F, 0.3351F, 0.3351F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.IceRing,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayIceRing"),
                            childName = "Sword",
                            localPos = new Vector3(-0.00076F, 0.18081F, -0.00001F),
                            localAngles = new Vector3(274.3965F, 90.00002F, 230.002F),
                            localScale = new Vector3(0.69889F, 0.69889F, 0.58687F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.FireRing,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFireRing"),
                            childName = "Sword",
                            localPos = new Vector3(0.00438F, 0.26471F, -0.00007F),
                            localAngles = new Vector3(274.3965F, 90F, 270F),
                            localScale = new Vector3(0.67664F, 0.67664F, 0.53723F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.UtilitySkillMagazine,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
                            childName = "Chest",
                            localPos = new Vector3(-0.22787F, 0.2429F, 0.00309F),
                            localAngles = new Vector3(10.5144F, 23.64493F, 229.4454F),
                            localScale = new Vector3(1.16631F, 1.16631F, 1.16631F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
                            childName = "Chest",
                            localPos = new Vector3(0.23902F, 0.2539F, 0.0102F),
                            localAngles = new Vector3(11.62711F, 351.9313F, 140.7097F),
                            localScale = new Vector3(1.16631F, 1.16631F, 1.16631F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.JumpBoost,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWaxBird"),
                            childName = "Head",
                            localPos = new Vector3(0F, -0.26514F, -0.05741F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(1.07913F, 1.07913F, 1.07913F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.ArmorReductionOnHit,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarhammer"),
                            childName = "HandL",
                            localPos = new Vector3(-0.06382F, 0.13177F, 0.5327F),
                            localAngles = new Vector3(0F, 0F, 297.256F),
                            localScale = new Vector3(0.386F, 0.386F, 0.386F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.NearbyDamageBonus,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDiamond"),
childName = "Sword",
localPos = new Vector3(-0.002F, -0.1622F, 0F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.1236F, 0.1236F, 0.1236F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.ArmorPlate,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRepulsionArmorPlate"),
                            childName = "ThighL",
                            localPos = new Vector3(-0.10249F, 0.58427F, -0.17409F),
                            localAngles = new Vector3(86.35638F, 163.7241F, 142.088F),
                            localScale = new Vector3(0.4224F, 0.4224F, 0.4224F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.CommandMissile,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMissileRack"),
                            childName = "Chest",
                            localPos = new Vector3(-0.24523F, 0.17274F, -0.39525F),
                            localAngles = new Vector3(73.64906F, 217.7356F, 8.9834F),
                            localScale = new Vector3(0.46965F, 0.51638F, 0.46965F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Feather,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFeather"),
childName = "LowerArmL",
localPos = new Vector3(-0.01F, 0.2291F, -0.0741F),
localAngles = new Vector3(270F, 333.876F, 0F),
localScale = new Vector3(0.0296F, 0.0296F, 0.0296F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Crowbar,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayCrowbar"),
                            childName = "Chest",
localPos = new Vector3(0.3003F, -0.09365F, 0.10324F),
localAngles = new Vector3(55.34839F, 0F, 340.98F),
localScale = new Vector3(0.63193F, 0.63193F, 0.63193F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.FallBoots,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGravBoots"),
childName = "FootL",
localPos = new Vector3(0F, -0.0117F, -0.0755F),
localAngles = new Vector3(52.8231F, 0F, 0F),
localScale = new Vector3(0.2769F, 0.2769F, 0.2769F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGravBoots"),
childName = "FootR",
localPos = new Vector3(0F, -0.0117F, -0.0755F),
localAngles = new Vector3(52.8231F, 0F, 0F),
localScale = new Vector3(0.2769F, 0.2769F, 0.2769F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.ExecuteLowHealthElite,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGuillotine"),
childName = "ThighL",
localPos = new Vector3(-0.1019F, 0.0999F, -0.2948F),
localAngles = new Vector3(69.2487F, 30.7051F, 9.5951F),
localScale = new Vector3(0.2827F, 0.2827F, 0.2827F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.EquipmentMagazine,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBattery"),
                            childName = "Chest",
localPos = new Vector3(0.0155F, -0.21842F, 0.35325F),
localAngles = new Vector3(0F, 270F, 270.1707F),
localScale = new Vector3(0.28086F, 0.28086F, 0.31495F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.NovaOnHeal,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDevilHorns"),
childName = "Head",
localPos = new Vector3(0.0949F, 0.0945F, 0.0654F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.5349F, 0.5349F, 0.5349F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDevilHorns"),
childName = "Head",
localPos = new Vector3(-0.0949F, 0.0945F, 0.0105F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(-0.5349F, 0.5349F, 0.5349F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Infusion,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayInfusion"),
                            childName = "Pelvis",
                            localPos = new Vector3(0.39826F, -0.04497F, -0.01683F),
                            localAngles = new Vector3(358.1862F, 103.6487F, 355.2555F),
                            localScale = new Vector3(0.60762F, 0.60762F, 0.60762F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Medkit,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMedkit"),
                            childName = "Chest",
                            localPos = new Vector3(0.15604F, -0.11509F, -0.23192F),
                            localAngles = new Vector3(273.9797F, 42.32616F, 131.5229F),
                            localScale = new Vector3(0.87062F, 0.87062F, 0.87062F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Bandolier,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBandolier"),
childName =    "Pelvis",
localPos =     new Vector3(0.06426F, 0.23938F, 0.01359F),
localAngles =  new Vector3(270F, 0F, 0F),
localScale =   new Vector3(0.70869F, 0.55526F, 0.95596F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.BounceNearby,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayHook"),
childName = "Chest",
localPos = new Vector3(0.2497F, 0.4516F, -0.0029F),
localAngles = new Vector3(20.4029F, 220.929F, 17.9384F),
localScale = new Vector3(0.5259F, 0.5259F, 0.5259F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.IgniteOnKill,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGasoline"),
                            childName = "ThighR",
                            localPos = new Vector3(0.14949F, 0.37864F, 0.10489F),
                            localAngles = new Vector3(81.5624F, 86.66078F, 121.9414F),
                            localScale = new Vector3(0.51681F, 0.51681F, 0.51681F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.StunChanceOnHit,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayStunGrenade"),
                            childName = "ThighR",
                            localPos = new Vector3(0.09323F, 0.38066F, 0.17491F),
                            localAngles = new Vector3(90F, 20.24317F, 0F),
                            localScale = new Vector3(1.1789F, 1.3164F, 1.1789F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Firework,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFirework"),
childName = "Pelvis",
localPos = new Vector3(0.0086F, 0.0069F, 0.0565F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.1194F, 0.1194F, 0.1194F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.LunarDagger,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLunarDagger"),
                            childName = "Chest",
                            localPos = new Vector3(0.21493F, 0.2578F, -0.34603F),
                            localAngles = new Vector3(35.08394F, 102.2764F, 108.5335F),
                            localScale = new Vector3(0.64557F, 0.64557F, 0.64557F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = DLC1Content.Items.EquipmentMagazineVoid,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayKnurl"),
                            childName = "LowerArmL",
                            localPos = new Vector3(0.10907F, 0.0794F, -0.03068F),
                            localAngles = new Vector3(78.87074F, 36.6722F, 105.8275F),
                            localScale = new Vector3(0.0848F, 0.0848F, 0.0848F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.BeetleGland,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBeetleGland"),
                            childName = "Chest",
                            localPos = new Vector3(-0.36553F, -0.46196F, -0.04279F),
                            localAngles = new Vector3(343.1537F, 334.3483F, 107.148F),
                            localScale = new Vector3(0.09533F, 0.09533F, 0.09533F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.SprintBonus,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySoda"),
                            childName = "Pelvis",
                            localPos = new Vector3(0.14635F, 0.12776F, 0.32446F),
                            localAngles = new Vector3(290.8455F, 161.0168F, 89.99998F),
                            localScale = new Vector3(0.56123F, 0.56123F, 0.67478F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.SecondarySkillMagazine,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDoubleMag"),
                            childName = "Sword",
                            localPos = new Vector3(0.24705F, 0.32655F, -0.2121F),
                            localAngles = new Vector3(81.25533F, 310.0088F, 357.9419F),
                            localScale = new Vector3(0.0835F, 0.0835F, 0.0835F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.StickyBomb,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayStickyBomb"),
                            childName = "Pelvis",
                            localPos = new Vector3(0.21888F, 0.04932F, -0.24647F),
                            localAngles = new Vector3(348.3564F, 182.7943F, 162.5857F),
                            localScale = new Vector3(0.36705F, 0.36705F, 0.36705F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.TreasureCache,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayKey"),
                            childName = "ThighL",
localPos = new Vector3(-0.18865F, 0.53931F, -0.02826F),
localAngles = new Vector3(31.98337F, 193.9092F, 258.6169F),
localScale = new Vector3(1.42076F, 1.42076F, 1.42076F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.BossDamageBonus,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAPRound"),
                            childName = "Pelvis",
                            localPos = new Vector3(0.31204F, 0.23809F, 0.1011F),
                            localAngles = new Vector3(270F, 62.4899F, 0F),
                            localScale = new Vector3(0.74894F, 0.74894F, 0.74894F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.SlowOnHit,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBauble"),
childName = "Pelvis",
localPos = new Vector3(-0.0074F, 0.076F, -0.0864F),
localAngles = new Vector3(0F, 23.7651F, 0F),
localScale = new Vector3(0.0687F, 0.0687F, 0.0687F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.ExtraLife,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayHippo"),
childName = "Chest",
localPos = new Vector3(0F, 0.4728F, 0.2285F),
localAngles = new Vector3(18.9577F, 0F, 0F),
localScale = new Vector3(0.2517F, 0.2517F, 0.2517F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.KillEliteFrenzy,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBrainstalk"),
                            childName = "Head",
                            localPos = new Vector3(0F, 0.29373F, 0F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.29283F, 0.34818F, 0.34818F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.RepeatHeal,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayCorpseFlower"),
                            childName = "UpperArmR",
                            localPos = new Vector3(-0.14749F, 0.35508F, 0.00001F),
                            localAngles = new Vector3(280.9339F, 90F, 0F),
                            localScale = new Vector3(0.30504F, 0.30504F, 0.30504F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.AutoCastEquipment,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFossil"),
                            childName = "Chest",
                            localPos = new Vector3(-0.30499F, -0.26301F, 0.00984F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.6221F, 0.6221F, 0.6221F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.IncreaseHealing,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAntler"),
childName = "Head",
localPos = new Vector3(0.1003F, 0.269F, 0F),
localAngles = new Vector3(0F, 90F, 0F),
localScale = new Vector3(0.3395F, 0.3395F, 0.3395F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAntler"),
childName = "Head",
localPos = new Vector3(-0.1003F, 0.269F, 0F),
localAngles = new Vector3(0F, 90F, 0F),
localScale = new Vector3(0.3395F, 0.3395F, -0.3395F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.TitanGoldDuringTP,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGoldHeart"),
                            childName = "Chest",
                            localPos = new Vector3(0.18022F, 0.21853F, 0.45628F),
                            localAngles = new Vector3(318.1987F, 339.556F, 6.69217F),
                            localScale = new Vector3(0.31485F, 0.31485F, 0.31485F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.SprintWisp,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBrokenMask"),
                            childName = "CalfR",
                            localPos = new Vector3(0.17945F, 0.26677F, -0.04186F),
                            localAngles = new Vector3(357.2185F, 90.88068F, 180.2081F),
                            localScale = new Vector3(0.31593F, 0.31593F, 0.31593F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.BarrierOnKill,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBrooch"),
                            childName = "UpperArmL",
                            localPos = new Vector3(0.17652F, 0.19117F, -0.13338F),
                            localAngles = new Vector3(288.8441F, 284.269F, 41.3635F),
                            localScale = new Vector3(0.87323F, 1.27421F, 0.87323F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.TPHealingNova,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGlowFlower"),
                            childName = "UpperArmL",
                            localPos = new Vector3(-0.02848F, 0.18307F, -0.14444F),
                            localAngles = new Vector3(344.8872F, 185.1642F, 269.1133F),
                            localScale = new Vector3(0.53685F, 0.53685F, 0.53685F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.LunarUtilityReplacement,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBirdFoot"),
                            childName = "Head",
                            localPos = new Vector3(-0.22824F, 0.23022F, -0.00176F),
                            localAngles = new Vector3(34.03704F, 341.4791F, 39.8481F),
                            localScale = new Vector3(0.6782F, 0.6782F, 0.6782F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Thorns,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRazorwireLeft"),
childName = "UpperArmL",
localPos = new Vector3(0F, 0F, 0F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.844F, 0.844F, 0.844F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.LunarPrimaryReplacement,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBirdEye"),
                            childName = "Head",
                            localPos = new Vector3(-0.0014F, 0.2029F, 0.20643F),
                            localAngles = new Vector3(270F, 0F, 0F),
                            localScale = new Vector3(0.29836F, 0.29836F, 0.29836F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.LunarSecondaryReplacement,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBirdClaw"),
childName = "LowerArmL",
localPos = new Vector3(0.2074F, 0.2609F, 0F),
localAngles = new Vector3(0F, 0F, 291.897F),
localScale = new Vector3(0.6914F, 0.6914F, 0.6914F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.LunarSpecialReplacement,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBirdHeart"),
                            childName = "Root",
localPos = new Vector3(-1.3761F, -0.06751F, 2.9999F),
localAngles = new Vector3(90F, 0F, 0F),
localScale = new Vector3(0.32864F, 0.32864F, 0.32864F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.ParentEgg,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayParentEgg"),
childName = "Chest",
localPos = new Vector3(0F, -0.0592F, 0.5884F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.2067F, 0.2067F, 0.2067F),
                            limbMask = LimbFlags.None
                        }
        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.LightningStrikeOnHit,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayChargedPerforator"),
                            childName = "SwordSmearBack",
                            localPos = new Vector3(-0.07734F, 0.0234F, 0.06404F),
                            localAngles = new Vector3(312.1713F, 309.1519F, 0.81581F),
                            localScale = new Vector3(2.61669F, 2.85994F, 2.85994F),
                            limbMask = LimbFlags.None
                        }
        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.NovaOnLowHealth,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayJellyGuts"),
                            childName = "LowerArmR",
                            localPos = new Vector3(-0.0484F, -0.0116F, 0.0283F),
                            localAngles = new Vector3(285.9941F, 117.7397F, 228.4666F),
                            localScale = new Vector3(0.14706F, 0.14706F, 0.14706F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.LunarTrinket,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBeads"),
                            childName = "LowerArmL",
                            localPos = new Vector3(0.02837F, 0.58684F, 0.02921F),
                            localAngles = new Vector3(25.94004F, 8.50985F, 125.7469F),
                            localScale = new Vector3(1.15324F, 1.15324F, 1.15324F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Plant,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayInterstellarDeskPlant"),
                            childName = "UpperArmL",
                            localPos = new Vector3(0.03594F, -0.02337F, -0.19957F),
                            localAngles = new Vector3(7.37013F, 180.0893F, 176.2962F),
                            localScale = new Vector3(0.11108F, 0.11108F, 0.11108F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Bear,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBear"),
childName = "Chest",
localPos = new Vector3(0F, 0.5822F, -0.0926F),
localAngles = new Vector3(29.2877F, 180F, 0F),
localScale = new Vector3(0.3416F, 0.3416F, 0.3416F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.DeathMark,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDeathMark"),
childName = "HandL",
localPos = new Vector3(0.0341F, 0.0741F, 0.0122F),
localAngles = new Vector3(59.9097F, 99.2932F, 193.9937F),
localScale = new Vector3(0.0382F, 0.0382F, 0.0382F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.ExplodeOnDeath,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWilloWisp"),
childName = "Pelvis",
localPos = new Vector3(0.3575F, 0.1348F, -0.0543F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.0761F, 0.0761F, 0.0761F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Seed,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySeed"),
                            childName = "UpperArmL",
                            localPos = new Vector3(0.23883F, -0.01097F, -0.05476F),
                            localAngles = new Vector3(341.5702F, 46.20142F, 280.1449F),
                            localScale = new Vector3(0.0577F, 0.0577F, 0.0577F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.SprintOutOfCombat,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWhip"),
                            childName = "Sword",
                            localPos = new Vector3(0.1908F, 0.12436F, -0.08379F),
                            localAngles = new Vector3(51.70532F, 9.75273F, 75.62265F),
                            localScale = new Vector3(0.50462F, 0.50462F, 0.50462F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = JunkContent.Items.CooldownOnCrit,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySkull"),
childName = "Head",
localPos = new Vector3(0F, 0.2767F, 0.0851F),
localAngles = new Vector3(270F, 0F, 0F),
localScale = new Vector3(0.4448F, 0.5076F, 0.4448F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Phasing,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayStealthkit"),
                            childName = "ThighL",
                            localPos = new Vector3(-0.10296F, 0.55226F, 0.16297F),
                            localAngles = new Vector3(83.8832F, 239.1878F, 75.82954F),
                            localScale = new Vector3(0.41544F, 0.68545F, 0.45716F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.PersonalShield,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldGenerator"),
                            childName = "Chest",
                            localPos = new Vector3(-0.00614F, 0.3481F, 0.37157F),
                            localAngles = new Vector3(50.22815F, 5.08769F, 186.6075F),
                            localScale = new Vector3(0.25187F, 0.25187F, 0.25187F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.ShockNearby,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTeslaCoil"),
                            childName = "Chest",
                            localPos = new Vector3(-0.00131F, 0.23948F, -0.31578F),
                            localAngles = new Vector3(276.7108F, 5.80786F, 354.264F),
                            localScale = new Vector3(0.4892F, 0.4892F, 0.4892F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.ShieldOnly,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldBug"),
childName = "Head",
localPos = new Vector3(0.0868F, 0.3114F, 0F),
localAngles = new Vector3(348.1819F, 268.0985F, 0.3896F),
localScale = new Vector3(0.3521F, 0.3521F, 0.3521F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldBug"),
childName = "Head",
localPos = new Vector3(-0.0868F, 0.3114F, 0F),
localAngles = new Vector3(11.8181F, 268.0985F, 359.6104F),
localScale = new Vector3(0.3521F, 0.3521F, -0.3521F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.AlienHead,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAlienHead"),
                            childName = "Chest",
                            localPos = new Vector3(-0.29994F, -0.36487F, 0.13877F),
                            localAngles = new Vector3(301.0492F, 143.9774F, 143.5849F),
                            localScale = new Vector3(1.01145F, 1.01145F, 1.01145F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.HeadHunter,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySkullCrown"),
childName = "Head",
localPos = new Vector3(0F, 0.2556F, 0.0285F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.6524F, 0.2175F, 0.2175F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.EnergizedOnEquipmentUse,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarHorn"),
                            childName = "Pelvis",
                            localPos = new Vector3(-0.34249F, 0.14439F, -0.21813F),
                            localAngles = new Vector3(9.39843F, 147.476F, 307.0711F),
                            localScale = new Vector3(0.4399F, 0.4399F, 0.4399F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.FlatHealth,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySteakCurved"),
                            childName = "Head",
                            localPos = new Vector3(-0.2334F, 0.45302F, -0.08396F),
                            localAngles = new Vector3(321.8209F, 269.3395F, 181.2475F),
                            localScale = new Vector3(0.14897F, 0.1382F, 0.1382F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Tooth,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayToothMeshLarge"),
childName = "Head",
localPos = new Vector3(0F, 0.0687F, 0.0998F),
localAngles = new Vector3(344.9017F, 0F, 0F),
localScale = new Vector3(7.5452F, 7.5452F, 7.5452F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Pearl,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayPearl"),
childName = "LowerArmR",
localPos = new Vector3(0F, 0F, 0F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.ShinyPearl,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShinyPearl"),
childName = "LowerArmL",
localPos = new Vector3(0F, 0F, 0F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.BonusGoldPackOnKill,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTome"),
childName = "ThighR",
localPos = new Vector3(0.179F, 0.0959F, -0.2884F),
localAngles = new Vector3(18.5931F, 143.5536F, 357.488F),
localScale = new Vector3(0.0789F, 0.0789F, 0.0789F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Squid,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySquidTurret"),
                            childName = "LowerArmR",
                            localPos = new Vector3(0F, 0.0478F, -0.1072F),
                            localAngles = new Vector3(270F, 0F, 0F),
                            localScale = new Vector3(0.07503F, 0.06961F, 0.07503F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Icicle,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFrostRelic"),
                            childName = "Root",
localPos = new Vector3(1.41676F, -0.23F, 2.99975F),
localAngles = new Vector3(0.00002F, -0.00001F, 257.5745F),
localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.Talisman,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTalisman"),
                            childName = "Root",
localPos = new Vector3(0.8357F, 1.06047F, 3F),
localAngles = new Vector3(90F, 0F, 0F),
localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.LaserTurbine,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLaserTurbine"),
                            childName = "Chest",
                            localPos = new Vector3(-0.3079F, 0.37775F, -0.22117F),
                            localAngles = new Vector3(355.0236F, 303.7166F, 54.02683F),
                            localScale = new Vector3(0.35813F, 0.37543F, 0.50094F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.FocusConvergence,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFocusedConvergence"),
                            childName = "Root",
localPos = new Vector3(-0.54443F, 1.50272F, 3.212F),
localAngles = new Vector3(90F, 0F, 0F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = JunkContent.Items.Incubator,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAncestralIncubator"),
childName = "Chest",
localPos = new Vector3(0F, 0.3453F, 0F),
localAngles = new Vector3(353.0521F, 317.2421F, 69.6292F),
localScale = new Vector3(0.0528F, 0.0528F, 0.0528F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.FireballsOnHit,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFireballsOnHit"),
                            childName = "SwordSmearBack",
                            localPos = new Vector3(-0.22063F, 0.49744F, 0.14964F),
                            localAngles = new Vector3(32.58852F, 306.9364F, 359.3285F),
                            localScale = new Vector3(0.14044F, 0.16044F, 0.16044F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.SiphonOnLowHealth,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySiphonOnLowHealth"),
                            childName = "Stomach",
                            localPos = new Vector3(0.24877F, 0.13603F, 0.10962F),
                            localAngles = new Vector3(305.9622F, 303.4368F, 334.5179F),
                            localScale = new Vector3(0.11425F, 0.11425F, 0.11425F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.BleedOnHitAndExplode,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBleedOnHitAndExplode"),
                            childName = "Pelvis",
                            localPos = new Vector3(-0.04443F, 0.15183F, -0.21391F),
                            localAngles = new Vector3(0.00001F, 200.0369F, 180.6941F),
                            localScale = new Vector3(0.12662F, 0.12662F, 0.12662F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.MonstersOnShrineUse,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMonstersOnShrineUse"),
                            childName =  "ThighR",
                            localPos =   new Vector3(0.01663F, 0.29137F, -0.26231F),
                            localAngles = new Vector3(36.48936F, 248.2491F, 4.94359F),
                            localScale = new Vector3(0.13501F, 0.13501F, 0.13501F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Items.RandomDamageZone,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRandomDamageZone"),
                            childName = "Chest",
                            localPos = new Vector3(0.01986F, 0.28445F, -0.29175F),
                            localAngles = new Vector3(358.4027F, 357.0433F, 1.42238F),
                            localScale = new Vector3(0.167F, 0.167F, 0.167F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.Fruit,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFruit"),
                            childName = "Chest",
                            localPos = new Vector3(-0.3874F, -0.35706F, 0.06709F),
                            localAngles = new Vector3(15.47195F, 115.1243F, 12.28921F),
                            localScale = new Vector3(0.55514F, 0.55514F, 0.55514F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.AffixRed,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteHorn"),
childName = "Head",
localPos = new Vector3(0.1201F, 0.2516F, 0F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.1036F, 0.1036F, 0.1036F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteHorn"),
childName = "Head",
localPos = new Vector3(-0.1201F, 0.2516F, 0F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(-0.1036F, 0.1036F, 0.1036F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.AffixBlue,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
                            childName = "Head",
                            localPos = new Vector3(0F, 0.2648F, 0.1528F),
                            localAngles = new Vector3(315F, 0F, 0F),
                            localScale = new Vector3(0.21885F, 0.21885F, 0.21885F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
childName = "Head",
localPos = new Vector3(0F, 0.3022F, 0.1055F),
localAngles = new Vector3(300F, 0F, 0F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.AffixWhite,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteIceCrown"),
                            childName = "Head",
                            localPos = new Vector3(0F, 0.38638F, 0.01853F),
                            localAngles = new Vector3(270F, 0F, 0F),
                            localScale = new Vector3(0.02983F, 0.03233F, 0.02983F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.AffixPoison,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteUrchinCrown"),
                            childName = "Head",
                            localPos = new Vector3(0F, 0.34205F, 0.05873F),
                            localAngles = new Vector3(270F, 0F, 0F),
                            localScale = new Vector3(0.06921F, 0.06921F, 0.06921F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.AffixHaunted,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteStealthCrown"),
                            childName = "Head",
                            localPos = new Vector3(0F, 0.40904F, 0.08126F),
                            localAngles = new Vector3(270F, 0F, 0F),
                            localScale = new Vector3(0.065F, 0.065F, 0.065F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.CritOnUse,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayNeuralImplant"),
                            childName = "Head",
                            localPos = new Vector3(0F, 0.18609F, 0.38943F),
                            localAngles = new Vector3(0F, 0F, 0F),
                            localScale = new Vector3(0.26868F, 0.26868F, 0.26868F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.DroneBackup,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRadio"),
                            childName = "ThighL",
                            localPos = new Vector3(-0.15613F, 0.26592F, 0.11726F),
                            localAngles = new Vector3(1.42229F, 309.1913F, 109.3525F),
                            localScale = new Vector3(1.08377F, 1.08377F, 1.08377F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.Lightning,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLightningArmRight"),
childName = "UpperArmR",
localPos = new Vector3(0F, 0F, 0F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(1.1682F, 1.1682F, 1.1682F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.BurnNearby,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayPotion"),
                            childName = "Chest",
                            localPos = new Vector3(0.32013F, -0.0097F, 0.37042F),
                            localAngles = new Vector3(314.3983F, 350.2056F, 348.0079F),
                            localScale = new Vector3(0.05009F, 0.05009F, 0.05009F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.CrippleWard,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEffigy"),
                            childName = "ThighL",
                            localPos = new Vector3(-0.05172F, 0.48476F, 0.25443F),
                            localAngles = new Vector3(7.44313F, 130.6053F, 223.2603F),
                            localScale = new Vector3(0.86843F, 0.86843F, 0.86843F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.QuestVolatileBattery,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBatteryArray"),
                            childName = "Chest",
                            localPos = new Vector3(0.00063F, 0.16204F, -0.37225F),
                            localAngles = new Vector3(0F, 12.9934F, 0F),
                            localScale = new Vector3(0.55121F, 0.55121F, 0.55121F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.GainArmor,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayElephantFigure"),
                            childName = "CalfR",
                            localPos = new Vector3(0.05821F, 0.27275F, -0.2591F),
                            localAngles = new Vector3(275.4603F, 140.7968F, 208.3909F),
                            localScale = new Vector3(0.93867F, 0.93867F, 0.93867F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.Recycle,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRecycler"),
                            childName = "Chest",
                            localPos = new Vector3(-0.20111F, 0.09796F, -0.31467F),
                            localAngles = new Vector3(0F, 117.8714F, 0F),
                            localScale = new Vector3(0.10077F, 0.10077F, 0.10077F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.FireBallDash,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEgg"),
                            childName = "Chest",
                            localPos = new Vector3(-0.21352F, 0.0402F, -0.32543F),
                            localAngles = new Vector3(299.4432F, 225.6099F, 143.8696F),
                            localScale = new Vector3(0.52439F, 0.52439F, 0.52439F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.Cleanse,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWaterPack"),
                            childName = "Chest",
                            localPos = new Vector3(-0.1517F, -0.08545F, -0.29272F),
                            localAngles = new Vector3(8.31785F, 196.1051F, 2.39181F),
                            localScale = new Vector3(0.11994F, 0.11994F, 0.11994F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.Tonic,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTonic"),
                            childName = "ThighL",
                            localPos = new Vector3(-0.17365F, 0.24399F, 0.13133F),
                            localAngles = new Vector3(8.78439F, 135.2354F, 178.6213F),
                            localScale = new Vector3(0.39535F, 0.39535F, 0.39535F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.Gateway,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayVase"),
                            childName = "ThighL",
                            localPos = new Vector3(-0.22193F, 0.30888F, 0.21205F),
                            localAngles = new Vector3(314.7436F, 246.4922F, 255.9787F),
                            localScale = new Vector3(0.37423F, 0.37423F, 0.37423F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.Meteor,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMeteor"),
                            childName = "Root",
localPos = new Vector3(-1.2F, 0.56994F, 3.39991F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.Saw,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySawmerang"),
                            childName = "Root",
localPos = new Vector3(-1.20444F, 0.32968F, 3.40442F),
localAngles = new Vector3(331.6471F, 24.28736F, -0.00001F),
localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.Blackhole,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGravCube"),
                            childName = "Root",
localPos = new Vector3(-1.21883F, 0.33493F, 3.47181F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(1.76854F, 1.76854F, 1.76854F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.Scanner,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayScanner"),
                            childName = "Chest",
                            localPos = new Vector3(-0.27364F, 0.21455F, -0.23639F),
                            localAngles = new Vector3(352.9223F, 220.4167F, 176.2378F),
                            localScale = new Vector3(0.22669F, 0.22669F, 0.22669F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.DeathProjectile,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDeathProjectile"),
                            childName = "ThighL",
                            localPos = new Vector3(-0.2492F, 0.32874F, 0.18074F),
                            localAngles = new Vector3(1.7537F, 301.4836F, 181.0824F),
                            localScale = new Vector3(0.14726F, 0.14726F, 0.14726F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.LifestealOnHit,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLifestealOnHit"),
childName = "Head",
localPos = new Vector3(-0.2175F, 0.4404F, -0.141F),
localAngles = new Vector3(44.0939F, 33.5151F, 43.5058F),
localScale = new Vector3(0.1246F, 0.1246F, 0.1246F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });

                itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                {
                    keyAsset = RoR2Content.Equipment.TeamWarCry,
                    displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = new ItemDisplayRule[]
                        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTeamWarCry"),
                            childName = "ThighL",
                            localPos = new Vector3(-0.13838F, 0.32051F, 0.11555F),
                            localAngles = new Vector3(355.0303F, 319.3217F, 183.2577F),
                            localScale = new Vector3(0.0986F, 0.0986F, 0.0986F),
                            limbMask = LimbFlags.None
                        }
                        }
                    }
                });
            #endregion

            try {
                if (PaladinPlugin.ancientScepterInstalled)
                    SetupScepterDisplay();
            }
            catch (System.Exception e) {
                //PaladinPlugin.logger.LogWarning($"could not add displays for Ancient Scepter\n{e}");
            }

            try {
                if (PaladinPlugin.aetheriumInstalled) { 
                    //todo CUM2 aetherium?
                    //AddAetheriumDisplays();
                    }
            }
            catch (System.Exception e) {
                //PaladinPlugin.logger.LogWarning($"could not add displays for Aetherium\n{e}");
            }

            try {
                if (PaladinPlugin.supplyDropInstalled) {
                    //todo CUM2 aetherium?
                    AddSupplyDropDisplays();
                }
            }
            catch (System.Exception e) {
                //PaladinPlugin.logger.LogWarning($"could not add displays for Supply Drop\n{e}");
            }
            
            itemDisplayRuleSet.keyAssetRuleGroups = itemDisplayRules.ToArray();
        }

        #region negro displays

        //enderbrine custom copy thing
/*

                                                           {childName},
                                                           {localPos},
                                                           {localAngles},
                                                           {localScale}
*/


        enum mod {
            AETH,
            SUPP,
            SIVS,
            GOLD,
            SCEP
        }
        private static void AddAetheriumDisplays() {
            itemDisplayRules.Add(CreateModRuleGroup(mod.AETH, "AccursedPotion",
                                                          "ThighL",
                                                          new Vector3(-0.23043F, 0.32284F, -0.12665F),
                                                          new Vector3(4.45731F, 185.4647F, 177.6174F),
                                                          new Vector3(0.9687F, 0.9687F, 0.9687F)));
            itemDisplayRules.Add(CreateModRuleGroup(mod.AETH, "VoidHeart",
                                                          "Chest",
                                                          new Vector3(-0.09543F, -0.19796F, 0.28118F),
                                                          new Vector3(16.96353F, 5.14273F, 10.3974F),
                                                          new Vector3(-0.16727F, 0.16658F, 0.15696F)));
            itemDisplayRules.Add(CreateModRuleGroup(mod.AETH, "SharkTeeth",
                                                          "CalfL",
                                                          new Vector3(0.06211F, 0.25415F, -0.08777F),
                                                          new Vector3(320.2775F, 38.01559F, 271.8767F),
                                                          new Vector3(0.78844F, 0.78844F, 0.78844F)));
            itemDisplayRules.Add(CreateModRuleGroup(mod.AETH, "BloodSoakedShield",
                                                          "LowerArmR",
                                                          new Vector3(0.10348F, 0.26132F, -0.01819F),
                                                          new Vector3(356.2767F, 97.25786F, 89.52616F),
                                                          new Vector3(0.34019F, 0.34019F, 0.34019F)));
            itemDisplayRules.Add(CreateModRuleGroup(mod.AETH, "FeatheredPlume",
                                                          "Head",
                                                          new Vector3(-0.00678F, 0.35193F, 0.02659F),
                                                          new Vector3(357.7291F, 285.8923F, 359.0515F),
                                                          new Vector3(0.50003F, 0.50003F, 0.50003F)));
            itemDisplayRules.Add(CreateModRuleGroup(mod.AETH, "ShieldingCore",
                                                          "LowerArmR",
                                                          new Vector3(-0.23784F, 0.3286F, -0.0176F),
                                                          new Vector3(0.97314F, 271.1276F, 357.6589F),
                                                          new Vector3(0.31628F, 0.31628F, 0.31628F)));
            itemDisplayRules.Add(CreateModRuleGroup(mod.AETH, "UnstableDesign",
                                                          "Stomach",
                                                          new Vector3(-0.05487F, 0.04344F, -0.28199F),
                                                          new Vector3(358.3217F, 224.4477F, 1.91643F),
                                                          new Vector3(1.24548F, 1.24548F, 0.9051F)));
            itemDisplayRules.Add(CreateModRuleGroup(mod.AETH, "WeightedAnklet",
                                                          "CalfR",
                                                          new Vector3(-0.00001F, 0.53876F, -0.01653F),
                                                          new Vector3(0F, 0F, 0F),
                                                          new Vector3(0.31933F, 0.31933F, 0.31933F)));
            itemDisplayRules.Add(CreateModRuleGroup(mod.AETH, "BlasterSword",
                                                          "SwordSmearFront",
                                                          new Vector3(-0.10833F, 2.5525F, 0.07879F),
                                                          new Vector3(0.08772F, 40.946F, 180.1556F),
                                                          new Vector3(0.32696F, 0.29056F, 0.12386F)));
            itemDisplayRules.Add(CreateModRuleGroup(mod.AETH, "EngiBelt",
                                                          "Pelvis",
                                                          new Vector3(0F, 0.39276F, 0.0787F),
                                                          new Vector3(0F, 116.6814F, 0F),
                                                          new Vector3(0.29462F, 0.29462F, 0.29462F)));

            itemDisplayRules.Add(CreateModRuleGroup(mod.AETH, "AlienMagnet",
                                                          "Root",
                                                          new Vector3(1.06397F, -1.08486F, 3.27534F),
                                                          new Vector3(0F, 0F, 359.3367F),
                                                          new Vector3(0.1541F, 0.1541F, 0.1541F)));
            itemDisplayRules.Add(CreateModRuleGroup(mod.AETH, "InspiringDrone",
                                                          "Root",
                                                          new Vector3(-1.44714F, -1.444F, 3.64135F),
                                                          new Vector3(291.0149F, 90.00006F, 89.99995F),
                                                          new Vector3(0.12676F, 0.12676F, 0.12929F)));

            itemDisplayRules.Add(CreateModRuleGroup(mod.AETH, "JarOfReshaping",
                                                          "Root",
                                                          new Vector3(-1.2588F, 0.72566F, 3.23362F),
                                                          new Vector3(0F, 0F, 0F),
                                                          new Vector3(0.11192F, 0.11192F, 0.11192F)));

            ItemDisplayRule ringRule = CreateDisplayRule(ItemDisplays.LoadAetheriumDisplay("WitchesRing"),
                                                          "Sword",
                                                          new Vector3(-0.01001F, 0.36895F, -0.01177F),
                                                          new Vector3(0F, 220.5148F, 0F),
                                                          new Vector3(0.49134F, 0.49134F, 0.37622F));

            ItemDisplayRule circleRule = CreateDisplayRule(ItemDisplays.LoadAetheriumDisplay("WitchesRingCircle"),
                                                          "Sword",
                                                          new Vector3(-0.00782F, 0.36927F, -0.00924F),
                                                          new Vector3(272.2881F, 220.515F, -0.00017F),
                                                          new Vector3(0.27066F, 0.27066F, 0.12244F));
            itemDisplayRules.Add(CreateDisplayRuleGroupWithRules(ItemDisplays.LoadAetheriumKeyAsset("WitchesRing"), ringRule, circleRule));
        }

        private static void AddSupplyDropDisplays() {
            itemDisplayRules.Add(CreateModRuleGroup(mod.SUPP, "PlagueMask",
                                                           "Head",
                                                           new Vector3(0.00094F, 0.14277F, 0.37744F),
                                                           new Vector3(350.8227F, 180F, 0F),
                                                           new Vector3(0.21812F, 0.34679F, 0.29885F)));
            itemDisplayRules.Add(CreateModRuleGroup(mod.SUPP, "PlagueHat",
                                                           "Head",
                                                           new Vector3(0.00001F, 0.3758F, 0.05085F),
                                                           new Vector3(0F, 0F, 0F),
                                                           new Vector3(0.21804F, 0.17341F, 0.29817F)));

            itemDisplayRules.Add(CreateModRuleGroup(mod.SUPP, "Bones",
                                                           "CalfR",
                                                           new Vector3(0.17997F, -0.05238F, 0.07133F),
                                                           new Vector3(13.68323F, 76.44486F, 191.9287F),
                                                           new Vector3(1.25683F, 1.25683F, 1.25683F)));
            itemDisplayRules.Add(CreateModRuleGroup(mod.SUPP, "Berries",
                                                           "loinFront2",
                                                           new Vector3(0.11782F, 0.27382F, -0.13743F),
                                                           new Vector3(341.1884F, 284.1298F, 180.0032F),
                                                           new Vector3(0.08647F, 0.08647F, 0.08647F)));
            itemDisplayRules.Add(CreateModRuleGroup(mod.SUPP, "UnassumingTie",
                                                           "Chest",
                                                           new Vector3(-0.08132F, 0.30226F, 0.34786F),
                                                           new Vector3(351.786F, 356.4574F, 0.73319F),
                                                           new Vector3(0.32213F, 0.35018F, 0.42534F)));
            itemDisplayRules.Add(CreateModRuleGroup(mod.SUPP, "SalvagedWires",
                                                           "UpperArmL",
                                                           new Vector3(-0.00419F, 0.10839F, -0.20693F),
                                                           new Vector3(21.68419F, 165.3445F, 132.0565F),
                                                           new Vector3(0.63809F, 0.63809F, 0.63809F)));

            itemDisplayRules.Add(CreateModRuleGroup(mod.SUPP, "ShellPlating",
                                                           "ThighR",
                                                           new Vector3(0.02115F, 0.52149F, -0.31269F),
                                                           new Vector3(319.6181F, 168.4007F, 184.779F),
                                                           new Vector3(0.24302F, 0.26871F, 0.26871F)));
            itemDisplayRules.Add(CreateModRuleGroup(mod.SUPP, "ElectroPlankton",
                                                           "ThighR",
                                                           new Vector3(0.21067F, 0.49094F, -0.08702F),
                                                           new Vector3(8.08377F, 285.087F, 164.4582F),
                                                           new Vector3(0.11243F, 0.11243F, 0.11243F)));

            itemDisplayRules.Add(CreateModRuleGroup(mod.SUPP, "BloodBook",
                                                           "Root",
                                                           new Vector3(2.19845F, -1.51445F, 1.59871F),
                                                           new Vector3(303.5005F, 271.0879F, 269.2205F),
                                                           new Vector3(0.12F, 0.12F, 0.12F)));
            itemDisplayRules.Add(CreateModRuleGroup(mod.SUPP, "QSGen",
                                                           "LowerArmL",
                                                           new Vector3(0.06003F, 0.1038F, -0.02042F),
                                                           new Vector3(0F, 18.75576F, 268.4665F),
                                                           new Vector3(0.12301F, 0.12301F, 0.12301F)));
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void SetupScepterDisplay()
        {
            //CUM2 fix item displays
        //    itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
        //    {
        //        keyAsset = AncientScepter.AncientScepterItem.instance.ItemDef,
        //        displayRuleGroup = new DisplayRuleGroup
        //        {
        //            rules = new ItemDisplayRule[]
        //{
        //                new ItemDisplayRule
        //                {
        //                    ruleType = ItemDisplayRuleType.ParentedPrefab,
        //                    followerPrefab = AncientScepter.AncientScepterItem.displayPrefab,
        //                    childName = "HandL",
        //                    localPos = new Vector3(-0.05522F, 0.14813F, -0.13256F),
        //                    localAngles = new Vector3(272.924F, 122.5896F, 239.0718F),
        //                    localScale = new Vector3(0.60001F, 0.70329F, 0.60001F),
        //                    limbMask = LimbFlags.None
        //                }
        //}
        //        }
        //    });
        }
        #endregion


        internal static void PopulateDisplays()
        {
            PopulateFromBody("Commando");
            PopulateFromBody("Croco");
        }

        private static void PopulateFromBody(string bodyName)
        {
            ItemDisplayRuleSet itemDisplayRuleSet = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/" + bodyName + "Body").GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet;

            ItemDisplayRuleSet.KeyAssetRuleGroup[] item = itemDisplayRuleSet.keyAssetRuleGroups;

            for (int i = 0; i < item.Length; i++)
            {
                ItemDisplayRule[] rules = item[i].displayRuleGroup.rules;

                for (int j = 0; j < rules.Length; j++)
                {
                    GameObject followerPrefab = rules[j].followerPrefab;
                    if (followerPrefab)
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
        }

        internal static GameObject LoadDisplay(string name)
        {
            if (itemDisplayPrefabs.ContainsKey(name.ToLower()))
            {
                if (itemDisplayPrefabs[name.ToLower()]) return itemDisplayPrefabs[name.ToLower()];
            }

            return null;
        }

        #region negro tools
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static GameObject LoadAetheriumDisplay(string name)
        {
            //todo CUM2 aetherium?
            //switch (name)
            //{
            //    case "AccursedPotion":
            //        return Aetherium.Items.AccursedPotion.ItemBodyModelPrefab;
            //    case "AlienMagnet":
            //        return Aetherium.Items.AlienMagnet.ItemFollowerPrefab;
            //    case "BlasterSword":
            //        return Aetherium.Items.BlasterSword.ItemBodyModelPrefab;
            //    case "BloodSoakedShield":
            //        return Aetherium.Items.BloodthirstyShield.ItemBodyModelPrefab;
            //    case "FeatheredPlume":
            //        return Aetherium.Items.FeatheredPlume.ItemBodyModelPrefab;
            //    case "InspiringDrone":
            //        return Aetherium.Items.InspiringDrone.ItemFollowerPrefab;
            //    case "SharkTeeth":
            //        return Aetherium.Items.SharkTeeth.ItemBodyModelPrefab;
            //    case "ShieldingCore":
            //        return Aetherium.Items.ShieldingCore.ItemBodyModelPrefab;
            //    case "UnstableDesign":
            //        return Aetherium.Items.UnstableDesign.ItemBodyModelPrefab;
            //    case "VoidHeart":
            //        return Aetherium.Items.Voidheart.ItemBodyModelPrefab;
            //    case "WeightedAnklet":
            //        return Aetherium.Items.WeightedAnklet.ItemBodyModelPrefab;
            //    case "WitchesRing":
            //        return Aetherium.Items.WitchesRing.ItemBodyModelPrefab;
            //    case "WitchesRingCircle":
            //        return Aetherium.Items.WitchesRing.CircleBodyModelPrefab;
            //    case "EngiBelt":
            //        return Aetherium.Items.EngineersToolbelt.ItemBodyModelPrefab;
            //    case "JarOfReshaping":
            //        return Aetherium.Equipment.JarOfReshaping.ItemBodyModelPrefab;
            //}
            return null;
        }
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static Object LoadAetheriumKeyAsset(string name) {
            //switch (name) {
            //    case "AccursedPotion":
            //        return Aetherium.Items.AccursedPotion.instance.ItemDef;
            //    case "AlienMagnet":
            //        return Aetherium.Items.AlienMagnet.instance.ItemDef;
            //    case "BlasterSword":
            //        return Aetherium.Items.BlasterSword.instance.ItemDef;
            //    case "BloodSoakedShield":
            //        return Aetherium.Items.BloodthirstyShield.instance.ItemDef;
            //    case "FeatheredPlume":
            //        return Aetherium.Items.FeatheredPlume.instance.ItemDef;
            //    case "InspiringDrone":
            //        return Aetherium.Items.InspiringDrone.instance.ItemDef;
            //    case "SharkTeeth":
            //        return Aetherium.Items.SharkTeeth.instance.ItemDef;
            //    case "ShieldingCore":
            //        return Aetherium.Items.ShieldingCore.instance.ItemDef;
            //    case "UnstableDesign":
            //        return Aetherium.Items.UnstableDesign.instance.ItemDef;
            //    case "VoidHeart":
            //        return Aetherium.Items.Voidheart.instance.ItemDef;
            //    case "WeightedAnklet":
            //        return Aetherium.Items.WeightedAnklet.instance.ItemDef;
            //    case "WitchesRing":
            //        return Aetherium.Items.WitchesRing.instance.ItemDef;
            //    case "EngiBelt":
            //        return Aetherium.Items.EngineersToolbelt.instance.ItemDef;
            //    case "JarOfReshaping":
            //        return Aetherium.Equipment.JarOfReshaping.instance.EquipmentDef;
            //}
            return null;
        }

        /*[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
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
        }*/

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static GameObject LoadSupplyDropDisplay(string name)
        {
            switch (name)
            {
                case "Bones":
                    return SupplyDrop.Items.HardenedBoneFragments.ItemBodyModelPrefab;
                case "Berries":
                    return SupplyDrop.Items.NumbingBerries.ItemBodyModelPrefab;
                case "UnassumingTie":
                    return SupplyDrop.Items.UnassumingTie.ItemBodyModelPrefab;
                case "SalvagedWires":
                    return SupplyDrop.Items.SalvagedWires.ItemBodyModelPrefab;

                case "ShellPlating":
                    return SupplyDrop.Items.ShellPlating.ItemBodyModelPrefab;
                case "ElectroPlankton":
                    return SupplyDrop.Items.ElectroPlankton.ItemBodyModelPrefab;
                case "PlagueHat":
                    return SupplyDrop.Items.PlagueHat.ItemBodyModelPrefab;
                case "PlagueMask":
                    GameObject masku = PrefabAPI.InstantiateClone(SupplyDrop.Items.PlagueMask.ItemBodyModelPrefab, "PlagueMask");
                    Material heeheehee = new Material(masku.GetComponent<ItemDisplay>().rendererInfos[0].defaultMaterial);
                    heeheehee.color = Color.green; ;
                    masku.GetComponent<ItemDisplay>().rendererInfos[0].defaultMaterial = heeheehee;
                    return masku;

                case "BloodBook":
                    return SupplyDrop.Items.BloodBook.ItemBodyModelPrefab;
                case "QSGen":
                    return SupplyDrop.Items.QSGen.ItemBodyModelPrefab;
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static Object LoadSupplyDropKeyAsset(string name) {
            //todo CUM2 supply drop?
            //switch (name) {
            //    //would be cool if these are enums maybe
            //    case "Bones":
            //        return SupplyDrop.Items.HardenedBoneFragments.instance.itemDef;
            //    case "Berries":
            //        return SupplyDrop.Items.NumbingBerries.instance.itemDef;
            //    case "UnassumingTie":
            //        return SupplyDrop.Items.UnassumingTie.instance.itemDef;
            //    case "SalvagedWires":
            //        return SupplyDrop.Items.SalvagedWires.instance.itemDef;

            //    case "ShellPlating":
            //        return SupplyDrop.Items.ShellPlating.instance.itemDef;
            //    case "ElectroPlankton":
            //        return SupplyDrop.Items.ElectroPlankton.instance.itemDef;
            //    case "PlagueHat":
            //        return SupplyDrop.Items.PlagueHat.instance.itemDef;
            //    case "PlagueMask":
            //        return SupplyDrop.Items.PlagueMask.instance.itemDef;

            //    case "BloodBook":
            //        return SupplyDrop.Items.BloodBook.instance.itemDef;
            //    case "QSGen":
            //        return SupplyDrop.Items.QSGen.instance.itemDef;

            //}
            return null;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static GameObject LoadStarstormDisplay(string name)
        {
            // make your fucking items public reeee
            /*switch (name)
            {
                case "Fork"://fuck you rob for doing fork first
                    return Starstorm2.Cores.Items.
            }*/
            return null;
        }
        #endregion


        //halp

        //use these
        private static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenericDisplayRuleGroup(Object keyAsset_, GameObject itemPrefab, string childName, Vector3 position, Vector3 rotation, Vector3 scale) {

            ItemDisplayRule singleRule = CreateDisplayRule(itemPrefab, childName, position, rotation, scale);
            return CreateDisplayRuleGroupWithRules(keyAsset_, singleRule);
        }
        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenericDisplayRuleGroup(string itemName, GameObject itemPrefab, string childName, Vector3 position, Vector3 rotation, Vector3 scale) {

            ItemDisplayRule singleRule = CreateDisplayRule(itemPrefab, childName, position, rotation, scale);
            return CreateDisplayRuleGroupWithRules(RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/" + itemName), singleRule);
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateFollowerDisplayRuleGroup(Object keyAsset_, GameObject itemPrefab, Vector3 position, Vector3 rotation, Vector3 scale) {

            ItemDisplayRule singleRule = CreateFollowerDisplayRule(itemPrefab, position, rotation, scale);
            return CreateDisplayRuleGroupWithRules(keyAsset_, singleRule);
        }
        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateFollowerDisplayRuleGroup(string itemName, GameObject itemPrefab, Vector3 position, Vector3 rotation, Vector3 scale) {

            ItemDisplayRule singleRule = CreateFollowerDisplayRule(itemPrefab, position, rotation, scale);
            return CreateDisplayRuleGroupWithRules(RoR2.LegacyResourcesAPI.Load<ItemDef>("ItemDefs/" + itemName), singleRule);
        }



        private static ItemDisplayRuleSet.KeyAssetRuleGroup CreateModRuleGroup(mod mod, string itemName, string childName, Vector3 position, Vector3 rotation, Vector3 scale) {

            ItemDisplayRuleSet.KeyAssetRuleGroup ruleGroup = new ItemDisplayRuleSet.KeyAssetRuleGroup();
            //god dang this function is tryhard
            //just use the original function
            //      oh i guess I wanted to generalize the trycatches
            //          but right now I'm like no they're way too verbose so idk i'll look at it again
            // oh hey. it should be going
            //      nevermind, it's not because it fuxk here with the load keyasset and not at the create part

            //it's not like it makes this any easier on a new look. to know what this does you'd have to go down the whole rabbit hole
            //guess comments would fix that but yeah
            //it's all interesting
            //try {

                switch (mod) {
                    case mod.AETH:
                        ruleGroup = CreateAetheriumRuleGroup(itemName, childName, position, rotation, scale);
                        break;
                    case mod.SUPP:
                        ruleGroup = CreateSupplyDropRuleGroup(itemName, childName, position, rotation, scale);
                        break;
                    case mod.SIVS:
                        break;
                }

                return ruleGroup;
            //}
            //catch (System.Exception e) {

            //    PaladinPlugin.logger.LogWarning($"could not create item display for {mod.ToString().ToLower()}'s {itemName}. skipping.\n(Error: {e.Message})");

            //    return ruleGroup;
            //}
            
        }

        private static ItemDisplayRuleSet.KeyAssetRuleGroup CreateSupplyDropRuleGroup(string itemName, string childName, Vector3 position, Vector3 rotation, Vector3 scale) {
            return CreateGenericDisplayRuleGroup(LoadSupplyDropKeyAsset(itemName), LoadSupplyDropDisplay(itemName), childName, position, rotation, scale);
        }
        private static ItemDisplayRuleSet.KeyAssetRuleGroup CreateAetheriumRuleGroup(string itemName, string childName, Vector3 position, Vector3 rotation, Vector3 scale) {
            return CreateGenericDisplayRuleGroup(LoadAetheriumKeyAsset(itemName), LoadAetheriumDisplay(itemName), childName, position, rotation, scale);
        }

        //they use these
        //but use these ones yourself if you are doing multiple
        private static ItemDisplayRuleSet.KeyAssetRuleGroup CreateDisplayRuleGroupWithRules(Object keyAsset_, params ItemDisplayRule[] rules) {
            try {

                return new ItemDisplayRuleSet.KeyAssetRuleGroup {
                    keyAsset = keyAsset_,
                    displayRuleGroup = new DisplayRuleGroup {
                        rules = rules
                    }
                };
            }
            catch (System.Exception e) {

                Debug.LogError($"couldn't create Item display \n{e}");

                return new ItemDisplayRuleSet.KeyAssetRuleGroup();
            }

        }

        private static ItemDisplayRule CreateFollowerDisplayRule(GameObject itemPrefab, Vector3 position, Vector3 rotation, Vector3 scale) {
            return CreateDisplayRule(itemPrefab, "Root", position, rotation, scale);
        }
        private static ItemDisplayRule CreateDisplayRule(GameObject itemPrefab, string childName, Vector3 position, Vector3 rotation, Vector3 scale) {
            return new ItemDisplayRule {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                childName = childName,
                followerPrefab = itemPrefab,
                limbMask = LimbFlags.None,
                localPos = position,
                localAngles = rotation,
                localScale = scale
            };
        }


    }
}