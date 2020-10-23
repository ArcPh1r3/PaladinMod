using UnityEngine;

namespace PaladinMod.States
{
    public class ChargeBolt : BaseChargeSpellState
    {
        private GameObject chargeEffect;

        public override void OnEnter()
        {
            this.baseDuration = StaticValues.lightningBoltChargeTime;
            this.chargeEffectPrefab = null;
            this.chargeSoundString = "Play_mage_m2_charge";
            this.crosshairOverridePrefab = Resources.Load<GameObject>("Prefabs/Crosshair/ToolbotGrenadeLauncherCrosshair");
            this.maxBloomRadius = 0.1f;
            this.minBloomRadius = 0.1f;

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

            this.chargeEffect.transform.localScale = Vector3.one * 0.75f * this.CalcCharge();
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
            return new ThrowBolt();
        }
    }
}
