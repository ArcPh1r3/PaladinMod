using EntityStates;
using RoR2;
using UnityEngine;
using System;
using PaladinMod.Misc;
using RoR2.Projectile;
using UnityEngine.Networking;

namespace PaladinMod.States
{
    public class GroundSweepAlt : BaseSkillState
    {
        public static float damageCoefficient = StaticValues.spinSlashDamageCoefficient;
        public float baseDuration = 0.6f;
        public static float attackRecoil = 3f;

        private float duration;
        private bool hasFired;
        private float hitPauseTimer;
        private OverlapAttack attack;
        private bool inHitPause;
        private float stopwatch;
        private Animator animator;
        private BaseState.HitStopCachedState hitStopCachedState;
        private PaladinSwordController swordController;
        private Vector3 storedVelocity;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.hasFired = false;
            this.animator = base.GetModelAnimator();
            this.swordController = base.GetComponent<PaladinSwordController>();
            base.StartAimMode(0.5f + this.duration, false);
            base.characterBody.isSprinting = false;

            base.skillLocator.secondary.DeductStock(1);

            if (this.swordController) this.swordController.attacking = true;

            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();

            if (NetworkServer.active) base.characterBody.AddBuff(RoR2Content.Buffs.Slow50);

            string hitboxString = "SpinSlash";
            if (this.swordController && this.swordController.swordActive) hitboxString = "SpinSlashLarge";

            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == hitboxString);
            }

            base.PlayAnimation("FullBody, Override", "GroundSweepContinuous", "Whirlwind.playbackRate", this.duration * 1.1f);
            Util.PlaySound(Modules.Sounds.Cloth3, base.gameObject);

            this.attack = new OverlapAttack();
            this.attack.damageType = DamageType.Stun1s;
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = GroundSweepAlt.damageCoefficient * this.damageStat;
            this.attack.procCoefficient = 1;
            this.attack.hitEffectPrefab = this.swordController.hitEffect;
            this.attack.forceVector = Vector3.up * 1600f;
            this.attack.pushAwayForce = -1500f;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();
            this.attack.impactSound = Modules.Assets.swordHitSoundEventM.index;
            if (this.swordController.isBlunt) this.attack.impactSound = Modules.Assets.batHitSoundEventM.index;
        }

        public override void OnExit()
        {
            base.OnExit();

            if (NetworkServer.active) base.characterBody.RemoveBuff(RoR2Content.Buffs.Slow50);

            if (this.swordController) this.swordController.attacking = false;
        }

        public void FireAttack()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                this.swordController.PlaySwingSound();

                if (base.isAuthority)
                {
                    base.AddRecoil(-1f * GroundSweepAlt.attackRecoil, -2f * GroundSweepAlt.attackRecoil, -0.5f * GroundSweepAlt.attackRecoil, 0.5f * GroundSweepAlt.attackRecoil);
                    if (this.swordController.swordActive) EffectManager.SimpleMuzzleFlash(this.swordController.empoweredSpinSlashEffect, base.gameObject, "SwingCenter", true);
                    else EffectManager.SimpleMuzzleFlash(this.swordController.spinSlashEffect, base.gameObject, "SwingCenter", true);

                    Ray aimRay = base.GetAimRay();

                    if (this.attack.Fire())
                    {
                        if (!this.inHitPause)
                        {
                            if (base.characterMotor.velocity != Vector3.zero) this.storedVelocity = base.characterMotor.velocity;
                            this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "Whirlwind.playbackRate");
                            this.hitPauseTimer = (4f * EntityStates.Merc.GroundLight.hitPauseDuration) / this.attackSpeedStat;
                            this.inHitPause = true;
                        }
                    }

                    base.characterMotor.velocity += (base.characterDirection.forward * 25f);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            this.hitPauseTimer -= Time.fixedDeltaTime;

            if (this.hitPauseTimer <= 0f && this.inHitPause)
            {
                base.ConsumeHitStopCachedState(this.hitStopCachedState, base.characterMotor, this.animator);
                this.inHitPause = false;
                if (this.storedVelocity != Vector3.zero) base.characterMotor.velocity = this.storedVelocity;
            }

            if (!this.inHitPause)
            {
                this.stopwatch += Time.fixedDeltaTime;
            }
            else
            {
                if (base.characterMotor) base.characterMotor.velocity = Vector3.zero;
                if (this.animator) this.animator.SetFloat("Whirlwind.playbackRate", 0f);
            }

            if (base.characterMotor)
            {
                base.characterMotor.moveDirection /= 2f;
            }

            if (this.stopwatch >= this.duration * 0.4f)
            {
                this.FireAttack();
            }

            if (this.stopwatch >= this.duration * 0.6f)
            {
                this.FireAttack();

                if (base.isAuthority && base.inputBank.skill2.down)
                {
                    if (base.skillLocator.secondary.stock > 0)
                    {
                        EntityState nextState = new GroundSweepAlt();
                        this.outer.SetNextState(nextState);
                        return;
                    }
                }
            }

            if (this.stopwatch >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if (this.stopwatch >= this.duration * 0.41f) return InterruptPriority.PrioritySkill;
            else return InterruptPriority.Frozen;
        }
    }
}