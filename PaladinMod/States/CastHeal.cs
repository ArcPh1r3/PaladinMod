namespace PaladinMod.States
{
    public class CastHeal : BaseCastSpellState
    {
        public override void OnEnter()
        {
            this.baseDuration = 0.3f;
            this.muzzleflashEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol.effectPrefab;
            this.projectilePrefab = Modules.Projectiles.heal;

            base.OnEnter();
        }
    }
}
