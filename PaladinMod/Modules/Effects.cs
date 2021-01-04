using RoR2;
using UnityEngine;

namespace PaladinMod.Modules
{
    public static class Effects
    {
        public static int[] skinEffectIndex;

        public static GameObject[] hitEffect;
        public static GameObject[] swingEffect;
        public static GameObject[] spinEffect;
        public static GameObject[] empoweredSpinEffect;

        public static void RegisterEffects()
        {
            //not sure how to really approach this all.... just gonna resort to hardcoding for now
            // all this code is complete shit
            //  gotta use brain and rewrite it all.......... one day
            // it all breaks the moment someone else makes a skin mod for paladin :D

            //0- default
            //1- green
            //2- yellow
            //3- blunt
            //4- white
            //5- red

            //skin order:
            //default
            //lunar
            //nkuhana
            //drip
            //solaire
            //artorias
            //faraam
            //ornstein
            //black knight
            //pursuer
            //giant dad
            skinEffectIndex = new int[]
            {
                0,
                0,
                1,
                3,
                2,
                0,
                0,
                2,
                4,
                5,
                2
            };
            //ugh

            hitEffect = new GameObject[]
            {
                Assets.hitFX,
                Assets.hitFXGreen,
                Assets.hitFXYellow,
                Assets.hitFXBlunt,
                Assets.hitFX,
                Assets.hitFXRed
            };

            swingEffect = new GameObject[]
            {
                Assets.swordSwing,
                Assets.swordSwingGreen,
                Assets.swordSwingYellow,
                Assets.swordSwingWhite,
                Assets.swordSwingWhite,
                Assets.swordSwingRed
            };

            spinEffect = new GameObject[]
            {
                Assets.spinningSlashFX,
                Assets.spinningSlashFXGreen,
                Assets.spinningSlashFXYellow,
                Assets.spinningSlashFX,
                Assets.spinningSlashFX,
                Assets.spinningSlashFXRed
            };

            empoweredSpinEffect = new GameObject[]
            {
                Assets.spinningSlashEmpoweredFX,
                Assets.spinningSlashEmpoweredFXGreen,
                Assets.spinningSlashEmpoweredFXYellow,
                Assets.spinningSlashEmpoweredFX,
                Assets.spinningSlashEmpoweredFX,
                Assets.spinningSlashEmpoweredFXRed
            };
        }

        public static int GetEffectIndex(CharacterBody body)
        {
            //there has to be a better way to do this
            if (body.skinIndex >= skinEffectIndex.Length) return 0;
            return skinEffectIndex[body.skinIndex];
        }// i hate it

        public static GameObject HitEffect(CharacterBody body)
        {
            return hitEffect[GetEffectIndex(body)];
        }

        public static GameObject SwingEffect(CharacterBody body)
        {
            return swingEffect[GetEffectIndex(body)];
        }

        public static GameObject SpinEffect(CharacterBody body)
        {
            return spinEffect[GetEffectIndex(body)];
        }

        public static GameObject EmpoweredSpinEffect(CharacterBody body)
        {
            return empoweredSpinEffect[GetEffectIndex(body)];
        }
    }
}
