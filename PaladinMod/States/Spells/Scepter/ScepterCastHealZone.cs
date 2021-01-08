namespace PaladinMod.States.Spell
{
    public class ScepterCastHealZone : BaseCastChanneledSpellState
    {
        public override void OnEnter()
        {
            this.baseDuration = 0.6f;
            this.muzzleflashEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol.effectPrefab;
            this.projectilePrefab = Modules.Projectiles.scepterHealZone;
            this.castSoundString = Modules.Sounds.CastHeal;

            base.OnEnter();
        }
    }
}
