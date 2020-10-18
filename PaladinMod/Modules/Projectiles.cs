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

        public static void RegisterProjectiles()
        {
            lightningSpear = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/MageIceBombProjectile"), "LightningSpear", true);
            GameObject spearGhost = Assets.lightningSpear.InstantiateClone("LightningSpearGhost", false);
            spearGhost.AddComponent<ProjectileGhostController>();

            lightningSpear.GetComponent<ProjectileController>().ghostPrefab = spearGhost;
            lightningSpear.GetComponent<ProjectileOverlapAttack>().impactEffect = Assets.lightningHitFX;
            lightningSpear.GetComponent<ProjectileDamage>().damageType = DamageType.Shock5s;
            lightningSpear.GetComponent<ProjectileSingleTargetImpact>().impactEffect = Assets.lightningImpactFX;

            swordBeam = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/FMJ"), "PaladinSwordBeam", true);
            GameObject beamGhost = Assets.swordBeam.InstantiateClone("SwordBeamGhost", false);
            beamGhost.AddComponent<ProjectileGhostController>();

            var particleStuff = beamGhost.GetComponentInChildren<ParticleSystem>().main;
            particleStuff.simulationSpace = ParticleSystemSimulationSpace.Local;

            swordBeam.GetComponent<ProjectileController>().ghostPrefab = beamGhost;
            swordBeam.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;

            swordBeam.AddComponent<DestroyOnTimer>().duration = 0.25f;

            ProjectileCatalog.getAdditionalEntries += list =>
            {
                list.Add(lightningSpear);
                list.Add(swordBeam);
            };
        }
    }
}
