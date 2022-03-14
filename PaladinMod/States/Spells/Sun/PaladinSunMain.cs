using RoR2;

namespace PaladinMod.States.Sun
{
	public class PaladinSunMain : PaladinSunBase
	{
		private GenericOwnership ownership;

		protected override bool shouldEnableSunController => true;

		protected override float desiredVfxScale => 1f;

		public override void OnEnter()
		{
			base.OnEnter();
			ownership = GetComponent<GenericOwnership>();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.isAuthority && !ownership.ownerObject)
			{
				outer.SetNextState(new PaladinSunDeath());
			}
		}
	}
}