using RoR2;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace PaladinMod.Modules
{
    internal class VRCompat
    {
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void SetupVR()
        {
            if (!VRAPI.VR.enabled || !VRAPI.MotionControls.enabled)
                return;

            PaladinPlugin.VREnabled = true;

            Modules.Asset.loadVRBundle();

            VRAPI.MotionControls.AddHandPrefab(Modules.Asset.vrPaladinDominantHand);
            VRAPI.MotionControls.AddHandPrefab(Modules.Asset.vrPaladinNonDominantHand);

            VRAPI.MotionControls.onHandPairSet += SetVRHandsMaterials;

            VRAPI.MotionControls.AddSkillBindingOverride("RobPaladinBody", SkillSlot.Primary, SkillSlot.Secondary, SkillSlot.Utility, SkillSlot.Special);
        }

        private static void SetVRHandsMaterials(CharacterBody body)
        {
            if (body.baseNameToken == "PALADIN_NAME")
            {

                SetVRHandRendererInfosToHopooShader(VRAPI.MotionControls.dominantHand);
                SetVRHandRendererInfosToHopooShader(VRAPI.MotionControls.nonDominantHand);
            }
        }

        private static void SetVRHandRendererInfosToHopooShader(VRAPI.MotionControls.HandController hand)
        {

            for (int i = 0; i < hand.rendererInfos.Length; i++)
            {
                CharacterModel.RendererInfo rend = VRAPI.MotionControls.dominantHand.rendererInfos[i];
                Modules.Materials.SetHotpooMaterial(rend.defaultMaterial);
            }
        }

    }
}
