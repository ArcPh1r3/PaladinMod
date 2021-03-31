using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PaladinMod.States
{
    public class SpinningSlashOld : BaseSkillState
    {
        public static float baseDuration = 0.6f;
        public static float damageCoefficient = StaticValues.spinSlashDamageCoefficient;

        private float duration;
        private float hitPauseTimer;
        private OverlapAttack attack;
        private bool inHitPause;
        private bool hasFired;
        private List<HurtBox> victimsStruck = new List<HurtBox>();

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = SpinningSlashOld.baseDuration / this.attackSpeedStat;
            this.hasFired = false;

            Util.PlayAttackSpeedSound(EntityStates.Merc.GroundLight.finisherAttackSoundString, base.gameObject, 0.5f);
            base.PlayAnimation("FullBody, Override", "SpinSlash", "Whirlwind.playbackRate", this.duration);

            EffectManager.SimpleMuzzleFlash(Modules.Assets.spinningSlashFX, base.gameObject, "SwingCenter", true);

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
            this.attack.damageType = DamageType.Stun1s;
            this.attack.damage = SpinSlashOld.chargeDamageCoefficient * this.damageStat;
            this.attack.hitEffectPrefab = Modules.Assets.hitFX;
            this.attack.forceVector = Vector3.up * EntityStates.Toolbot.ToolbotDash.upwardForceMagnitude;
            this.attack.pushAwayForce = EntityStates.Toolbot.ToolbotDash.awayForceMagnitude;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();

            if (base.characterMotor) base.SmallHop(base.characterMotor, 16f);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterBody.isSprinting = false;

            if (base.isAuthority)
            {
                if (base.fixedAge >= this.duration)
                {
                    this.outer.SetNextStateToMain();
                    return;
                }

                if (!this.inHitPause)
                {
                    this.attack.damage = this.damageStat * SpinningSlashOld.damageCoefficient;

                    if (base.fixedAge >= this.duration * 0.2f)
                    {
                        if (!this.hasFired)
                        {
                            this.hasFired = true;

                            if (this.attack.Fire(this.victimsStruck))
                            {
                                Util.PlayAttackSpeedSound(EntityStates.Merc.GroundLight.hitSoundString, base.gameObject, 0.7f);
                                this.inHitPause = true;
                                this.hitPauseTimer = EntityStates.Toolbot.ToolbotDash.hitPauseDuration;
                                base.AddRecoil(-0.5f * EntityStates.Toolbot.ToolbotDash.recoilAmplitude, -0.5f * EntityStates.Toolbot.ToolbotDash.recoilAmplitude, -0.5f * EntityStates.Toolbot.ToolbotDash.recoilAmplitude, 0.5f * EntityStates.Toolbot.ToolbotDash.recoilAmplitude);
                                return;
                            }
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
    }
}
