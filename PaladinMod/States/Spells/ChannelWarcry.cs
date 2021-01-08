using UnityEngine;

namespace PaladinMod.States.Spell
{
    public class ChannelWarcry : BaseChannelSpellState
    {
        private GameObject chargeEffect;

        public override void OnEnter()
        {
            this.chargeEffectPrefab = null;
            this.chargeSoundString = Modules.Sounds.ChannelHeal;
            this.maxSpellRadius = StaticValues.warcryRadius;
            this.baseDuration = StaticValues.warcryChannelDuration;

            base.OnEnter();

            ChildLocator childLocator = base.GetModelChildLocator();
            if (childLocator)
            {
                this.chargeEffect = childLocator.FindChild("WarcryChannelEffect").gameObject;
                this.chargeEffect.SetActive(false);
                this.chargeEffect.SetActive(true);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
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
            return new CastChanneledWarcry();
        }
    }
}
