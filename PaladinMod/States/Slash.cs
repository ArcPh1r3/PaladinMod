﻿using EntityStates;
using RoR2;
using UnityEngine;
using System;
using PaladinMod.Misc;
using RoR2.Projectile;
using UnityEngine.Networking;

namespace PaladinMod.States
{
    public class Slash : BaseSkillState
    {
        public static float damageCoefficient = StaticValues.slashDamageCoefficient;
        public float baseDuration = 1.6f;
        public static float attackRecoil = 1.5f;
        public static float hitHopVelocity = 5.5f;
        public static float earlyExitTime = 0.575f;
        public int swingIndex;

        private bool inCombo;
        private float earlyExitDuration;
        private float duration;
        private bool hasFired;
        private float hitPauseTimer;
        private OverlapAttack attack;
        private bool inHitPause;
        private bool hasHopped;
        private float stopwatch;
        private bool cancelling;
        private Animator animator;
        private BaseState.HitStopCachedState hitStopCachedState;
        private PaladinSwordController swordController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.earlyExitDuration = this.duration * Slash.earlyExitTime;
            this.hasFired = false;
            this.cancelling = false;
            this.animator = base.GetModelAnimator();
            this.swordController = base.GetComponent<PaladinSwordController>();
            base.StartAimMode(0.5f + this.duration, false);
            base.characterBody.isSprinting = false;
            this.inCombo = false;

            if (this.swordController) this.swordController.attacking = true;

            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "Sword");
            }

            if (this.swingIndex > 1)
            {
                this.swingIndex = 0;
                this.inCombo = true;
            }

            Util.PlaySound(Modules.Sounds.Cloth1, base.gameObject);

            string animString = "Slash" + (1 + swingIndex).ToString();

            if (this.inCombo)
            {
                if (!this.animator.GetBool("isMoving") && this.animator.GetBool("isGrounded")) base.PlayCrossfade("FullBody, Override", "SlashCombo1", "Slash.playbackRate", this.duration, 0.05f);
                base.PlayCrossfade("Gesture, Override", "SlashCombo1", "Slash.playbackRate", this.duration, 0.05f);
            }
            else
            {
                if (!this.animator.GetBool("isMoving") && this.animator.GetBool("isGrounded")) base.PlayCrossfade("FullBody, Override", animString, "Slash.playbackRate", this.duration, 0.05f);
                base.PlayCrossfade("Gesture, Override", animString, "Slash.playbackRate", this.duration, 0.05f);
            }

            float dmg = Slash.damageCoefficient;

            this.attack = new OverlapAttack();
            this.attack.damageType = DamageType.Generic;
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = dmg * this.damageStat;
            this.attack.procCoefficient = 1;
            this.attack.hitEffectPrefab = this.swordController.hitEffect;
            this.attack.forceVector = Vector3.zero;
            this.attack.pushAwayForce = 750f;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();
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

            //base.PlayAnimation("FullBody, Override", "BufferEmpty");
            //base.PlayAnimation("Gesture, Override", "BufferEmpty");

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
                    string muzzleString = null;
                    if (this.swingIndex == 0) muzzleString = "SwingRight";
                    else muzzleString = "SwingLeft";

                    base.AddRecoil(-1f * Slash.attackRecoil, -2f * Slash.attackRecoil, -0.5f * Slash.attackRecoil, 0.5f * Slash.attackRecoil);
                    EffectManager.SimpleMuzzleFlash(this.swordController.swingEffect, base.gameObject, muzzleString, true);

                    Ray aimRay = base.GetAimRay();

                    if (this.swordController && this.swordController.swordActive)
                    {
                        ProjectileManager.instance.FireProjectile(Modules.Projectiles.swordBeam, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, StaticValues.beamDamageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.WeakPoint, null, StaticValues.beamSpeed);
                    }

                    if (this.attack.Fire())
                    {
                        this.swordController.PlayHitSound(0);

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
                            this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "Slash.playbackRate");
                            this.hitPauseTimer = (2f * EntityStates.Merc.GroundLight.hitPauseDuration) / this.attackSpeedStat;
                            this.inHitPause = true;
                        }
                    }
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

            if (this.stopwatch >= this.duration * 0.225f && this.stopwatch <= this.duration * 0.5f)
            {
                this.FireAttack();
            }

            if (this.stopwatch >= (this.duration * 0.5f) && base.inputBank.skill2.down && base.skillLocator.secondary.skillDef.skillNameToken == "PALADIN_SECONDARY_LUNARSHARD_NAME")
            {
                this.cancelling = true;
                base.skillLocator.secondary.ExecuteIfReady();
                return;
            }

            if (base.isAuthority)
            {
                if (base.fixedAge >= this.earlyExitDuration && base.inputBank.skill1.down)
                {
                    var nextSwing = new Slash();
                    nextSwing.swingIndex = this.swingIndex + 1;
                    this.outer.SetNextState(nextSwing);
                    return;
                }

                if (base.fixedAge >= this.duration)
                {
                    this.outer.SetNextStateToMain();
                    return;
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if (this.cancelling) return InterruptPriority.Any;
            else return InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.swingIndex);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.swingIndex = reader.ReadInt32();
        }
    }
}
