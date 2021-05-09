﻿using EntityStates;
using PaladinMod.Misc;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.States.Rage
{
    public class RageEnterOut : BaseSkillState
    {
        public static float baseDuration = 0.8f;

        public static float shockwaveRadius = 32f;
        public static float shockwaveForce = 8000f;
        public static float shockwaveBonusForce = 1500f;

        private float duration;
        private PaladinRageController rageController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = (1.5f * RageEnter.baseDuration) / this.attackSpeedStat;
            this.rageController = this.gameObject.GetComponent<PaladinRageController>();

            if (NetworkServer.active) base.characterBody.AddBuff(Modules.Buffs.rageBuff);

            base.cameraTargetParams.cameraParams = Modules.CameraParams.rageEnterOutCameraParamsPaladin;
            base.cameraTargetParams.aimMode = CameraTargetParams.AimType.Aura;

            this.FireShockwave();

            base.skillLocator.utility.SetSkillOverride(base.skillLocator.utility, Modules.Skills.berserkDashSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.special.SetSkillOverride(base.skillLocator.special, Modules.Skills.berserkOutSkillDef, GenericSkill.SkillOverridePriority.Contextual);
        }

        private void FireShockwave()
        {
            Util.PlaySound("HenryFrenzyShockwave", base.gameObject);

            EffectData effectData = new EffectData();
            effectData.origin = base.characterBody.corePosition;
            effectData.scale = 1;

            //EffectManager.SpawnEffect(Modules.Assets.frenzyShockwaveEffect, effectData, false);

            if (base.isAuthority)
            {
                BlastAttack blastAttack = new BlastAttack();
                blastAttack.attacker = base.gameObject;
                blastAttack.inflictor = base.gameObject;
                blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                blastAttack.position = base.characterBody.corePosition;
                blastAttack.procCoefficient = 0f;
                blastAttack.radius = RageEnterOut.shockwaveRadius;
                blastAttack.baseForce = RageEnterOut.shockwaveForce;
                blastAttack.bonusForce = Vector3.up * RageEnterOut.shockwaveBonusForce;
                blastAttack.baseDamage = 0f;
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.damageColorIndex = DamageColorIndex.Item;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHit;
                blastAttack.Fire();
            }

            if (this.rageController) this.rageController.EnterBerserk();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterMotor.velocity = Vector3.zero;

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            base.cameraTargetParams.cameraParams = Modules.CameraParams.defaultCameraParamsPaladin;
            base.cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;

            if (NetworkServer.active) base.characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}