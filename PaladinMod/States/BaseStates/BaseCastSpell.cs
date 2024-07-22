using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace PaladinMod.States
{
    public abstract class BaseCastSpellState : BaseSkillState
    {
        public GameObject projectilePrefab;
        public GameObject muzzleflashEffectPrefab;
        public float baseDuration;
        public Vector3 spellPosition;
        public Quaternion spellRotation;
        private float duration;
        public float charge;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;

            base.PlayAnimation("Gesture, Underride", "ThrowSpell", "Spell.playbackRate", this.duration);

            if (this.muzzleflashEffectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(this.muzzleflashEffectPrefab, base.gameObject, "HandL", false);
            }

            this.Fire();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();

                if (this.projectilePrefab != null)
                {
                    FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
                    {
                        projectilePrefab = this.projectilePrefab,
                        position = this.spellPosition,
                        rotation = this.spellRotation,
                        owner = base.gameObject,
                        damage = this.damageStat,
                        force = 0f,
                        crit = base.RollCrit()
                    };

                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
