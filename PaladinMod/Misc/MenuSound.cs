using RoR2;
using UnityEngine;

namespace PaladinMod.Misc
{
    public class MenuSound : MonoBehaviour
    {
        private uint playID;

        private void OnEnable()
        {
            this.Invoke("PlayEffect", 0.05f);
        }

        private void PlayEffect()
        {
            this.playID = Util.PlaySound(Modules.Sounds.MenuSound, base.gameObject);

            ChildLocator childLocator = this.GetComponentInChildren<ChildLocator>();
            if (childLocator)
            {
                UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/GenericHugeFootstepDust"), childLocator.FindChild("MenuEffect").position, childLocator.FindChild("MenuEffect").rotation);
            }
        }

        private void OnDestroy()
        {
            if (this.playID != 0) AkSoundEngine.StopPlayingID(this.playID);
        }
    }
}
