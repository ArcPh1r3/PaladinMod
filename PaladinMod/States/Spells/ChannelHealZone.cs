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

            /*ChildLocator childLocator = base.GetModelChildLocator();
            if (childLocator)
            {
                this.chargeEffect = childLocator.FindChild("HealAimEffect").gameObject;
                this.chargeEffect.SetActive(true);
            }*/
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();

            /*if (this.chargeEffect)
            {
                this.chargeEffect.SetActive(false);
            }*/
        }

        protected override BaseCastChanneledSpellState GetNextState()
        {
            return new CastChanneledHealZone();
        }
    }
}
