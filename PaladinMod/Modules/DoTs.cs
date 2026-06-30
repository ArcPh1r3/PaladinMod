using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;

namespace PaladinMod.Modules
{
    public class DoTs
    {
        public static DotController.DotIndex FuckingCruelSunBurn;

        public static void Init()
        {
            FuckingCruelSunBurn = R2API.DotAPI.RegisterDotDef(
                interval: StaticValues.cruelSunBurnDotInterval, 
                damageCoefficient: StaticValues.cruelSunBurnDotInterval, 
                colorIndex: DamageColorIndex.Item, 
                associatedBuff: Addressables.LoadAsset<BuffDef>("RoR2/Base/Common/bdOnFire.asset").WaitForCompletion(),
                customDotBehaviour: null,
                customDotVisual: null,
                customDotDamageEvaluation: null);
        }
    }
}
