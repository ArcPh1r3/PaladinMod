//using R2API;
using BepInEx;
using System;
using System.IO;

namespace PaladinMod.Modules {
    internal static class Files {

        public static PluginInfo PluginInfo;
        
        internal static string assemblyDir {
            get {
                return System.IO.Path.GetDirectoryName(PluginInfo.Location);
            }
        }

        internal static void Init(PluginInfo info) {
            PluginInfo = info;
        }

        internal static string GetPluginFilePath(string folderName, string fileName) {
            return Path.Combine(assemblyDir, folderName, fileName);
        }
    }
}