//using R2API;
using System;

namespace PaladinMod.Modules {

    public static class Tokens
    {

        public static void Init() {
            AddTokens();
            Languages.PrintOutput("paladin.txt");
        }
        public static void AddTokens()
        {
            string desc = "The Paladin is a versatile spellblade that can opt for otherworldly magic or devastating swordsmanship to aid allies and decimate foes.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Your passive makes up a good portion of your damage, try to keep it up as much as possible." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Spinning Slash can serve as either a powerful crowd control tool or a form of limited mobility." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Quickstep's cooldown is lowered with each hit, rewarding you for staying in the thick of it." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Vow of Silence is a great way to deal with flying enemies, as it drags all affected down to the ground." + Environment.NewLine + Environment.NewLine;

            //string desc = "Paladin" + Environment.NewLine + Environment.NewLine;

            Languages.Add("PALADIN_NAME", StaticValues.characterName);
            Languages.Add("PALADIN_DESCRIPTION", desc);
            Languages.Add("PALADIN_SUBTITLE", StaticValues.characterSubtitle);
            Languages.Add("PALADIN_LORE", StaticValues.characterLore);
            Languages.Add("PALADIN_OUTRO_FLAVOR", StaticValues.characterOutro);
            Languages.Add("PALADIN_OUTRO_FAILURE", StaticValues.characterOutroFailure);


            Languages.Add("PALADINBODY_DEFAULT_SKIN_NAME", "Default");
            Languages.Add("PALADINBODY_LUNAR_SKIN_NAME", "Lunar");
            Languages.Add("PALADINBODY_LUNARKNIGHT_SKIN_NAME", "Lunar Knight");
            Languages.Add("PALADINBODY_TYPHOON_SKIN_NAME", "Sovereign");
            Languages.Add("PALADINBODY_TYPHOONLEGACY_SKIN_NAME", "Sovereign (legacy)");
            Languages.Add("PALADINBODY_POISON_SKIN_NAME", "Corruption");
            Languages.Add("PALADINBODY_POISONLEGACY_SKIN_NAME", "Corruption (legacy)");
            Languages.Add("PALADINBODY_CLAY_SKIN_NAME", "Aphelian");
            Languages.Add("PALADINBODY_SPECTER_SKIN_NAME", "Specter");
            Languages.Add("PALADINBODY_DRIP_SKIN_NAME", "Drip");
            Languages.Add("PALADINBODY_MINECRAFT_SKIN_NAME", "Minecraft");


            //LanguageAPI.Add("LUNAR_KNIGHT_BODY_NAME", "Lunar Knight");
            //LanguageAPI.Add("LUNAR_KNIGHT_BODY_DESCRIPTION", desc);
            //LanguageAPI.Add("LUNAR_KNIGHT_BODY_SUBTITLE", "Acolyte of Mithrix");
            //LanguageAPI.Add("LUNAR_KNIGHT_BODY_LORE", StaticValues.characterLore);
            //LanguageAPI.Add("LUNAR_KNIGHT_BODY_OUTRO_FLAVOR", StaticValues.characterOutro);


            //LanguageAPI.Add("NEMPALADIN_NAME", "Nemesis Paladin");
            //LanguageAPI.Add("NEMPALADIN_DESCRIPTION", desc);
            //LanguageAPI.Add("NEMPALADIN_SUBTITLE", StaticValues.characterSubtitle);
            //LanguageAPI.Add("NEMPALADIN_LORE", StaticValues.characterLore);
            //LanguageAPI.Add("NEMPALADIN_OUTRO_FLAVOR", StaticValues.characterOutro);
            //LanguageAPI.Add("NEMPALADIN_OUTRO_FAILURE", StaticValues.characterOutroFailure);


            Languages.Add("PALADIN_PASSIVE_NAME", "Unwavering Faith");
            Languages.Add("PALADIN_PASSIVE_DESCRIPTION", "While above <style=cIsHealth>90% health</style> or while having active <style=cIsHealth>barrier</style>, the Paladin is <style=cIsHealing>blessed</style>, empowering all sword skills and <style=cIsHealing>blessing all nearby allies as well</style>.");

            desc = "Slash for <style=cIsDamage>" + 100f * StaticValues.slashDamageCoefficient + "% damage</style>. Fires a <style=cIsUtility>beam of light</style> for <style=cIsDamage>" + 100f * StaticValues.beamDamageCoefficient + "% damage</style> if the Paladin is <style=cIsHealing>blessed</style>.";

            Languages.Add("PALADIN_PRIMARY_SLASH_NAME", "Divine Blade");
            Languages.Add("PALADIN_PRIMARY_SLASH_DESCRIPTION", desc);

            desc = "Slash for <style=cIsDamage>" + 100f * StaticValues.cursedBladeDamageCoefficient + "% damage</style>. Fires a <style=cIsUtility>beam of light</style> for <style=cIsDamage>" + 100f * StaticValues.beamDamageCoefficient + "% damage</style> if the Paladin is <style=cIsHealing>blessed</style>.";

            Languages.Add("PALADIN_PRIMARY_CURSESLASH_NAME", "Accursed Blade");
            Languages.Add("PALADIN_PRIMARY_CURSESLASH_DESCRIPTION", desc);

            desc = "Perform a <style=cIsUtility>wide stunning slash</style> for <style=cIsDamage>" + 100f * StaticValues.spinSlashDamageCoefficient + "% damage</style>, gaining range if <style=cIsHealing>blessed</style>. While airborne, instead perform a leap strike, firing a <style=cIsUtility>shockwave</style> if <style=cIsHealing>blessed</style>.";

            Languages.Add("PALADIN_SECONDARY_SPINSLASH_NAME", "Spinning Slash");
            Languages.Add("PALADIN_SECONDARY_SPINSLASH_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Shocking.</style> <style=cIsUtility>Agile.</style> Charge up and throw a <style=cIsUtility>lightning bolt</style>, dealing up to <style=cIsDamage>" + 100f * StaticValues.lightningSpearMaxDamageCoefficient + "% damage</style>. Throw at your feet to coat your blade in <style=cIsUtility>lightning</style> for <style=cIsUtility>4 seconds</style>.";

            Languages.Add("PALADIN_SECONDARY_LIGHTNING_NAME", "Lightning Spear");
            Languages.Add("PALADIN_SECONDARY_LIGHTNING_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Agile.</style> Fire a volley of <style=cIsUtility>lunar shards</style>, dealing <style=cIsDamage>" + 100f * StaticValues.lunarShardDamageCoefficient + "% damage</style> each. Hold up to <style=cIsDamage>" + StaticValues.lunarShardMaxStock + "</style> shards.";

            Languages.Add("PALADIN_SECONDARY_LUNARSHARD_NAME", "Lunar Shards");
            Languages.Add("PALADIN_SECONDARY_LUNARSHARD_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Dash</style> a short distance and gain <style=cIsHealing>" + StaticValues.dashBarrierAmount * 100f + "% barrier</style>. Successful hits from <style=cIsDamage>Divine Blade</style> <style=cIsUtility>lower cooldown</style> by <style=cIsDamage>1 second</style>. <style=cIsUtility>Store up to 2 dashes.<style=cIsHealing>";

            Languages.Add("PALADIN_UTILITY_DASH_NAME", "Quickstep");
            Languages.Add("PALADIN_UTILITY_DASH_DESCRIPTION", desc);

            desc = "Restore <style=cIsHealing>" + StaticValues.healAmount * 100f + "% max health</style> and grant <style=cIsHealing>" + StaticValues.healBarrier * 100f + "% barrier</style> to all allies in an area.";

            Languages.Add("PALADIN_UTILITY_HEAL_NAME", "Replenish");
            Languages.Add("PALADIN_UTILITY_HEAL_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Channel</style> for <style=cIsDamage>" + StaticValues.healZoneChannelDuration + "</style> seconds, then release to <style=cIsUtility>Bless</style> an area for <style=cIsDamage>" + StaticValues.healZoneDuration + " seconds</style>, gradually <style=cIsHealing>restoring health</style> and granting <style=cIsHealing>barrier</style> to all allies inside.";

            Languages.Add("PALADIN_SPECIAL_HEALZONE_NAME", "Sacred Sunlight");
            Languages.Add("PALADIN_SPECIAL_HEALZONE_DESCRIPTION", desc);

            desc += Helpers.ScepterDescription("Double healing. Double barrier. Cleanses debuffs.");

            Languages.Add("PALADIN_SPECIAL_SCEPTERHEALZONE_NAME", "Hallowed Sunlight");
            Languages.Add("PALADIN_SPECIAL_SCEPTERHEALZONE_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Channel</style> for <style=cIsDamage>" + StaticValues.torporChannelDuration + "</style> seconds, then release to <style=cIsUtility>Silence</style> an area for <style=cIsDamage>" + StaticValues.torporDuration + " seconds</style>, inflicting <style=cIsHealth>torpor</style> on all enemies in the vicinity.";

            Languages.Add("PALADIN_SPECIAL_TORPOR_NAME", "Vow of Silence");
            Languages.Add("PALADIN_SPECIAL_TORPOR_DESCRIPTION", desc);

            desc += Helpers.ScepterDescription("Stronger debuff. Larger radius. Destroys projectiles.");

            Languages.Add("PALADIN_SPECIAL_SCEPTERTORPOR_NAME", "Oath of Silence");
            Languages.Add("PALADIN_SPECIAL_SCEPTERTORPOR_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Channel</style> for <style=cIsDamage>" + StaticValues.warcryChannelDuration + "</style> seconds, then release to <style=cIsUtility>Empower</style> an area for <style=cIsDamage>" + StaticValues.warcryDuration + " seconds</style>, increasing <style=cIsDamage>damage</style> and <style=cIsDamage>attack speed</style> for all allies inside.";

            Languages.Add("PALADIN_SPECIAL_WARCRY_NAME", "Sacred Oath");
            Languages.Add("PALADIN_SPECIAL_WARCRY_DESCRIPTION", desc);

            desc += Helpers.ScepterDescription("Faster cast speed. Double damage. Double attack speed.");

            Languages.Add("PALADIN_SPECIAL_SCEPTERWARCRY_NAME", "Sacred Oath (Scepter)");
            Languages.Add("PALADIN_SPECIAL_SCEPTERWARCRY_DESCRIPTION", desc);

            desc = "<style=cIsHealth>Overheat</style>. <style=cIsUtility>Channel</style> for <style=cIsDamage>" + StaticValues.cruelSunChannelDuration + "</style> seconds to create a <style=cIsUtility>miniature star</style> for <style=cIsDamage>" + StaticValues.cruelSunDuration + "</style> seconds that overheats <style=cDeath>EVERYTHING</style> around it. At <style=cIsHealth>" + StaticValues.cruelSunMinimumStacksBeforeApplyingBurns + "</style> stacks or more, targets burn for <style=cIsDamage>" + StaticValues.cruelSunBurnDamageCoefficient * 100f + "% damage</style>.";
            
            Languages.Add("PALADIN_SPECIAL_SUN_NAME", "Cruel Sun");
            Languages.Add("PALADIN_SPECIAL_SUN_DESCRIPTION", desc);

            Languages.Add("PALADIN_SPECIAL_SUN_CANCEL_NAME", "Cancel Cruel Sun");
            Languages.Add("PALADIN_SPECIAL_SUN_CANCEL_DESCRIPTION", "Stop channelling the current Cruel Sun.");

            desc += Helpers.ScepterDescription("Cast again and hold to aim, then release to throw the star, exploding for <style=cIsDamage>" + StaticValues.prideFlareDamageCoefficient * Config.prideFlareMultiplier.Value * 100f + "% damage</style> to <style=cDeath>EVERYTHING</style> around it.");

            Languages.Add("PALADIN_SPECIAL_SCEPSUN_NAME", "Pride Flare");
            Languages.Add("PALADIN_SPECIAL_SCEPSUN_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Channel</style> for <style=cIsDamage>" + StaticValues.cruelSunChannelDurationOld + "</style> seconds, then release to create a <style=cIsUtility>miniature star</style> that <style=cIsDamage>drains health</style> from <style=cIsHealth>ALL</style> entities around it.";

            Languages.Add("PALADIN_SPECIAL_SUN_LEGACY_NAME", "Cruel Sun (Legacy)");
            Languages.Add("PALADIN_SPECIAL_SUN_LEGACY_DESCRIPTION", desc);

            desc += Helpers.ScepterDescription("Explodes for a massive burst of " + PaladinMod.States.Spell.ScepterCastCruelSunOld.flareDamageCoefficient* 100f + "% damage.");

            Languages.Add("PALADIN_SPECIAL_SCEPSUN_LEGACY_NAME", "Pride Flare");
            Languages.Add("PALADIN_SPECIAL_SCEPSUN_LEGACY_DESCRIPTION", desc);

            desc = "While below <style=cIsHealth>25% health</style>, generate <style=cIsDamage>Rage</style>. When at max <style=cIsDamage>Rage</style>, use to enter <color=#dc0000>Berserker Mode</color>, gaining a <style=cIsHealing>massive buff</style> and a <style=cIsUtility>new set of skills</style>.";

            Languages.Add("PALADIN_SPECIAL_BERSERK_NAME", "Berserk");
            Languages.Add("PALADIN_SPECIAL_BERSERK_DESCRIPTION", desc);

            Languages.Add("KEYWORD_SWORDBEAM", "<style=cKeywordName>Sword Beam</style><style=cSub>A piercing, short range beam of light that deals <style=cIsDamage>" + 100f * StaticValues.beamDamageCoefficient + "% damage</style>.");
            Languages.Add("KEYWORD_TORPOR", "<style=cKeywordName>Torpor</style><style=cSub>Applies a <style=cIsHealth>" + 100 * StaticValues.torporSlowAmount + "%</style> attack and movement speed <style=cIsDamage>slow</style>. <style=cIsHealth>Drags enemies to the ground.</style>");
            Languages.Add("KEYWORD_OVERHEAT", "<style=cKeywordName>Overheat</style><style=cSub>Multiplies the damage received from <style=cIsDamage>the sun</style>.</style>");

            Languages.Add(GetAchievementNameToken(Achievements.PaladinUnlockAchievement.identifier), "A Paladin's Vow");
            Languages.Add(GetAchievementDescriptionToken(Achievements.PaladinUnlockAchievement.identifier), "Use the Beads of Fealty and become whole once more.");

            Languages.Add(GetAchievementNameToken(Achievements.MasteryAchievement.identifier), "Paladin: Mastery");
            Languages.Add(GetAchievementDescriptionToken(Achievements.MasteryAchievement.identifier), "As Paladin, beat the game or obliterate on Monsoon.");

            Languages.Add(GetAchievementNameToken(Achievements.GrandMasteryAchievement.identifier),        "Paladin: Grand Mastery");
            Languages.Add(GetAchievementDescriptionToken(Achievements.GrandMasteryAchievement.identifier), "As Paladin, beat the game or obliterate on Typhoon or Eclipse.\n<color=#8888>(Counts any difficulty Typhoon or higher)</color>");

            Languages.Add(GetAchievementNameToken(Achievements.PoisonAchievement.identifier), "Paladin: Her Disciple");
            Languages.Add(GetAchievementDescriptionToken(Achievements.PoisonAchievement.identifier), "As Paladin, form a covenant with a goddess of corruption.");

            Languages.Add(GetAchievementNameToken(Achievements.SunlightSpearAchievement.identifier), "Paladin: Jolly Cooperation");
            Languages.Add(GetAchievementDescriptionToken(Achievements.SunlightSpearAchievement.identifier), "As Paladin, strike an enemy with a Royal Capacitor. <color=#c11>Host only</color>");

            Languages.Add(GetAchievementNameToken(Achievements.LunarShardAchievement.identifier), "Paladin: Herald of the Lost King");
            Languages.Add(GetAchievementDescriptionToken(Achievements.LunarShardAchievement.identifier), "As Paladin, hold 8 Lunar items at once.");

            Languages.Add(GetAchievementNameToken(Achievements.HealAchievement.identifier), "Paladin: Warm Embrace");
            Languages.Add(GetAchievementDescriptionToken(Achievements.HealAchievement.identifier), "As Paladin, heal an ally with a Gnarled Woodsprite. <color=#c11>Host only</color>");

            Languages.Add(GetAchievementNameToken(Achievements.TorporAchievement.identifier), "Paladin: Suppression");
            Languages.Add(GetAchievementDescriptionToken(Achievements.TorporAchievement.identifier), "As Paladin, stack 4 debuffs on one enemy.");

            Languages.Add(GetAchievementNameToken(Achievements.CruelSunAchievement.identifier), "Paladin: Sunshine");
            Languages.Add(GetAchievementDescriptionToken(Achievements.CruelSunAchievement.identifier), "As Paladin, bear the full brunt of a Sun and survive.");

            Languages.Add(GetAchievementNameToken(Achievements.ClayAchievement.identifier), "Paladin: Ancient Relic");
            Languages.Add(GetAchievementDescriptionToken(Achievements.ClayAchievement.identifier), "As Paladin, acquire a certain parasitic urn.");


            Languages.Add("BROTHER_SEE_PALADIN_1", "Brother? No. Cheap imitation.");
            Languages.Add("BROTHER_SEE_PALADIN_2", "I will answer to your faith.");
            Languages.Add("BROTHER_SEE_PALADIN_3", "Wasted potential.");

            Languages.Add("BROTHER_KILL_PALADIN_1", "Your crude armor fails you.");
            Languages.Add("BROTHER_KILL_PALADIN_2", "Look where your faith has brought you.");
            Languages.Add("BROTHER_KILL_PALADIN_3", "Return to nothing, foolish devotee.");

            Languages.Add("FALSESON_SEE_PALADIN_1", "Familiar...");
            Languages.Add("FALSESON_SEE_PALADIN_2", "That sword...");
            Languages.Add("FALSESON_SEE_PALADIN_3", "An echo?");
        }

        /// <summary>
        /// gets langauge token from achievement's registered identifier
        /// </summary>
        ///</BEARD SHAMPOO>
        public static string GetAchievementNameToken(string identifier)
        {
            return $"ACHIEVEMENT_{identifier.ToUpperInvariant()}_NAME";
        }
        /// <summary>
        /// gets langauge token from achievement's registered identifier
        /// </summary>
        public static string GetAchievementDescriptionToken(string identifier)
        {
            return $"ACHIEVEMENT_{identifier.ToUpperInvariant()}_DESCRIPTION";
        }
    }
}