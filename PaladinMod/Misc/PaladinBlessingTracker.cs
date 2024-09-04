using UnityEngine;

namespace PaladinMod.Misc
{
    public class PaladinBlessingTracker : MonoBehaviour
    {
        public RoR2.TemporaryOverlayInstance Overlay;
        public RoR2.CharacterBody Body;

        public void FixedUpdate()
        {
            if (!Body.HasBuff(Modules.Buffs.blessedBuff))
            {
                if(Overlay != null) Overlay.Destroy();
                UnityEngine.Object.Destroy(this);
            }
        }
    }
}