using EntityStates;
using RoR2;
using UnityEngine;
using System;
using PaladinMod.Misc;
using RoR2.Projectile;
using UnityEngine.Networking;
using RoR2.Skills;

namespace PaladinMod.States
{
    public class Slash : BaseSkillState, SteppedSkillDef.IStepSetter
    {
        public static float damageCoefficient = StaticValues.slashDamageCoefficient;
        public float baseDuration = 2f;
        public static float attackRecoil = 1.5f;
        public static float hitHopVelocity = 5.5f;
        private const float attackStartTime = 0.21f;
        private const float attackStartTimAlt = 0.25f;
        private const float attackEndTime = 0.35f;
        public static float earlyExitTime = 0.46f;
        public int swingIndex;

        private bool inCombo;
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
        private PaladinSwordController swordController;
        private Vector3 storedVelocity;
        private PaladinVRController vrController;
        private GameObject swingEffectInstance;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.earlyExitDuration = this.duration * Slash.earlyExitTime;
            this.hasFired = false;
            this.cancelling = false;
            this.animator = base.GetModelAnimator();
            this.swordController = base.GetComponent<PaladinSwordController>();
            this.vrController = base.GetComponent<PaladinVRController>();
            base.StartAimMode(0.5f + this.duration, false);
            base.characterBody.isSprinting = false;
            base.characterBody.outOfCombatStopwatch = 0f;

            if (this.swordController) this.swordController.attacking = true;

            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "Sword");
            }

            //if (this.swingIndex > 1) {
            //    ((SteppedSkillDef.InstanceData)activatorSkillSlot.skillInstanceData).step = 0;
            //    this.swingIndex = 0;
            //   this.inCombo = true;
            //}

            Util.PlaySound(Modules.Sounds.Cloth1, base.gameObject);

            //Slash1 is first swing, Slash2 is second swing
            string animString = "Slash" + (1 + swingIndex).ToString();
            //SlashCombo1 is Slash1 but coming off Slash2
            if (this.inCombo)
            {
                animString = "SlashCombo1";
            }

            base.PlayCrossfade("Gesture, Override", animString, "Slash.playbackRate", this.duration, 0.05f * duration);
            if (!this.animator.GetBool("isMoving") && this.animator.GetBool("isGrounded")) 
            {
                base.PlayCrossfade("FullBody, Override", animString, "Slash.playbackRate", this.duration, 0.05f * duration);
            }
            

            float dmg = Slash.damageCoefficient;
            //14.5
            //20
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
            this.attack.impactSound = Modules.Asset.swordHitSoundEventS.index;
            if (this.swordController.isBlunt) this.attack.impactSound = Modules.Asset.batHitSoundEventS.index;

            if (base.characterBody.HasBuff(Modules.Buffs.overchargeBuff))
            {
                this.attack.damageType = DamageType.Stun1s;
                this.attack.damage = 4f * this.damageStat;
            }
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

        protected virtual void FireSwordBeam()
        {
            if (PaladinPlugin.IsLocalVRPlayer(base.characterBody))
            {
                if (this.swordController && this.swordController.swordActive)
                {
                    // using mainly the camera's looking direction, but mixed with a little of sword pointing direaction
                    Camera.main.transform.Rotate(5, 0, 0);
                    Vector3 direction = Vector3.Lerp(Camera.main.transform.forward, vrController.GetSwordPointing(), 0.1f);
                    Camera.main.transform.Rotate(-5, 0, 0);
                    // try to make the sword beam match slash direction
                    var rotation = Util.QuaternionSafeLookRotation(direction, vrController.GetSlashUpward());
                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.swordBeamProjectile, Camera.main.transform.position, rotation, base.gameObject, StaticValues.beamDamageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.WeakPoint, null, StaticValues.beamSpeed);
                }
            }
            else
            {
                Ray aimRay = base.GetAimRay();

                if (this.swordController && this.swordController.swordActive)
                {
                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.swordBeamProjectile, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, StaticValues.beamDamageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.WeakPoint, null, StaticValues.beamSpeed);
                }
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
                this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "Slash.playbackRate");
                this.hitPauseTimer = (2f * EntityStates.Merc.GroundLight.hitPauseDuration) / this.attackSpeedStat;
                this.inHitPause = true;

                if (this.swingEffectInstance)
                {
                    ScaleParticleSystemDuration fuck = this.swingEffectInstance.GetComponent<ScaleParticleSystemDuration>();
                    if (fuck) fuck.newDuration = 20f;
                }
            }
        }

        public void FireAttack()
        {
            // wah
            if (base.isAuthority)
            {
                Vector3 direction = this.GetAimRay().direction;
                direction.y = Mathf.Max(direction.y, direction.y * 0.5f);
                this.FindModelChild("SwordHitboxAnchor").rotation = Util.QuaternionSafeLookRotation(direction);
            }

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

                    Transform muzzleTransform = this.FindModelChild(muzzleString);
                    if (muzzleTransform)
                    {
                        this.swingEffectInstance = UnityEngine.Object.Instantiate<GameObject>(this.swordController.swingEffect, muzzleTransform);
                        ScaleParticleSystemDuration fuck = this.swingEffectInstance.GetComponent<ScaleParticleSystemDuration>();
                        if (fuck) fuck.newDuration = fuck.initialDuration;
                    }

                    this.FireSwordBeam();
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
            this.hitPauseTimer -= Time.deltaTime;

            if (this.hitPauseTimer <= 0f && this.inHitPause)
            {
                base.ConsumeHitStopCachedState(this.hitStopCachedState, base.characterMotor, this.animator);
                this.inHitPause = false;
                if (this.storedVelocity != Vector3.zero) base.characterMotor.velocity = this.storedVelocity;

                if (this.swingEffectInstance)
                {
                    ScaleParticleSystemDuration fuck = this.swingEffectInstance.GetComponent<ScaleParticleSystemDuration>();
                    if (fuck) fuck.newDuration = fuck.initialDuration;
                }
            }

            if (!this.inHitPause)
            {
                this.stopwatch += Time.deltaTime;
            }
            else
            {
                this.stopwatch += 0.1f * Time.deltaTime;
                if (base.characterMotor) base.characterMotor.velocity = Vector3.zero;
                if (this.animator) this.animator.SetFloat("Slash.playbackRate", 0.05f);
            }

            float startTime = swingIndex == 1 ? attackStartTimAlt : attackStartTime;
            if (this.stopwatch >= this.duration * startTime && this.stopwatch <= this.duration * attackEndTime || PaladinPlugin.IsLocalVRPlayer(characterBody))
            {
                this.FireAttack(); 
            }

            if (base.isAuthority)
            {
                //if (base.fixedAge >= this.earlyExitDuration && base.inputBank.skill1.down)
                //{
                //    var nextSwing = new Slash();
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
        
        public void SetStep(int i) {
            swingIndex = i;

            if (this.swingIndex > 1) {
                ((SteppedSkillDef.InstanceData)activatorSkillSlot.skillInstanceData).step = 0;
                this.swingIndex = 0;
                this.inCombo = true;
            }
        }
        
        public override void OnSerialize(NetworkWriter writer) {
            base.OnSerialize(writer);
            writer.Write((byte)this.swingIndex);
        }

        public override void OnDeserialize(NetworkReader reader) {
            base.OnDeserialize(reader);
            this.swingIndex = (int)reader.ReadByte();
        }
    }
}