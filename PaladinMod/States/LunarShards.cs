using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace PaladinMod.States
{
    public class LunarShards : BaseSkillState
    {
        public static float baseDuration = 0.1f;
        public static float recoilAmplitude = 0.8f;
        public static float spreadBloomValue = 1f;
        public static string muzzleString = "HandL";

        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = LunarShards.baseDuration / this.attackSpeedStat;

            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();

                FireProjectileInfo fireProjectileInfo = default(FireProjectileInfo);
                fireProjectileInfo.position = aimRay.origin;
                fireProjectileInfo.rotation = Quaternion.LookRotation(aimRay.direction);
                fireProjectileInfo.crit = base.characterBody.RollCrit();
                fireProjectileInfo.damage = base.characterBody.damage * StaticValues.lunarShardDamageCoefficient;
                fireProjectileInfo.damageColorIndex = DamageColorIndex.Default;
                fireProjectileInfo.owner = base.gameObject;
                fireProjectileInfo.procChainMask = default(ProcChainMask);
                fireProjectileInfo.force = 0f;
                fireProjectileInfo.useFuseOverride = false;
                fireProjectileInfo.useSpeedOverride = false;
                fireProjectileInfo.target = null;
                fireProjectileInfo.projectilePrefab = Modules.Projectiles.lunarShard;

                ProjectileManager.instance.FireProjectile(fireProjectileInfo);
            }

            base.PlayAnimation("LeftArm, Override", "LunarShard", "LunarShard.playbackRate", this.duration * 5f);

            base.AddRecoil(-0.4f * LunarShards.recoilAmplitude, -0.8f * LunarShards.recoilAmplitude, -0.3f * LunarShards.recoilAmplitude, 0.3f * LunarShards.recoilAmplitude);
            base.characterBody.AddSpreadBloom(LunarShards.spreadBloomValue);

            EffectManager.SimpleMuzzleFlash(EntityStates.BrotherMonster.Weapon.FireLunarShards.muzzleFlashEffectPrefab, base.gameObject, "HandL", false);
            Util.PlaySound(EntityStates.BrotherMonster.Weapon.FireLunarShards.fireSound, base.gameObject);

            base.skillLocator.secondary.rechargeStopwatch = 0f;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}