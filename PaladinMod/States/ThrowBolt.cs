namespace PaladinMod.States
{
    public class ThrowBolt : BaseThrowSpellState
    {
        public override void OnEnter()
        {
            this.baseDuration = 0.4f;
            this.force = 5f;
            this.maxDamageCoefficient = StaticValues.lightningBoltDamageCoefficient;
            this.minDamageCoefficient = StaticValues.lightningBoltDamageCoefficient;
            this.muzzleflashEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol.effectPrefab;
            this.projectilePrefab = Modules.Projectiles.lightningSpear;
            this.selfForce = 0f;

            base.OnEnter();

            ChildLocator childLocator = base.GetModelChildLocator();
            if (childLocator)
            {
                childLocator.FindChild("SpearThrowEffect").gameObject.SetActive(true);
            }
        }
    }
}
