using BepInEx.Configuration;
using System;
using UnityEngine;

namespace PaladinMod.Modules
{
    public static class PaladinConfig
    {
        public const string Section1General = "01 - General Settings";
        public static ConfigEntry<bool> forceUnlock;
        public static ConfigEntry<bool> itemDisplays;
        public static ConfigEntry<bool> cape;
        public static ConfigEntry<bool> cursed;
        public static ConfigEntry<bool> legacySkins;
        [Configure(Section1General,
            0.5f,
            name = "Passive Intensity",
            description = "Intensity of the white body lines on the passive effect.\n0 to be basically invisible. 1 for full intensity",
            restartRequired = true)]
        public static ConfigEntry<float> passiveIntensity;
        //public static ConfigEntry<bool> spookyArms;
        public const string Section2Keybinds = "02 - Keybinds";
        public static ConfigEntry<KeyCode> praiseKeybind;
        public static ConfigEntry<KeyCode> pointKeybind;
        public static ConfigEntry<KeyCode> restKeybind;
        public static ConfigEntry<KeyCode> swordPoseKeybind;
        public const string Section3Legacy = "03 - Legacy";
        public static ConfigEntry<bool> legacyCruelSun;
        public static ConfigEntry<float> prideFlareMultiplier;
        public const string Section4Numbers = "04 - Numbers";
        //used in staticvalues.cs

        public static void ReadConfig()
        {
            OldConfig();

            ConfigButEpic.InitConfigAttributes(typeof(PaladinConfig));
        }

        private static void OldConfig()
        {
            forceUnlock =
                ConfigButEpic.BindAndOptions<bool>(Section1General,
                                                         "Force Unlock",
                                                         false,
                                                         "Makes Paladin unlocked by default");
            cape =
                ConfigButEpic.BindAndOptions<bool>(Section1General,
                                                         "Cape",
                                                         false,
                                                         "Gives Paladin's default skin a cape");
            cursed =
                ConfigButEpic.BindAndOptions<bool>(Section1General,
                                                         "Cursed",
                                                         false,
                                                         "Enables extra skills/skins");

            //spookyArms =
            //    ConfigButEpic.BindAndOptions<bool>("01 - General Settings",
            //                                             "Spoky arms",
            //                                             false,
            //                                             "Enables spooky arms from Corrupted skin on all skins");
            restKeybind =
                ConfigButEpic.BindAndOptions<KeyCode>(Section2Keybinds,
                                                            "Rest",
                                                            KeyCode.Alpha1,
                                                            "Keybind used to perform the Rest emote");
            praiseKeybind =
                ConfigButEpic.BindAndOptions<KeyCode>(Section2Keybinds,
                                                            "Praise The Sun",
                                                            KeyCode.Alpha2,
                                                            "Keybind used to perform the Praise The Sun emote");
            pointKeybind =
                ConfigButEpic.BindAndOptions<KeyCode>(Section2Keybinds,
                                                            "Point Down",
                                                            KeyCode.Alpha3,
                                                            "Keybind used to perform the Point Down emote");
            swordPoseKeybind =
                ConfigButEpic.BindAndOptions<KeyCode>(Section2Keybinds,
                                                            "Sword Pose",
                                                            KeyCode.Alpha4,
                                                            "Keybind used to perform the Sword Pose emote (css pose)");
            //what is this?
            if (pointKeybind.Value == praiseKeybind.Value && pointKeybind.Value == KeyCode.Alpha2) pointKeybind.ActualConfigEntry.Value = KeyCode.Alpha3;

            legacySkins =
                ConfigButEpic.BindAndOptions<bool>(Section3Legacy,
                                                         "Skins",
                                                         false,
                                                         "Adds previous versions of lunar, sovereign and corruption skins");
            legacyCruelSun =
                ConfigButEpic.BindAndOptions<bool>(Section3Legacy,
                                                         "Cruel Sun",
                                                         false,
                                                         "Add the old version of Cruel Sun.");

            prideFlareMultiplier =
                ConfigButEpic.BindAndOptions<float>(Section3Legacy,
                                                          "Pride Flare Damage",
                                                          4000f,
                                                          0,
                                                          10000,
                                                          "Multiplier for Scepter Cruel Sun damage and force" +
                                                          "\n originally 9001 for shitposting purposes. feel free to reset" +
                                                          "\n unlike most other configs, this damage is in %"
                                                          );
        }
    }
}