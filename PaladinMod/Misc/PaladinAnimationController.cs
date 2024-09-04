using IL.RoR2.Projectile;
using RoR2;
using System;
using UnityEngine;

namespace PaladinMod.Misc
{
    public class PaladinAnimationController : MonoBehaviour
    {
        private CharacterBody body;
        private CharacterModel model;
        private bool wasInCombat;
        private bool wasSprinting;
        private bool wasDragging;

        private Animator animator;

        private int combatLayerIndex;
        private int draggeLayerIndex;

        private float combatBufferTimer = -1f;
        private float combatLayerLerpTimer;
        private float combatLerpDeltaMultiplier = -1;
        private float HACKStopDragTimer = -1;

        void Start()
        {
            this.model = base.GetComponent<CharacterModel>();
            this.body = model.body;
            this.animator = GetComponent<Animator>();
            combatLayerIndex = animator.GetLayerIndex("Body, Combat");
            draggeLayerIndex = animator.GetLayerIndex("Body, Dragging");
        }

        void FixedUpdate()
        {
            UpdateAnimation();
            UpdateLayer();
        }

        void Update()
        {
        }

        private void UpdateAnimation()
        {
            // combat animation shit
            if (!this.animator || !this.body)
                return;

            if (combatBufferTimer >= 0)
            {
                combatBufferTimer -= Time.fixedDeltaTime;
            }
            combatLayerLerpTimer += Time.fixedDeltaTime * combatLerpDeltaMultiplier;

            bool inCombat = !this.body.outOfDanger || !this.body.outOfCombat;
            inCombat |= combatBufferTimer >= 0;
            bool isSprinting = body.isSprinting;

            if (inCombat != this.wasInCombat)
            {
                //standing still
                if (this.body.characterMotor.isGrounded && this.body.inputBank.moveVector == Vector3.zero)
                {
                    //from combat to rest
                    if (!inCombat)
                    {
                        BootlegPlayCrossfade(this.animator, "Body", "ToRestIdle", 0.05f);
                        SetCombatLerp(false);
                    }
                    //from rest to combat
                    else
                    {
                        BootlegPlayCrossfade(this.animator, "Transition", "ToCombat", 0.05f);
                        SetCombatLerp(true);
                    }
                }
                //sprinting
                else if (this.body.characterMotor.isGrounded && isSprinting)
                {
                    //always handled in the animator
                    //from combat to rest
                    //if (!inCombat) BootlegPlayCrossfade(this.animator, "Body", "SprintToRest2");
                }
                //not sprinting or not grounded
                else
                {
                    //from rest to combat
                    if (inCombat)
                    {
                        BootlegPlayCrossfade(this.animator, "Transition", "ToCombat", 0.1f);
                        SetCombatLerp(true);
                    }
                    //from combat to rest
                    else
                    {
                        BootlegPlayCrossfade(this.animator, "Transition", "ToRest", 0.1f);
                        SetCombatLerp(false);
                    }
                }
            }
            HACKStopDragTimer -= Time.deltaTime;
            if (animator.GetFloat("HACKStopDragging") < -0.1f)
            {
                HACKStopDragTimer = 1f;
            }

            float draggingParameter = animator.GetFloat("isDragging");
            if (HACKStopDragTimer < 0)
            {
                this.animator.SetLayerWeight(draggeLayerIndex, draggingParameter);
            } else
            {
                this.animator.SetLayerWeight(draggeLayerIndex, 0);
            }

            bool isDragging = draggingParameter > 0.5f  && HACKStopDragTimer < 0;

            //if (isDragging != wasDragging && !isDragging)
            //{
            //    combatBufferTimer = 1;
            //}

            if (wasSprinting != isSprinting)
            {
                //is dragging and stopped sprinting
                if (!isSprinting && isDragging && this.body.inputBank.moveVector != Vector3.zero)
                {
                    BootlegPlayCrossfade(this.animator, "Transition", "DragToRun");
                }
            }

            this.wasInCombat = inCombat;
            this.wasSprinting = isSprinting;
        }

        private void SetCombatLerp(bool toCombat)
        {
            combatLayerLerpTimer = toCombat? 0: 1;
            combatLerpDeltaMultiplier = toCombat ? 5 : -5;
        }

        private void UpdateLayer()
        {
            //PaladinPlugin.logger.LogWarning(currentCombatLayerWeight);
            animator.SetLayerWeight(combatLayerIndex, Mathf.Lerp(0, 1, combatLayerLerpTimer));
        }

        private static void BootlegPlayCrossfade(Animator animator, string layerName, string animationStateName, float crossfadeDuration = 0.2f)
        {
            animator.speed = 1f;
            animator.Update(0f);
            int layerIndex = animator.GetLayerIndex(layerName);
            animator.CrossFadeInFixedTime(animationStateName, crossfadeDuration, layerIndex);
        }

        private static void BootlegPlayAnimation(Animator animator, string layerName, string animationStateName)
        {
            EntityStates.EntityState.PlayAnimationOnAnimator(animator, layerName, animationStateName);
        }
    }
}