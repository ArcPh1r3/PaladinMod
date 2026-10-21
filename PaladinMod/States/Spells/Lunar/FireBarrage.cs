using EntityStates;
using PaladinMod.Modules;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace PaladinMod.States.Spells.Lunar
{
    public class FireBarrage : BaseSkillState
    {
        public static float baseDuration = 0.1f;
        public static float recoilAmplitude = 0.8f;
        public static float spreadBloomValue = 1f;
        public static string muzzleString = "HandL";
        public static int totalShardCount = 6;
        public static float angle => StaticValues.LunarBarrageAngle

        private float duration;
        private float stopwatch;
        private int shardCount = 0;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = LunarShards.baseDuration / this.attackSpeedStat;

            this.FireShard();
        }

        private void FireShard()
        {
            this.shardCount++;
            base.PlayAnimation("LeftArm, Override", "LunarShard", "LunarShard.playbackRate", this.duration * 5f);

            // Too shiny in VR
            if (!PaladinPlugin.IsLocalVRPlayer(characterBody))
                EffectManager.SimpleMuzzleFlash(Asset.lunarShardMuzzleFlash, base.gameObject, "HandL", false);
            Util.PlaySound(EntityStates.BrotherMonster.Weapon.FireLunarShards.fireSound, base.gameObject);

            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();

                FireProjectileInfo fireProjectileInfo = default(FireProjectileInfo);
                fireProjectileInfo.position = aimRay.origin;
                fireProjectileInfo.rotation = Quaternion.LookRotation(aimRay.direction);
                fireProjectileInfo.crit = base.characterBody.RollCrit();
                fireProjectileInfo.damage = base.characterBody.damage * StaticValues.lunarBarrageDamageCoefficient;
                fireProjectileInfo.damageTypeOverride = DamageTypeCombo.GenericSecondary | DamageType.FruitOnHit;
                fireProjectileInfo.damageColorIndex = DamageColorIndex.Default;
                fireProjectileInfo.owner = base.gameObject;
                fireProjectileInfo.procChainMask = default(ProcChainMask);
                fireProjectileInfo.force = 0f;
                fireProjectileInfo.useFuseOverride = false;
                fireProjectileInfo.useSpeedOverride = false;
                fireProjectileInfo.target = null;
                fireProjectileInfo.projectilePrefab = Modules.Projectiles.lunarShard;

                ProjectileManager.instance.FireProjectile(fireProjectileInfo);

                base.AddRecoil(-0.4f * LunarShards.recoilAmplitude, -0.8f * LunarShards.recoilAmplitude, -0.3f * LunarShards.recoilAmplitude, 0.3f * LunarShards.recoilAmplitude);
            }

            base.characterBody.AddSpreadBloom(LunarShards.spreadBloomValue);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.stopwatch += Time.fixedDeltaTime;
            this.StartAimMode(0.5f);

            if (this.stopwatch >= this.duration)
            {
                if (this.shardCount > FireBarrage.totalShardCount)
                {
                    if (base.isAuthority) this.outer.SetNextStateToMain();
                }
                else
                {
                    this.stopwatch = 0f;
                    this.FireShard();
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}