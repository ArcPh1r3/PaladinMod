namespace PaladinMod.States.Spell
{
    public class CastChanneledWarcry : BaseCastChanneledSpellState
    {
        public override void OnEnter()
        {
            this.baseDuration = 0.6f;
            this.muzzleflashEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol.effectPrefab;
            this.projectilePrefab = Modules.Projectiles.warcry;
            this.castSoundString = Modules.Sounds.CastHeal;

            base.OnEnter();
        }
    }
}
