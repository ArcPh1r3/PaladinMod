using UnityEngine;

namespace PaladinMod.Misc
{
    public class PaladinTorporTracker : MonoBehaviour
    {
        public RoR2.TemporaryOverlay Overlay;
        public RoR2.CharacterBody Body;

        public void FixedUpdate()
        {
            if (!Body.HasBuff(Modules.Buffs.torporDebuff) && !Body.HasBuff(Modules.Buffs.scepterTorporDebuff))
            {
                UnityEngine.Object.Destroy(Overlay);
                UnityEngine.Object.Destroy(this);
            }
        }
    }
}