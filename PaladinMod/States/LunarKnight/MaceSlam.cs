using System;
using EntityStates;
using RoR2;
using UnityEngine;

namespace PaladinMod.States.LunarKnight
{
    public class MaceSlam : BaseState
    {
        public static float baseDuration = 2f;
        public static float damageCoefficient = 12f;
        public static float weaponDamageCoefficient = 16f;

        public static float forceMagnitude = 4000f;
        public static float upwardForce = 1500f;
        public static float radius = 8f;

        public static float durationBeforePriorityReduces = 0.5f;

        public static float weaponForce = 800f;

        private float duration;
        private float priorityReduceDuration;
        private BlastAttack blastAttack;
        private OverlapAttack weaponAttack;
        private Animator modelAnimator;
        private Transform modelTransform;
        private bool hasFired;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = MaceSlam.baseDuration / this.attackSpeedStat;
            this.priorityReduceDuration = this.duration * MaceSlam.durationBeforePriorityReduces;
            this.modelAnimator = base.GetModelAnimator();
            this.modelTransform = base.GetModelTransform();

            base.PlayCrossfade("FullBody, Override", "MaceSlam", "Whirlwind.playbackRate", this.duration, 0.1f);
            Util.PlayAttackSpeedSound(EntityStates.BrotherMonster.WeaponSlam.attackSoundString, base.gameObject, this.attackSpeedStat);

            if (base.characterDirection) base.characterDirection.moveVector = base.GetAimRay().direction;

            if (base.isAuthority)
            {
                OverlapAttack overlapAttack = new OverlapAttack();
                overlapAttack.attacker = base.gameObject;
                overlapAttack.damage = MaceSlam.damageCoefficient * this.damageStat;
                overlapAttack.damageColorIndex = DamageColorIndex.Default;
                overlapAttack.damageType = DamageTypeCombo.GenericPrimary;
                overlapAttack.hitEffectPrefab = EntityStates.BrotherMonster.WeaponSlam.weaponHitEffectPrefab;
                overlapAttack.hitBoxGroup = Array.Find<HitBoxGroup>(this.modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "LeapStrike");
                overlapAttack.impactSound = EntityStates.BrotherMonster.WeaponSlam.weaponImpactSound.index;
                overlapAttack.inflictor = base.gameObject;
                overlapAttack.procChainMask = default(ProcChainMask);
                overlapAttack.pushAwayForce = MaceSlam.weaponForce;
                overlapAttack.procCoefficient = 1f;
                overlapAttack.teamIndex = base.GetTeam();

                this.weaponAttack = overlapAttack;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.isAuthority && base.inputBank && base.skillLocator && base.skillLocator.utility.IsReady() && base.inputBank.skill3.justPressed)
            {
                base.skillLocator.utility.ExecuteIfReady();
                return;
            }

            if (this.modelAnimator)
            {
                if (base.fixedAge >= (0.5f * this.duration) && !this.hasFired)
                {
                    this.hasFired = true;

                    EffectManager.SimpleMuzzleFlash(EntityStates.BrotherMonster.WeaponSlam.slamImpactEffect, base.gameObject, "SwingCenter", false);

                    if (base.isAuthority)
                    {
                        if (base.characterDirection) base.characterDirection.moveVector = base.characterDirection.forward;

                        if (this.modelTransform)
                        {
                            Transform transform = base.FindModelChild("SwingCenter");
                            if (transform)
                            {
                                this.blastAttack = new BlastAttack();
                                this.blastAttack.attacker = base.gameObject;
                                this.blastAttack.inflictor = base.gameObject;
                                this.blastAttack.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
                                this.blastAttack.baseDamage = this.damageStat * MaceSlam.damageCoefficient;
                                this.blastAttack.baseForce = MaceSlam.forceMagnitude;
                                this.blastAttack.position = transform.position;
                                this.blastAttack.radius = MaceSlam.radius;
                                this.blastAttack.bonusForce = Vector3.up * MaceSlam.upwardForce;

                                this.blastAttack.Fire();
                            }

                            this.weaponAttack.Fire();
                        }
                    }
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
            if (base.fixedAge <= (this.duration * this.priorityReduceDuration))
            {
                return InterruptPriority.PrioritySkill;
            }
            return InterruptPriority.Skill;
        }
    }
}
