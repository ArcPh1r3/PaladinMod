﻿using UnityEngine;

namespace PaladinMod.Misc
{
    public class StupidFuckingBullshit : MonoBehaviour
    {
        public ParticleSystem faggot;

        private void Awake()
        {
            this.faggot = this.GetComponentInChildren<ParticleSystem>();
            this.faggot.transform.SetParent(null);
        }

        private void FixedUpdate()
        {
            this.faggot.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
        }
    }
}