using EntityStates;
using RoR2;
using UnityEngine;
using System;
using PaladinMod.Misc;
using UnityEngine.Networking;
using RoR2.Projectile;

namespace PaladinMod.States
{
    public class AirSlamAlt : BaseSkillState
    {
        public static float damageCoefficient = StaticValues.spinSlashDamageCoefficient;
        public static float leapDuration = 0.4f;
        public static float dropVelocity = 30f;
        public static float hopVelocity = 15f;

        private float duration;
        private bool hasFired;
        private bool hasLanded;
        private float hitPauseTimer;
        private OverlapAttack attack;
        private bool inHitPause;
        private float stopwatch;
        private Animator animator;
        private BaseState.HitStopCachedState hitStopCachedState;
        private PaladinSwordController swordController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = AirSlam.leapDuration / this.attackSpeedStat;
            this.hasFired = false;
            this.hasLanded = false;
            this.animator = base.GetModelAnimator();
            this.swordController = base.GetComponent<PaladinSwordController>();

            base.skillLocator.secondary.DeductStock(1);

            Vector3 direction = base.GetAimRay().direction;

            if (base.isAuthority)
            {
                base.characterBody.isSprinting = true;

                base.characterMotor.velocity *= 0.1f;
                base.SmallHop(base.characterMotor, AirSlamAlt.hopVelocity);
            }

            base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;

            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();

            string hitboxString = "LeapStrike";

            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == hitboxString);
            }

            base.PlayAnimation("FullBody, Override", "LeapSlam2", "Whirlwind.playbackRate", this.duration * 1.5f);
            Util.PlaySound(Modules.Sounds.Lunge, base.gameObject);
            Util.PlaySound(Modules.Sounds.Cloth2, base.gameObject);

            float dmg = AirSlam.damageCoefficient;

            this.attack = new OverlapAttack();
            this.attack.damageType = DamageType.Generic;
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = dmg * this.damageStat;
            this.attack.procCoefficient = 1;
            this.attack.hitEffectPrefab = Modules.Assets.hitFX;
            this.attack.forceVector = -Vector3.up * 6000f;
            this.attack.pushAwayForce = 500f;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();
        }

        public override void OnExit()
        {
            base.OnExit();

            base.PlayAnimation("FullBody, Override", "BufferEmpty");

            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
        }

        public void FireAttack()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                Util.PlayScaledSound(Modules.Sounds.Swing, base.gameObject, 0.5f);

                if (base.isAuthority)
                {
                    base.AddRecoil(-1f * GroundSweep.attackRecoil, -2f * GroundSweep.attackRecoil, -0.5f * GroundSweep.attackRecoil, 0.5f * GroundSweep.attackRecoil);
                    EffectManager.SimpleMuzzleFlash(Modules.Assets.swordSwing, base.gameObject, "SwingDown", true);

                    base.characterMotor.velocity *= 0.1f;
                    base.characterMotor.velocity += Vector3.up * -AirSlamAlt.dropVelocity;
                }
            }

            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();

                if (this.attack.Fire())
                {
                    Util.PlaySound(Modules.Sounds.HitL, base.gameObject);

                    if (!this.inHitPause)
                    {
                        this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "Whirlwind.playbackRate");
                        this.hitPauseTimer = (4f * EntityStates.Merc.GroundLight.hitPauseDuration) / this.attackSpeedStat;
                        this.inHitPause = true;
                    }
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.StartAimMode(0.5f, false);
            this.stopwatch += Time.fixedDeltaTime;

            if (this.stopwatch >= this.duration)
            {
                this.FireAttack();

                if (base.isAuthority && base.inputBank.skill2.down)
                {
                    if (base.skillLocator.secondary.stock > 0)
                    {
                        EntityState nextState = new AirSlamAlt();
                        this.outer.SetNextState(nextState);
                        return;
                    }
                }
            }

            if (this.stopwatch >= this.duration && base.isAuthority && base.characterMotor.isGrounded)
            {
                this.GroundImpact();
                this.outer.SetNextStateToMain();
                return;
            }
        }

        private void FireShockwave()
        {
            Transform shockwaveTransform = base.FindModelChild("SwingCenter");
            Vector3 shockwavePosition = base.characterBody.footPosition;
            Vector3 forward = base.characterDirection.forward;

            ProjectileManager.instance.FireProjectile(Modules.Projectiles.shockwave, shockwavePosition, Util.QuaternionSafeLookRotation(forward), base.gameObject, base.characterBody.damage * StaticValues.beamDamageCoefficient, EntityStates.BrotherMonster.WeaponSlam.waveProjectileForce, base.RollCrit(), DamageColorIndex.Default, null, -1f);
        }

        private void GroundImpact()
        {
            if (!this.hasLanded)
            {
                this.hasLanded = true;

                if (this.swordController && this.swordController.swordActive)
                {
                    this.FireShockwave();
                }

                Util.PlaySound(Modules.Sounds.GroundImpact, base.gameObject);

                EffectData effectData = new EffectData();
                effectData.origin = base.characterBody.footPosition;
                effectData.scale = 2f;

                EffectManager.SpawnEffect(Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/ParentSlamEffect"), effectData, true);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
