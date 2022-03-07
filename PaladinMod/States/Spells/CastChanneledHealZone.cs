using UnityEngine;

namespace PaladinMod.States.Spell
{
    public class CastChanneledHealZone : BaseCastChanneledSpellState
    {
        public override void OnEnter()
        {
            this.baseDuration = 0.6f;
            this.muzzleflashEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/CrocoDiseaseImpactEffect");
            this.projectilePrefab = Modules.Projectiles.healZone;
            this.castSoundString = Modules.Sounds.CastHeal;

            base.OnEnter();
        }
    }
}