using UnityEngine;

namespace PaladinMod.States
{
    public class ChargeLightningSpear : BaseChargeSpellState
    {
        private GameObject chargeEffect;

        public override void OnEnter()
        {
            this.baseDuration = StaticValues.lightningSpearChargeTime;
            this.chargeEffectPrefab = null;
            this.chargeSoundString = "Play_mage_m2_charge";
            this.crosshairOverridePrefab = Resources.Load<GameObject>("Prefabs/Crosshair/ToolbotGrenadeLauncherCrosshair");
            this.maxBloomRadius = 0.1f;
            this.minBloomRadius = 5;

            base.OnEnter();

            ChildLocator childLocator = base.GetModelChildLocator();
            if (childLocator)
            {
                this.chargeEffect = childLocator.FindChild("SpearChargeEffect").gameObject;
                this.chargeEffect.SetActive(true);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            this.chargeEffect.transform.localScale = Vector3.one * 2 * this.CalcCharge();
        }

        public override void OnExit()
        {
            base.OnExit();

            if (this.chargeEffect)
            {
                this.chargeEffect.SetActive(false);
            }
        }

        protected override BaseThrowSpellState GetNextState()
        {
            return new ThrowLightningSpear();
        }
    }
}
