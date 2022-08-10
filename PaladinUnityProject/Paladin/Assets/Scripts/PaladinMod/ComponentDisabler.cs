using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentDisabler : MonoBehaviour
{
    [SerializeField]
    private Behaviour[] MoreComponentsToDisable;

    // there is a very thin line between jank and geinus
    void OnDisable()
    {
        for (int i = 0; i < MoreComponentsToDisable.Length; i++) {

            Behaviour nig = MoreComponentsToDisable[i];

            nig.enabled = false;
        }
    }
}
