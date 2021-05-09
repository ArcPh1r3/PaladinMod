using RoR2;
using UnityEngine;

namespace PaladinMod.Modules
{
    internal static class CameraParams
    {
        internal static CharacterCameraParams defaultCameraParamsPaladin;
        internal static CharacterCameraParams channelCameraParamsPaladin;
        internal static CharacterCameraParams channelFullCameraParamsPaladin;
        internal static CharacterCameraParams rageEnterCameraParamsPaladin;
        internal static CharacterCameraParams rageEnterOutCameraParamsPaladin;
        internal static CharacterCameraParams emoteCameraParamsPaladin;

        internal static void InitializeParams()
        {
            defaultCameraParamsPaladin = NewCameraParams("ccpPaladin", 70f, 1.37f, new Vector3(0, 0.75f, -10.5f));
            channelCameraParamsPaladin = NewCameraParams("ccpPaladinSpellChannel", 70f, 1.37f, new Vector3(2f, 0.5f, -8f));
            channelFullCameraParamsPaladin = NewCameraParams("ccpPaladinSpellChannel", 70f, 1.37f, new Vector3(2f, 0.75f, -12f));
            rageEnterCameraParamsPaladin = NewCameraParams("ccpPaladinRageEnter", 70f, 1.37f, new Vector3(0f, -1.2f, -6.5f));
            rageEnterOutCameraParamsPaladin = NewCameraParams("ccpPaladinRageEnterOut", 70f, 1.37f, new Vector3(0f, 0.75f, -12f));
            emoteCameraParamsPaladin = NewCameraParams("ccpPaladinEmote", 70f, 1.37f, new Vector3(0f, -1.1f, -6.5f));
        }

        private static CharacterCameraParams NewCameraParams(string name, float pitch, float pivotVerticalOffset, Vector3 standardPosition)
        {
            return NewCameraParams(name, pitch, pivotVerticalOffset, standardPosition, 0.1f);
        }

        private static CharacterCameraParams NewCameraParams(string name, float pitch, float pivotVerticalOffset, Vector3 standardPosition, float wallCushion)
        {
            CharacterCameraParams newParams = ScriptableObject.CreateInstance<CharacterCameraParams>();

            newParams.maxPitch = pitch;
            newParams.minPitch = -pitch;
            newParams.pivotVerticalOffset = pivotVerticalOffset;
            newParams.standardLocalCameraPos = standardPosition;
            newParams.wallCushion = wallCushion;

            return newParams;
        }
    }
}