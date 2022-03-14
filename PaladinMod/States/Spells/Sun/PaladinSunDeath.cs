using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.States.Sun
{
	public class PaladinSunDeath : PaladinSunBase
	{
		public static float baseDuration;

		private float duration;

		protected override float desiredVfxScale => 1f - Mathf.Clamp01(base.age / duration);

		public override void OnEnter()
		{
			base.OnEnter();
			duration = baseDuration;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (NetworkServer.active && base.fixedAge >= duration)
			{
				NetworkServer.Destroy(base.gameObject);
			}
		}
	}
}
