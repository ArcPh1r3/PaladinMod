using RoR2.Projectile;

namespace PaladinMod.States
{
    public class ThrowLightningSpear : BaseThrowSpellState
    {
        public override void OnEnter()
        {
            this.baseDuration = 0.8f;
            this.force = 5f;
            this.maxDamageCoefficient = StaticValues.lightningSpearMaxDamageCoefficient;
            this.minDamageCoefficient = StaticValues.lightningSpearMinDamageCoefficient;
            this.muzzleflashEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab;
            this.projectilePrefab = Modules.Projectiles.lightningSpear;
            this.selfForce = 0f;

            base.OnEnter();

            /*ChildLocator childLocator = base.GetModelChildLocator();
            if (childLocator)
            {
                childLocator.FindChild("SpearThrowEffect").gameObject.SetActive(true);
            }*/
        }
        protected override void ModifyProjectileInfo(FireProjectileInfo fireProjectileInfo)
        {
            base.ModifyProjectileInfo(fireProjectileInfo);
            fireProjectileInfo.damageTypeOverride = RoR2.DamageTypeCombo.GenericSecondary;
        }
    }
}