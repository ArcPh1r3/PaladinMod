using UnityEngine;

namespace PaladinMod.States
{
    public class AimTorpor : BaseAimSpellState
    {
        public override void OnEnter()
        {
            this.chargeEffectPrefab = null;
            this.chargeSoundString = "Play_mage_m2_charge";
            this.spellRadius = StaticValues.torporRadius;

            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        protected override BaseCastSpellState GetNextState()
        {
            return new CastHealZone();
        }
    }
}
