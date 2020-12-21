using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace PaladinMod.Modules
{
    public static class Projectiles
    {
        public static GameObject lightningSpear;
        public static GameObject swordBeam;
        public static GameObject lunarShard;

        public static GameObject heal;
        public static GameObject healZone;
        public static GameObject torpor;

        public static void RegisterProjectiles()
        {
            lunarShard = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/LunarShardProjectile"), "PaladinLunarShard", true);
            PaladinPlugin.Destroy(lunarShard.GetComponent<ProjectileSteerTowardTarget>());
            lunarShard.GetComponent<ProjectileImpactExplosion>().blastDamageCoefficient = 1f;

            lightningSpear = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/MageIceBombProjectile"), "LightningSpear", true);
            GameObject spearGhost = Assets.lightningSpear.InstantiateClone("LightningSpearGhost", false);
            spearGhost.AddComponent<ProjectileGhostController>();

            lightningSpear.transform.localScale *= 2f;

            lightningSpear.GetComponent<ProjectileController>().ghostPrefab = spearGhost;
            lightningSpear.GetComponent<ProjectileOverlapAttack>().impactEffect = Assets.lightningHitFX;
            lightningSpear.GetComponent<ProjectileDamage>().damageType = DamageType.Shock5s;
            lightningSpear.GetComponent<ProjectileSingleTargetImpact>().impactEffect = Assets.lightningImpactFX;

            swordBeam = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/FMJ"), "PaladinSwordBeam", true);
            swordBeam.transform.localScale *= 2f;
            GameObject beamGhost = Assets.swordBeam.InstantiateClone("SwordBeamGhost", false);
            beamGhost.AddComponent<ProjectileGhostController>();

            swordBeam.GetComponent<ProjectileController>().ghostPrefab = Resources.Load<GameObject>("Prefabs/Projectiles/EvisProjectile").GetComponent<ProjectileController>().ghostPrefab;
            swordBeam.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;

            if (swordBeam.GetComponent<ProjectileProximityBeamController>()) PaladinPlugin.Destroy(swordBeam.GetComponent<ProjectileProximityBeamController>());

            swordBeam.AddComponent<DestroyOnTimer>().duration = 0.3f;

            heal = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "PaladinHeal", true);
            heal.transform.localScale = Vector3.one;

            PaladinMod.PaladinPlugin.Destroy(heal.GetComponent<ProjectileDotZone>());

            heal.AddComponent<DestroyOnTimer>().duration = 5f;

            Misc.PaladinHealController healController = heal.AddComponent<Misc.PaladinHealController>();

            healController.radius = StaticValues.healRadius;
            healController.healAmount = StaticValues.healAmount;

            PaladinMod.PaladinPlugin.Destroy(heal.transform.GetChild(0).gameObject);
            GameObject healFX = Assets.healEffectPrefab.InstantiateClone("HealEffect", false);
            healFX.transform.parent = heal.transform;
            healFX.transform.localPosition = Vector3.zero;

            healFX.transform.localScale = Vector3.one * StaticValues.healRadius * 2f;

            healZone = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "PaladinHealZone", true);
            healZone.transform.localScale = Vector3.one;

            PaladinMod.PaladinPlugin.Destroy(healZone.GetComponent<ProjectileDotZone>());

            healZone.AddComponent<DestroyOnTimer>().duration = StaticValues.healZoneDuration + 2f;

            Misc.PaladinHealZoneController healZoneController = healZone.AddComponent<Misc.PaladinHealZoneController>();

            healZoneController.radius = StaticValues.healZoneRadius;
            healZoneController.interval = 0.25f;
            healZoneController.rangeIndicator = null;
            healZoneController.buffType = Buffs.healZoneArmorBuff;
            healZoneController.buffDuration = 1.5f;
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

            ProjectileCatalog.getAdditionalEntries += list =>
            {
                list.Add(lightningSpear);
                list.Add(swordBeam);
                list.Add(lunarShard);
                list.Add(heal);
                list.Add(healZone);
                list.Add(torpor);
            };
        }

        public static void InitSpellEffect(GameObject target, float radius, float duration)
        {
            target.transform.localScale = Vector3.one * radius * 2f;

            var particleSystem = target.GetComponentInChildren<ParticleSystem>().main;
            particleSystem.startLifetime = duration;
        }
    }
}
