using R2API;
using RoR2;
using UnityEngine;

namespace PaladinMod.Modules
{
    public static class Buffs
    {
        public static BuffIndex healZoneArmorBuff;
        public static BuffIndex torporDebuff;

        public static void RegisterBuffs()
        {
            BuffDef healZoneArmorDef = new BuffDef
            {
                name = "Divine Blessing",
                iconPath = "Textures/BuffIcons/texBuffGenericShield",
                buffColor = PaladinPlugin.characterColor,
                canStack = false,
                isDebuff = false,
                eliteIndex = EliteIndex.None
            };
            CustomBuff healZoneArmor = new CustomBuff(healZoneArmorDef);
            healZoneArmorBuff = BuffAPI.Add(healZoneArmor);

            BuffDef torporDebuffDef = new BuffDef
            {
                name = "Torpor",
                iconPath = "Textures/BuffIcons/texBuffGenericShield",
                buffColor = Color.black,
                canStack = false,
                isDebuff = true,
                eliteIndex = EliteIndex.None
            };
            CustomBuff torpor = new CustomBuff(torporDebuffDef);
            torporDebuff = BuffAPI.Add(torpor);
        }
    }
}
