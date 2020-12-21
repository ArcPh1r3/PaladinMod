using EntityStates;
using UnityEngine;

namespace PaladinMod.States.Quickstep
{
    public class QuickstepEntry : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            bool flag = false;

            if (base.inputBank && base.isAuthority)
            {
                Vector3 normalized = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
                Vector3 forward = base.characterDirection.forward;
                Vector3 rhs = Vector3.Cross(Vector3.up, forward);

                float num = Vector3.Dot(normalized, forward);
                float num2 = Vector3.Dot(normalized, rhs);

                if (base.characterDirection)
                {
                    base.characterDirection.moveVector = base.inputBank.aimDirection;
                }

                if (Mathf.Abs(num2) > Mathf.Abs(num))
                {
                    if (num2 <= 0f)
                    {
                        flag = true;
                        this.outer.SetNextState(new QuickstepLeft());
                    }
                    else
                    {
                        flag = true;
                        this.outer.SetNextState(new QuickstepRight());
                    }
                }
                else if (num <= 0f)
                {
                    flag = true;
                    this.outer.SetNextState(new QuickstepBack());
                }
                else
                {
                    flag = true;
                    this.outer.SetNextState(new QuickstepForward());
                }
            }

            if (!flag)
            {
                this.outer.SetNextState(new QuickstepForward());
            }
        }
    }
}
