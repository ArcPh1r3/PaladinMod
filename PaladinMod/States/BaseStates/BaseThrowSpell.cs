using EntityStates;
using PaladinMod.Misc;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;

namespace PaladinMod.States
{
    public abstract class BaseThrowSpellState : BaseSkillState
    {
        public GameObject projectilePrefab;
        public GameObject muzzleflashEffectPrefab;
        public float baseDuration;
        public float minDamageCoefficient;
        public float maxDamageCoefficient;
        public float force;
        public float selfForce;
        private float duration;
        public float charge;
        private PaladinSwordController swordController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.swordController = base.GetComponent<PaladinSwordController>();

            if (this.swordController) this.swordController.attacking = true;

            base.PlayAnimation("Gesture, Override", "ThrowSpell", "ChargeSpell.playbackRate", this.duration);

            if (this.muzzleflashEffectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(this.muzzleflashEffectPrefab, base.gameObject, "HandL", false);
            }

            Util.PlaySound(Modules.Sounds.ThrowLightningSpear, base.gameObject);

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

            if (this.swordController) this.swordController.attacking = false;
        }

        private void Fire()
        {
            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();

                if (this.projectilePrefab != null)
                {
                    float num = Util.Remap(this.charge, 0f, 1f, this.minDamageCoefficient, this.maxDamageCoefficient);
                    float num2 = this.charge * this.force;

                    FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
                    {
                        projectilePrefab = this.projectilePrefab,
                        position = aimRay.origin,
                        rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                        owner = base.gameObject,
                        damage = this.damageStat * num,
                        force = num2,
                        crit = base.RollCrit(),
                        speedOverride = 120f
                    };

                    ModifyProjectileInfo(fireProjectileInfo);

                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                }

                if (base.characterMotor)
                {
                    base.characterMotor.ApplyForce(aimRay.direction * (-this.selfForce * this.charge), false, false);
                }
            }
        }

        protected virtual void ModifyProjectileInfo(FireProjectileInfo fireProjectileInfo)
        {
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
