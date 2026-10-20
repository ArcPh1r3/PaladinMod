using PaladinMod.Modules;
using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.Misc
{
    public class PaladinSoulTetherBodyAttachmentBase : MonoBehaviour
    {
        // Token: 0x04006B4D RID: 27469
        public TetherVfxOrigin tetherVfxOrigin;

        public CharacterBody tetherHost;
        public CharacterBody tetherTarget;

        protected float _gatheredHealth;
        protected float _gatheredBarrier;

        private float _healTimer;

        private float _lifetimeTimer;

        protected virtual bool procChainMasked => true;

        public virtual void InitServer(CharacterBody tetherHost_, CharacterBody tetherTarget_)
        {
            tetherHost = tetherHost_;
            tetherTarget = tetherTarget_;
            tetherVfxOrigin = GetComponent<TetherVfxOrigin>();

            tetherVfxOrigin.SetTetheredTransforms(new List<Transform> { tetherTarget_.hurtBoxGroup.mainHurtBox.transform });
        }

        public void FixedUpdate()
        {
            if (!NetworkServer.active)
                return;

            _healTimer -= Time.deltaTime;

            if (_healTimer < 0)
            {
                _healTimer = StaticValues.SoulTetherHealInterval;
                if (_gatheredHealth > 0)
                {
                    var procChainMask = new ProcChainMask();
                    if (procChainMasked)
                    {
                        procChainMask.AddModdedProc(ProcTypes.SoulTetherShareProcType);
                    }
                    tetherTarget.healthComponent.Heal(_gatheredHealth, procChainMask);
                    _gatheredHealth = 0;

                    //send an orb vfx
                }
                if (_gatheredBarrier > 0)
                {
                    ManualAddBarrierServer(tetherTarget.healthComponent, _gatheredBarrier);
                    _gatheredBarrier = 0;

                    //send an orb vfx
                }
            }


            _lifetimeTimer += Time.deltaTime;
            if (_lifetimeTimer > StaticValues.SoulTetherLifetime)
            {
                Destroy(gameObject);
            }
        }

        //jank: avoids the function we are hooking so we don't have an infinite loop
        public static void ManualAddBarrierServer(HealthComponent healthComponent, float amount)
        {
            if (!healthComponent.alive)
            {
                return;
            }
            healthComponent.Networkbarrier = Mathf.Min(healthComponent.barrier + amount, healthComponent.fullBarrier);
        }
    }

    public class PaladinSoulTetherBodyAttachmentEnemy : PaladinSoulTetherBodyAttachmentBase
    {
        //don't mask this heal as it is not being applied in a loop
        protected override bool procChainMasked => false;

        public override void InitServer(CharacterBody tetherHost_, CharacterBody tetherTarget_)
        {
            base.InitServer(tetherHost_, tetherTarget_);
            //host is the enemy, sending its damage taken as healing to the target which is the caster
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
        }

        private void GlobalEventManager_onServerDamageDealt(DamageReport damageReport)
        {
            if (damageReport.victim && damageReport.victim.body == tetherHost)
            {
                _gatheredHealth += damageReport.damageDealt * StaticValues.SoulTetherLifeStealFraction;
                _gatheredBarrier += damageReport.damageDealt * StaticValues.SoulTetherLifeStealFraction;
            }
        }
    }

    public class PaladinSoulTetherBodyAttachmentAlly : PaladinSoulTetherBodyAttachmentBase
    {
        public override void InitServer(CharacterBody tetherHost_, CharacterBody tetherTarget_)
        {
            base.InitServer(tetherHost_, tetherTarget_);

            HealthComponent.onCharacterHealServer += HealthComponent_onCharacterHealServer;
            HealthComponent.onCharacterRegenServer += HealthComponent_onCharacterRegenServer;
            PaladinPlugin.GlobalOnBarrierAddedServer += PaladinPlugin_GlobalOnBarrierAddedServer;
        }

        private void HealthComponent_onCharacterHealServer(HealthComponent healthComponent, float amount, ProcChainMask procChainMask)
        {
            if (procChainMask.HasModdedProc(ProcTypes.SoulTetherShareProcType))
                return;
            
            if (healthComponent && healthComponent.body == tetherHost)
            {
                _gatheredHealth += amount;
            }
        }

        private void HealthComponent_onCharacterRegenServer(HealthComponent healthComponent, float amount)
        {
            if (healthComponent && healthComponent.body == tetherHost)
            {
                _gatheredHealth += amount;
            }
        }

        private void PaladinPlugin_GlobalOnBarrierAddedServer(HealthComponent healthComponent, float amount)
        {
            if (healthComponent && healthComponent.body == tetherHost)
            {
                _gatheredBarrier += amount;
            }
        }
    }
}
