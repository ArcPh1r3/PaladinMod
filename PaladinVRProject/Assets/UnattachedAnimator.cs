using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnattachedAnimator : MonoBehaviour
{

    [SerializeField]
    private Animator animator;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            animator.SetTrigger("FastSpellTrigger");
        }
    }
}
