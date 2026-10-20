using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PaladinMod.States.Spell
{
    public class CastSmiteAMotherfucker : BaseCastChanneledSpellStateTargeted
    {
        public override void OnEnter()
        {
            base.OnEnter();

            BlastAttack blast = new BlastAttack
            {
                baseDamage = 100000 * damageStat,
                radius = 1,
                position = trackingTarget.transform.position,
                attacker = gameObject,
                attackerFiltering = AttackerFiltering.AlwaysHit,
                teamIndex = teamComponent.teamIndex
            };
            blast.Fire();

            EffectData effect = new EffectData
            {
                origin = trackingTarget.healthComponent.transform.position
            };
            EffectManager.SpawnEffect(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC2/meridian/DisableSkillsLightning/FalseSonLightningStrikeImpactEffect.prefab").WaitForCompletion(), effect, false);
            EffectManager.SpawnEffect(Addressables.LoadAssetAsync<GameObject>("RoR2/DLC2/meridian/DisableSkillsLightning/LightningStrikeImpactEffect.prefab").WaitForCompletion(), effect, false);
            EffectManager.SpawnEffect(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Lightning/LightningStrikeImpact.prefab").WaitForCompletion(), effect, false);
        }
    }
}