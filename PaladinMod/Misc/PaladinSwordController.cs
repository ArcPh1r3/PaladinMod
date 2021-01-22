using RoR2;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PaladinMod.Misc
{
    public class PaladinSwordController : MonoBehaviour
    {
        public bool swordActive;
        public bool attacking;

        public int airSlamStacks;

        public string skinName;

        private Modules.Effects.PaladinSkinInfo skinInfo;
        private CharacterBody body;
        private CharacterModel model;
        private bool isBlunt;

        private void Start()
        {
            this.body = base.GetComponent<CharacterBody>();
            this.model = base.GetComponentInChildren<CharacterModel>();

            if (this.body)
            {
                this.skinInfo = Modules.Effects.GetSkinInfo(this.model.GetComponent<ModelSkinController>().skins[this.body.skinIndex].nameToken);
                this.skinName = this.skinInfo.skinName;
                this.isBlunt = this.skinInfo.isWeaponBlunt;
            }

            this.InitItemDisplays();

            Invoke("CheckInventory", 0.2f);
        }

        public void CheckInventory()
        {
            if (this.body && this.body.master)
            {
                Inventory inventory = this.body.master.inventory;
                if (inventory)
                {
                    bool hasLeftHandWeapon = (inventory.GetItemCount(ItemIndex.BleedOnHit) > 0 || inventory.GetItemCount(ItemIndex.ArmorReductionOnHit) > 0);

                    if (PaladinPlugin.aetheriumInstalled)
                    {
                        if (CheckForBlasterSword(inventory)) hasLeftHandWeapon = true;
                    }

                    if (hasLeftHandWeapon)
                    {
                        Animator animator = this.model.GetComponent<Animator>();
                        if (animator) animator.SetBool("fistClosed", true);
                    }
                    else
                    {
                        Animator animator = this.model.GetComponent<Animator>();
                        if (animator) animator.SetBool("fistClosed", false);
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private bool CheckForBlasterSword(Inventory inventory)
        {
            if (inventory.GetItemCount(ItemCatalog.FindItemIndex("ITEM_BLASTER_SWORD")) > 0) return true;
            return false;
        }

        private void InitItemDisplays()
        {
            if (this.model)
            {
                ItemDisplayRuleSet newRuleset = Instantiate(this.model.itemDisplayRuleSet);
                this.model.itemDisplayRuleSet = newRuleset;

                //aegis
                switch (this.skinName)
                {
                    case "PALADINBODY_SOLAIRE_SKIN_NAME":
                        this.SetAegisDisplay(Modules.Assets.sunlightShield, "ElbowL", new Vector3(-0.0002f, 0.001f, 0), new Vector3(20, 270, 310), new Vector3(0.0001f, 0.0001f, 0.0001f));
                        break;
                    case "PALADINBODY_ARTORIAS_SKIN_NAME":
                        this.SetAegisDisplay(Modules.Assets.artoriasShield, "ElbowL", new Vector3(-0.0004f, 0.003f, 0), new Vector3(20, 270, 270), new Vector3(0.0001f, 0.0001f, 0.0001f));
                        break;
                    case "PALADINBODY_FARAAM_SKIN_NAME":
                        this.SetAegisDisplay(Modules.Assets.goldenShield, "ElbowL", new Vector3(0.0004f, 0, 0), new Vector3(70, 90, 180), new Vector3(1, 1, 1));
                        break;
                    case "PALADINBODY_BLACKKNIGHT_SKIN_NAME":
                        this.SetAegisDisplay(Modules.Assets.blackKnightShield, "ElbowL", new Vector3(0, 0.003f, 0), new Vector3(350, 270, 90), new Vector3(0.0001f, 0.0001f, 0.0001f));
                        break;
                    case "PALADINBODY_PURSUER_SKIN_NAME":
                        this.SetAegisDisplay(Modules.Assets.pursuerShield, "ElbowL", new Vector3(0, 0.0025f, 0), new Vector3(0, 0, 90), new Vector3(0.75f, 0.75f, 0.75f));
                        break;
                    case "PALADINBODY_GIANTDAD_SKIN_NAME":
                        this.SetAegisDisplay(Modules.Assets.giantShield, "ElbowL", new Vector3(-0.0005f, 0.003f, 0), new Vector3(30, 270, 270), new Vector3(0.0001f, 0.0001f, 0.0001f));
                        break;
                    case "PALADINBODY_HAVEL_SKIN_NAME":
                        this.SetAegisDisplay(Modules.Assets.havelShield, "ElbowL", new Vector3(-0.0005f, 0.003f, 0), new Vector3(30, 270, 270), new Vector3(0.0001f, 0.0001f, 0.0001f));
                        break;
                }

                //razorwire
                if (this.skinName == "PALADINBODY_DRIP_SKIN_NAME")
                {
                    this.SetRazorwireDisplay(new Vector3(-0.0004f, 0.01f, -0.0005f), new Vector3(270, 300, 0), new Vector3(0.004f, 0.004f, 0.008f));
                }
                else this.SetRazorwireDisplay(new Vector3(0, 0.006f, -0.001f), new Vector3(270, 300, 0), new Vector3(0.006f, 0.009f, 0.012f));

                //tri tip
                // nvm the skin already holds the dagger
                /*if (this.skinName == "PALADINBODY_ABYSSWATCHER_SKIN_NAME")
                {
                    this.SetDaggerDisplay(Modules.Assets.watcherDagger, new Vector3(-0.0004f, 0.01f, -0.0005f), new Vector3(270, 300, 0), new Vector3(0.004f, 0.004f, 0.008f));
                }*/
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

        private void SetRazorwireDisplay(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet ruleset = this.model.itemDisplayRuleSet;

            ruleset.FindItemDisplayRuleGroup("Thorns").rules[0].localPos = position;
            ruleset.FindItemDisplayRuleGroup("Thorns").rules[0].localAngles = rotation;
            ruleset.FindItemDisplayRuleGroup("Thorns").rules[0].localScale = scale;
        }

        private void SetDaggerDisplay(GameObject displayPrefab, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet ruleset = this.model.itemDisplayRuleSet;

            ruleset.FindItemDisplayRuleGroup("BleedOnHit").rules[0].followerPrefab = displayPrefab;
            ruleset.FindItemDisplayRuleGroup("BleedOnHit").rules[0].localPos = position;
            ruleset.FindItemDisplayRuleGroup("BleedOnHit").rules[0].localAngles = rotation;
            ruleset.FindItemDisplayRuleGroup("BleedOnHit").rules[0].localScale = scale;
        }

        public void PlaySwingSound()
        {
            Util.PlaySound(this.skinInfo.swingSoundString, base.gameObject);
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

        public GameObject swingEffect
        {
            get
            {
                return this.skinInfo.swingEffect;
            }
        }

        public GameObject hitEffect
        {
            get
            {
                return this.skinInfo.hitEffect;
            }
        }

        public GameObject spinSlashEffect
        {
            get
            {
                return this.skinInfo.spinSlashEffect;
            }
        }

        public GameObject empoweredSpinSlashEffect
        {
            get
            {
                return this.skinInfo.empoweredSpinSlashEffect;
            }
        }
    }
}