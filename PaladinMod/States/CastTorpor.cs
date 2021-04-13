using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.States
{
    public class CastTorpor : BaseCastSpellState
    {
        public override void OnEnter()
        {
            this.baseDuration = 0.4f;
            this.muzzleflashEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab;
            this.projectilePrefab = Modules.Projectiles.torpor;

            if (NetworkServer.active)
            {
                var info = new DamageInfo
                {
                    attacker = base.gameObject,
                    inflictor = base.gameObject,
                    damage = 1,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.BypassArmor,
                    crit = false,
                    dotIndex = DotController.DotIndex.None,
                    force = Vector3.zero,
                    position = base.transform.position,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 0
                };

                IEnumerable<TeamComponent> recipients = TeamComponent.GetTeamMembers(base.teamComponent.teamIndex);
                foreach (TeamComponent teamComponent in recipients)
                {
                    float radiusSqr = StaticValues.torporRadius * StaticValues.torporRadius;
                    if ((teamComponent.transform.position - this.spellPosition).sqrMagnitude <= radiusSqr)
                    {
                        if (teamComponent.body)
                        {
                            HealthComponent healthComponent = teamComponent.body.healthComponent;
                            if (healthComponent)
                            {
                                healthComponent.TakeDamage(info);
                            }
                        }
                    }
                }
            }

            base.OnEnter();
        }
    }
}