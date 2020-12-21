using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.States
{
    public class SpinSlashEntry : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();

            if (base.isAuthority)
            {
                EntityState nextState = new AirSlam();
                if (base.characterMotor.isGrounded) nextState = new GroundSweep();
                
                this.outer.SetNextState(nextState);
                return;
            }
        }
    }
}
