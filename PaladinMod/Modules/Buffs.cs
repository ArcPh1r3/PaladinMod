using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinMod.Modules
{
    public static class Buffs
    {
        public static BuffDef blessedBuff;

        public static BuffDef torporDebuff;
        public static BuffDef warcryBuff;

        public static BuffDef scepterTorporDebuff;
        public static BuffDef scepterWarcryBuff;

        public static BuffDef overchargeBuff;
        public static BuffDef rageBuff;

        internal static List<BuffDef> buffDefs = new List<BuffDef>();

        public static void RegisterBuffs()
        {
            blessedBuff =
    AddNewBuff("Blessed",
               LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
               PaladinPlugin.characterColor,
               false,
               false);

            warcryBuff = 
                AddNewBuff("Divine Blessing", 
                           LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite, 
                           PaladinPlugin.characterColor, 
                           false, 
                           false);
            scepterWarcryBuff = 
                AddNewBuff("Divine Blessing (Scepter)",
                           LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                           PaladinPlugin.characterColor, 
                           false, 
                           false);
                                            
            torporDebuff = 
                AddNewBuff("Torpor",
                           LegacyResourcesAPI.Load<BuffDef>("BuffDefs/Cloak").iconSprite,
                           Color.black, 
                           false, 
                           true);
            scepterTorporDebuff = 
                AddNewBuff("Torpor (Scepter)",
                           LegacyResourcesAPI.Load<BuffDef>("BuffDefs/Cloak").iconSprite,
                           Color.black, 
                           false, 
                           true);

            overchargeBuff = 
                AddNewBuff("PaladinOvercharge",
                           LegacyResourcesAPI.Load<BuffDef>("BuffDefs/TeslaField").iconSprite,
                           Color.yellow,
                           false,
                           false);
            rageBuff = 
                AddNewBuff("PaladinBerserk",
                           LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                           Color.red, 
                           false, 
                           false);
        }

        // simple helper method
        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;
            buffDef.isHidden = false;

            buffDefs.Add(buffDef);

            return buffDef;
        }
    }
}