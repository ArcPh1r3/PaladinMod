namespace PaladinMod.States
{
    public class CastHealZone : BaseCastSpellState
    {
        public override void OnEnter()
        {
            this.baseDuration = 1.2f;
            this.muzzleflashEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol.effectPrefab;
            this.projectilePrefab = Modules.Projectiles.healZone;

            base.OnEnter();
        }
    }
}
