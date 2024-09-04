using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.States.Spell
{
    public class ScepterCastCruelSun : CastCruelSun
    {
        protected bool sunAim;
        protected bool sunLaunched;
        protected float aimTime;
        protected Vector3 sunTarget;

        protected override GameObject sunPrefab => Modules.Asset.paladinScepterSunPrefab;

        protected GameObject areaIndicatorInstance { get; set; }

        public override void OnEnter()
        {
            sunAim = false;
            sunLaunched = false;
            aimTime = 0f;
            sunTarget = Vector3.zero;
            areaIndicatorInstance = null;

            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            if (!sunLaunched && base.isAuthority && base.inputBank.skill4.down && base.fixedAge >= 0.5f)
            {
                //Only set up the aim indicator once.
                if (!sunAim) {
                    areaIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(EntityStates.Huntress.ArrowRain.areaIndicatorPrefab);
                    areaIndicatorInstance.transform.localScale = new Vector3(StaticValues.prideFlareExplosionRadius * 0.33f, StaticValues.prideFlareExplosionRadius * 0.33f, StaticValues.prideFlareExplosionRadius * 0.33f);
                    areaIndicatorInstance.GetComponentInChildren<MeshRenderer>().material = Modules.Asset.areaIndicatorMat;
                    sunAim = true;
                }
                base.StartAimMode(0.5f, true);
                //Extend the duration of the skill while the player is aiming, but only to an upper limit.
                base.duration += Time.deltaTime;
                aimTime += Time.deltaTime;
            }
            if (sunAim && (!base.inputBank.skill4.down || aimTime >= StaticValues.prideFlareAimTimeMax))
            {
                sunLaunched = true;
                sunAim = false;
                this.outer.SetNextStateToMain();
            }
            base.FixedUpdate();
        }
        
        protected override void SetCancelSkillOverride(bool shouldOverride = true) {
            return;
        }

        public override void Update()
        {
            UpdateAreaIndicator();
            base.Update();
        }
        
        protected override void OnChanneledSpellExit()
        {
            if (areaIndicatorInstance)
            {
                Destroy(areaIndicatorInstance.gameObject);
                Fire();
                base.PlayAnimation("Gesture, Override", "ThrowSpell", "Spell.playbackRate", 0.5f);
            }
            else
            {
                if ((bool)Modules.Asset.paladinSunSpawnPrefab && NetworkServer.active)
                {
                    EffectManager.SimpleImpactEffect(Modules.Asset.paladinSunSpawnPrefab, sunInstance.transform.position, Vector3.up, transmit: true);
                }
                base.PlayAnimation("Gesture, Override", "CastSunEnd", "Spell.playbackRate", 0.8f);
            }

            if (NetworkServer.active && sunInstance)
            {
                this.sunInstance.GetComponent<GenericOwnership>().ownerObject = null;
                this.sunInstance = null;
            }
        }
 
        private void Fire()
        {
            if (base.isAuthority)
            {
                var tempPos = base.gameObject.transform.position + new Vector3(0, 10f, 0);
                if (sunInstance) tempPos = sunInstance.transform.position;

                FireProjectileInfo sunShot = default(FireProjectileInfo);
                sunShot.projectilePrefab = Modules.Projectiles.scepterCruelSun;
                sunShot.position = tempPos;
                sunShot.rotation = Util.QuaternionSafeLookRotation(sunTarget - tempPos);
                sunShot.owner = base.gameObject;
                sunShot.damage = damageStat;
                sunShot.force = StaticValues.prideFlareForce;
                sunShot.crit = RollCrit();

                ProjectileManager.instance.FireProjectile(sunShot);

                if ((bool)base.characterMotor)
                {
                    base.characterMotor.ApplyForce((sunTarget - tempPos) * -StaticValues.prideFlareSelfForce);
                }
            }
        }

        private void UpdateAreaIndicator()
        {
            if (areaIndicatorInstance)
            {
                Ray aimRay = GetAimRay();
                RaycastHit raycastHit;
                if (Physics.Raycast(aimRay, out raycastHit, StaticValues.prideFlareMaxIndicatorRange, LayerIndex.CommonMasks.bullet))
                {
                    sunTarget = areaIndicatorInstance.transform.position = raycastHit.point;
                    
                }
                else
                {
                    sunTarget = areaIndicatorInstance.transform.position = aimRay.GetPoint(StaticValues.prideFlareMaxIndicatorRange);
                }
            }
        }
    }
}