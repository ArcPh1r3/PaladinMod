using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.States
{
    public abstract class BaseCastChanneledSpellState : BaseSkillState
    {
        public GameObject projectilePrefab;
        public GameObject muzzleflashEffectPrefab;
        public float baseDuration;
        public Vector3 spellPosition;
        public Quaternion spellRotation;
        public string castSoundString;

        private float duration;
        public float charge;
        private bool hasFired;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;

            base.PlayAnimation("Gesture, Override", "CastSpell", "Spell.playbackRate", this.duration);

            if (this.muzzleflashEffectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(this.muzzleflashEffectPrefab, base.gameObject, "SpellCastEffect", false);
            }

            if (NetworkServer.active) base.characterBody.AddBuff(RoR2Content.Buffs.Slow50);

            if (base.cameraTargetParams)
            {
                base.cameraTargetParams.aimMode = CameraTargetParams.AimType.Aura;
            }

            ChildLocator childLocator = base.GetModelChildLocator();
            if (childLocator)
            {
                GameObject castEffect = childLocator.FindChild("SpellCastEffect").gameObject;
                castEffect.SetActive(false);
                castEffect.SetActive(true);
            }

            Util.PlaySound(this.castSoundString, base.gameObject);

            this.Fire();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= (0.5f * this.duration))
            {
                this.Fire();
            }

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (NetworkServer.active) base.characterBody.RemoveBuff(RoR2Content.Buffs.Slow50);

            if (base.cameraTargetParams)
            {
                base.cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;
            }
        }

        private void Fire()
        {
            if (this.hasFired) return;
            this.hasFired = true;

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
