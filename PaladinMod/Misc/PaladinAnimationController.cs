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

        private float currentCombatLayerWeight = 1;
        private float targetCombatLayerWeight = 1;

        private float combatBufferTimer = -1f;

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

        private void UpdateLayer()
        {
            currentCombatLayerWeight = Mathf.Lerp(currentCombatLayerWeight, targetCombatLayerWeight, 0.1f);
            animator.SetLayerWeight(combatLayerIndex, currentCombatLayerWeight);
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

            bool inCombat = !this.body.outOfDanger || !this.body.outOfCombat;
            inCombat |= combatBufferTimer >= 0;
            bool isSprinting = body.isSprinting;

            targetCombatLayerWeight = inCombat ? 1 : 0;

            if (inCombat != this.wasInCombat)
            {
                //standing still
                if (this.body.characterMotor.isGrounded && this.body.inputBank.moveVector == Vector3.zero)
                {
                    //from combat to rest
                    if (!inCombat)
                    {
                        BootlegPlayAnimation(this.animator, "Body", "ToRestIdle");
                    }
                    //from rest to combat
                    else
                    {
                        BootlegPlayCrossfade(this.animator, "Transition", "ToCombat", 0.05f);
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
                    }
                    //from combat to rest
                    else
                    {
                        BootlegPlayCrossfade(this.animator, "Transition", "ToRest", 0.1f);
                    }
                }
            }

            float draggingParameter = animator.GetFloat("isDragging");
            this.animator.SetLayerWeight(draggeLayerIndex, draggingParameter);

            bool isDragging = draggingParameter > 0.5f;

            if (isDragging != wasDragging && !isDragging)
            {
                combatBufferTimer = 1;
            }

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