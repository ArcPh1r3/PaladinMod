using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.States
{
    public class SpinSlash : BaseSkillState
    {
        public static float baseDuration = 1.25f;
        public static float chargeDamageCoefficient = StaticValues.spinSlashDamageCoefficient;

        public static float initialSpeedCoefficient = 8f;
        public static float finalSpeedCoefficient = 0.1f;

        private float dashSpeed;
        private Vector3 forwardDirection;
        private Vector3 previousPosition;
        private float duration;
        private float hitPauseTimer;
        private OverlapAttack attack;
        private bool inHitPause;
        private List<HealthComponent> victimsStruck = new List<HealthComponent>();

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = SpinSlash.baseDuration / this.attackSpeedStat;

            Util.PlayScaledSound(EntityStates.Croco.Leap.leapSoundString, base.gameObject, 1.75f);
            base.PlayAnimation("FullBody, Override", "Whirlwind", "Whirlwind.playbackRate", this.duration);

            if (base.isAuthority && base.inputBank && base.characterDirection)
            {
                this.forwardDirection = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
            }

            this.RecalculateSpeed();

            if (base.characterMotor && base.characterDirection)
            {
                base.characterMotor.velocity.y *= 0.2f;
                base.characterMotor.velocity = this.forwardDirection * this.dashSpeed;
            }

            Vector3 b = base.characterMotor ? base.characterMotor.velocity : Vector3.zero;
            this.previousPosition = base.transform.position - b;

            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "SpinSlash");
            }

            this.attack = new OverlapAttack();
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = SpinSlash.chargeDamageCoefficient * this.damageStat;
            this.attack.hitEffectPrefab = EntityStates.Loader.SwingChargedFist.overchargeImpactEffectPrefab;
            this.attack.forceVector = Vector3.up * EntityStates.Toolbot.ToolbotDash.upwardForceMagnitude;
            this.attack.pushAwayForce = EntityStates.Toolbot.ToolbotDash.awayForceMagnitude;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();

            //EffectManager.SimpleMuzzleFlash(EnforcerPlugin.Assets.shoulderBashFX, base.gameObject, "ShieldHitbox", true);
        }

        private void RecalculateSpeed()
        {
            this.dashSpeed = (2 + (0.5f * this.moveSpeedStat)) * Mathf.Lerp(SpinSlash.initialSpeedCoefficient, SpinSlash.finalSpeedCoefficient, base.fixedAge / this.duration);
        }

        public override void OnExit()
        {
            if (base.characterMotor) base.characterMotor.disableAirControlUntilCollision = false;

            if (base.cameraTargetParams)
            {
                base.cameraTargetParams.fovOverride = -1f;
            }

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            base.characterBody.isSprinting = false;

            if (base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
                //this.outer.SetNextState(new WhirlwindGround());
                return;
            }

            this.RecalculateSpeed();

            if (base.cameraTargetParams)
            {
                base.cameraTargetParams.fovOverride = Mathf.Lerp(EntityStates.Commando.DodgeState.dodgeFOV, 60f, base.fixedAge / this.duration);
            }

            if (base.isAuthority)
            {
                if (!this.inHitPause)
                {
                    Vector3 normalized = (base.transform.position - this.previousPosition).normalized;

                    if (base.characterDirection)
                    {
                        if (normalized != Vector3.zero)
                        {
                            Vector3 vector = normalized * this.dashSpeed;
                            float d = Mathf.Max(Vector3.Dot(vector, this.forwardDirection), 0f);
                            vector = this.forwardDirection * d;
                            vector.y = base.characterMotor.velocity.y;
                            base.characterMotor.velocity = vector;
                        }

                        base.characterDirection.forward = this.forwardDirection;
                    }

                    this.previousPosition = base.transform.position;

                    this.attack.damage = this.damageStat * SpinSlash.chargeDamageCoefficient;

                    if (base.fixedAge >= this.duration * 0.2f)
                    {
                        if (this.attack.Fire(this.victimsStruck))
                        {
                            Util.PlaySound(EntityStates.Merc.GroundLight.hitSoundString, base.gameObject);
                            this.inHitPause = true;
                            this.hitPauseTimer = EntityStates.Toolbot.ToolbotDash.hitPauseDuration;
                            base.AddRecoil(-0.5f * EntityStates.Toolbot.ToolbotDash.recoilAmplitude, -0.5f * EntityStates.Toolbot.ToolbotDash.recoilAmplitude, -0.5f * EntityStates.Toolbot.ToolbotDash.recoilAmplitude, 0.5f * EntityStates.Toolbot.ToolbotDash.recoilAmplitude);
                            return;
                        }
                    }
                }
                else
                {
                    base.characterMotor.velocity = Vector3.zero;
                    this.hitPauseTimer -= Time.fixedDeltaTime;
                    if (this.hitPauseTimer < 0f)
                    {
                        this.inHitPause = false;
                    }
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.forwardDirection);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.forwardDirection = reader.ReadVector3();
        }
    }

}
