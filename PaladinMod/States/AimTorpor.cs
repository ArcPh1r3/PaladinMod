using UnityEngine;

namespace PaladinMod.States
{
    public class AimTorpor : BaseAimSpellState
    {
        private GameObject chargeEffect;

        public override void OnEnter()
        {
            this.chargeEffectPrefab = null;
            this.chargeSoundString = "Play_mage_m2_charge";
            this.spellRadius = StaticValues.torporRadius;

            base.OnEnter();

            ChildLocator childLocator = base.GetModelChildLocator();
            if (childLocator)
            {
                this.chargeEffect = childLocator.FindChild("TorporAimEffect").gameObject;
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
                this.chargeEffect.SetActive(false);
            }
        }

        protected override BaseCastSpellState GetNextState()
        {
            return new CastTorpor();
        }
    }
}
