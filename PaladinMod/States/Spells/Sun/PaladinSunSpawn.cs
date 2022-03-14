using UnityEngine;

namespace PaladinMod.States.Sun
{
	public class PaladinSunSpawn : PaladinSunBase
	{
		public static float baseDuration;

		private float duration;

		protected override float desiredVfxScale => Mathf.Clamp01(base.age / duration);

		public override void OnEnter()
		{
			base.OnEnter();
			duration = baseDuration;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.isAuthority && base.fixedAge >= duration)
			{
				outer.SetNextState(new PaladinSunMain());
			}
		}
	}
}