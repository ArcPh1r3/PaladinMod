using System.Collections.Generic;
using RoR2;
using RoR2.Audio;
using UnityEngine;
using UnityEngine.Networking;
using PaladinMod;

[RequireComponent(typeof(TeamFilter))]
[RequireComponent(typeof(GenericOwnership))]
public class PaladinSunController : MonoBehaviour
{
	private TeamFilter teamFilter;

	private GenericOwnership ownership;

	public BuffDef buffDef;

	public GameObject buffApplyEffect;

	[SerializeField]
	public LoopSoundDef activeLoopDef;

	[SerializeField]
	public LoopSoundDef damageLoopDef;

	[SerializeField]
	public string stopSoundName = "Play_grandParent_attack3_sun_destroy";

	private Run.FixedTimeStamp previousCycle = Run.FixedTimeStamp.negativeInfinity;

	private int cycleIndex;

	private List<HurtBox> cycleTargets = new List<HurtBox>();

	private BullseyeSearch bullseyeSearch = new BullseyeSearch();

	private bool isLocalPlayerDamaged;

	private void Awake()
	{
		teamFilter = GetComponent<TeamFilter>();
		ownership = GetComponent<GenericOwnership>();
	}

	private void Start()
	{
		if ((bool)activeLoopDef)
		{
			Util.PlaySound(activeLoopDef.startSoundName, base.gameObject);
		}
	}

	private void OnDestroy()
	{
		if ((bool)activeLoopDef)
		{
			Util.PlaySound(activeLoopDef.stopSoundName, base.gameObject);
		}
		if ((bool)damageLoopDef)
		{
			Util.PlaySound(damageLoopDef.stopSoundName, base.gameObject);
		}
		if (stopSoundName != null)
		{
			Util.PlaySound(stopSoundName, base.gameObject);
		}
	}

	private void FixedUpdate()
	{
		if (NetworkServer.active)
		{
			ServerFixedUpdate();
		}
		if (!damageLoopDef)
		{
			return;
		}
		bool flag = isLocalPlayerDamaged;
		isLocalPlayerDamaged = false;
		foreach (HurtBox cycleTarget in cycleTargets)
		{
			CharacterBody characterBody = null;
			if ((bool)cycleTarget && (bool)cycleTarget.healthComponent)
			{
				characterBody = cycleTarget.healthComponent.body;
			}
			if ((bool)characterBody && (characterBody.bodyFlags & CharacterBody.BodyFlags.OverheatImmune) != 0 && characterBody.hasEffectiveAuthority)
			{
				Vector3 position = base.transform.position;
				Vector3 corePosition = characterBody.corePosition;
				if (!Physics.Linecast(position, corePosition, out var _, LayerIndex.world.mask, QueryTriggerInteraction.Ignore))
				{
					isLocalPlayerDamaged = true;
				}
			}
		}
		if (isLocalPlayerDamaged && !flag)
		{
			Util.PlaySound(damageLoopDef.startSoundName, base.gameObject);
		}
		else if (!isLocalPlayerDamaged && flag)
		{
			Util.PlaySound(damageLoopDef.stopSoundName, base.gameObject);
		}
	}

	private void ServerFixedUpdate()
	{
		float num = Mathf.Clamp01(previousCycle.timeSince / StaticValues.cruelSunCycleInterval);
		int num2 = ((num == 1f) ? cycleTargets.Count : Mathf.FloorToInt((float)cycleTargets.Count * num));
		Vector3 position = base.transform.position;
		while (cycleIndex < num2)
		{
			HurtBox hurtBox = cycleTargets[cycleIndex];
			if ((bool)hurtBox && (bool)hurtBox.healthComponent)
			{
				CharacterBody body = hurtBox.healthComponent.body;
				CharacterBody ownerBody = ownership?.ownerObject?.GetComponent<CharacterBody>();
				//Debug.Log(body.teamComponent.teamIndex.ToString() + " vs " + ownerBody.teamComponent.teamIndex.ToString());
				if ( ( (body.teamComponent.teamIndex != ownerBody.teamComponent.teamIndex) || StaticValues.cruelSunAllyDamageMultiplier > 0 ) 
					& ( (body.bodyFlags & CharacterBody.BodyFlags.OverheatImmune) == 0 || body.teamComponent.teamIndex != TeamIndex.Player ) ){
					Vector3 corePosition = body.corePosition;
					Ray ray = new Ray(position, corePosition - position);
					if (!Physics.Linecast(position, corePosition, out var hitInfo, LayerIndex.world.mask, QueryTriggerInteraction.Ignore))
					{
						float distanceFromSun = Mathf.Max(1f, hitInfo.distance);
						body.AddTimedBuff(buffDef, StaticValues.cruelSunOverheatDuration / distanceFromSun);
						if ((bool)buffApplyEffect)
						{
							EffectData effectData = new EffectData
							{
								origin = corePosition,
								rotation = Util.QuaternionSafeLookRotation(-ray.direction),
								scale = body.bestFitRadius
							};
							effectData.SetHurtBoxReference(hurtBox);
							EffectManager.SpawnEffect(buffApplyEffect, effectData, transmit: true);
						}

						int burnCount = body.GetBuffCount(buffDef) - StaticValues.cruelSunMinimumStacksBeforeApplyingBurns;
						if (burnCount > 0)
						{
							InflictDotInfo dotInfo = default(InflictDotInfo);
							dotInfo.dotIndex = DotController.DotIndex.Burn;
							dotInfo.attackerObject = ownership.ownerObject;
							dotInfo.victimObject = body.gameObject;
							if ((bool)ownerBody && (bool)ownerBody.inventory)
							{
								TeamDef teamDef = TeamCatalog.GetTeamDef(ownerBody.teamComponent.teamIndex);
								float ffScale = 1f;
								if (teamDef != null && teamDef.friendlyFireScaling > 0f) { ffScale /= teamDef.friendlyFireScaling; }
								if (body.teamComponent.teamIndex == ownerBody.teamComponent.teamIndex){ ffScale *= StaticValues.cruelSunAllyDamageMultiplier; }

								dotInfo.totalDamage = 0.5f * ownerBody.damage * StaticValues.cruelSunBurnDuration * (float)burnCount * ffScale;
								dotInfo.damageMultiplier = 1f * ffScale;
								StrengthenBurnUtils.CheckDotForUpgrade(ownerBody.inventory, ref dotInfo);
							}
							if (dotInfo.totalDamage > 0) DotController.InflictDot(ref dotInfo);
						}
					}
				}
			}
			cycleIndex++;
		}
		if (previousCycle.timeSince >= StaticValues.cruelSunCycleInterval)
		{
			previousCycle = Run.FixedTimeStamp.now;
			cycleIndex = 0;
			cycleTargets.Clear();
			SearchForTargets(cycleTargets);
		}
	}

	private void SearchForTargets(List<HurtBox> dest)
	{
		bullseyeSearch.searchOrigin = base.transform.position;
		bullseyeSearch.minAngleFilter = 0f;
		bullseyeSearch.maxAngleFilter = 180f;
		bullseyeSearch.maxDistanceFilter = StaticValues.cruelSunRange;
		bullseyeSearch.filterByDistinctEntity = true;
		bullseyeSearch.sortMode = BullseyeSearch.SortMode.Distance;
		bullseyeSearch.viewer = null;
		bullseyeSearch.RefreshCandidates();
		dest.AddRange(bullseyeSearch.GetResults());
	}
}
