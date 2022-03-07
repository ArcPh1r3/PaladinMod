using RoR2;
using UnityEngine;

namespace PaladinMod.States.Spell
{
    public class ScepterCastCruelSun : CastCruelSun
    {
        public static float flareRadius = 80f;
        public static float flareDamageCoefficient = 90.01f;
        public static float flareProcCoefficient = 1f;
        public static float flareForce = 8000f;

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            EffectData effectData = new EffectData();
            effectData.origin = this.swordController.sunPosition;

            EffectManager.SpawnEffect(RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/GrandparentSpawnImpact"), effectData, false);
            EffectManager.SpawnEffect(RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/ClayBossDeath"), effectData, false);

            if (base.isAuthority)
            {
                BlastAttack blastAttack = new BlastAttack();
                blastAttack.attacker = base.gameObject;
                blastAttack.inflictor = base.gameObject;
                blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                blastAttack.position = this.swordController.sunPosition;
                blastAttack.procCoefficient = ScepterCastCruelSun.flareProcCoefficient;
                blastAttack.radius = ScepterCastCruelSun.flareRadius;
                blastAttack.baseForce = ScepterCastCruelSun.flareForce;
                blastAttack.bonusForce = Vector3.zero;
                blastAttack.baseDamage = ScepterCastCruelSun.flareDamageCoefficient * this.damageStat;
                blastAttack.falloffModel = BlastAttack.FalloffModel.Linear;
                blastAttack.damageColorIndex = DamageColorIndex.Item;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                blastAttack.Fire();
            }

            base.OnExit();

            base.PlayAnimation("Gesture, Override", "CastSunEnd", "Spell.playbackRate", 1.5f);
        }
    }
}