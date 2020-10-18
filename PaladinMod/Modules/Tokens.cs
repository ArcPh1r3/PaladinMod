using R2API;
using System;

namespace PaladinMod.Modules
{
    public static class Tokens
    {
        public static void AddTokens()
        {
            /*string desc = "The Miner is a fast paced and highly mobile melee survivor who prioritizes getting long kill combos to build stacks of his passive.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Once you get a good number of stacks of Adrenaline, Crush will be your best source of damage." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Note that charging Drill Charge only affects damage dealt. Aim at the ground or into enemies to deal concentrated damage." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > You can tap Backblast to travel a short distance. Hold it to go further." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > To The Stars when used low to the ground is great at dealing high amounts of damage to enemies with large hitboxes." + Environment.NewLine + Environment.NewLine;*/

            string desc = "Paladin" + Environment.NewLine + Environment.NewLine;

            string outro = StaticValues.characterOutro;

            LanguageAPI.Add("PALADIN_NAME", StaticValues.characterName);
            LanguageAPI.Add("PALADIN_DESCRIPTION", desc);
            LanguageAPI.Add("PALADIN_SUBTITLE", StaticValues.characterSubtitle);
            LanguageAPI.Add("PALADIN_LORE", StaticValues.characterLore);
            LanguageAPI.Add("PALADIN_OUTRO_FLAVOR", outro);


            LanguageAPI.Add("PALADINBODY_DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add("PALADINBODY_LUNAR_SKIN_NAME", "Lunar");
            LanguageAPI.Add("PALADINBODY_POISON_SKIN_NAME", "Poison");


            LanguageAPI.Add("PALADIN_PASSIVE_NAME", "Based Beyond Recognition");
            LanguageAPI.Add("PALADIN_PASSIVE_DESCRIPTION", "While above <style=cIsHealth>90% health</style>, the Paladin is <style=cIsHealing>based</style> and cannot be harmed by <style=cIsUtility>cringe of any kind</style>.");


            desc = "Slash at nearby enemies for <style=cIsDamage>" + 100f * StaticValues.slashDamageCoefficient + "% damage</style>. Deals <style=cIsDamage>" + 100f * StaticValues.slashBuffDamageCoefficient + "% damage</style> and fires a beam of <style=cIsUtility>holy light</style> if the Paladin is <style=cIsHealing>based</style>.";

            LanguageAPI.Add("PALADIN_PRIMARY_SLASH_NAME", "Divine Blade");
            LanguageAPI.Add("PALADIN_PRIMARY_SLASH_DESCRIPTION", desc);


            desc = "Spin forward with your blade dealing <style=cIsDamage>" + 100f * StaticValues.spinSlashDamageCoefficient + "% damage</style>.";

            LanguageAPI.Add("PALADIN_SECONDARY_SPINSLASH_NAME", "Spinning Slash");
            LanguageAPI.Add("PALADIN_SECONDARY_SPINSLASH_DESCRIPTION", desc);


            desc = "<style=cIsUtility>Shocking.</style> Charge up and throw a <style=cIsUtility>piercing lightning spear</style>, dealing up to <style=cIsDamage>" + 100f * StaticValues.lightningSpearMaxDamageCoefficient + "% damage</style>.";

            LanguageAPI.Add("PALADIN_UTILITY_LIGHTNINGSPEAR_NAME", "Lightning Spear");
            LanguageAPI.Add("PALADIN_UTILITY_LIGHTNINGSPEAR_DESCRIPTION", desc);
        }
    }
}
