using RoR2;
using UnityEngine;

namespace PaladinMod.States.Spell
{
    public class ScepterCastCruelSun : CastCruelSun
    {
        protected bool sunAim = false;
        protected bool sunLaunched = false;
        protected float aimTime;
        protected Vector3 sunTarget;
        protected GameObject areaIndicatorInstance { get; set; }

        public override void OnEnter()
        {
            sunAim = false;
            sunLaunched = false;
            aimTime = 0f;
            sunTarget = Vector3.zero;

            if (EntityStates.Huntress.ArrowRain.areaIndicatorPrefab)
            {
                areaIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(EntityStates.Huntress.ArrowRain.areaIndicatorPrefab);
                areaIndicatorInstance.transform.localScale = new Vector3(StaticValues.cruelSunRange, StaticValues.cruelSunRange, StaticValues.cruelSunRange);
                areaIndicatorInstance.GetComponentInChildren<MeshRenderer>().material = Modules.Assets.areaIndicatorMat;
            }

            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            Debug.Log("ScepterCastCruelSun fixedupdate start");
            if (!sunLaunched && base.isAuthority && base.IsKeyDownAuthority() && base.fixedAge >= 0.2f)
            {
                Debug.Log("ScepterCastCruelSun fixedupdate AIMING");
                sunAim = true;
                base.StartAimMode(0.5f, true);
                //Extend the duration of the skill while the player is aiming, but only to an upper limit.
                base.duration += Time.fixedDeltaTime;
                aimTime += Time.fixedDeltaTime;
            }
            else if (sunAim && (!base.IsKeyDownAuthority() || aimTime >= StaticValues.prideFlareAimTimeMax))
            {
                Debug.Log("ScepterCastCruelSun fixedupdate FIRING");
                sunLaunched = true;
                sunAim = false;
                sunInstance.transform.parent = null;
                base.PlayAnimation("Gesture, Override", "ThrowSpell", "Spell.playbackRate", 0.5f);
                if (areaIndicatorInstance) Destroy(areaIndicatorInstance.gameObject);
            }
            if (sunLaunched)
            {
                Debug.Log("ScepterCastCruelSun fixedupdate FIRED");
                float maxDistanceToMoveThisFrame = StaticValues.prideFlareSpeed * Time.fixedDeltaTime;
                sunInstance.transform.position = Vector3.MoveTowards(sunInstance.transform.position, sunTarget, maxDistanceToMoveThisFrame);
                if (Vector3.Distance(sunInstance.transform.position, sunTarget) < 0.001f)
                {
                    this.outer.SetNextStateToMain();
                }
                //Extends the duration of the skill until it reaches its destination. Pretty insane, but fuck it, we're not using Scepter to be 'fair'.
                base.duration += Time.fixedDeltaTime;
            }
            base.FixedUpdate();
        }

        public void OnUpdate()
        {
            UpdateAreaIndicator();
        }

        public override void OnExit()
        {
            EffectData effectData = new EffectData();
            effectData.origin = base.sunInstance.transform.position;
            EffectManager.SpawnEffect(RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/GrandparentSpawnImpact"), effectData, false);
            EffectManager.SpawnEffect(RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/ClayBossDeath"), effectData, false);

            if (base.isAuthority)
            {
                BlastAttack blastAttack = new BlastAttack();
                blastAttack.attacker = base.gameObject;
                blastAttack.inflictor = base.gameObject;
                blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                blastAttack.position = base.sunInstance.transform.position;
                blastAttack.procCoefficient = StaticValues.prideFlareProcCoefficient;
                blastAttack.radius = StaticValues.prideFlareRadius;
                blastAttack.baseForce = StaticValues.prideFlareForce;
                blastAttack.bonusForce = Vector3.zero;
                blastAttack.baseDamage = StaticValues.prideFlareDamageCoefficient * this.damageStat;
                blastAttack.falloffModel = BlastAttack.FalloffModel.SweetSpot;
                blastAttack.damageColorIndex = DamageColorIndex.Item;
                blastAttack.attackerFiltering = AttackerFiltering.AlwaysHit;
                blastAttack.Fire();
            }

            base.OnExit();
        }

        private void UpdateAreaIndicator()
        {
            if (areaIndicatorInstance)
            {
                Ray aimRay = GetAimRay();
                RaycastHit raycastHit;
                if (Physics.Raycast(aimRay, out raycastHit, StaticValues.prideFlareMaxRange, LayerIndex.CommonMasks.bullet))
                {
                    areaIndicatorInstance.transform.position = raycastHit.point;
                    areaIndicatorInstance.transform.up = raycastHit.normal;
                }
                else
                {
                    areaIndicatorInstance.transform.position = aimRay.GetPoint(StaticValues.prideFlareMaxRange);
                    areaIndicatorInstance.transform.up = -aimRay.direction;
                }
                sunTarget = areaIndicatorInstance.transform.position;
            }
        }
    }
}