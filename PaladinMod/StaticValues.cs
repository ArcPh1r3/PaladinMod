namespace PaladinMod
{
    class StaticValues
    {
        //
        public const string characterName = "Paladin";
        public const string characterSubtitle = "Acolyte of Providence";
        public const string characterOutro = "..and so he left, faith in his doctrine shaken.";
        public const string characterLore = "\nsample text";

        //Misc
        public const float maxSwordGlow = 3f;
        public const float swordGlowSpeed = 8f;

        //Base stats
        public const float baseDamage = 13f;
        public const float baseDamagePerLevel = baseDamage * 0.2f;
        public const float armorPerLevel = 1f;

        //Slash
        public const float slashDamageCoefficient = 3.5f;

        public const float beamDamageCoefficient = 3f;
        public const float beamSpeed = 160f;

        //Spinning Slash
        public const float spinSlashDamageCoefficient = 8f;

        //Sunlight Spear
        public const float lightningSpearMinDamageCoefficient = 2f;
        public const float lightningSpearMaxDamageCoefficient = 10f;
        public const float lightningSpearChargeTime = 1.5f;

        //Lunar Shards
        public const float lunarShardDamageCoefficient = 2.25f;
        public const int lunarShardMaxStock = 12;

        //Quickstep
        public const float dashBarrierAmount = 0.1f;

        //Replenish
        public const float healRadius = 16f;
        public const float healAmount = 0.1f;
        public const float healBarrier = 0.1f;

        //Sacred Sunlight
        public const float healZoneChannelDuration = 1.5f;
        public const float healZoneRadius = 16f;
        public const float healZoneAmount = 0.01f;
        public const float healZoneBarrier = 0.01f;
        public const float healZoneDuration = 12f;

        //Hallowed Sunlight
        public const float scepterHealZoneChannelDuration = 2f;
        public const float scepterHealZoneRadius = 24f;
        public const float scepterHealZoneAmount = 0.02f;
        public const float scepterHealZoneBarrier = 0.015f;
        public const float scepterHealZoneDuration = 12f;

        //Vow of Silence
        public const float torporChannelDuration = 2f;
        public const float torporRadius = 24f;
        public const float torporSlowAmount = 0.6f;
        public const float torporDuration = 10;

        //Oath of Silence
        public const float scepterTorporChannelDuration = 2f;
        public const float scepterTorporRadius = 24f;
        public const float scepterTorporSlowAmount = 0.8f;
        public const float scepterTorporDuration = 12;

        //Sacred Oath
        public const float warcryChannelDuration = 2.5f;
        public const float warcryRadius = 20f;
        public const float warcryDamageMultiplier = 0.5f;
        public const float warcryAttackSpeedBuff = 1;
        public const float warcryDuration = 8;

        //Sacred Oath(Scepter)
        public const float scepterWarcryChannelDuration = 2f;
        public const float scepterWarcryRadius = 24f;
        public const float scepterWarcryDamageMultiplier = 1f;
        public const float scepterWarcryAttackSpeedBuff = 2f;
        public const float scepterWarcryDuration = 12;
    }
}