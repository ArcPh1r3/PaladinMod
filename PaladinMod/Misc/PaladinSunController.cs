using System.Collections.Generic;
using RoR2;
using RoR2.Audio;
using UnityEngine;
using UnityEngine.Networking;
using PaladinMod;
using RoR2.Projectile;
using System;

public class PaladinSunNetworkController : NetworkBehaviour {
    
	[ClientRpc]
	public void RpcPosition(GameObject parentTransform) {
		transform.SetParent(parentTransform.transform);
		transform.localPosition = new Vector3(0, 11, 0);
	}
}

[RequireComponent(typeof(TeamFilter))]
public class PaladinSunController : MonoBehaviour
{
	private TeamFilter teamFilter;

	private GameObject ownerObj;

	private CharacterBody ownerBody;

	public BuffDef buffDef;

	public GameObject buffApplyEffect;

	[SerializeField]
	public LoopSoundDef activeLoopDef;

	[SerializeField]
	public LoopSoundDef damageLoopDef;

	[SerializeField]
	public string stopSoundName;

	private float cycleInterval;

	private Run.FixedTimeStamp previousCycle = Run.FixedTimeStamp.negativeInfinity;

	private int cycleIndex;

	private List<HurtBox> cycleTargets = new List<HurtBox>();

	private BullseyeSearch bullseyeSearch = new BullseyeSearch();

	private bool isLocalPlayerDamaged;

	private bool crit;

	private void Awake()
	{
		teamFilter = GetComponent<TeamFilter>();
	}

	private void Start()
	{
		ownerObj = GetComponent<GenericOwnership>() ? GetComponent<GenericOwnership>().ownerObject : GetComponent<ProjectileController>().owner;

		ownerBody = ownerObj.GetComponent<CharacterBody>();
		cycleInterval = StaticValues.cruelSunCycleInterval/ownerBody.attackSpeed;
		crit = ownerBody.RollCrit();

		if ((bool)activeLoopDef)
		{
			Util.PlaySound(activeLoopDef.startSoundName, base.gameObject);
		}
	}

	private void OnDisable()
    {
		OnDestroy();
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
		//Fix the damn loop sound
		AkSoundEngine.StopPlayingID(3203163036);
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
		float num = Mathf.Clamp01(previousCycle.timeSince / cycleInterval);
		int num2 = ((num == 1f) ? cycleTargets.Count : Mathf.FloorToInt((float)cycleTargets.Count * num));
		Vector3 position = base.transform.position;
		while (cycleIndex < num2)
		{
			HurtBox hurtBox = cycleTargets[cycleIndex];
			if ((bool)hurtBox && (bool)hurtBox.healthComponent)
			{
				CharacterBody body = hurtBox.healthComponent.body;
				//Only perform extra logic IF ALL ARE TRUE:
				//ownerBody still exists (avoids NRE)
				//The target is an enemy OR The target is an ally and has less than 3 stacks of overheat (capping ally overheat at 3)
				//The target is NOT immune to overheat, OR they are not a player (gets around Grandparent immunity)
				//Known possible issue (untested): might still affect enemies who have Ben's Raincoat because of this logic

				if(ownerBody) {

					bool isEnemy = (body.teamComponent.teamIndex != ownerBody.teamComponent.teamIndex);
					bool affectPlayer = !isEnemy && StaticValues.cruelSunAllyDamageMultiplier > 0 && body.GetBuffCount(RoR2Content.Buffs.Overheat) < StaticValues.cruelSunMaximumAllyStacks;
					bool overrideEnemyImmune = ((body.bodyFlags & CharacterBody.BodyFlags.OverheatImmune) == 0 || body.teamComponent.teamIndex != TeamIndex.Player);
                    if ((isEnemy || affectPlayer) && overrideEnemyImmune) {

						Vector3 corePosition = body.corePosition;
						Ray ray = new Ray(position, corePosition - position);
						if (!Physics.Linecast(position, corePosition, out var hitInfo, LayerIndex.world.mask, QueryTriggerInteraction.Ignore))
						{
							//Grandparent's Overheat debuff duration gets longer the closer you are to it to discourage approaching, but this mechanic seems unneccessary here.
							//float distanceFromSun = Mathf.Max(1f, hitInfo.distance);
							//body.AddTimedBuff(buffDef, StaticValues.cruelSunOverheatDuration / distanceFromSun);
							body.AddTimedBuff(buffDef, StaticValues.cruelSunOverheatDuration);
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

							int overheatCount = body.GetBuffCount(buffDef);
							if (overheatCount >= StaticValues.cruelSunMinimumStacksBeforeApplyingBurns)
							{
								InflictDotInfo dotInfo = default(InflictDotInfo);
								dotInfo.dotIndex = DotController.DotIndex.Burn;
								dotInfo.attackerObject = ownerObj;
								dotInfo.victimObject = body.gameObject;
								if ((bool)ownerBody && (bool)ownerBody.inventory)
								{
									TeamDef teamDef = TeamCatalog.GetTeamDef(ownerBody.teamComponent.teamIndex);
									float ffScale = 1f;
									if (teamDef != null && teamDef.friendlyFireScaling > 0f) { 
										ffScale *= teamDef.friendlyFireScaling; 
									}
									float critScale = isEnemy && crit ? 2 : 1; 
									if (body.teamComponent.teamIndex == ownerBody.teamComponent.teamIndex & body != ownerBody){ ffScale *= StaticValues.cruelSunAllyDamageMultiplier; }
									dotInfo.totalDamage = StaticValues.cruelSunBurnDamageCoefficient * ownerBody.damage * (float)overheatCount * ffScale * critScale;
									dotInfo.damageMultiplier = 1f * ffScale;
									StrengthenBurnUtils.CheckDotForUpgrade(ownerBody.inventory, ref dotInfo);
								}
								if (dotInfo.totalDamage > 0) DotController.InflictDot(ref dotInfo);
							}
						}
					}
				}

			}
			cycleIndex++;
		}
		if (previousCycle.timeSince >= cycleInterval)
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
		bullseyeSearch.maxDistanceFilter = StaticValues.cruelSunAoE;
		bullseyeSearch.filterByDistinctEntity = true;
		bullseyeSearch.sortMode = BullseyeSearch.SortMode.Distance;
		bullseyeSearch.viewer = null;
		bullseyeSearch.RefreshCandidates();
		dest.AddRange(bullseyeSearch.GetResults());
	}
}
