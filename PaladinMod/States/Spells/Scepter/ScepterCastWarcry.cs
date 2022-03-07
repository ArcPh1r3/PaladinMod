using UnityEngine;

namespace PaladinMod.States.Spell
{
    public class ScepterCastWarcry : BaseCastChanneledSpellState
    {
        public override void OnEnter()
        {
            this.baseDuration = 0.6f;
            this.muzzleflashEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXArchWispCannonImpact");
            this.projectilePrefab = Modules.Projectiles.scepterWarcry;
            this.castSoundString = Modules.Sounds.CastHeal;

            base.OnEnter();
        }
    }
}