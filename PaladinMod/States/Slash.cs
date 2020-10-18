using EntityStates;
using RoR2;
using UnityEngine;
using System;
using PaladinMod.Misc;
using RoR2.Projectile;

namespace PaladinMod.States
{
    public class Slash1 : BaseSkillState
    {
        public static float damageCoefficient = StaticValues.slashDamageCoefficient;
        public static float buffDamageCoefficient = StaticValues.slashBuffDamageCoefficient;
        public float baseDuration = 1.6f;
        public static float baseAttackDuration = 0.15f;
        public static float attackRecoil = 1.25f;
        public static float hitHopVelocity = 5.5f;
        public static float earlyExitDuration = 0.6f;

        private float duration;
        private float attackDuration;
        private float earlyExit;
        private bool hasFired;
        private float hitPauseTimer;
        private OverlapAttack attack;
        private bool inHitPause;
        private bool hasHopped;
        private float stopwatch;
        private Animator animator;
        private BaseState.HitStopCachedState hitStopCachedState;
        private PaladinSwordController swordController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.characterBody.isSprinting = false;
            this.attackDuration = Slash1.baseAttackDuration / this.attackSpeedStat;
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.earlyExit = Slash1.earlyExitDuration / this.attackSpeedStat;
            this.hasFired = false;
            this.animator = base.GetModelAnimator();
            this.swordController = base.GetComponent<PaladinSwordController>();
            base.StartAimMode(0.5f + this.duration, false);
            base.PlayAnimation("Gesture, Override", "Slash1", "Slash.playbackRate", this.duration);

            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "Sword");
            }

            float dmg = Slash1.damageCoefficient;
            if (this.swordController && this.swordController.swordActive) dmg = Slash1.buffDamageCoefficient;

            this.attack = new OverlapAttack();
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = dmg * this.damageStat;
            this.attack.procCoefficient = 1;
            this.attack.hitEffectPrefab = EntityStates.Merc.GroundLight.comboHitEffectPrefab;
            this.attack.forceVector = Vector3.zero;
            this.attack.pushAwayForce = -0.5f;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public void FireAttack()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                Util.PlayScaledSound(EntityStates.Merc.GroundLight.comboAttackSoundString, base.gameObject, 0.5f);
                base.AddRecoil(-1f * Slash1.attackRecoil, -2f * Slash1.attackRecoil, -0.5f * Slash1.attackRecoil, 0.5f * Slash1.attackRecoil);

                Ray aimRay = base.GetAimRay();

                if (base.isAuthority && this.swordController && this.swordController.swordActive)
                {
                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.swordBeam, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, StaticValues.beamDamageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.WeakPoint, null, StaticValues.beamSpeed);
                }
                //SlashMuzzle
            }

            if (base.isAuthority)
            {
                if (this.attack.Fire())
                {
                    if (!this.hasHopped)
                    {
                        if (base.characterMotor && !base.characterMotor.isGrounded)
                        {
                            base.SmallHop(base.characterMotor, Slash1.hitHopVelocity);
                        }

                        this.hasHopped = true;
                    }

                    if (!this.inHitPause)
                    {
                        this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "Slash.playbackRate");
                        this.hitPauseTimer = (0.6f * EntityStates.Merc.GroundLight.hitPauseDuration) / this.attackSpeedStat;
                        this.inHitPause = true;
                    }
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
            }

            if (!this.inHitPause)
            {
                this.stopwatch += Time.fixedDeltaTime;
            }
            else
            {
                if (base.characterMotor) base.characterMotor.velocity = Vector3.zero;
                if (this.animator) this.animator.SetFloat("Slash.playbackRate", 0f);
            }

            bool flag = false;
            if (this.stopwatch < this.duration * (0.35f + this.attackDuration)) flag = true;
            if (this.stopwatch >= this.duration * (0.35f + this.attackDuration) && !this.hasFired) flag = true;

            if (this.stopwatch >= this.duration * 0.15f && flag)
            {
                this.FireAttack();
            }

            if (base.fixedAge >= this.duration * this.earlyExit)
            {
                if (base.inputBank && base.inputBank.skill1.down)
                {
                    base.outer.SetNextState(new Slash2());
                    return;
                }
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }

    public class Slash2 : BaseSkillState
    {
        public static float damageCoefficient = StaticValues.slashDamageCoefficient;
        public static float buffDamageCoefficient = StaticValues.slashBuffDamageCoefficient;
        public float baseDuration = 2f;
        public static float attackRecoil = 1.25f;
        public static float hitHopVelocity = 5.5f;
        public static float earlyExitDuration = 0.5f;

        private float duration;
        private float attackDuration;
        private float earlyExit;
        private bool hasFired;
        private float hitPauseTimer;
        private OverlapAttack attack;
        private bool inHitPause;
        private bool hasHopped;
        private float stopwatch;
        private Animator animator;
        private BaseState.HitStopCachedState hitStopCachedState;
        private PaladinSwordController swordController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.characterBody.isSprinting = false;
            this.attackDuration = Slash1.baseAttackDuration / this.attackSpeedStat;
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.earlyExit = Slash2.earlyExitDuration / this.attackSpeedStat;
            this.hasFired = false;
            this.animator = base.GetModelAnimator();
            this.swordController = base.GetComponent<PaladinSwordController>();
            base.StartAimMode(0.5f + this.duration, false);
            base.PlayAnimation("Gesture, Override", "Slash2", "Slash.playbackRate", this.duration);

            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "Sword");
            }

            float dmg = Slash2.damageCoefficient;
            if (this.swordController && this.swordController.swordActive) dmg = Slash2.buffDamageCoefficient;

            this.attack = new OverlapAttack();
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = dmg * this.damageStat;
            this.attack.procCoefficient = 1;
            this.attack.hitEffectPrefab = EntityStates.Merc.GroundLight.comboHitEffectPrefab;
            this.attack.forceVector = Vector3.zero;
            this.attack.pushAwayForce = -0.5f;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public void FireAttack()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                Util.PlayScaledSound(EntityStates.Merc.GroundLight.comboAttackSoundString, base.gameObject, 0.5f);
                base.AddRecoil(-1f * Slash2.attackRecoil, -2f * Slash2.attackRecoil, -0.5f * Slash2.attackRecoil, 0.5f * Slash2.attackRecoil);

                Ray aimRay = base.GetAimRay();

                if (base.isAuthority && this.swordController && this.swordController.swordActive)
                {
                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.swordBeam, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, StaticValues.beamDamageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.WeakPoint, null, StaticValues.beamSpeed);
                }
                //SlashMuzzle
            }

            if (base.isAuthority)
            {
                if (this.attack.Fire())
                {
                    if (!this.hasHopped)
                    {
                        if (base.characterMotor && !base.characterMotor.isGrounded)
                        {
                            base.SmallHop(base.characterMotor, Slash2.hitHopVelocity);
                        }

                        this.hasHopped = true;
                    }

                    if (!this.inHitPause)
                    {
                        this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "Slash.playbackRate");
                        this.hitPauseTimer = (0.6f * EntityStates.Merc.GroundLight.hitPauseDuration) / this.attackSpeedStat;
                        this.inHitPause = true;
                    }
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
            }

            if (!this.inHitPause)
            {
                this.stopwatch += Time.fixedDeltaTime;
            }
            else
            {
                if (base.characterMotor) base.characterMotor.velocity = Vector3.zero;
                if (this.animator) this.animator.SetFloat("Slash.playbackRate", 0f);
            }

            bool flag = false;
            if (this.stopwatch < this.duration * (0.35f + this.attackDuration)) flag = true;
            if (this.stopwatch >= this.duration * (0.35f + this.attackDuration) && !this.hasFired) flag = true;

            if (this.stopwatch >= this.duration * 0.15f && flag)
            {
                this.FireAttack();
            }

            if (base.fixedAge >= this.duration * this.earlyExit)
            {
                if (base.inputBank && base.inputBank.skill1.down)
                {
                    base.outer.SetNextState(new Slash1());
                    return;
                }
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
