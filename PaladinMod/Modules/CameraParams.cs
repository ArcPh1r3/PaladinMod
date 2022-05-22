using RoR2;
using UnityEngine;

    internal enum PaladinCameraParams {
        DEFAULT,
        CHANNEL,
        CHANNEL_FULL,
        CRUEL_SUN,
        RAGE_ENTER,
        RAGE_ENTER_OUT,
        EMOTE,
    }

namespace PaladinMod.Modules
{

    internal static class CameraParams
    {
        internal static CharacterCameraParamsData defaultPaladinCameraParams;

        internal static CharacterCameraParamsData channelCameraParams;
        internal static CharacterCameraParamsData channelFullCameraParams;

        internal static CharacterCameraParamsData cruelSunCameraParams;

        internal static CharacterCameraParamsData rageEnterCameraParams;
        internal static CharacterCameraParamsData rageEnterOutCameraParams;

        internal static CharacterCameraParamsData emoteCameraParams;

        internal static float defaultVerticalOffset = 1.53f;

        internal static void InitializeParams()
        {
            defaultPaladinCameraParams = NewCameraParams("ccpPaladin", 70f, defaultVerticalOffset, new Vector3(0f, 1.25f, -12f));

            channelCameraParams = NewCameraParams("ccpPaladinSpellChannel", 70f, 1.6f, new Vector3(0f, 2.8f, -14f));
            channelFullCameraParams = NewCameraParams("ccpPaladinSpellChannelFull", 70f, 1.65f, new Vector3(0f, 3.5f, -16f));

            //these settings are extreme for testing purposes, camera is borked anyway
            //cruelSunCameraParams = NewCameraParams("ccpPaladinCruelSun", 70f, 3f, new Vector3(0f, 100f, -240f));
            cruelSunCameraParams = NewCameraParams("ccpPaladinCruelSun", 70f, 1.65f, new Vector3(0f, 4f, -18f));

            rageEnterCameraParams = NewCameraParams("ccpPaladinRageEnter", 70f, defaultVerticalOffset, new Vector3(0f, -1.2f, -8.5f));
            rageEnterOutCameraParams = NewCameraParams("ccpPaladinRageEnterOut", 70f, defaultVerticalOffset, new Vector3(0f, 0.75f, -12f));

            emoteCameraParams = NewCameraParams("ccpPaladinEmote", 70f, defaultVerticalOffset, new Vector3(0f, -0.6f, -8.5f));

        }

        private static CharacterCameraParamsData NewCameraParams(string name, float pitch, float pivotVerticalOffset, Vector3 idealPosition)
        {
            return NewCameraParams(name, pitch, pivotVerticalOffset, idealPosition, 0.1f);
        }

        private static CharacterCameraParamsData NewCameraParams(string name, float pitch, float pivotVerticalOffset, Vector3 idealPosition, float wallCushion)
        {
            CharacterCameraParamsData newParams = new CharacterCameraParamsData();

            newParams.maxPitch = pitch;
            newParams.minPitch = -pitch;
            newParams.pivotVerticalOffset = pivotVerticalOffset;
            newParams.idealLocalCameraPos = idealPosition;
            newParams.wallCushion = wallCushion;

            return newParams;
        }

        internal static CameraTargetParams.CameraParamsOverrideHandle OverridePaladinCameraParams(CameraTargetParams camParams, PaladinCameraParams camera, float transitionDuration = 0.5f) {

            CharacterCameraParamsData paramsData = GetNewPaladinParams(camera);

            CameraTargetParams.CameraParamsOverrideRequest request = new CameraTargetParams.CameraParamsOverrideRequest {
                cameraParamsData = paramsData,
                priority = 0,
            };

            return camParams.AddParamsOverride(request, transitionDuration);
        }

        internal static CharacterCameraParams CreateCameraParamsWithData(PaladinCameraParams camera) {

            CharacterCameraParams newPaladinCameraParams = ScriptableObject.CreateInstance<CharacterCameraParams>();

            newPaladinCameraParams.name = camera.ToString().ToLower() + "Params";

            newPaladinCameraParams.data = GetNewPaladinParams(camera);

            return newPaladinCameraParams;
        }

        internal static CharacterCameraParamsData GetNewPaladinParams(PaladinCameraParams camera) {
            CharacterCameraParamsData paramsData = defaultPaladinCameraParams;

            switch (camera) {

                default:
                case PaladinCameraParams.DEFAULT:
                    paramsData = defaultPaladinCameraParams;
                    break;
                case PaladinCameraParams.CHANNEL:
                    paramsData = channelCameraParams;
                    break;
                case PaladinCameraParams.CHANNEL_FULL:
                    paramsData = channelFullCameraParams;
                    break;
                case PaladinCameraParams.CRUEL_SUN:
                    paramsData = cruelSunCameraParams;
                    break;
                case PaladinCameraParams.RAGE_ENTER:
                    paramsData = rageEnterCameraParams;
                    break;
                case PaladinCameraParams.RAGE_ENTER_OUT:
                    paramsData = rageEnterOutCameraParams;
                    break;
                case PaladinCameraParams.EMOTE:
                    paramsData = emoteCameraParams;
                    break;
            }

            return paramsData;
        }
    }
}