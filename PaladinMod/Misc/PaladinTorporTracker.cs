using UnityEngine;

namespace PaladinMod.Misc
{
    public class PaladinTorporTracker : MonoBehaviour
    {
        public RoR2.TemporaryOverlayInstance Overlay;
        public RoR2.CharacterBody Body;

        public void FixedUpdate()
        {
            if (!Body.HasBuff(Modules.Buffs.torporDebuff) && !Body.HasBuff(Modules.Buffs.scepterTorporDebuff))
            {
                if (Overlay != null) Overlay.Destroy(); ;
                UnityEngine.Object.Destroy(this);
            }
        }
    }
}