using RoR2;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.Misc
{
    public class PaladinSwordController : MonoBehaviour
    {
        public bool swordActive = true;
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

        public Material buffMat;

        public bool isBlunt;

        private void Awake()
        {
            this.body = base.GetComponent<CharacterBody>();
            this.model = base.GetComponentInChildren<CharacterModel>();
            this.animator = model?.GetComponent<Animator>();
            this.childLocator = base.GetComponentInChildren<ChildLocator>();

            this.buffMat = Modules.Asset.blessingMat;

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

            if (this.body) this.swordActive = this.body.HasBuff(Modules.Buffs.blessedBuff);

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
        }

        public void ApplyLightningBuff()
        {
            if (!this.body.HasBuff(Modules.Buffs.overchargeBuff) && NetworkServer.active)
            {
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
                        inventory.GetItemCountEffective(RoR2Content.Items.BleedOnHit) > 0 || 
                        inventory.GetItemCountEffective(RoR2Content.Items.ArmorReductionOnHit) > 0 || 
                        inventory.GetItemCountEffective(RoR2Content.Items.HealOnCrit) > 0 ||
                        inventory.GetItemCountEffective(ItemCatalog.FindItemIndex("ITEM_BLASTER_SWORD")) > 0 ||
                        inventory.GetItemCountEffective(ItemCatalog.FindItemIndex("TIME_ANCIENT_SCEPTER")) > 0;

                    if (animator) animator.SetBool("fistClosed", hasLeftHandWeapon);
                }
            }
        }

        public void PlaySwingSound()
        {
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
                if (this.skinInfo.swingEffect == Modules.Asset.swordSwing && this.swordActive) 
                    return Modules.Asset.swordSwingEmpowered;
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