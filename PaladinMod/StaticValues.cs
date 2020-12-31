namespace PaladinMod
{
    class StaticValues
    {
        public const string characterName = "Paladin";
        public const string characterSubtitle = "Acolyte of Providence";
        public const string characterOutro = "..and so he left, faith in his doctrine shaken.";
        public const string characterLore = "\nsample text";

        public const float maxSwordGlow = 2f;
        public const float swordGlowSpeed = 1f;

        public const float baseDamage = 13f;
        public const float baseDamagePerLevel = baseDamage * 0.2f;
        public const float armorPerLevel = 1f;

        public const float slashDamageCoefficient = 3.75f;

        public const float beamDamageCoefficient = 3f;
        public const float beamSpeed = 160f;

        public const float spinSlashDamageCoefficient = 8f;

        public const float lightningSpearMinDamageCoefficient = 2f;
        public const float lightningSpearMaxDamageCoefficient = 12f;
        public const float lightningSpearChargeTime = 1.5f;

        public const float lunarShardDamageCoefficient = 2f;
        public const int lunarShardMaxStock = 12;

        public const float dashBarrierAmount = 0.2f;

        public const float healRadius = 12f;
        public const float healAmount = 0.2f;

        public const float healZoneRadius = 14f;
        public const float healZoneAmount = 0.01f;
        public const float healZoneArmor = 10f;
        public const float healZoneBarrier = 0.015f;
        public const float healZoneDuration = 8f;

        public const float torporRadius = 24f;
        public const float torporSlowAmount = 0.6f;
        public const float torporDuration = 8f;
    }
}
