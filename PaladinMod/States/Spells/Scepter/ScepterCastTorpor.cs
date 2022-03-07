using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.States.Spell
{
    public class ScepterCastTorpor : BaseCastChanneledSpellState
    {
        public override void OnEnter()
        {
            this.baseDuration = 1.2f;
            this.muzzleflashEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXArchWispCannonImpact");
            this.projectilePrefab = Modules.Projectiles.scepterTorpor;
            this.castSoundString = Modules.Sounds.CastTorpor;

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

                IEnumerable<TeamComponent> recipients = TeamComponent.GetTeamMembers(TeamIndex.Monster);
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