using UnityEngine;

namespace PaladinMod.States.Dash
{
    public class GroundDash : AirDash
    {
        protected override Vector3 GetDashVector()
        {
            return ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
        }
    }
}
