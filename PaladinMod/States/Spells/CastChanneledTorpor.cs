namespace PaladinMod.States.Spell
{
    public class CastChanneledTorpor : BaseCastChanneledSpellState
    {
        public override void OnEnter()
        {
            this.baseDuration = 1.2f;
            this.muzzleflashEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol.effectPrefab;
            this.projectilePrefab = Modules.Projectiles.torpor;
            this.castSoundString = Modules.Sounds.CastTorpor;

            base.OnEnter();
        }
    }
}
