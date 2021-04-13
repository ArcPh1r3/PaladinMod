using EntityStates;
using RoR2;
using UnityEngine;
using System;
using PaladinMod.Misc;
using RoR2.Projectile;
using UnityEngine.Networking;

namespace PaladinMod.States
{
    public class CurseSlash : Slash
    {
        public override void OnEnter()
        {
            base.OnEnter();
            this.attack.damageType = DamageType.BlightOnHit;
        }

        protected override void FireSwordBeam()
        {
        }

        protected override void OnHitAuthority()
        {
            base.OnHitAuthority();
        }
    }
}