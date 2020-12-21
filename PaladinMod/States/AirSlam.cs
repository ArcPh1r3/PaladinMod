using EntityStates;
using RoR2;
using UnityEngine;
using System;
using PaladinMod.Misc;
using UnityEngine.Networking;

namespace PaladinMod.States
{
    public class AirSlam : BaseSkillState
    {
        public static float damageCoefficient = StaticValues.spinSlashDamageCoefficient;
        public static float boostedDamageCoefficient = 12f;
        public static float leapDuration = 0.5f;
        public static float dropVelocity = 18f;

        private float duration;
        private bool hasFired;
        private float hitPauseTimer;
        private OverlapAttack attack;
        private bool inHitPause;
        private float stopwatch;
        private Animator animator;
        private BaseState.HitStopCachedState hitStopCachedState;
        private PaladinSwordController swordController;
        private float previousAirControl;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = AirSlam.leapDuration / this.attackSpeedStat;
            this.hasFired = false;
            this.animator = base.GetModelAnimator();
            this.swordController = base.GetComponent<PaladinSwordController>();

            this.previousAirControl = base.characterMotor.airControl;
            base.characterMotor.airControl = EntityStates.Croco.Leap.airControl;

            Vector3 direction = base.GetAimRay().direction;

            if (base.isAuthority)
            {
                base.characterBody.isSprinting = true;

                direction.y = Mathf.Max(direction.y, 1.25f * EntityStates.Croco.Leap.minimumY);
                Vector3 a = direction.normalized * (0.75f * EntityStates.Croco.Leap.aimVelocity) * this.moveSpeedStat;
                Vector3 b = Vector3.up * 0.95f * EntityStates.Croco.Leap.upwardVelocity;
                Vector3 b2 = new Vector3(direction.x, 0f, direction.z).normalized * (1.25f * EntityStates.Croco.Leap.forwardVelocity);

                base.characterMotor.Motor.ForceUnground();
                base.characterMotor.velocity = a + b + b2;
            }

            base.characterDirection.moveVector = direction;

            base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;

            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();

            string hitboxString = "Sword";

            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == hitboxString);
            }

            base.PlayAnimation("FullBody, Override", "AirSlamStart", "Whirlwind.playbackRate", this.duration);

            float dmg = AirSlam.damageCoefficient;
            if (this.swordController && this.swordController.swordActive) dmg = AirSlam.boostedDamageCoefficient;

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

            /*if (base.isAuthority)
            {
                Vector3 footPosition = base.characterBody.footPosition;
                EffectManager.SpawnEffect(EntityStates.ParentMonster.GroundSlam.slamImpactEffect, new EffectData
                {
                    origin = footPosition,
                    scale = 8f
                }, true);
            }

            Util.PlaySound(EntityStates.ParentMonster.GroundSlam.attackSoundString, base.gameObject);*/
            base.PlayAnimation("FullBody, Override", "BufferEmpty");

            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
            base.characterMotor.airControl = this.previousAirControl;
        }

        public void FireAttack()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                Util.PlayScaledSound(EntityStates.Merc.GroundLight.comboAttackSoundString, base.gameObject, 0.5f);
                if (base.isAuthority)
                {
                    base.AddRecoil(-1f * GroundSweep.attackRecoil, -2f * GroundSweep.attackRecoil, -0.5f * GroundSweep.attackRecoil, 0.5f * GroundSweep.attackRecoil);
                    EffectManager.SimpleMuzzleFlash(Modules.Assets.swordSwing, base.gameObject, "SwingDown", true);

                    base.characterMotor.velocity += Vector3.up * -AirSlam.dropVelocity;
                }
            }

            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();

                if (this.attack.Fire())
                {
                    Util.PlaySound(EntityStates.Merc.GroundLight.hitSoundString, base.gameObject);
                    //Util.PlaySound(MinerPlugin.Sounds.Hit, base.gameObject);

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
            }

            if (this.stopwatch >= this.duration && base.isAuthority && base.characterMotor.isGrounded)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
