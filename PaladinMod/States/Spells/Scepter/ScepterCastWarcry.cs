namespace PaladinMod.States.Spell
{
    public class ScepterCastWarcry : BaseCastChanneledSpellState
    {
        public override void OnEnter()
        {
            this.baseDuration = 0.6f;
            this.muzzleflashEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol.effectPrefab;
            this.projectilePrefab = Modules.Projectiles.scepterWarcry;
            this.castSoundString = Modules.Sounds.CastHeal;

            base.OnEnter();
        }
    }
}
