using EntityStates;
using RoR2;
using UnityEngine;
using System;
using PaladinMod.Misc;
using RoR2.Projectile;
using UnityEngine.Networking;

namespace PaladinMod.States
{
    public class GroundSweep : BaseSkillState
    {
        public static float damageCoefficient = StaticValues.spinSlashDamageCoefficient;
        public float baseDuration = 1.5f;
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
        private bool hasHit;
        //private int startingJumpCount;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.hasFired = false;
            this.animator = base.GetModelAnimator();
            this.swordController = base.GetComponent<PaladinSwordController>();
            base.StartAimMode(0.5f + this.duration, false);
            base.characterBody.isSprinting = false;
            //this.startingJumpCount = base.characterMotor.jumpCount;
            //base.characterMotor.jumpCount = base.characterBody.maxJumpCount;

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

            base.PlayAnimation("FullBody, Override", "GroundSweep", "Whirlwind.playbackRate", this.duration * 1.1f);
            Util.PlaySound(Modules.Sounds.Cloth3, base.gameObject);

            this.attack = new OverlapAttack();
            this.attack.damageType = DamageType.Stun1s;
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = GroundSweep.damageCoefficient * this.damageStat;
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
            //base.characterMotor.jumpCount = this.startingJumpCount;
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
                    base.AddRecoil(-1f * GroundSweep.attackRecoil, -2f * GroundSweep.attackRecoil, -0.5f * GroundSweep.attackRecoil, 0.5f * GroundSweep.attackRecoil);
                    if (this.swordController.swordActive) EffectManager.SimpleMuzzleFlash(this.swordController.empoweredSpinSlashEffect, base.gameObject, "SwingCenter", true);
                    else EffectManager.SimpleMuzzleFlash(this.swordController.spinSlashEffect, base.gameObject, "SwingCenter", true);

                    Ray aimRay = base.GetAimRay();

                    Vector3 forwardDirection = base.characterDirection.forward;
                    if (PaladinPlugin.IsLocalVRPlayer(characterBody)) {
                        forwardDirection = Camera.main.transform.forward;
                        forwardDirection.y = 0;
                        forwardDirection = forwardDirection.normalized;
                    }

                    base.characterMotor.velocity += forwardDirection * 25f;
                }
            }

            if (this.attack.Fire())
            {
                if (!this.inHitPause)
                {
                    if (base.characterMotor.velocity != Vector3.zero) this.storedVelocity = base.characterMotor.velocity;
                    this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "Whirlwind.playbackRate");
                    this.hitPauseTimer = (4f * EntityStates.Merc.GroundLight.hitPauseDuration) / this.attackSpeedStat;
                    this.inHitPause = true;
                }

                if (!this.hasHit)
                {
                    this.hasHit = true;
                    if (base.skillLocator.utility.skillDef.skillNameToken == "PALADIN_UTILITY_DASH_NAME") base.skillLocator.utility.RunRecharge(1f);
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

            if (base.characterMotor && stopwatch < duration * 0.6f) 
            {
                base.characterMotor.moveDirection /= 2f;
            }


            bool skillStarted = this.stopwatch >= this.duration * 0.45f;
            bool skillEnded = this.stopwatch > this.duration * 0.7f;

            if ((skillStarted && !skillEnded) || (skillStarted && skillEnded && !hasFired))
            {
                this.FireAttack();
            }

            if (this.stopwatch >= this.duration * 0.6f)
            {
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