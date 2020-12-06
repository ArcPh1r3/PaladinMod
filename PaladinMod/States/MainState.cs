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
        private GameObject swordTrailEffect;
        private GameObject swordEmpoweredTrailEffect;
        private PaladinSwordController swordController;
        private ChildLocator childLocator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.childLocator = base.GetModelChildLocator();
            this.swordActive = true;
            this.swordTransition = StaticValues.maxSwordGlow;
            this.swordController = base.characterBody.GetComponent<PaladinSwordController>();

            if (base.characterBody)
            {
                Transform modelTransform = base.GetModelTransform();
                if (modelTransform)
                {
                    this.swordMat = modelTransform.GetComponent<CharacterModel>().baseRendererInfos[6].defaultMaterial;
                }
            }

            if (this.childLocator)
            {
                this.swordActiveEffect = this.childLocator.FindChild("SwordActiveEffect").gameObject;
                this.swordTrailEffect = this.childLocator.FindChild("SwordTrailEffect").gameObject;
                this.swordEmpoweredTrailEffect = this.childLocator.FindChild("SwordPassiveTrailEffect").gameObject;
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
                else if (Input.GetKeyDown(Modules.Config.pointKeybind.Value))
                {
                    this.outer.SetInterruptState(EntityState.Instantiate(new SerializableEntityStateType(typeof(Emotes.PointDown))), InterruptPriority.Any);
                    return;
                }
                /*else if (Input.GetKeyDown("3"))
                {
                    this.outer.SetInterruptState(EntityState.Instantiate(new SerializableEntityStateType(typeof(Emotes.TestPose))), InterruptPriority.Any);
                    return;
                }*/
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.healthComponent && this.swordMat)
            {
                if (base.healthComponent.health >= (0.9f * base.healthComponent.fullHealth) || base.healthComponent.barrier > 0)
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
            }

            /*if (base.characterBody)
            {
                if (base.characterBody.isSprinting)
                {
                    if (this.swordTrailEffect) this.swordTrailEffect.SetActive(true);
                    //if (this.swordEmpoweredTrailEffect) this.swordEmpoweredTrailEffect.SetActive(true);
                }
                else
                {
                    if (this.swordTrailEffect) this.swordTrailEffect.SetActive(false);
                    //if (this.swordEmpoweredTrailEffect) this.swordEmpoweredTrailEffect.SetActive(false);
                }
            }*/
        }

        public override void OnExit()
        {
            //if (this.swordTrailEffect) this.swordTrailEffect.SetActive(false);
            base.OnExit();
        }
    }
}
