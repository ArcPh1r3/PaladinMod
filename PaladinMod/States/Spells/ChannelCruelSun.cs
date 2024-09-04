using PaladinMod.Misc;
using UnityEngine;

namespace PaladinMod.States.Spell
{
    public class ChannelCruelSun : BaseChannelSpellState
    {
        private GameObject chargeEffect;
        protected virtual string chargeEffectChild => "CruelSunChannelEffect";

        public override void OnEnter()
        {
            this.chargeEffectPrefab = null;
            this.chargeSoundString = Modules.Sounds.ChannelTorpor;
            this.baseDuration = StaticValues.cruelSunChannelDuration;
            this.castAtMaxCharge = true;

            base.OnEnter();

            ChildLocator childLocator = base.GetModelChildLocator();
            if (childLocator)
            {
                this.chargeEffect = childLocator.FindChild(chargeEffectChild).gameObject;
                this.chargeEffect.SetActive(false);
                this.chargeEffect.SetActive(true);
            }
        }

        protected override void PlayChannelAnimation()
        {
            base.PlayCrossfade("Gesture, Override", "ChannelSun", "Spell.playbackRate", 2f, 0.05f);
        }

        public override void OnExit()
        {
            base.OnExit();

            if (this.chargeEffect)
            {
                this.chargeEffect.GetComponentInChildren<ParticleSystem>().Stop();
            }
        }

        protected override BaseCastChanneledSpellState GetNextState()
        {
            return new CastCruelSun();
        }
    }
}