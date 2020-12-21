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
            LanguageAPI.Add("PALADINBODY_POISON_SKIN_NAME", "Corruption");
            LanguageAPI.Add("PALADINBODY_HUNTER_SKIN_NAME", "Hunter");


            LanguageAPI.Add("PALADIN_PASSIVE_NAME", "Bulwark's Blessing");
            LanguageAPI.Add("PALADIN_PASSIVE_DESCRIPTION", "Gain <style=cIsHealth>" + StaticValues.armorPerLevel + " armor</style> per level. While above <style=cIsHealth>80% health</style> or while having active <style=cIsHealth>barrier</style>, the Paladin is <style=cIsHealing>blessed</style>, empowering all sword skills.");

            desc = "Slash forward for <style=cIsDamage>" + 100f * StaticValues.slashDamageCoefficient + "% damage</style>. Deals <style=cIsDamage>" + 100f * StaticValues.slashBuffDamageCoefficient + "% damage</style> if the Paladin is <style=cIsHealing>blessed</style>.";

            LanguageAPI.Add("PALADIN_PRIMARY_SLASH_NAME", "Divine Blade");
            LanguageAPI.Add("PALADIN_PRIMARY_SLASH_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Stunning.</style> Perform a wide sweeping slash, knocking up enemies for <style=cIsDamage>" + 100f * StaticValues.spinSlashDamageCoefficient + "% damage</style> and gaining bonus range if <style=cIsHealing>blessed</style>. Use while airborne to perform a leaping downward slash for 800%, increased to 1500% while <style=cIsHealing>blessed</style>.";

            LanguageAPI.Add("PALADIN_SECONDARY_SPINSLASH_NAME", "Spinning Slash");
            LanguageAPI.Add("PALADIN_SECONDARY_SPINSLASH_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Shocking.</style> <style=cIsUtility>Agile.</style> Charge up and throw a <style=cIsUtility>lightning bolt</style>, dealing up to <style=cIsDamage>" + 100f * StaticValues.lightningSpearMaxDamageCoefficient + "% damage</style>.";

            LanguageAPI.Add("PALADIN_SECONDARY_LIGHTNING_NAME", "Sunlight Spear");
            LanguageAPI.Add("PALADIN_SECONDARY_LIGHTNING_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Agile.</style> Fire a volley of <style=cIsUtility>lunar shards</style>, dealing <style=cIsDamage>" + 100f * StaticValues.lunarShardDamageCoefficient + "% damage</style> each. Hold up to <style=cIsDamage>" + StaticValues.lunarShardMaxStock + "</style> shards.";

            LanguageAPI.Add("PALADIN_SECONDARY_LUNARSHARD_NAME", "Lunar Shards");
            LanguageAPI.Add("PALADIN_SECONDARY_LUNARSHARD_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Dash</style> a short distance and gain <style=cIsHealing>" + StaticValues.dashBarrierAmount * 100f + "% barrier</style>. Successful hits from <style=cIsDamage>Divine Blade</style> reduce cooldown by 1s. Store up to 2 dashes.";

            LanguageAPI.Add("PALADIN_UTILITY_DASH_NAME", "Quickstep");
            LanguageAPI.Add("PALADIN_UTILITY_DASH_DESCRIPTION", desc);

            desc = "Restore <style=cIsHealing>" + StaticValues.healAmount * 100f + "% max health</style> to all allies in an area.";

            LanguageAPI.Add("PALADIN_UTILITY_HEAL_NAME", "Heal");
            LanguageAPI.Add("PALADIN_UTILITY_HEAL_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Bless</style> an area for " + StaticValues.healZoneDuration + " seconds, gradually <style=cIsHealing>restoring health</style> and granting <style=cIsHealing>barrier</style> to all allies inside.";

            LanguageAPI.Add("PALADIN_SPECIAL_HEALZONE_NAME", "Sacred Sunlight");
            LanguageAPI.Add("PALADIN_SPECIAL_HEALZONE_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Silence</style> an area for 8 seconds, inflicting <style=cIsHealth>torpor</style> on all enemies in the vicinity.";

            LanguageAPI.Add("PALADIN_SPECIAL_TORPOR_NAME", "Vow of Silence");
            LanguageAPI.Add("PALADIN_SPECIAL_TORPOR_DESCRIPTION", desc);

            LanguageAPI.Add("KEYWORD_SWORDBEAM", "<style=cKeywordName>Sword Beam</style><style=cSub>A piercing, short range beam of light that deals <style=cIsDamage>" + StaticValues.beamDamageCoefficient + "% damage</style>.");
            LanguageAPI.Add("KEYWORD_TORPOR", "<style=cKeywordName>Torpor</style><style=cSub>Applies a <style=cIsHealth>" + StaticValues.torporSlowAmount + "%</style> attack and movement speed <style=cIsDamage>slow</style>. Drags enemies to the ground.");


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
