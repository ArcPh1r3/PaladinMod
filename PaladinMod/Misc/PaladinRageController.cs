using RoR2;
using UnityEngine;

namespace PaladinMod.Misc
{
    public class PaladinRageController : MonoBehaviour
    {
        public float maxRage = 100f;
        public float currentRage;

        public float currentRegen;

        private float rageStopwatch;
        private bool inRage;
        private float lastRage;
        private CharacterBody characterBody;
        private HealthComponent healthComponent;
        private CharacterMaster characterMaster;
        private CharacterModel model;
        private ChildLocator childLocator;
        private Animator modelAnimator;

        private void Awake()
        {
            this.characterBody = this.gameObject.GetComponent<CharacterBody>();
            this.healthComponent = this.gameObject.GetComponent<HealthComponent>();
            this.childLocator = this.gameObject.GetComponentInChildren<ChildLocator>();
            this.model = this.gameObject.GetComponentInChildren<CharacterModel>();
            this.modelAnimator = this.gameObject.GetComponentInChildren<Animator>();

            this.InvokeRepeating("CheckHealth", 0.25f, 0.25f);
            this.Invoke("CheckSkill", 0.2f);
        }

        private void Start()
        {
            this.characterMaster = this.characterBody.master;
        }

        private void FixedUpdate()
        {
            if (this.inRage)
            {
                this.rageStopwatch -= Time.fixedDeltaTime;

                if (this.rageStopwatch <= 0f)
                {
                    this.currentRegen -= 4f * Time.fixedDeltaTime;
                }
            }

            if (Input.GetKey("z")) this.AddRage(100f);
        }

        private void CheckSkill()
        {
            if (this.characterBody)
            {
                if (this.characterBody.skillLocator)
                {
                    if (this.characterBody.skillLocator.special.skillNameToken != "PALADIN_SPECIAL_BERSERK_NAME")
                    {
                        Destroy(this);
                    }
                }
            }
        }

        private void CheckHealth()
        {
            if (!this.inRage)
            {
                if (this.healthComponent)
                {
                    if (this.healthComponent.alive)
                    {
                        if (this.healthComponent.combinedHealth <= 0.5f * this.healthComponent.fullCombinedHealth) this.AddRage(0.25f);
                    }
                }
            }
        }

        public bool AddRage()
        {
            return this.AddRage(2f);
        }

        public bool AddRage(float amount)
        {
            if (this.characterBody.HasBuff(Modules.Buffs.rageBuff)) return false;
            if (this.currentRage >= this.maxRage) return false;

            //if (this.characterBody.healthComponent.combinedHealth <= (0.5f * this.characterBody.healthComponent.fullCombinedHealth)) amount *= 2f;
            if (this.characterBody.healthComponent.combinedHealth <= (0.25f * this.characterBody.healthComponent.fullCombinedHealth)) amount *= 3f;

            this.currentRage = Mathf.Clamp(this.currentRage + amount, 0f, this.maxRage);

            if (this.currentRage >= this.maxRage && this.lastRage < this.maxRage) Util.PlaySound("HenryFrenzyReady", this.gameObject);

            this.lastRage = this.currentRage;
            return true;
        }

        public bool SpendRage(float amount)
        {
            this.currentRage = Mathf.Clamp(this.currentRage - amount, 0f, this.maxRage);
            this.lastRage = this.currentRage;
            return true;
        }

        public void EnterBerserk()
        {
            this.rageStopwatch = 8f;
            this.currentRegen = 50f;
            this.inRage = true;

            if (this.modelAnimator) this.modelAnimator.SetLayerWeight(this.modelAnimator.GetLayerIndex("Rage"), 1f);
        }

        public void ExitBerserk()
        {
            this.rageStopwatch = 0f;
            this.currentRegen = 0f;
            this.inRage = false;

            if (this.modelAnimator) this.modelAnimator.SetLayerWeight(this.modelAnimator.GetLayerIndex("Rage"), 0f);
        }
    }
}