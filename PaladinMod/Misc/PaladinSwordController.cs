using RoR2;
using UnityEngine;

namespace PaladinMod.Misc
{
    public class PaladinSwordController : MonoBehaviour
    {
        public bool swordActive;
        public bool attacking;

        public int airSlamStacks;

        private CharacterBody body;
        private CharacterModel model;
        private bool isBlunt;

        private void Start()
        {
            this.body = base.GetComponent<CharacterBody>();
            this.model = base.GetComponentInChildren<CharacterModel>();
            //this code sucks.
            this.isBlunt = false;
            if (body)
            {
                if (body.skinIndex == 3 || body.skinIndex == 12) this.isBlunt = true;
            }

            this.InitItemDisplays();
        }

        private void InitItemDisplays()
        {
            if (this.model)
            {
                switch (this.body.skinIndex)
                {
                    case 4:
                        this.SetAegisDisplay(Modules.Assets.sunlightShield, "ElbowL", new Vector3(-0.0002f, 0.001f, 0), new Vector3(20, 270, 310), new Vector3(0.0001f, 0.0001f, 0.0001f));
                        break;
                    case 5:
                        this.SetAegisDisplay(Modules.Assets.artoriasShield, "ElbowL", new Vector3(-0.0004f, 0.003f, 0), new Vector3(20, 270, 270), new Vector3(0.0001f, 0.0001f, 0.0001f));
                        break;
                    case 6:
                        this.SetAegisDisplay(Modules.Assets.goldenShield, "ElbowL", new Vector3(0.0004f, 0, 0), new Vector3(70, 90, 180), new Vector3(1, 1, 1));
                        break;
                    case 7:
                        this.SetAegisDisplay(Modules.Assets.goldenShield, "ElbowL", new Vector3(0.0004f, 0, 0), new Vector3(70, 90, 180), new Vector3(1, 1, 1));
                        break;
                    case 8:
                        this.SetAegisDisplay(Modules.Assets.blackKnightShield, "ElbowL", new Vector3(0, 0.003f, 0), new Vector3(350, 270, 90), new Vector3(0.0001f, 0.0001f, 0.0001f));
                        break;
                    case 9:
                        this.SetAegisDisplay(Modules.Assets.pursuerShield, "ElbowL", new Vector3(0, 0.0025f, 0), new Vector3(0, 0, 90), new Vector3(0.75f, 0.75f, 0.75f));
                        break;
                    case 11:
                        this.SetAegisDisplay(Modules.Assets.giantShield, "ElbowL", new Vector3(-0.0005f, 0.003f, 0), new Vector3(30, 270, 270), new Vector3(0.0001f, 0.0001f, 0.0001f));
                        break;
                    case 12:
                        this.SetAegisDisplay(Modules.Assets.havelShield, "ElbowL", new Vector3(-0.0005f, 0.003f, 0), new Vector3(30, 270, 270), new Vector3(0.0001f, 0.0001f, 0.0001f));
                        break;
                }
            }
        }

        private void SetAegisDisplay(GameObject displayPrefab, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet ruleset = this.model.itemDisplayRuleSet;

            ruleset.FindItemDisplayRuleGroup("BarrierOnOverHeal").rules[0].followerPrefab = displayPrefab;
            ruleset.FindItemDisplayRuleGroup("BarrierOnOverHeal").rules[0].childName = childName;
            ruleset.FindItemDisplayRuleGroup("BarrierOnOverHeal").rules[0].localPos = position;
            ruleset.FindItemDisplayRuleGroup("BarrierOnOverHeal").rules[0].localAngles = rotation;
            ruleset.FindItemDisplayRuleGroup("BarrierOnOverHeal").rules[0].localScale = scale;
        }

        public void PlaySwingSound()
        {
            if (this.isBlunt) Util.PlaySound(Modules.Sounds.SwingBlunt, base.gameObject);
            else Util.PlaySound(Modules.Sounds.Swing, base.gameObject);
        }

        public void PlayHitSound(int index)
        {
            switch (index)
            {
                case 0:
                    if (this.isBlunt) Util.PlaySound(Modules.Sounds.HitBluntS, base.gameObject);
                    else Util.PlaySound(Modules.Sounds.HitS, base.gameObject);
                    break;
                case 1:
                    if (this.isBlunt) Util.PlaySound(Modules.Sounds.HitBluntM, base.gameObject);
                    else Util.PlaySound(Modules.Sounds.HitM, base.gameObject);
                    break;
                case 2:
                    if (this.isBlunt) Util.PlaySound(Modules.Sounds.HitBluntL, base.gameObject);
                    else Util.PlaySound(Modules.Sounds.HitL, base.gameObject);
                    break;
            }
        }
    }
}
