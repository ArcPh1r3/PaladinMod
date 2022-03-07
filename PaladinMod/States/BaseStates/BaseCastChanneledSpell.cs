using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using static RoR2.CameraTargetParams;

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
        public string muzzleString = "SpellCastEffect";

        public CameraParamsOverrideHandle camParamsOverrideHandle;
        public AimRequest aimRequest;

        protected float overrideDuration;

        private float duration;
        public float charge;
        private bool hasFired;

        public override void OnEnter()
        {
            base.OnEnter();
            if (this.overrideDuration == 0) this.duration = this.baseDuration / this.attackSpeedStat;
            else this.duration = this.overrideDuration;

            this.PlayCastAnimation();

            if (this.muzzleflashEffectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(this.muzzleflashEffectPrefab, base.gameObject, this.muzzleString, false);
            }

            if (NetworkServer.active) base.characterBody.AddBuff(RoR2Content.Buffs.Slow50);

            if (base.cameraTargetParams)
            {

                //base.cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);
            }

            if (this.muzzleString == "SpellCastEffect")
            {
                ChildLocator childLocator = base.GetModelChildLocator();
                if (childLocator)
                {
                    GameObject castEffect = childLocator.FindChild("SpellCastEffect").gameObject;
                    castEffect.SetActive(false);
                    castEffect.SetActive(true);
                }
            }

            if (this.castSoundString != "") Util.PlaySound(this.castSoundString, base.gameObject);

            this.Fire();
        }

        protected virtual void PlayCastAnimation()
        {
            base.PlayAnimation("Gesture, Override", "CastSpell", "Spell.playbackRate", this.duration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterBody.outOfCombatStopwatch = 0f;

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

            if (base.cameraTargetParams) {

                base.cameraTargetParams.RemoveParamsOverride(camParamsOverrideHandle, 1f);
                //base.cameraTargetParams.RemoveRequest(aimRequest);
            }
        }

        private void Fire()
        {
            if (this.hasFired) return;
            this.hasFired = true;

            if (!this.projectilePrefab) return;

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