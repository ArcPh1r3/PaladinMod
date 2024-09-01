using EntityStates;
using RoR2;
using UnityEngine;
using System;
using UnityEngine.Networking;
using RoR2.Skills;

namespace PaladinMod.States.Nemesis
{
    public class PunchCombo : BaseSkillState, SteppedSkillDef.IStepSetter
    {
        public static float damageCoefficient = 3.8f;
        public float baseDuration = 0.8f;
        public float shortBaseDuration = 0.6f;
        public static float attackRecoil = 1.5f;
        public static float hitHopVelocity = 5.5f;
        public static float earlyExitTime = 0.8f;
        public int swingIndex;

        private float earlyExitDuration;
        private float duration;
        private bool hasFired;
        private float hitPauseTimer;
        protected OverlapAttack attack;
        private bool inHitPause;
        private bool hasHopped;
        private float stopwatch;
        private bool cancelling;
        private Animator animator;
        private BaseState.HitStopCachedState hitStopCachedState;
        private Vector3 storedVelocity;
        private bool isHealing;

        public override void OnEnter()
        {
            base.OnEnter();
            if (this.swingIndex == 1) this.isHealing = true;
            if (this.isHealing) this.duration = this.baseDuration / this.attackSpeedStat;
            else this.duration = this.shortBaseDuration / this.attackSpeedStat;
            this.earlyExitDuration = this.duration * PunchCombo.earlyExitTime;
            this.hasFired = false;
            this.cancelling = false;
            this.animator = base.GetModelAnimator();
            base.StartAimMode(0.5f + this.duration, false);
            base.characterBody.isSprinting = false;
            base.characterBody.outOfCombatStopwatch = 0f;

            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "Punch");
            }

            if (this.swingIndex > 1)
            {
                this.swingIndex = 0;
            }

            Util.PlaySound(Modules.Sounds.Cloth1, base.gameObject);

            string animString = "Punch" + (1 + swingIndex).ToString();

            if (!this.animator.GetBool("isMoving") && this.animator.GetBool("isGrounded")) base.PlayCrossfade("FullBody, Override", animString, "Punch.playbackRate", this.duration, 0.05f);
            base.PlayCrossfade("Gesture, Override", animString, "Punch.playbackRate", this.duration, 0.05f);

            float dmg = PunchCombo.damageCoefficient;

            this.attack = new OverlapAttack();
            this.attack.damageType = DamageType.Generic;
            if (this.isHealing) this.attack.damageType = DamageType.BlightOnHit;
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = dmg * this.damageStat;
            this.attack.procCoefficient = 1;
            this.attack.hitEffectPrefab = Modules.Asset.hitFXBlunt;
            this.attack.forceVector = Vector3.zero;
            this.attack.pushAwayForce = 750f;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();
            this.attack.impactSound = Modules.Asset.batHitSoundEventL.index;
        }

        public override void OnExit()
        {
            base.OnExit();

            if (!this.hasFired) this.FireAttack();

            if (this.inHitPause)
            {
                base.ConsumeHitStopCachedState(this.hitStopCachedState, base.characterMotor, this.animator);
                this.inHitPause = false;
            }
        }

        protected virtual void OnHitAuthority()
        {
            if (!this.hasHopped)
            {
                if (base.characterMotor && !base.characterMotor.isGrounded)
                {
                    base.SmallHop(base.characterMotor, Slash.hitHopVelocity);
                }

                if (base.skillLocator.utility.skillDef.skillNameToken == "PALADIN_UTILITY_DASH_NAME") base.skillLocator.utility.RunRecharge(1f);

                this.hasHopped = true;
            }

            if (!this.inHitPause)
            {
                if (base.characterMotor.velocity != Vector3.zero) this.storedVelocity = base.characterMotor.velocity;
                this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "Punch.playbackRate");
                this.hitPauseTimer = (2f * EntityStates.Merc.GroundLight.hitPauseDuration) / this.attackSpeedStat;
                this.inHitPause = true;
            }
        }

        public void FireAttack()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                Util.PlaySound(Modules.Sounds.SwingBlunt, base.gameObject);

                if (base.isAuthority)
                {
                    string muzzleString = null;
                    if (this.swingIndex == 0) muzzleString = "SwingRight";
                    else muzzleString = "SwingLeft";

                    muzzleString = "SwingLeft";

                    base.AddRecoil(-1f * PunchCombo.attackRecoil, -2f * PunchCombo.attackRecoil, -0.5f * PunchCombo.attackRecoil, 0.5f * PunchCombo.attackRecoil);
                    EffectManager.SimpleMuzzleFlash(Modules.Asset.swordSwingPurple, base.gameObject, muzzleString, true);
                }
            }

            if (base.isAuthority)
            {
                if (this.attack.Fire())
                {
                    this.OnHitAuthority();
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (this.animator) this.animator.SetBool("inCombat", true);
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
                if (this.animator) this.animator.SetFloat("Punch.playbackRate", 0f);
            }

            if (this.stopwatch >= this.duration * 0.45f && this.stopwatch <= this.duration * 0.65f)
            {
                this.FireAttack();
            }

            if (base.isAuthority)
            {
                //if (base.fixedAge >= this.earlyExitDuration && base.inputBank.skill1.down)
                //{
                //    var nextSwing = new PunchCombo();
                //    nextSwing.swingIndex = this.swingIndex + 1;
                //    this.outer.SetNextState(nextSwing);
                //    return;
                //}

                if (base.fixedAge >= this.duration)
                {
                    this.outer.SetNextStateToMain();
                    return;
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if (this.cancelling) 
                return InterruptPriority.Any;

            if (base.fixedAge >= this.earlyExitDuration)
                return InterruptPriority.Any;

            return InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write((byte)this.swingIndex);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.swingIndex = (int)reader.ReadByte();
        }

        public void SetStep(int i) {
            swingIndex = i;
        }
    }
}