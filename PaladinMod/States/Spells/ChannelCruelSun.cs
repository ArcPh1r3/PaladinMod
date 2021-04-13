using PaladinMod.Misc;
using UnityEngine;

namespace PaladinMod.States.Spell
{
    public class ChannelCruelSun : BaseChannelSpellState
    {
        private GameObject chargeEffect;
        private PaladinSwordController swordController;

        public override void OnEnter()
        {
            this.chargeEffectPrefab = null;
            this.chargeSoundString = Modules.Sounds.ChannelTorpor;
            this.maxSpellRadius = CastCruelSun.sunPrefabDiameter * 0.5f;
            this.baseDuration = StaticValues.cruelSunChannelDuration;
            this.swordController = base.gameObject.GetComponent<PaladinSwordController>();
            this.overrideAreaIndicatorMat = Modules.Assets.areaIndicatorMat;

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
            if (this.swordController) this.swordController.sunPosition = this.areaIndicatorInstance.transform.position;
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
