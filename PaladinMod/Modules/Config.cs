using BepInEx.Configuration;
using System;
using UnityEngine;

namespace PaladinMod.Modules
{
    public static class Config
    {
        public static ConfigEntry<bool> forceUnlock;
        public static ConfigEntry<bool> itemDisplays;
        public static ConfigEntry<bool> cursed;
        public static ConfigEntry<KeyCode> praiseKeybind;
        public static ConfigEntry<KeyCode> pointKeybind;
        public static ConfigEntry<KeyCode> restKeybind;

        public static void ReadConfig()
        {
            forceUnlock = PaladinPlugin.instance.Config.Bind<bool>(new ConfigDefinition("01 - General Settings", "Force Unlock"), false, new ConfigDescription("Makes Paladin unlocked by default", null, Array.Empty<object>()));
            cursed = PaladinPlugin.instance.Config.Bind<bool>(new ConfigDefinition("01 - General Settings", "Cursed"), false, new ConfigDescription("Enables extra skills/skins", null, Array.Empty<object>()));
            itemDisplays = PaladinPlugin.instance.Config.Bind<bool>(new ConfigDefinition("01 - General Settings", "Enable Item Displays"), true, new ConfigDescription("Only the unworthy disable this", null, Array.Empty<object>()));
            restKeybind = PaladinPlugin.instance.Config.Bind<KeyCode>(new ConfigDefinition("02 - Keybinds", "Rest"), KeyCode.Alpha1, new ConfigDescription("Keybind used to perform the Rest emote", null, Array.Empty<object>()));
            praiseKeybind = PaladinPlugin.instance.Config.Bind<KeyCode>(new ConfigDefinition("02 - Keybinds", "Praise The Sun"), KeyCode.Alpha2, new ConfigDescription("Keybind used to perform the Praise The Sun emote", null, Array.Empty<object>()));
            pointKeybind = PaladinPlugin.instance.Config.Bind<KeyCode>(new ConfigDefinition("02 - Keybinds", "Point Down"), KeyCode.Alpha3, new ConfigDescription("Keybind used to perform the Point Down emote", null, Array.Empty<object>()));
            if (pointKeybind.Value == praiseKeybind.Value && pointKeybind.Value == KeyCode.Alpha2) pointKeybind.Value = KeyCode.Alpha3;
        }
    }
}