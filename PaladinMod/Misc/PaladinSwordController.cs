using RoR2;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.Misc
{
    public class PaladinSwordController : MonoBehaviour
    {
        public bool swordActive;
        public bool attacking;

        public int airSlamStacks;

        public string skinName;

        public Vector3 sunPosition;

        public ParticleSystem lightningEffect;
        private Modules.Effects.PaladinSkinInfo skinInfo;
        private CharacterBody body;
        private CharacterModel model;
        private ChildLocator childLocator;
        private Animator animator;
        private float lightningBuffTimer;
        private bool hasLightningBuff;
        private float passiveBuffTimer;
        private PaladinPassiveBuffController buffController;
        private bool wasInCombat;

        public Material buffMat;

        public bool isBlunt;

        private void Awake()
        {
            this.body = base.GetComponent<CharacterBody>();
            this.model = base.GetComponentInChildren<CharacterModel>();
            this.animator = model?.GetComponent<Animator>();
            this.childLocator = base.GetComponentInChildren<ChildLocator>();

            this.buffMat = Modules.Assets.blessingMat;

            this.buffController = this.gameObject.AddComponent<PaladinPassiveBuffController>();
            this.buffController.buffDef = Modules.Buffs.blessedBuff;

            this.lightningEffect = this.childLocator.FindChild("SwordLightningEffect").GetComponentInChildren<ParticleSystem>();
        }
        
        private void Start()
        {
            if (this.body)
            {
                this.skinInfo = Modules.Effects.GetSkinInfo(this.model.GetComponent<ModelSkinController>().skins[this.body.skinIndex].nameToken);
                this.skinName = this.skinInfo.skinNameToken;
                this.isBlunt = this.skinInfo.isWeaponBlunt;
                this.EditEyeTrail();
            }

            this.InvokeRepeating("CheckInventory", 0.5f, 2f);
        }

        private void FixedUpdate()
        {
            this.lightningBuffTimer -= Time.fixedDeltaTime;
            this.passiveBuffTimer -= Time.fixedDeltaTime;

            if (this.lightningBuffTimer <= 0f)
            {
                if (this.hasLightningBuff)
                {
                    this.KillLightningBuff();
                }
            }

            // passive buff
            if (this.body && this.body.healthComponent)
            {
                bool isActive = false;

                if (this.body.healthComponent.combinedHealth >= (0.9f * this.body.healthComponent.fullHealth) || this.body.healthComponent.barrier > 0)
                {
                    isActive = true;
                }

                if (isActive)
                {
                    if (this.passiveBuffTimer <= 0f)
                    {
                        this.passiveBuffTimer = 0.35f;
                        if (this.buffController) this.buffController.TryBuff();
                    }
                }
            }

            // combat animation shit
            if (this.animator && this.body)
            {
                bool inCombat = true;
                if (this.body.outOfDanger && this.body.outOfCombat) inCombat = false;

                if (inCombat) this.animator.SetLayerWeight(this.animator.GetLayerIndex("Body, Combat"), 1f);
                else this.animator.SetLayerWeight(this.animator.GetLayerIndex("Body, Combat"), 0f);

                if (inCombat != this.wasInCombat)
                {
                    if (this.body.characterMotor.isGrounded && this.body.inputBank.moveVector == Vector3.zero)
                    {
                        if (!inCombat) EntityStates.EntityState.PlayAnimationOnAnimator(this.animator, "Body", "ToRestIdle");
                    }
                    else if (this.body.characterMotor.isGrounded && this.body.isSprinting)
                    {
                        if (!inCombat) EntityStates.EntityState.PlayAnimationOnAnimator(this.animator, "Body", "SprintToRest2");
                    }
                    else
                    {
                        if (inCombat) EntityStates.EntityState.PlayAnimationOnAnimator(this.animator, "Transition", "ToCombat");
                        else EntityStates.EntityState.PlayAnimationOnAnimator(this.animator, "Transition", "ToRest");
                    }
                }

                this.wasInCombat = inCombat;
            }
        }

        public void ApplyLightningBuff()
        {
            if (!this.body.HasBuff(Modules.Buffs.overchargeBuff) && NetworkServer.active) {
                Debug.LogWarning("applying lightning");
                this.body.AddBuff(Modules.Buffs.overchargeBuff);
            }

            this.hasLightningBuff = true;
            this.lightningBuffTimer = 4f;
            if (this.lightningEffect) this.lightningEffect.Play();
        }

        private void KillLightningBuff()
        {
            if (NetworkServer.active) this.body.RemoveBuff(Modules.Buffs.overchargeBuff);
            if (this.lightningEffect) this.lightningEffect.Stop();
        }

        private void EditEyeTrail()
        {
            TrailRenderer eyeTrail = this.childLocator.FindChild("EyeTrail").GetComponentInChildren<TrailRenderer>();

            eyeTrail.startColor = this.skinInfo.eyeTrailColor;
        }

        public void CheckInventory()
        {
            if (this.body && this.body.master)
            {
                Inventory inventory = this.body.master.inventory;
                if (inventory)
                {
                    bool hasLeftHandWeapon = 
                        inventory.GetItemCount(RoR2Content.Items.BleedOnHit) > 0 || 
                        inventory.GetItemCount(RoR2Content.Items.ArmorReductionOnHit) > 0 || 
                        inventory.GetItemCount(RoR2Content.Items.HealOnCrit) > 0 ||
                        inventory.GetItemCount(ItemCatalog.FindItemIndex("ITEM_BLASTER_SWORD")) > 0 ||
                        inventory.GetItemCount(ItemCatalog.FindItemIndex("TIME_ANCIENT_SCEPTER")) > 0;

                    if (animator) animator.SetBool("fistClosed", hasLeftHandWeapon);
                }
            }
        }

        private void InitItemDisplays()
        {
            if (this.model)
            {
                ItemDisplayRuleSet newRuleset = Instantiate(this.model.itemDisplayRuleSet);
                this.model.itemDisplayRuleSet = newRuleset;

                //aegis
                /*switch (this.skinName)
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
                */

                //razorwire
                //if (this.skinName == "PALADINBODY_DRIP_SKIN_NAME") {
                //    this.SetRazorwireDisplay(new Vector3(-0.0004f, 0.01f, -0.0005f), new Vector3(270, 300, 0), new Vector3(0.004f, 0.004f, 0.008f));
                //} else this.SetRazorwireDisplay(new Vector3(0, 0.006f, -0.001f), new Vector3(270, 300, 0), new Vector3(0.006f, 0.009f, 0.012f));

                //tri tip
                // nvm the skin already holds the dagger
                //if (this.skinName == "PALADINBODY_ABYSSWATCHER_SKIN_NAME")
                //{
                //    this.SetDaggerDisplay(Modules.Assets.watcherDagger, new Vector3(-0.0004f, 0.01f, -0.0005f), new Vector3(270, 300, 0), new Vector3(0.004f, 0.004f, 0.008f));
                //}
            }
        }

        //private void SetAegisDisplay(GameObject displayPrefab, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
        //{
        //    ItemDisplayRuleSet ruleset = this.model.itemDisplayRuleSet;

        //    ruleset.FindItemDisplayRuleGroup("BarrierOnOverHeal").rules[0].followerPrefab = displayPrefab;
        //    ruleset.FindItemDisplayRuleGroup("BarrierOnOverHeal").rules[0].childName = childName;
        //    ruleset.FindItemDisplayRuleGroup("BarrierOnOverHeal").rules[0].localPos = position;
        //    ruleset.FindItemDisplayRuleGroup("BarrierOnOverHeal").rules[0].localAngles = rotation;
        //    ruleset.FindItemDisplayRuleGroup("BarrierOnOverHeal").rules[0].localScale = scale;
        //}

        private void SetRazorwireDisplay(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            ItemDisplayRuleSet ruleset = this.model.itemDisplayRuleSet;

            ruleset.FindDisplayRuleGroup(RoR2Content.Items.Thorns).rules[0].localPos = position;
            ruleset.FindDisplayRuleGroup(RoR2Content.Items.Thorns).rules[0].localAngles = rotation;
            ruleset.FindDisplayRuleGroup(RoR2Content.Items.Thorns).rules[0].localScale = scale;
        }

        //private void SetDaggerDisplay(GameObject displayPrefab, Vector3 position, Vector3 rotation, Vector3 scale)
        //{
        //    ItemDisplayRuleSet ruleset = this.model.itemDisplayRuleSet;

        //    ruleset.FindItemDisplayRuleGroup("BleedOnHit").rules[0].followerPrefab = displayPrefab;
        //    ruleset.FindItemDisplayRuleGroup("BleedOnHit").rules[0].localPos = position;
        //    ruleset.FindItemDisplayRuleGroup("BleedOnHit").rules[0].localAngles = rotation;
        //    ruleset.FindItemDisplayRuleGroup("BleedOnHit").rules[0].localScale = scale;
        //}

        public void PlaySwingSound() {
            Util.PlaySound(skinInfo.swingSoundString, GetSoundObject());
        }

        private GameObject GetSoundObject() {
            if (PaladinPlugin.IsLocalVRPlayer(body)) {
                return GetVRMuzzleObject();
            } else {
                return gameObject;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static GameObject GetVRMuzzleObject() {
            return VRAPI.MotionControls.dominantHand.muzzle.gameObject;
        }

        public void PlayHitSound(int index)
        {
            switch (index)
            {
                case 0:
                    if (this.isBlunt) Util.PlaySound(Modules.Sounds.HitBluntS, base.gameObject);
                    else Util.PlaySound(Modules.Sounds.HitS, GetSoundObject());
                    break;
                case 1:
                    if (this.isBlunt) Util.PlaySound(Modules.Sounds.HitBluntM, base.gameObject);
                    else Util.PlaySound(Modules.Sounds.HitM, GetSoundObject());
                    break;
                case 2:
                    if (this.isBlunt) Util.PlaySound(Modules.Sounds.HitBluntL, base.gameObject);
                    else Util.PlaySound(Modules.Sounds.HitL, GetSoundObject());
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