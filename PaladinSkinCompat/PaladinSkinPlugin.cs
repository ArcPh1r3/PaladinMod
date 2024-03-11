using BepInEx;
using PaladinMod.Modules;
using RoR2;
using System;
using System.Linq;
using UnityEngine;

namespace PaladinSkinCompat {
    [BepInDependency("com.rob.Paladin", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("com.paladin_alliance.FunnySkinPlugin", "FunnySkinPlugin", "1.0.0")]
    public class PaladinSkinPlugin : BaseUnityPlugin {
        void Awake() {

            Effects.OnRegisterEffects += RegisterMySkinEffects;
        }

        private void RegisterMySkinEffects() {

            Effects.PaladinSkinInfo berserkSkininfo = new Effects.PaladinSkinInfo {
                skinNameToken = "KRONONCONSPIRATOR_SKIN_PALADINBLACK_NAME",
                passiveEffectType = PassiveEffect.SwordActiveEffectRed,
                swingSoundString = "",
                isWeaponBlunt = false,
                //hitEffect = Assets.hitFXRed,
                hitEffectType = HitEffect.Red,
                //swingEffect = Assets.swordSwingRed,
                swingEffectType = SwingEffect.Red,
                //spinSlashEffect = Assets.spinningSlashFXRed,
                spinSlashEffectType = SpinSlashEffect.Red,
                //empoweredSpinSlashEffect = Assets.spinningSlashEmpoweredFXRed,
                empoweredSpinSlashEffectType = EmpwoeredSpinSlashEffect.Red,
                eyeTrailColor = Color.red,
                swordBeamProjectileColor = Color.red,
                CSSEffect = PaladinCSSEffect.RED,
            };

            Effects.AddSkinInfo(berserkSkininfo);
        }
    }
}
