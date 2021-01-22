using UnityEngine;

namespace PaladinMod.States.Spell
{
    public class CastChanneledWarcry : BaseCastChanneledSpellState
    {
        public override void OnEnter()
        {
            this.baseDuration = 0.6f;
            this.muzzleflashEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXLemurianBruiserFireballImpact");
            this.projectilePrefab = Modules.Projectiles.warcry;
            this.castSoundString = Modules.Sounds.CastHeal;

            base.OnEnter();
        }
    }
}