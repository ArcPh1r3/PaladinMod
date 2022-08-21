using BepInEx.Configuration;
using System;
using UnityEngine;

namespace PaladinMod.Modules
{
    public static class Config
    {
        public static ConfigEntry<bool> forceUnlock;
        public static ConfigEntry<bool> itemDisplays;
        public static ConfigEntry<bool> cape;
        public static ConfigEntry<bool> cursed;
        public static ConfigEntry<bool> legacySkins;
        //public static ConfigEntry<bool> spookyArms;
        public static ConfigEntry<KeyCode> praiseKeybind;
        public static ConfigEntry<KeyCode> pointKeybind;
        public static ConfigEntry<KeyCode> restKeybind;
        public static ConfigEntry<KeyCode> swordPoseKeybind;
        public static ConfigEntry<bool> legacyCruelSun;
        public static ConfigEntry<float> cruelSunMultiplier;

        public static void ReadConfig()
        {
            forceUnlock = 
                PaladinPlugin.instance.Config.Bind<bool>("01 - General Settings", 
                                                         "Force Unlock", 
                                                         false, 
                                                         "Makes Paladin unlocked by default");
            cape = 
                PaladinPlugin.instance.Config.Bind<bool>("01 - General Settings", 
                                                         "Cape", 
                                                         false, 
                                                         "Gives Paladin's default skin a cape");
            cursed = 
                PaladinPlugin.instance.Config.Bind<bool>("01 - General Settings",
                                                         "Cursed", 
                                                         false, 
                                                         "Enables extra skills/skins");

            //spookyArms =
            //    PaladinPlugin.instance.Config.Bind<bool>("01 - General Settings",
            //                                             "Spoky arms",
            //                                             false,
            //                                             "Enables spooky arms from Corrupted skin on all skins");
            restKeybind = 
                PaladinPlugin.instance.Config.Bind<KeyCode>("02 - Keybinds",
                                                            "Rest", 
                                                            KeyCode.Alpha1, 
                                                            "Keybind used to perform the Rest emote");
            praiseKeybind = 
                PaladinPlugin.instance.Config.Bind<KeyCode>("02 - Keybinds",
                                                            "Praise The Sun",
                                                            KeyCode.Alpha2, 
                                                            "Keybind used to perform the Praise The Sun emote");
            pointKeybind = 
                PaladinPlugin.instance.Config.Bind<KeyCode>("02 - Keybinds",
                                                            "Point Down", 
                                                            KeyCode.Alpha3, 
                                                            "Keybind used to perform the Point Down emote");
            swordPoseKeybind = 
                PaladinPlugin.instance.Config.Bind<KeyCode>("02 - Keybinds",
                                                            "Sword Pose", 
                                                            KeyCode.Alpha4, 
                                                            "Keybind used to perform the Sword Pose emote (css pose)");
            if (pointKeybind.Value == praiseKeybind.Value && pointKeybind.Value == KeyCode.Alpha2) pointKeybind.Value = KeyCode.Alpha3;

            legacySkins =
                PaladinPlugin.instance.Config.Bind<bool>("03 - Legacy",
                                                         "Skins",
                                                         false,
                                                         "Adds previous versions of lunar, sovereign and corruption skins");
            legacyCruelSun =
                PaladinPlugin.instance.Config.Bind<bool>("03 - Legacy",
                                                         "Cruel Sun",
                                                         false,
                                                         "Use the old version of Cruel Sun.");
            cruelSunMultiplier =
                PaladinPlugin.instance.Config.Bind<float>("03 - Legacy",
                                                          "Pride Flare Multiplier",
                                                          9001f,
                                                          "Multiplier for Scepter Cruel Sun damage and force" +
                                                          "\n will be set to a reasonable value in a later update (2000). For now, have fun c:");
        }


    }
}