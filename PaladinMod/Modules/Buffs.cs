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
        public static BuffDef torporDebuff;
        public static BuffDef warcryBuff;

        public static BuffDef scepterTorporDebuff;
        public static BuffDef scepterWarcryBuff;

        public static BuffDef overchargeBuff;
        public static BuffDef rageBuff;

        internal static List<BuffDef> buffDefs = new List<BuffDef>();

        public static void RegisterBuffs()
        {
            warcryBuff = AddNewBuff("Divine Blessing", Resources.Load<Sprite>("Textures/BuffIcons/texBuffGenericShield"), PaladinPlugin.characterColor, false, false);
            scepterWarcryBuff = AddNewBuff("Divine Blessing (Scepter)", Resources.Load<Sprite>("Textures/BuffIcons/texBuffGenericShield"), PaladinPlugin.characterColor, false, false);

            torporDebuff = AddNewBuff("Torpor", Resources.Load<Sprite>("Textures/BuffIcons/texBuffCloakIcon"), Color.black, false, true);
            scepterTorporDebuff = AddNewBuff("Torpor (Scepter)", Resources.Load<Sprite>("Textures/BuffIcons/texBuffCloakIcon"), Color.black, false, true);

            overchargeBuff = AddNewBuff("PaladinOvercharge", Resources.Load<Sprite>("Textures/BuffIcons/texBuffTeslaIcon"), Color.yellow, false, false);
            rageBuff = AddNewBuff("PaladinBerserk", Resources.Load<Sprite>("Textures/BuffIcons/texBuffGenericShield"), Color.red, false, false);
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

            buffDefs.Add(buffDef);

            return buffDef;
        }
    }
}