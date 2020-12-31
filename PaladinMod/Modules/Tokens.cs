using R2API;
using System;

namespace PaladinMod.Modules
{
    public static class Tokens
    {
        public static void AddTokens()
        {
            string desc = "The Paladin is a heavy hitting tank that can opt for otherworldly magic or devastating swordsmanship to aid allies and decimate foes.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Your passive makes up a good portion of your damage, try to keep it up as much as possible." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Spinning Slash can serve as either a powerful crowd control tool or a form of limited mobility." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Quickstep's cooldown is lowered with each hit, rewarding you for staying in the thick of it." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Vow of Silence is a great way to deal with flying enemies, as it drags all affected down to the ground." + Environment.NewLine + Environment.NewLine;

            //string desc = "Paladin" + Environment.NewLine + Environment.NewLine;

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

            desc = "Slash forward for <style=cIsDamage>" + 100f * StaticValues.slashDamageCoefficient + "% damage</style>. Fires a <style=cIsUtility>beam of light</style> for <style=cIsDamage>" + 100f * StaticValues.beamDamageCoefficient + "% damage</style> if the Paladin is <style=cIsHealing>blessed</style>.";

            LanguageAPI.Add("PALADIN_PRIMARY_SLASH_NAME", "Divine Blade");
            LanguageAPI.Add("PALADIN_PRIMARY_SLASH_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Stunning.</style> Perform a wide sweeping slash for <style=cIsDamage>" + 100f * StaticValues.spinSlashDamageCoefficient + "% damage</style>, gaining range if <style=cIsHealing>blessed</style>. Use while airborne to perform a leap strike for <style=cIsDamage>" + StaticValues.spinSlashDamageCoefficient * 100f + "% damage</style>, firing a <style=cIsUtility>shockwave</style> if <style=cIsHealing>blessed</style>.";

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
