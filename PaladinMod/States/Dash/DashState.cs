using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.States.Dash
{
    public class DashState : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();

            if (base.isAuthority)
            {
                EntityState nextState = (base.characterMotor && base.characterMotor.isGrounded) ? new GroundDash() : new AirDash();
                this.outer.SetNextState(nextState);
                return;
            }
        }
    }
}
