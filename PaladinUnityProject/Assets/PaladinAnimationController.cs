using UnityEngine;

public class PaladinAnimationController : MonoBehaviour {

    [SerializeField]
    private Animator[] animators;

    private bool wasInCombat;
    private bool wasSprinting;
    private bool wasDragging;

    private int combatLayerIndex;
    private int draggeLayerIndex;

    private float currentCombatLayerWeight = 1;
    private float targetCombatLayerWeight = 1;

    private float combatBufferTimer = -1f;

    void Start() {
        combatLayerIndex = animators[0].GetLayerIndex("Body, Combat");
        draggeLayerIndex = animators[0].GetLayerIndex("Body, Dragging");
    }

    void FixedUpdate() {
        UpdateAnimation();
        UpdateLayer();
    }

    void Update() {
    }

    private void UpdateLayer() {

        for (int i = 0; i < animators.Length; i++) {
            Animator animator = animators[i];

            currentCombatLayerWeight = Mathf.Lerp(currentCombatLayerWeight, targetCombatLayerWeight, 0.1f);
            animator.SetLayerWeight(combatLayerIndex, currentCombatLayerWeight);
        }
    }

    private void UpdateAnimation() {

        if(combatBufferTimer >= 0) {
            combatBufferTimer -= Time.fixedDeltaTime;
        }
        for (int i = 0; i < animators.Length; i++) {
            Animator animator = animators[i];

            // combat animation shit
            bool inCombat = animator.GetBool("inCombat") || combatBufferTimer > 0;
            bool isSprinting = animator.GetBool("isSprinting");

            bool isGrounded = animator.GetBool("isGrounded");
            bool isMoving = animator.GetBool("isMoving");

            targetCombatLayerWeight = inCombat ? 1 : 0;

            if (inCombat != this.wasInCombat) {
                //standing still
                if (isGrounded && !isMoving) {
                    //from combat to rest
                    if (!inCombat) {
                        BootlegPlayAnimation(animator, "Body", "ToRestIdle");
                    }
                    //from rest to combat
                    else {
                        BootlegPlayCrossfade(animator, "Transition", "ToRest");
                    }
                }
                //sprinting
                else if (isGrounded && isSprinting) {
                    //always handled in the animator
                    //from combat to rest
                    //if (!inCombat) BootlegPlayCrossfade(this.animator, "Body", "SprintToRest2");
                }
                //not sprinting or not grounded
                else {
                    //from rest to combat
                    if (inCombat) {
                        BootlegPlayCrossfade(animator, "Transition", "ToCombat", 0.1f);
                    }
                    //from combat to rest
                    else {
                        BootlegPlayCrossfade(animator, "Transition", "ToRest", 0.1f);
                    }
                }
            }

            float draggingParameter = animator.GetFloat("isDragging");
            animator.SetLayerWeight(draggeLayerIndex, draggingParameter);

            bool isDragging = draggingParameter > 0.5f;

            if (isDragging != wasDragging && !isDragging) {
                combatBufferTimer = 1;
            }

            if (wasSprinting != isSprinting) {
                //is dragging and stopped sprinting
                if (!isSprinting && isDragging && isMoving) {
                    BootlegPlayCrossfade(animator, "Transition", "DragToRun");
                }
            }

            this.wasInCombat = inCombat;
            this.wasSprinting = isSprinting;
            this.wasDragging = isDragging;
        }
        //if (inCombat) animators[i].SetLayerWeight(animators[i].GetLayerIndex("Body, Combat"), 1f);
        //else animators[i].SetLayerWeight(animators[i].GetLayerIndex("Body, Combat"), 0f);

        //if (inCombat != this.wasInCombat) {
        //    if (animators[i].GetBool("isGrounded") && !animators[i].GetBool("isMoving")) {
        //        if (!inCombat) animators[i].Play("ToRestIdle", animators[i].GetLayerIndex("Body"));
        //    } else if (animators[i].GetBool("isGrounded") && animators[i].GetBool("isSprinting")) {
        //        //if (!inCombat) animators[i].Play("SprintToRest2", animators[i].GetLayerIndex("Body"));
        //    } else {
        //        if (inCombat) animators[i].Play("ToCombat", animators[i].GetLayerIndex("Transition"));
        //        else animators[i].Play("ToRest", animators[i].GetLayerIndex("Transition"));
        //    }
        //}
    }

    private static void BootlegPlayCrossfade(Animator animator, string layerName, string animationStateName, float crossfadeDuration = 0.2f) {
        animator.speed = 1f;
        animator.Update(0f);
        int layerIndex = animator.GetLayerIndex(layerName);
        animator.CrossFadeInFixedTime(animationStateName, crossfadeDuration, layerIndex);
    }

    private static void BootlegPlayAnimation(Animator animator, string layerName, string animationStateName) {

        int layerIndex = animator.GetLayerIndex(layerName);
        animator.Play(animationStateName, layerIndex);
    }
}
