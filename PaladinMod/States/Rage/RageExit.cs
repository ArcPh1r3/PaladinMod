using EntityStates;
using PaladinMod.Misc;
using RoR2;
using UnityEngine.Networking;

namespace PaladinMod.States.Rage
{
    public class RageExit : BaseSkillState
    {
        public static float baseDuration = 0.2f;

        private float duration;
        private PaladinRageController rageController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = RageExit.baseDuration / this.attackSpeedStat;
            this.rageController = this.gameObject.GetComponent<PaladinRageController>();

            if (NetworkServer.active) base.characterBody.RemoveBuff(Modules.Buffs.rageBuff);
            if (this.rageController) this.rageController.ExitBerserk();

            base.skillLocator.utility.UnsetSkillOverride(base.skillLocator.utility, Modules.Skills.berserkDashSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.special.UnsetSkillOverride(base.skillLocator.special, Modules.Skills.berserkOutSkillDef, GenericSkill.SkillOverridePriority.Contextual);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}