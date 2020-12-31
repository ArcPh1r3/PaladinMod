using UnityEngine;

namespace PaladinMod.States
{
    public class ChargeLightningSpear : BaseChargeSpellState
    {
        private GameObject chargeEffect;
        private Vector3 originalScale;

        public override void OnEnter()
        {
            this.baseDuration = StaticValues.lightningSpearChargeTime;
            this.chargeEffectPrefab = null;
            this.chargeSoundString = "Play_mage_m2_charge";
            this.crosshairOverridePrefab = Resources.Load<GameObject>("Prefabs/Crosshair/ToolbotGrenadeLauncherCrosshair");
            this.maxBloomRadius = 0.1f;
            this.minBloomRadius = 1;

            base.OnEnter();

            ChildLocator childLocator = base.GetModelChildLocator();
            if (childLocator)
            {
                this.chargeEffect = childLocator.FindChild("SpearChargeEffect").gameObject;
                this.chargeEffect.SetActive(true);
                this.originalScale = this.chargeEffect.transform.localScale;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            this.chargeEffect.transform.localScale = this.originalScale * 3 * this.CalcCharge();
        }

        public override void OnExit()
        {
            base.OnExit();

            if (this.chargeEffect)
            {
                this.chargeEffect.transform.localScale = this.originalScale;
                this.chargeEffect.SetActive(false);
            }
        }

        protected override BaseThrowSpellState GetNextState()
        {
            return new ThrowLightningSpear();
        }
    }
}
