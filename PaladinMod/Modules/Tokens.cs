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


            if (UnityEngine.Random.value <= 0.1f)
            {
                LanguageAPI.Add("PALADIN_PASSIVE_NAME", "Based Beyond Recognition");
                LanguageAPI.Add("PALADIN_PASSIVE_DESCRIPTION", "While above <style=cIsHealth>90% health</style>, the Paladin is <style=cIsHealing>based</style> and cannot be harmed by <style=cIsUtility>cringe of any kind</style>.");

                desc = "Slash at nearby enemies for <style=cIsDamage>" + 100f * StaticValues.slashDamageCoefficient + "% damage</style>. Deals <style=cIsDamage>" + 100f * StaticValues.slashBuffDamageCoefficient + "% damage</style> and fires a beam of <style=cIsUtility>holy light</style> if the Paladin is <style=cIsHealing>based</style>.";
            }
            else
            {
                LanguageAPI.Add("PALADIN_PASSIVE_NAME", "Bulwark's Blessing");
                LanguageAPI.Add("PALADIN_PASSIVE_DESCRIPTION", "While above <style=cIsHealth>90% health</style>, the Paladin is <style=cIsHealing>blessed</style>, and using <style=cIsHealing>Divine Blade</style> fires beams of light that deal <style=cIsDamage>" + 100f * StaticValues.beamDamageCoefficient + "% damage</style>.");

                desc = "Slash at nearby enemies for <style=cIsDamage>" + 100f * StaticValues.slashDamageCoefficient + "% damage</style>. Deals <style=cIsDamage>" + 100f * StaticValues.slashBuffDamageCoefficient + "% damage</style> if the Paladin is <style=cIsHealing>blessed</style>.";
            }

            LanguageAPI.Add("PALADIN_PRIMARY_SLASH_NAME", "Divine Blade");
            LanguageAPI.Add("PALADIN_PRIMARY_SLASH_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Stunning.</style> Dash forward, then perform a wide spinning slash, dealing <style=cIsDamage>" + 100f * StaticValues.spinSlashDamageCoefficient + "% damage</style>.";

            LanguageAPI.Add("PALADIN_SECONDARY_SPINSLASH_NAME", "Spinning Slash");
            LanguageAPI.Add("PALADIN_SECONDARY_SPINSLASH_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Shocking.</style> Charge up and throw a <style=cIsUtility>piercing lightning spear</style>, dealing up to <style=cIsDamage>" + 100f * StaticValues.lightningSpearMaxDamageCoefficient + "% damage</style>.";

            LanguageAPI.Add("PALADIN_UTILITY_LIGHTNINGSPEAR_NAME", "Sunlight Spear");
            LanguageAPI.Add("PALADIN_UTILITY_LIGHTNINGSPEAR_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Shocking.</style> Throw a small <style=cIsUtility>piercing lightning spear</style> that deals <style=cIsDamage>" + 100f * StaticValues.lightningBoltDamageCoefficient + "% damage</style>. <style=cIsUtility>Hold up to 3 charges.</style>";

            LanguageAPI.Add("PALADIN_UTILITY_LIGHTNINGBOLT_NAME", "Lightning Bolt");
            LanguageAPI.Add("PALADIN_UTILITY_LIGHTNINGBOLT_DESCRIPTION", desc);

            desc = "Restore <style=cIsHealing>" + StaticValues.healAmount * 100f + "% max health</style> to all allies in an area.";

            LanguageAPI.Add("PALADIN_UTILITY_HEAL_NAME", "Heal");
            LanguageAPI.Add("PALADIN_UTILITY_HEAL_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Bless</style> an area for a duration, boosting <style=cIsHealth>armor</style> and gradually <style=cIsHealing>restoring health</style> to all allies in the vicinity.";

            LanguageAPI.Add("PALADIN_SPECIAL_HEALZONE_NAME", "Sacred Sunlight");
            LanguageAPI.Add("PALADIN_SPECIAL_HEALZONE_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Silence</style> an area for a duration, inflicting <style=cIsHealth>torpor</style> on all enemies in the vicinity as well as <style=cIsDamage>destroying</style> all <style=cIsUtility>projectiles</style>.";

            LanguageAPI.Add("PALADIN_SPECIAL_TORPOR_NAME", "Vow of Silence");
            LanguageAPI.Add("PALADIN_SPECIAL_TORPOR_DESCRIPTION", desc);

            LanguageAPI.Add("KEYWORD_TORPOR", "<style=cKeywordName>Torpor</style><style=cSub>Applies a <style=cIsHealth>" + StaticValues.torporSlowAmount + "%</style> attack and movement speed <style=cIsDamage>slow</style>.");


            LanguageAPI.Add("PALADIN_UNLOCKABLE_ACHIEVEMENT_NAME", "A Paladin's Vow");
            LanguageAPI.Add("PALADIN_UNLOCKABLE_ACHIEVEMENT_DESC", "Use the Beads of Fealty and become whole once more.");
            LanguageAPI.Add("PALADIN_UNLOCKABLE_UNLOCKABLE_NAME", "A Paladin's Vow");

            LanguageAPI.Add("PALADIN_MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Paladin: Mastery");
            LanguageAPI.Add("PALADIN_MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Paladin, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add("PALADIN_MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Paladin: Mastery");

            LanguageAPI.Add("PALADIN_POISONUNLOCKABLE_ACHIEVEMENT_NAME", "Her Disciple");
            LanguageAPI.Add("PALADIN_POISONUNLOCKABLE_ACHIEVEMENT_DESC", "As Paladin, form a covenant with a goddess of corruption.");
            LanguageAPI.Add("PALADIN_POISONUNLOCKABLE_UNLOCKABLE_NAME", "Her Discple");
        }
    }
}
