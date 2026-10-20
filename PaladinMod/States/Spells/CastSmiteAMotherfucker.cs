using RoR2;

namespace PaladinMod.States.Spell
{
    public class CastSmiteAMotherfucker : BaseCastChanneledSpellStateTargeted
    {
        public override void OnEnter()
        {
            base.OnEnter();

            BlastAttack blast = new BlastAttack
            {
                baseDamage = 100000 * damageStat,
                radius = 1,
                position = trackingTarget.transform.position,
                attacker = gameObject,
                attackerFiltering = AttackerFiltering.AlwaysHit,
                teamIndex = teamComponent.teamIndex
            };
            blast.Fire();

            EffectData effect = new EffectData
            {
                origin = trackingTarget.transform.position
            };
            EffectManager.SpawnEffect(Modules.Asset.airSlamFX, effect, false);
        }
    }
}