using RoR2;
using EntityStates;
using UnityEngine;
using PaladinMod.Misc;
using System;

namespace PaladinMod.States
{
    public class PaladinMain : GenericCharacterMain
    {
        public static event Action<Run> onSunSurvived = delegate { };

        private bool swordActive;
        private Material swordMat;
        private float swordTransition;
        private GameObject swordActiveEffect;
        private PaladinSwordController swordController;
        private ChildLocator childLocator;
        private Animator animator;
        private ParticleSystem swordTrailEffect;
        private bool trailEffectIsPlaying;
        private bool wasActive;
        private uint trailEffectPlayID;
        private float sunSurvivalStopwatch;
        private bool hasBeenBurned;

        public LocalUser localUser;

        public override void OnEnter()
        {
            base.OnEnter();
            this.childLocator = base.GetModelChildLocator();
            this.animator = base.GetModelAnimator();
            this.localUser = LocalUserManager.readOnlyLocalUsersList[0];
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
                this.childLocator.FindChild("SwordActiveEffect")?.gameObject.SetActive(false);
                string effectString = Modules.Effects.GetSkinInfo(this.swordController.skinName).passiveEffectName;
                if (effectString != "") this.swordActiveEffect = this.childLocator.FindChild(effectString)?.gameObject;

                //ugly hack because for some reason the sparks are being retarded in this one scene
                string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                if (sceneName != "sulfurpools"){
                    this.swordTrailEffect = this.childLocator.FindChild("SwordSparksEffect")?.GetComponent<ParticleSystem>();
                } 
            }
        }

        public override void Update()
        {
            base.Update();

            if (base.isAuthority && base.characterMotor.isGrounded && !this.localUser.isUIFocused)
            {
                if (Input.GetKeyDown(Modules.Config.praiseKeybind.Value))
                {
                    this.outer.SetInterruptState(new Emotes.PraiseTheSun(), InterruptPriority.Any);
                    return;
                }
                else if (Input.GetKeyDown(Modules.Config.restKeybind.Value))
                {
                    this.outer.SetInterruptState(new Emotes.Rest(), InterruptPriority.Any);
                    return;
                }
                else if (Input.GetKeyDown(Modules.Config.pointKeybind.Value))
                {
                    this.outer.SetInterruptState(new Emotes.PointDown(), InterruptPriority.Any);
                    return;
                } 
                else if (Input.GetKeyDown(Modules.Config.swordPoseKeybind.Value)) 
                {
                    this.outer.SetInterruptState(new Emotes.TestPose(), InterruptPriority.Any);
                    return;
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.sunSurvivalStopwatch -= Time.fixedDeltaTime;

            // cruel sun unlock
            if (base.characterBody.HasBuff(RoR2Content.Buffs.Overheat))
            {
                if (base.characterBody.GetBuffCount(RoR2Content.Buffs.Overheat) >= 23)
                {
                    this.hasBeenBurned = true;
                    this.sunSurvivalStopwatch = 1.5f;
                }
            }
            if (this.hasBeenBurned == this.sunSurvivalStopwatch <= 0f)
            {
                onSunSurvived(Run.instance);
            }

            if (base.healthComponent && this.swordMat)
            {
                // set to fullCombinedHealth to count shields- check for supply drop and aetherium when those are working again
                if (base.healthComponent.combinedHealth >= (0.9f * base.healthComponent.fullHealth) || base.healthComponent.barrier > 0)
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
                //this.animator.SetFloat("sprintValue", base.characterBody.isSprinting ? -1 : 0, 0.2f, Time.fixedDeltaTime);
                this.animator.SetBool("inCombat", (!base.characterBody.outOfCombat || !base.characterBody.outOfDanger));
                this.animator.SetBool("isAttacking", !base.characterBody.outOfCombat);
            }

            if (base.characterBody.isSprinting && this.isGrounded && base.characterBody.outOfCombat && this.swordTrailEffect)
            {
                RaycastHit raycastHit = default(RaycastHit);
                Vector3 raycastOrigin = this.swordTrailEffect.transform.position;
                raycastOrigin.y += 0.5f;

                bool isSwordOnGround = false;
                if (Physics.Raycast(new Ray(raycastOrigin, Vector3.down), out raycastHit, 1.25f, LayerIndex.world.mask | LayerIndex.water.mask, QueryTriggerInteraction.Collide))
                {
                    isSwordOnGround = true;
                }

                if (this.trailEffectIsPlaying)
                {
                    if (!isSwordOnGround)
                    {
                        this.StopDraggingSword();
                    }

                    //this.swordTrailEffect.transform.position = raycastHit.point;
                    //this.swordTrailEffect.transform.rotation = Quaternion.Euler(raycastHit.normal);
                }
                else
                {
                    if (isSwordOnGround)
                    {
                        this.StartDraggingSword();
                    }
                }
            }
            else
            {
                if (this.trailEffectIsPlaying)
                {
                    this.StopDraggingSword();
                }
            }
        }

        private void StartDraggingSword()
        {
            this.trailEffectIsPlaying = true;
            if (this.swordTrailEffect) this.swordTrailEffect.Play();
            //this.trailEffectPlayID = Util.PlaySound("PaladinDragSword", base.gameObject);
        }

        private void StopDraggingSword()
        {
            this.trailEffectIsPlaying = false;
            if (this.swordTrailEffect) this.swordTrailEffect.Stop();
            //AkSoundEngine.StopPlayingID(this.trailEffectPlayID);
            //this.trailEffectPlayID = 0;
        }

        public override void OnExit()
        {
            base.OnExit();
            this.StopDraggingSword();
        }
    }
}