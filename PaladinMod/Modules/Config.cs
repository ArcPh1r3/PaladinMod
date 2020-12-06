using BepInEx.Configuration;
using System;
using UnityEngine;

namespace PaladinMod.Modules
{
    public static class Config
    {
        public static ConfigEntry<bool> forceUnlock;
        public static ConfigEntry<KeyCode> praiseKeybind;
        public static ConfigEntry<KeyCode> pointKeybind;

        public static void ReadConfig()
        {
            forceUnlock = PaladinPlugin.instance.Config.Bind<bool>(new ConfigDefinition("01 - General Settings", "Force Unlock"), false, new ConfigDescription("Makes Paladin unlocked by default", null, Array.Empty<object>()));
            praiseKeybind = PaladinPlugin.instance.Config.Bind<KeyCode>(new ConfigDefinition("02 - Keybinds", "Praise The Sun"), KeyCode.Alpha1, new ConfigDescription("Keybind used to perform the Praise The Sun emote", null, Array.Empty<object>()));
            pointKeybind = PaladinPlugin.instance.Config.Bind<KeyCode>(new ConfigDefinition("02 - Keybinds", "Point Down"), KeyCode.Alpha2, new ConfigDescription("Keybind used to perform the Point Down emote", null, Array.Empty<object>()));
        }
    }
}
