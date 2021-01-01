using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.Misc
{
    [RequireComponent(typeof(TeamFilter))]
    public class PaladinHealController : NetworkBehaviour
    {
        [SyncVar]
        [Tooltip("The area of effect.")]
        public float radius;
        [Tooltip("Heal amount (based on percentage)")]
        public float healAmount;
        [Tooltip("Heal amount (based on percentage)")]
        public float barrierAmount;
        private TeamFilter teamFilter;

        private void Awake()
        {
            this.teamFilter = base.GetComponent<TeamFilter>();
        }

        private void Start()
        {
            float radiusSqr = this.radius * this.radius;
            this.HealTeam(TeamComponent.GetTeamMembers(this.teamFilter.teamIndex), radiusSqr, base.transform.position);
        }

        private void HealTeam(IEnumerable<TeamComponent> recipients, float radiusSqr, Vector3 currentPosition)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            foreach (TeamComponent teamComponent in recipients)
            {
                if ((teamComponent.transform.position - currentPosition).sqrMagnitude <= radiusSqr)
                {
                    CharacterBody charBody = teamComponent.body;
                    if (charBody)
                    {
                        if (healAmount != 0 && charBody.healthComponent)
                        {
                            charBody.healthComponent.HealFraction(this.healAmount, default(ProcChainMask));
                            charBody.healthComponent.AddBarrier(this.barrierAmount * charBody.healthComponent.fullBarrier);
                        }
                    }
                }
            }
        }

        public override bool OnSerialize(NetworkWriter writer, bool forceAll)
        {
            if (forceAll)
            {
                writer.Write(this.radius);
                return true;
            }

            bool flag = false;

            if ((base.syncVarDirtyBits & 1U) != 0U)
            {
                if (!flag)
                {
                    writer.WritePackedUInt32(base.syncVarDirtyBits);
                    flag = true;
                }

                writer.Write(this.radius);
            }

            if (!flag)
            {
                writer.WritePackedUInt32(base.syncVarDirtyBits);
            }
            return flag;
        }

        public override void OnDeserialize(NetworkReader reader, bool initialState)
        {
            if (initialState)
            {
                this.radius = reader.ReadSingle();
                return;
            }

            int num = (int)reader.ReadPackedUInt32();
            if ((num & 1) != 0)
            {
                this.radius = reader.ReadSingle();
            }
        }
    }
}
