using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PaladinMod.Modules
{
    public class DoTs
    {
        public static DotController.DotIndex FuckingCruelSunBurn;
        public static DotController.DotIndex FuckingCruelSunBurnStrengthened;

        public static void Init()
        {
            FuckingCruelSunBurn = R2API.DotAPI.RegisterDotDef(
                interval: StaticValues.cruelSunBurnDotInterval, 
                damageCoefficient: StaticValues.cruelSunBurnDotInterval / StaticValues.cruelSunBurnStackDuration, //set the damage to the interval, so the total damage is an easy 1, and we decide the total damage based on the damage multiplier
                colorIndex: DamageColorIndex.Item, 
                associatedBuff: Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdOnFire.asset").WaitForCompletion(),
                customDotBehaviour: null,
                customDotVisual: null,
                customDotDamageEvaluation: FuckingCruelSunDamageEvaluationDelegate);

            FuckingCruelSunBurnStrengthened = R2API.DotAPI.RegisterDotDef(
                interval: StaticValues.cruelSunBurnDotInterval,
                damageCoefficient: StaticValues.cruelSunBurnDotInterval / StaticValues.cruelSunBurnStackDuration,
                colorIndex: DamageColorIndex.Item,
                associatedBuff: Addressables.LoadAssetAsync<BuffDef>("RoR2/DLC1/StrengthenBurn/bdStrongerBurn.asset").WaitForCompletion(),
                customDotBehaviour: null,
                customDotVisual: null,
                customDotDamageEvaluation: FuckingCruelSunDamageEvaluationDelegate);
        }

        private static R2API.DotAPI.CustomDotDamageEvaluation FuckingCruelSunDamageEvaluationDelegate = null;// new R2API.DotAPI.CustomDotDamageEvaluation(FuckingCruelSunDamageEvaluation);

        private static void FuckingCruelSunDamageEvaluation(DotController self, DotController.PendingDamage pendingDamage)
        {
            pendingDamage.damageType.damageSource = DamageSource.Special;
            pendingDamage.damageType |= DamageTypeExtended.FireNoIgnite;

            //do I have to do this
            DamageInfo damageInfo = new DamageInfo();
            damageInfo.attacker = pendingDamage.attackerObject;
            damageInfo.crit = false;
            damageInfo.damage = pendingDamage.totalDamage;//all that FUCKING work to figure out how to get damage intervals how I want via multiplier bullshit and I can just fucking do this now god damn it
            damageInfo.force = Vector3.zero;
            damageInfo.inflictor = self.gameObject;
            damageInfo.position = self.victimBody.corePosition;
            damageInfo.procChainMask = pendingDamage.procChainMask;
            damageInfo.procCoefficient = 0.2f;
            damageInfo.damageColorIndex = DotController.GetDotDef(pendingDamage.dotIndex).damageColorIndex;
            damageInfo.damageType = (pendingDamage.damageType | DamageType.DoT);
            damageInfo.dotIndex = pendingDamage.dotIndex;
            damageInfo.inflictedHurtbox = pendingDamage.hitHurtBox;
            self.victimHealthComponent.TakeDamage(damageInfo);
        }
    }
}
