using System.Collections.Generic;
using UnityEngine;

namespace PaladinMod.Modules
{
    public static class Effects
    {
        private static List<PaladinSkinInfo> skinList;

        public static PaladinSkinInfo[] skinInfos;

        public struct PaladinSkinInfo
        {
            public string skinName;

            public string passiveEffectName;

            public string swingSoundString;
            public bool isWeaponBlunt;

            public GameObject hitEffect;
            public GameObject swingEffect;
            public GameObject spinSlashEffect;
            public GameObject empoweredSpinSlashEffect;
        }

        public static void RegisterEffects()
        {
            skinList = new List<PaladinSkinInfo>();

            #region Skins
            skinList.Add(new PaladinSkinInfo
            {
                skinName = "PALADINBODY_DEFAULT_SKIN_NAME",
                passiveEffectName = "SwordActiveEffect",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Assets.hitFX,
                swingEffect = Assets.swordSwing,
                spinSlashEffect = Assets.spinningSlashFX,
                empoweredSpinSlashEffect = Assets.spinningSlashEmpoweredFX
            });

            skinList.Add(new PaladinSkinInfo
            {
                skinName = "PALADINBODY_LUNAR_SKIN_NAME",
                passiveEffectName = "SwordActiveEffect",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Assets.hitFX,
                swingEffect = Assets.swordSwing,
                spinSlashEffect = Assets.spinningSlashFX,
                empoweredSpinSlashEffect = Assets.spinningSlashEmpoweredFX
            });

            skinList.Add(new PaladinSkinInfo
            {
                skinName = "PALADINBODY_POISON_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectGreen",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Assets.hitFXGreen,
                swingEffect = Assets.swordSwingGreen,
                spinSlashEffect = Assets.spinningSlashFXGreen,
                empoweredSpinSlashEffect = Assets.spinningSlashEmpoweredFXGreen
            });

            skinList.Add(new PaladinSkinInfo
            {
                skinName = "PALADINBODY_CLAY_SKIN_NAME",
                passiveEffectName = "",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Assets.hitFXClay,
                swingEffect = Assets.swordSwingClay,
                spinSlashEffect = Assets.spinningSlashFXClay,
                empoweredSpinSlashEffect = Assets.spinningSlashEmpoweredFXClay
            });

            skinList.Add(new PaladinSkinInfo
            {
                skinName = "PALADINBODY_DRIP_SKIN_NAME",
                passiveEffectName = "SwordActiveEffect",
                swingSoundString = Sounds.SwingBlunt,
                isWeaponBlunt = true,
                hitEffect = Assets.hitFXBlunt,
                swingEffect = Assets.swordSwingBat,
                spinSlashEffect = Assets.spinningSlashFX,
                empoweredSpinSlashEffect = Assets.spinningSlashEmpoweredFX
            });

            skinList.Add(new PaladinSkinInfo
            {
                skinName = "PALADINBODY_MINECRAFT_SKIN_NAME",
                passiveEffectName = "SwordActiveEffect",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Assets.hitFX,
                swingEffect = Assets.swordSwing,
                spinSlashEffect = Assets.spinningSlashFX,
                empoweredSpinSlashEffect = Assets.spinningSlashEmpoweredFX
            });
            #endregion

            #region DarkSoulsSkins
            skinList.Add(new PaladinSkinInfo
            {
                skinName = "PALADINBODY_ABYSSWATCHER_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectFlame",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Assets.hitFXRed,
                swingEffect = Assets.swordSwingFlame,
                spinSlashEffect = Assets.spinningSlashFXFlame,
                empoweredSpinSlashEffect = Assets.spinningSlashEmpoweredFXFlame
            });
            skinList.Add(new PaladinSkinInfo
            {
                skinName = "PALADINBODY_ARTORIAS_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectPurple",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Assets.hitFXPurple,
                swingEffect = Assets.swordSwingPurple,
                spinSlashEffect = Assets.spinningSlashFXPurple,
                empoweredSpinSlashEffect = Assets.spinningSlashEmpoweredFXPurple
            });
            skinList.Add(new PaladinSkinInfo
            {
                skinName = "PALADINBODY_BLACKKNIGHT_SKIN_NAME",
                passiveEffectName = "",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Assets.hitFX,
                swingEffect = Assets.swordSwingWhite,
                spinSlashEffect = Assets.spinningSlashFX,
                empoweredSpinSlashEffect = Assets.spinningSlashEmpoweredFX
            });
            skinList.Add(new PaladinSkinInfo
            {
                skinName = "PALADINBODY_FARAAM_SKIN_NAME",
                passiveEffectName = "SwordActiveEffect",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Assets.hitFX,
                swingEffect = Assets.swordSwing,
                spinSlashEffect = Assets.spinningSlashFX,
                empoweredSpinSlashEffect = Assets.spinningSlashEmpoweredFX
            });
            skinList.Add(new PaladinSkinInfo
            {
                skinName = "PALADINBODY_GIANTDAD_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectSun",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Assets.hitFXYellow,
                swingEffect = Assets.swordSwingYellow,
                spinSlashEffect = Assets.spinningSlashFXYellow,
                empoweredSpinSlashEffect = Assets.spinningSlashEmpoweredFXYellow
            });
            skinList.Add(new PaladinSkinInfo
            {
                skinName = "PALADINBODY_GWYN_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectFlame",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Assets.hitFXRed,
                swingEffect = Assets.swordSwingFlame,
                spinSlashEffect = Assets.spinningSlashFXFlame,
                empoweredSpinSlashEffect = Assets.spinningSlashEmpoweredFXFlame
            });
            skinList.Add(new PaladinSkinInfo
            {
                skinName = "PALADINBODY_HAVEL_SKIN_NAME",
                passiveEffectName = "",
                swingSoundString = Sounds.SwingBlunt,
                isWeaponBlunt = true,
                hitEffect = Assets.hitFXBlunt,
                swingEffect = Assets.swordSwingWhite,
                spinSlashEffect = Assets.spinningSlashFX,
                empoweredSpinSlashEffect = Assets.spinningSlashEmpoweredFX
            });
            skinList.Add(new PaladinSkinInfo
            {
                skinName = "PALADINBODY_ORNSTEIN_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectSun",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Assets.hitFXYellow,
                swingEffect = Assets.swordSwingYellow,
                spinSlashEffect = Assets.spinningSlashFXYellow,
                empoweredSpinSlashEffect = Assets.spinningSlashEmpoweredFXYellow
            });
            skinList.Add(new PaladinSkinInfo
            {
                skinName = "PALADINBODY_PURSUER_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectRed",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Assets.hitFXRed,
                swingEffect = Assets.swordSwingRed,
                spinSlashEffect = Assets.spinningSlashFXRed,
                empoweredSpinSlashEffect = Assets.spinningSlashEmpoweredFXRed
            });
            skinList.Add(new PaladinSkinInfo
            {
                skinName = "PALADINBODY_RINGEDKNIGHT_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectFlame",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Assets.hitFXRed,
                swingEffect = Assets.swordSwingFlame,
                spinSlashEffect = Assets.spinningSlashFXFlame,
                empoweredSpinSlashEffect = Assets.spinningSlashEmpoweredFXFlame
            });
            skinList.Add(new PaladinSkinInfo
            {
                skinName = "PALADINBODY_SOLAIRE_SKIN_NAME",
                passiveEffectName = "SwordActiveEffectSun",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Assets.hitFXYellow,
                swingEffect = Assets.swordSwingYellow,
                spinSlashEffect = Assets.spinningSlashFXYellow,
                empoweredSpinSlashEffect = Assets.spinningSlashEmpoweredFXYellow
            });
            skinList.Add(new PaladinSkinInfo
            {
                skinName = "PALADINBODY_DARKWRAITH_SKIN_NAME",
                passiveEffectName = "",
                swingSoundString = Sounds.Swing,
                isWeaponBlunt = false,
                hitEffect = Assets.hitFXBlack,
                swingEffect = Assets.swordSwingBlack,
                spinSlashEffect = Assets.spinningSlashFXBlack,
                empoweredSpinSlashEffect = Assets.spinningSlashEmpoweredFXBlack
            });
            #endregion

            skinInfos = skinList.ToArray();
        }

        public static PaladinSkinInfo GetSkinInfo(string skinName)
        {
            for (int i = 0; i < skinInfos.Length; i++)
            {
                if (skinInfos[i].skinName == skinName)
                {
                    return skinInfos[i];
                }
            }
            return skinInfos[0];
        }
    }
}