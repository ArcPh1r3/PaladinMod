namespace PaladinMod.States
{
    public class CastTorpor : BaseCastSpellState
    {
        public override void OnEnter()
        {
            this.baseDuration = 0.4f;
            this.muzzleflashEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol.effectPrefab;
            this.projectilePrefab = Modules.Projectiles.torpor;

            base.OnEnter();
        }
    }
}
