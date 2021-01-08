using R2API;
using RoR2;
using UnityEngine;

namespace PaladinMod.Modules
{
    public static class Buffs
    {
        public static BuffIndex torporDebuff;
        public static BuffIndex warcryBuff;

        public static BuffIndex scepterTorporDebuff;
        public static BuffIndex scepterWarcryBuff;

        public static void RegisterBuffs()
        {
            BuffDef paladinWarcryBuff = new BuffDef
            {
                name = "Divine Blessing",
                iconPath = "Textures/BuffIcons/texBuffGenericShield",
                buffColor = PaladinPlugin.characterColor,
                canStack = false,
                isDebuff = false,
                eliteIndex = EliteIndex.None
            };
            CustomBuff paladinWarcry = new CustomBuff(paladinWarcryBuff);
            warcryBuff = BuffAPI.Add(paladinWarcry);

            BuffDef scepterPaladinWarcryBuff = new BuffDef
            {
                name = "Divine Blessing (Scepter)",
                iconPath = "Textures/BuffIcons/texBuffGenericShield",
                buffColor = PaladinPlugin.characterColor,
                canStack = false,
                isDebuff = false,
                eliteIndex = EliteIndex.None
            };
            CustomBuff scepterPaladinWarcry = new CustomBuff(scepterPaladinWarcryBuff);
            scepterWarcryBuff = BuffAPI.Add(scepterPaladinWarcry);

            BuffDef torporDebuffDef = new BuffDef
            {
                name = "Torpor",
                iconPath = "Textures/BuffIcons/texBuffCloakIcon",
                buffColor = Color.black,
                canStack = false,
                isDebuff = true,
                eliteIndex = EliteIndex.None
            };
            CustomBuff torpor = new CustomBuff(torporDebuffDef);
            torporDebuff = BuffAPI.Add(torpor);

            BuffDef scepterTorporDebuffDef = new BuffDef
            {
                name = "Torpor (Scepter)",
                iconPath = "Textures/BuffIcons/texBuffCloakIcon",
                buffColor = Color.black,
                canStack = false,
                isDebuff = true,
                eliteIndex = EliteIndex.None
            };
            CustomBuff scepterTorpor = new CustomBuff(scepterTorporDebuffDef);
            scepterTorporDebuff = BuffAPI.Add(scepterTorpor);
        }
    }
}
