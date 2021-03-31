using EntityStates;
using RoR2;
using UnityEngine;

namespace PaladinMod.States
{
    public class WhirlwindBase : BaseState
    {
        public static GameObject swingEffectPrefab = EntityStates.Merc.WhirlwindBase.swingEffectPrefab;
        public static GameObject hitEffectPrefab = EntityStates.Merc.WhirlwindBase.hitEffectPrefab;
        public static string attackSoundString = EntityStates.Merc.WhirlwindBase.attackSoundString;
        public static string hitSoundString = EntityStates.Merc.WhirlwindBase.hitSoundString;
        public static float slashPitch = 0.6f;
        public static float hitPauseDuration = 0.1f;
        public float baseDuration = 0.75f;

        public float baseDamageCoefficient;
        public string slashChildName;
        public float selfForceMagnitude;
        public float moveSpeedBonusCoefficient;
        public float smallHopVelocity;
        public string hitboxString;
        protected Animator animator;
        protected float duration;
        protected float hitInterval;
        protected float hitPauseTimer;
        protected bool isInHitPause;
        private bool hasSwung;
        protected OverlapAttack overlapAttack;
        protected BaseState.HitStopCachedState hitStopCachedState;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.hasSwung = false;

            this.overlapAttack = base.InitMeleeOverlap(this.baseDamageCoefficient, WhirlwindBase.hitEffectPrefab, base.GetModelTransform(), this.hitboxString);

            if (base.characterDirection && base.inputBank)
            {
                base.characterDirection.forward = base.inputBank.aimDirection;
            }

            base.SmallHop(base.characterMotor, this.smallHopVelocity);
            this.PlayAnim();
        }

        protected virtual void PlayAnim()
        {
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.hitPauseTimer -= Time.fixedDeltaTime;

            if (base.fixedAge >= this.duration * 0.2f)
            {
                Util.PlayAttackSpeedSound(WhirlwindBase.attackSoundString, base.gameObject, WhirlwindBase.slashPitch);
                EffectManager.SimpleMuzzleFlash(WhirlwindBase.swingEffectPrefab, base.gameObject, this.slashChildName, false);

                if (base.isAuthority && !this.hasSwung)
                {
                    this.hasSwung = true;

                    //unfinished
                    //if (base.attack(this.overlapAttack))
                }
            }

            if (base.isAuthority)
            {
                if (base.FireMeleeOverlap(this.overlapAttack, this.animator, "Sword.active", 0f, true))
                {
                    Util.PlaySound(WhirlwindBase.hitSoundString, base.gameObject);
                    if (!this.isInHitPause)
                    {
                        this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "Whirlwind.playbackRate");
                        this.hitPauseTimer = WhirlwindBase.hitPauseDuration / this.attackSpeedStat;
                        this.isInHitPause = true;
                    }
                }

                if (this.hitPauseTimer <= 0f && this.isInHitPause)
                {
                    base.ConsumeHitStopCachedState(this.hitStopCachedState, base.characterMotor, this.animator);
                    this.isInHitPause = false;
                }

                if (!this.isInHitPause)
                {
                    if (base.characterMotor && base.characterDirection)
                    {
                        Vector3 velocity = base.characterDirection.forward * this.moveSpeedStat * Mathf.Lerp(this.moveSpeedBonusCoefficient, 1f, base.age / this.duration);
                        velocity.y = base.characterMotor.velocity.y;
                        base.characterMotor.velocity = velocity;
                    }
                }
                else
                {
                    base.characterMotor.velocity = Vector3.zero;
                    this.hitPauseTimer -= Time.fixedDeltaTime;
                    this.animator.SetFloat("Whirlwind.playbackRate", 0f);
                }

                if (base.fixedAge >= this.duration)
                {
                    this.outer.SetNextStateToMain();
                }
            }
        }
    }
}
