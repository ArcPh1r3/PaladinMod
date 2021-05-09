using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;

namespace PaladinMod.Misc
{
    public class PaladinRageSkillDef : SkillDef
    {
        public float cost;

        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new PaladinRageSkillDef.InstanceData
            {
                rageComponent = skillSlot.GetComponent<PaladinRageController>()
            };
        }

        private static bool HasSufficientRage([NotNull] GenericSkill skillSlot)
        {
            PaladinRageController rageComponent = ((PaladinRageSkillDef.InstanceData)skillSlot.skillInstanceData).rageComponent;
            return (rageComponent != null) ? (rageComponent.currentRage >= skillSlot.rechargeStock) : false;
        }

        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            return PaladinRageSkillDef.HasSufficientRage(skillSlot) && base.CanExecute(skillSlot);
        }

        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && PaladinRageSkillDef.HasSufficientRage(skillSlot);
        }

        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {
            public PaladinRageController rageComponent;
        }
    }
}