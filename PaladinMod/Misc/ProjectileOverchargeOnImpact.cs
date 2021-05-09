using UnityEngine;
using RoR2;
using RoR2.Projectile;
using System.Linq;

namespace PaladinMod.Misc
{
    [RequireComponent(typeof(ProjectileController))]
    public class ProjectileOverchargeOnImpact : MonoBehaviour, IProjectileImpactBehavior
    {
        private bool alive = true;
        private Rigidbody rigidbody;

        private void Awake()
        {
            this.rigidbody = base.GetComponent<Rigidbody>();
        }

        public void OnProjectileImpact(ProjectileImpactInfo impactInfo)
        {
            if (!this.alive)
            {
                return;
            }

            BullseyeSearch search = new BullseyeSearch
            {
                teamMaskFilter = TeamMask.all,
                filterByLoS = false,
                searchOrigin = this.transform.position,
                searchDirection = Random.onUnitSphere,
                sortMode = BullseyeSearch.SortMode.Distance,
                maxDistanceFilter = 24f,
                maxAngleFilter = 360f
            };

            search.RefreshCandidates();
            search.FilterOutGameObject(base.gameObject);

            HurtBox target = search.GetResults().FirstOrDefault<HurtBox>();
            if (target)
            {
                if (target.healthComponent && target.healthComponent.body)
                {
                    if (target.healthComponent.body.baseNameToken == "PALADIN_NAME")
                    {
                        PaladinSwordController swordController = target.healthComponent.body.GetComponent<PaladinSwordController>();
                        if (swordController)
                        {
                            swordController.ApplyLightningBuff();
                        }
                    }
                }
            }
        }
    }
}