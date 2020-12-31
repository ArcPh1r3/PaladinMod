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

            skinEffectIndex = new int[]
            {
                0,
                0,
                1,
                2,
                0,
                0,
                2,
                2
            };
            //ugh

            hitEffect = new GameObject[]
            {
                Assets.hitFX,
                Assets.hitFXGreen,
                Assets.hitFXYellow
            };

            swingEffect = new GameObject[]
            {
                Assets.swordSwing,
                Assets.swordSwingGreen,
                Assets.swordSwingYellow
            };

            spinEffect = new GameObject[]
            {
                Assets.spinningSlashFX,
                Assets.spinningSlashFXGreen,
                Assets.spinningSlashFXYellow
            };

            empoweredSpinEffect = new GameObject[]
            {
                Assets.spinningSlashEmpoweredFX,
                Assets.spinningSlashEmpoweredFXGreen,
                Assets.spinningSlashEmpoweredFXYellow
            };
        }

        public static int GetEffectIndex(CharacterBody body)
        {
            //there has to be a better way to do this
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
