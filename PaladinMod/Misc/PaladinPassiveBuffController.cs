using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.Misc
{
    [RequireComponent(typeof(TeamFilter))]
    public class PaladinPassiveBuffController : NetworkBehaviour
    {
        [SyncVar]
        [Tooltip("The area of effect.")]
        public float radius = 48;
        [Tooltip("The buff type to grant")]
        public BuffDef buffDef;
        [Tooltip("The buff duration")]
        public float buffDuration = 2f;
        [Tooltip("If set, applies to all teams BUT the one selected.")]
        public bool invertTeamFilter = false;

        private TeamFilter teamFilter;
        private TeamComponent teamComponent;

        private void Start()
        {
            this.teamFilter = this.GetComponent<TeamFilter>();
            this.teamComponent = this.GetComponent<TeamComponent>();

            if (!this.teamFilter)
            {
                this.teamFilter = this.gameObject.AddComponent<TeamFilter>();
            }

            this.InvokeRepeating("SetTeamFilter", 0.5f, 0.5f);
        }

        private void SetTeamFilter()
        {
            if (this.teamComponent && this.teamFilter) this.teamFilter.teamIndex = this.teamComponent.teamIndex;
        }

        public void TryBuff()
        {
            if (NetworkServer.active)
            {
                float radiusSqr = this.radius * this.radius;
                Vector3 pos = base.transform.position;

                if (this.invertTeamFilter)
                {
                    for (TeamIndex teamIndex = TeamIndex.Neutral; teamIndex < TeamIndex.Count; teamIndex += 1)
                    {
                        if (teamIndex != this.teamFilter.teamIndex)
                        {
                            this.BuffTeam(TeamComponent.GetTeamMembers(teamIndex), radiusSqr, pos);
                        }
                    }
                    return;
                }

                this.BuffTeam(TeamComponent.GetTeamMembers(this.teamFilter.teamIndex), radiusSqr, pos);
            }
        }

        private void BuffTeam(IEnumerable<TeamComponent> recipients, float radiusSqr, Vector3 currentPosition)
        {
            if (!NetworkServer.active) return;

            foreach (TeamComponent teamComponent in recipients)
            {
                if ((teamComponent.transform.position - currentPosition).sqrMagnitude <= radiusSqr)
                {
                    CharacterBody charBody = teamComponent.body;
                    if (charBody)
                    {
                        if (this.buffDef) charBody.AddTimedBuff(this.buffDef, this.buffDuration);
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