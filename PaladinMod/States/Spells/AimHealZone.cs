using UnityEngine;

namespace PaladinMod.States
{
    public class AimHealZone : BaseAimSpellState
    {
        private GameObject chargeEffect;

        public override void OnEnter()
        {
            this.chargeEffectPrefab = null;
            this.chargeSoundString = "Play_mage_m2_charge";
            this.spellRadius = StaticValues.healZoneRadius;

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

        protected override BaseCastSpellState GetNextState()
        {
            return new CastHealZone();
        }
    }
}