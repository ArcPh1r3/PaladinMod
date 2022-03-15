using RoR2;
using UnityEngine;

    internal enum PaladinCameraParams {
        DEFAULT,
        CHANNEL,
        CHANNEL_FULL,
        RAGE_ENTER,
        RAGE_ENTER_OUT,
        EMOTE,
        CRUEL_SUN,
    }

namespace PaladinMod.Modules
{

    internal static class CameraParams
    {
        internal static CharacterCameraParamsData defaultCameraParamsPaladin;

        internal static CharacterCameraParamsData channelCameraParamsPaladin;
        internal static CharacterCameraParamsData channelFullCameraParamsPaladin;

        internal static CharacterCameraParamsData rageEnterCameraParamsPaladin;
        internal static CharacterCameraParamsData rageEnterOutCameraParamsPaladin;

        internal static CharacterCameraParamsData emoteCameraParamsPaladin;

        internal static CharacterCameraParamsData cruelSunCameraParamsPaladin;

        internal static float defaultVerticalOffset = 1.53f;


        internal static void InitializeParams()
        {
            defaultCameraParamsPaladin = NewCameraParams("ccpPaladin", 70f, defaultVerticalOffset, new Vector3(0f, 1.25f, -12f));

            channelCameraParamsPaladin = NewCameraParams("ccpPaladinSpellChannel", 70f, 1.6f, new Vector3(0f, 2.8f, -14f));
            channelFullCameraParamsPaladin = NewCameraParams("ccpPaladinSpellChannelFull", 70f, 1.65f, new Vector3(0f, 3.5f, -16f));

            rageEnterCameraParamsPaladin = NewCameraParams("ccpPaladinRageEnter", 70f, defaultVerticalOffset, new Vector3(0f, -1.2f, -8.5f));
            rageEnterOutCameraParamsPaladin = NewCameraParams("ccpPaladinRageEnterOut", 70f, defaultVerticalOffset, new Vector3(0f, 0.75f, -12f));

            emoteCameraParamsPaladin = NewCameraParams("ccpPaladinEmote", 70f, defaultVerticalOffset, new Vector3(0f, -0.6f, -8.5f));

            cruelSunCameraParamsPaladin = NewCameraParams("ccpPaladinCruelSun", 70f, 3f, new Vector3(0f, 10f, -24f));
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
            CharacterCameraParamsData paramsData = defaultCameraParamsPaladin;

            switch (camera) {

                default:
                case PaladinCameraParams.DEFAULT:
                    paramsData = defaultCameraParamsPaladin;
                    break;
                case PaladinCameraParams.CHANNEL:
                    paramsData = channelCameraParamsPaladin;
                    break;
                case PaladinCameraParams.CHANNEL_FULL:
                    paramsData = channelFullCameraParamsPaladin;
                    break;
                case PaladinCameraParams.RAGE_ENTER:
                    paramsData = rageEnterCameraParamsPaladin;
                    break;
                case PaladinCameraParams.RAGE_ENTER_OUT:
                    paramsData = rageEnterOutCameraParamsPaladin;
                    break;
                case PaladinCameraParams.EMOTE:
                    paramsData = emoteCameraParamsPaladin;
                    break;
                case PaladinCameraParams.CRUEL_SUN:
                    paramsData = cruelSunCameraParamsPaladin;
                    break;
            }

            return paramsData;
        }
    }
}