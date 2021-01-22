using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace PaladinMod.Modules
{
    public static class Projectiles
    {
        public static GameObject swordBeam;
        public static GameObject shockwave;
        public static GameObject lightningSpear;
        public static GameObject lunarShard;

        public static GameObject heal;
        public static GameObject healZone;
        public static GameObject torpor;
        public static GameObject warcry;

        public static GameObject scepterHealZone;
        public static GameObject scepterTorpor;
        public static GameObject scepterWarcry;

        public static void LateSetup()
        {
            //fuck man
            var overlapAttack = swordBeam.GetComponent<ProjectileOverlapAttack>();
            if (overlapAttack) overlapAttack.damageCoefficient = 1f;
        }

        public static void RegisterProjectiles()
        {
            //would like to simplify this all eventually....
            #region SpinningSlashShockwave
            shockwave = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/BrotherSunderWave"), "PaladinShockwave", true);
            shockwave.transform.GetChild(0).transform.localScale = new Vector3(10, 1.5f, 1);
            shockwave.GetComponent<ProjectileCharacterController>().lifetime = 0.5f;
            shockwave.GetComponent<ProjectileDamage>().damageType = DamageType.Stun1s;

            GameObject shockwaveGhost = PrefabAPI.InstantiateClone(shockwave.GetComponent<ProjectileController>().ghostPrefab, "PaladinShockwaveGhost", false);
            shockwaveGhost.transform.GetChild(0).transform.localScale = new Vector3(10, 1, 1);
            shockwaveGhost.transform.GetChild(1).transform.localScale = new Vector3(10, 1.5f, 1);
            PaladinPlugin.Destroy(shockwaveGhost.transform.GetChild(0).Find("Infection, World").gameObject);
            PaladinPlugin.Destroy(shockwaveGhost.transform.GetChild(0).Find("Water").gameObject);

            Material shockwaveMat = Resources.Load<GameObject>("Prefabs/Projectiles/LunarWispTrackingBomb").GetComponent<ProjectileController>().ghostPrefab.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material;
            shockwaveGhost.transform.GetChild(1).GetComponent<MeshRenderer>().material = shockwaveMat;

            shockwave.GetComponent<ProjectileController>().ghostPrefab = shockwaveGhost;
            #endregion

            #region LunarShard
            lunarShard = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/LunarShardProjectile"), "PaladinLunarShard", true);
            PaladinPlugin.Destroy(lunarShard.GetComponent<ProjectileSteerTowardTarget>());
            lunarShard.GetComponent<ProjectileImpactExplosion>().blastDamageCoefficient = 1f;
            #endregion

            #region SunlightSpear
            lightningSpear = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/MageLightningBombProjectile"), "LightningSpear", true);
            GameObject spearGhost = Assets.lightningSpear.InstantiateClone("LightningSpearGhost", false);
            spearGhost.AddComponent<ProjectileGhostController>();

            lightningSpear.transform.localScale *= 2f;

            lightningSpear.GetComponent<ProjectileController>().ghostPrefab = spearGhost;
            //lightningSpear.GetComponent<ProjectileOverlapAttack>().impactEffect = Assets.lightningImpactFX;
            lightningSpear.GetComponent<ProjectileDamage>().damageType = DamageType.Shock5s;
            lightningSpear.GetComponent<ProjectileImpactExplosion>().impactEffect = Assets.altLightningImpactFX;
            lightningSpear.GetComponent<Rigidbody>().useGravity = false;

            PaladinPlugin.Destroy(lightningSpear.GetComponent<AntiGravityForce>());
            PaladinPlugin.Destroy(lightningSpear.GetComponent<ProjectileProximityBeamController>());
            #endregion

            #region SwordBeam
            swordBeam = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/FMJ"), "PaladinSwordBeam", true);
            swordBeam.transform.localScale *= 2f;
            //GameObject beamGhost = Assets.swordBeam.InstantiateClone("SwordBeamGhost", false);
            //beamGhost.AddComponent<ProjectileGhostController>();

            //swordBeam.GetComponent<ProjectileController>().ghostPrefab = Assets.swordBeamGhost;
            swordBeam.GetComponent<ProjectileController>().ghostPrefab = Resources.Load<GameObject>("Prefabs/Projectiles/EvisProjectile").GetComponent<ProjectileController>().ghostPrefab;
            swordBeam.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;

            //run this in case moffein's phase round lightning is installed
            if (swordBeam.GetComponent<ProjectileProximityBeamController>()) PaladinPlugin.Destroy(swordBeam.GetComponent<ProjectileProximityBeamController>());

            swordBeam.AddComponent<DestroyOnTimer>().duration = 0.3f;
            #endregion

            #region Replenish
            heal = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "PaladinHeal", true);
            heal.transform.localScale = Vector3.one;

            PaladinMod.PaladinPlugin.Destroy(heal.GetComponent<ProjectileDotZone>());

            heal.AddComponent<DestroyOnTimer>().duration = 5f;

            Misc.PaladinHealController healController = heal.AddComponent<Misc.PaladinHealController>();

            healController.radius = StaticValues.healRadius;
            healController.healAmount = StaticValues.healAmount;
            healController.barrierAmount = StaticValues.healBarrier;

            PaladinMod.PaladinPlugin.Destroy(heal.transform.GetChild(0).gameObject);
            GameObject healFX = Assets.healEffectPrefab.InstantiateClone("HealEffect", false);
            healFX.transform.parent = heal.transform;
            healFX.transform.localPosition = Vector3.zero;

            healFX.transform.localScale = Vector3.one * StaticValues.healRadius * 2f;
            #endregion

            #region SacredSunlight
            healZone = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "PaladinHealZone", true);
            healZone.transform.localScale = Vector3.one;

            PaladinMod.PaladinPlugin.Destroy(healZone.GetComponent<ProjectileDotZone>());

            healZone.AddComponent<DestroyOnTimer>().duration = StaticValues.healZoneDuration + 2f;

            Misc.PaladinHealZoneController healZoneController = healZone.AddComponent<Misc.PaladinHealZoneController>();

            healZoneController.radius = StaticValues.healZoneRadius;
            healZoneController.interval = 0.25f;
            healZoneController.rangeIndicator = null;
            healZoneController.buffType = BuffIndex.None;
            healZoneController.buffDuration = 0f;
            healZoneController.floorWard = false;
            healZoneController.expires = true;
            healZoneController.invertTeamFilter = false;
            healZoneController.expireDuration = StaticValues.healZoneDuration;
            healZoneController.animateRadius = false;
            healZoneController.healAmount = StaticValues.healZoneAmount;
            healZoneController.barrierAmount = StaticValues.healZoneBarrier;
            healZoneController.freezeProjectiles = false;

            PaladinMod.PaladinPlugin.Destroy(healZone.transform.GetChild(0).gameObject);
            GameObject healZoneFX = Assets.healZoneEffectPrefab.InstantiateClone("HealZoneEffect", false);
            healZoneFX.transform.parent = healZone.transform;
            healZoneFX.transform.localPosition = Vector3.zero;

            InitSpellEffect(healZoneFX, StaticValues.healZoneRadius, StaticValues.healZoneDuration);
            #endregion

            #region ScepterSacredSunlight
            scepterHealZone = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "PaladinScepterHealZone", true);
            scepterHealZone.transform.localScale = Vector3.one;

            PaladinMod.PaladinPlugin.Destroy(scepterHealZone.GetComponent<ProjectileDotZone>());

            scepterHealZone.AddComponent<DestroyOnTimer>().duration = StaticValues.scepterHealZoneDuration + 2f;

            Misc.PaladinHealZoneController scepterHealZoneController = scepterHealZone.AddComponent<Misc.PaladinHealZoneController>();

            scepterHealZoneController.radius = StaticValues.scepterHealZoneRadius;
            scepterHealZoneController.interval = 0.25f;
            scepterHealZoneController.rangeIndicator = null;
            scepterHealZoneController.buffType = BuffIndex.None;
            scepterHealZoneController.buffDuration = 0f;
            scepterHealZoneController.floorWard = false;
            scepterHealZoneController.expires = true;
            scepterHealZoneController.invertTeamFilter = false;
            scepterHealZoneController.expireDuration = StaticValues.scepterHealZoneDuration;
            scepterHealZoneController.animateRadius = false;
            scepterHealZoneController.healAmount = StaticValues.scepterHealZoneAmount;
            scepterHealZoneController.barrierAmount = StaticValues.scepterHealZoneBarrier;
            scepterHealZoneController.freezeProjectiles = false;

            PaladinMod.PaladinPlugin.Destroy(scepterHealZone.transform.GetChild(0).gameObject);
            GameObject scepterHealZoneFX = Assets.healZoneEffectPrefab.InstantiateClone("ScepterHealZoneEffect", false);
            scepterHealZoneFX.transform.parent = scepterHealZone.transform;
            scepterHealZoneFX.transform.localPosition = Vector3.zero;

            InitSpellEffect(scepterHealZoneFX, StaticValues.scepterHealZoneRadius, StaticValues.scepterHealZoneDuration);
            #endregion

            #region VowOfSilence
            torpor = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "PaladinTorpor", true);
            torpor.transform.localScale = Vector3.one;

            PaladinMod.PaladinPlugin.Destroy(torpor.GetComponent<ProjectileDotZone>());

            torpor.AddComponent<DestroyOnTimer>().duration = StaticValues.torporDuration + 2f;

            Misc.PaladinHealZoneController torporController = torpor.AddComponent<Misc.PaladinHealZoneController>();

            torporController.radius = StaticValues.torporRadius;
            torporController.interval = 1f;
            torporController.rangeIndicator = null;
            torporController.buffType = Buffs.torporDebuff;
            torporController.buffDuration = 1f;
            torporController.floorWard = false;
            torporController.expires = true;
            torporController.invertTeamFilter = true;
            torporController.expireDuration = StaticValues.torporDuration;
            torporController.animateRadius = false;
            torporController.healAmount = 0f;
            torporController.freezeProjectiles = false;
            torporController.grounding = true;

            PaladinMod.PaladinPlugin.Destroy(torpor.transform.GetChild(0).gameObject);
            GameObject torporFX = Assets.torporEffectPrefab.InstantiateClone("TorporEffect", false);
            torporFX.transform.parent = torpor.transform;
            torporFX.transform.localPosition = Vector3.zero;

            InitSpellEffect(torporFX, StaticValues.torporRadius, StaticValues.torporDuration);
            #endregion

            #region ScepterVowOfSilence
            scepterTorpor = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "PaladinScepterTorpor", true);
            scepterTorpor.transform.localScale = Vector3.one;

            PaladinMod.PaladinPlugin.Destroy(scepterTorpor.GetComponent<ProjectileDotZone>());

            scepterTorpor.AddComponent<DestroyOnTimer>().duration = StaticValues.scepterTorporDuration + 2f;

            Misc.PaladinHealZoneController scepterTorporController = scepterTorpor.AddComponent<Misc.PaladinHealZoneController>();

            scepterTorporController.radius = StaticValues.scepterTorporRadius;
            scepterTorporController.interval = 1f;
            scepterTorporController.rangeIndicator = null;
            scepterTorporController.buffType = Buffs.scepterTorporDebuff;
            scepterTorporController.buffDuration = 1f;
            scepterTorporController.floorWard = false;
            scepterTorporController.expires = true;
            scepterTorporController.invertTeamFilter = true;
            scepterTorporController.expireDuration = StaticValues.scepterTorporDuration;
            scepterTorporController.animateRadius = false;
            scepterTorporController.healAmount = 0f;
            scepterTorporController.freezeProjectiles = true;
            scepterTorporController.grounding = true;

            PaladinMod.PaladinPlugin.Destroy(scepterTorpor.transform.GetChild(0).gameObject);
            GameObject scepterTorporFX = Assets.torporEffectPrefab.InstantiateClone("ScepterTorporEffect", false);
            scepterTorporFX.transform.parent = scepterTorpor.transform;
            scepterTorporFX.transform.localPosition = Vector3.zero;

            InitSpellEffect(scepterTorporFX, StaticValues.scepterTorporRadius, StaticValues.scepterTorporDuration);
            #endregion

            #region SacredOath
            warcry = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "PaladinWarcry", true);
            warcry.transform.localScale = Vector3.one;

            PaladinMod.PaladinPlugin.Destroy(warcry.GetComponent<ProjectileDotZone>());

            warcry.AddComponent<DestroyOnTimer>().duration = StaticValues.warcryDuration + 2f;

            BuffWard warcryController = warcry.AddComponent<BuffWard>();

            warcryController.radius = StaticValues.warcryRadius;
            warcryController.interval = 0.25f;
            warcryController.rangeIndicator = null;
            warcryController.buffType = Buffs.warcryBuff;
            warcryController.buffDuration = 1f;
            warcryController.floorWard = false;
            warcryController.expires = true;
            warcryController.invertTeamFilter = false;
            warcryController.expireDuration = StaticValues.warcryDuration;
            warcryController.animateRadius = false;

            PaladinMod.PaladinPlugin.Destroy(warcry.transform.GetChild(0).gameObject);
            GameObject warcryFX = Assets.warcryEffectPrefab.InstantiateClone("WarcryEffect", false);
            warcryFX.transform.parent = warcry.transform;
            warcryFX.transform.localPosition = Vector3.zero;

            InitSpellEffect(warcryFX, StaticValues.warcryRadius, StaticValues.warcryDuration);
            #endregion

            #region ScepterSacredOath
            scepterWarcry = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "PaladinScepterWarcry", true);
            scepterWarcry.transform.localScale = Vector3.one;

            PaladinMod.PaladinPlugin.Destroy(scepterWarcry.GetComponent<ProjectileDotZone>());

            scepterWarcry.AddComponent<DestroyOnTimer>().duration = StaticValues.scepterWarcryDuration + 2f;

            BuffWard scepterWarcryController = scepterWarcry.AddComponent<BuffWard>();

            scepterWarcryController.radius = StaticValues.scepterWarcryRadius;
            scepterWarcryController.interval = 0.25f;
            scepterWarcryController.rangeIndicator = null;
            scepterWarcryController.buffType = Buffs.scepterWarcryBuff;
            scepterWarcryController.buffDuration = 1f;
            scepterWarcryController.floorWard = false;
            scepterWarcryController.expires = true;
            scepterWarcryController.invertTeamFilter = false;
            scepterWarcryController.expireDuration = StaticValues.scepterWarcryDuration;
            scepterWarcryController.animateRadius = false;

            PaladinMod.PaladinPlugin.Destroy(scepterWarcry.transform.GetChild(0).gameObject);
            GameObject scepterWarcryFX = Assets.warcryEffectPrefab.InstantiateClone("ScepterWarcryEffect", false);
            scepterWarcryFX.transform.parent = scepterWarcry.transform;
            scepterWarcryFX.transform.localPosition = Vector3.zero;

            InitSpellEffect(scepterWarcryFX, StaticValues.scepterWarcryRadius, StaticValues.scepterWarcryDuration);
            #endregion

            ProjectileCatalog.getAdditionalEntries += list =>
            {
                list.Add(swordBeam);
                list.Add(shockwave);
                list.Add(lightningSpear);
                list.Add(lunarShard);
                list.Add(heal);
                list.Add(healZone);
                list.Add(scepterHealZone);
                list.Add(torpor);
                list.Add(scepterTorpor);
                list.Add(warcry);
                list.Add(scepterWarcry);
            };
        }

        public static void InitSpellEffect(GameObject target, float radius, float duration)
        {
            target.transform.localScale = Vector3.one * radius * 2f;

            foreach(ParticleSystem i in target.GetComponentsInChildren<ParticleSystem>())
            {
                var particleSystem = i.main;
                particleSystem.startLifetime = duration;
            }
        }
    }
}
