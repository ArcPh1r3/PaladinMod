using UnityEngine;

namespace PaladinMod.States
{
    public class CastSmallHeal : BaseCastChanneledSpellState
    {
        public override void OnEnter()
        {
            this.baseDuration = 0.4f;
            this.muzzleString = "HandL";
            this.muzzleflashEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/CrocoDiseaseImpactEffect");
            this.projectilePrefab = Modules.Projectiles.heal;
            this.castSoundString = Modules.Sounds.CastHeal;

            base.OnEnter();
        }

        protected override void PlayCastAnimation()
        {
            base.PlayAnimation("Gesture, Override", "CastHeal", "Spell.playbackRate", this.baseDuration * 1.5f);
        }
    }
}