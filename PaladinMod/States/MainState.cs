using RoR2;
using EntityStates;
using UnityEngine;
using PaladinMod.Misc;

namespace PaladinMod.States
{
    public class PaladinMain : GenericCharacterMain
    {
        private bool swordActive;
        private Material swordMat;
        private float swordTransition;
        private GameObject swordActiveEffect;
        private PaladinSwordController swordController;
        private ChildLocator childLocator;
        private Animator animator;
        private bool wasActive;

        public override void OnEnter()
        {
            base.OnEnter();
            this.childLocator = base.GetModelChildLocator();
            this.animator = base.GetModelAnimator();
            if (base.healthComponent.combinedHealth >= (0.9f * base.healthComponent.fullCombinedHealth) || base.healthComponent.barrier > 0)
            {
                this.swordActive = true;
                this.swordTransition = StaticValues.maxSwordGlow;
            }
            else
            {
                this.swordActive = false;
                this.swordTransition = 0;
            }
            this.swordController = base.characterBody.GetComponent<PaladinSwordController>();

            if (base.characterBody)
            {
                Transform modelTransform = base.GetModelTransform();
                if (modelTransform)
                {
                    this.swordMat = modelTransform.GetComponent<CharacterModel>().baseRendererInfos[0].defaultMaterial;
                }
            }

            if (this.childLocator)
            {
                string effectString = Modules.Effects.GetSkinInfo(this.swordController.skinName).passiveEffectName;
                if (effectString != "") this.swordActiveEffect = this.childLocator.FindChild(effectString).gameObject;
            }
        }

        public override void Update()
        {
            base.Update();

            if (base.isAuthority && base.characterMotor.isGrounded)
            {
                if (Input.GetKeyDown(Modules.Config.praiseKeybind.Value))
                {
                    this.outer.SetInterruptState(EntityState.Instantiate(new SerializableEntityStateType(typeof(Emotes.PraiseTheSun))), InterruptPriority.Any);
                    return;
                }
                else if (Input.GetKeyDown(Modules.Config.restKeybind.Value))
                {
                    this.outer.SetInterruptState(EntityState.Instantiate(new SerializableEntityStateType(typeof(Emotes.Rest))), InterruptPriority.Any);
                    return;
                }
                else if (Input.GetKeyDown(Modules.Config.pointKeybind.Value))
                {
                    this.outer.SetInterruptState(EntityState.Instantiate(new SerializableEntityStateType(typeof(Emotes.PointDown))), InterruptPriority.Any);
                    return;
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.healthComponent && this.swordMat)
            {
                if (base.healthComponent.combinedHealth >= (0.9f * base.healthComponent.fullCombinedHealth) || base.healthComponent.barrier > 0)
                {
                    this.swordActive = true;
                }
                else this.swordActive = false;

                if (this.swordActive)
                {
                    if (this.swordActiveEffect) this.swordActiveEffect.SetActive(true);
                    this.swordTransition += StaticValues.swordGlowSpeed * Time.fixedDeltaTime;
                }
                else
                {
                    if (this.swordActiveEffect) this.swordActiveEffect.SetActive(false);
                    this.swordTransition -= StaticValues.swordGlowSpeed * Time.fixedDeltaTime;
                }

                if (this.swordController) this.swordController.swordActive = this.swordActive;

                this.swordTransition = Mathf.Clamp(this.swordTransition, 0, StaticValues.maxSwordGlow);

                this.swordMat.SetFloat("_EmPower", this.swordTransition);

                if (this.swordActive && !this.wasActive) Util.PlaySound(Modules.Sounds.SwordActive, base.gameObject);
                this.wasActive = this.swordActive;
            }

            if (this.animator)
            {
                this.animator.SetFloat("sprintValue", base.characterBody.isSprinting ? -1 : 0, 0.2f, Time.fixedDeltaTime);
                this.animator.SetBool("inCombat", (!base.characterBody.outOfCombat || !base.characterBody.outOfDanger));
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
