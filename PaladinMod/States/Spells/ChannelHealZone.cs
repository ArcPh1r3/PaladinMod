using UnityEngine;

namespace PaladinMod.States.Spell
{
    public class ChannelHealZone : BaseChannelSpellState
    {
        private GameObject chargeEffect;

        public override void OnEnter()
        {
            this.chargeEffectPrefab = null;
            this.chargeSoundString = Modules.Sounds.ChannelHeal;
            this.maxSpellRadius = StaticValues.healZoneRadius;
            this.baseDuration = StaticValues.healZoneChannelDuration;

            base.OnEnter();

            ChildLocator childLocator = base.GetModelChildLocator();
            if (childLocator)
            {
                this.chargeEffect = childLocator.FindChild("HealChannelEffect").gameObject;
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
            return new CastChanneledHealZone();
        }
    }
}
