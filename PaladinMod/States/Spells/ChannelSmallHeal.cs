using EntityStates;
using PaladinMod.Misc;
using UnityEngine;

namespace PaladinMod.States.Spell
{
    public class ChannelSmallHeal : BaseChannelSpellState
    {
        private GameObject chargeEffect;
        private PaladinSwordController swordController;

        public override void OnEnter()
        {
            this.chargeEffectPrefab = null;
            this.chargeSoundString = Modules.Sounds.ChannelHeal;
            this.maxSpellRadius = StaticValues.healRadius;
            this.baseDuration = 0.25f * StaticValues.healZoneChannelDuration;
            this.swordController = base.gameObject.GetComponent<PaladinSwordController>();
            this.zooming = false;

            base.OnEnter();

            ChildLocator childLocator = base.GetModelChildLocator();
            if (childLocator)
            {
                this.chargeEffect = childLocator.FindChild("SmallHealChannelEffect").gameObject;
                this.chargeEffect.SetActive(false);
                this.chargeEffect.SetActive(true);
            }
        }

        protected override void PlayChannelAnimation()
        {
            base.PlayAnimation("Gesture, Override", "ChannelHeal", "Spell.playbackRate", this.baseDuration);
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
            return new CastSmallHeal();
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Frozen;
        }
    }
}