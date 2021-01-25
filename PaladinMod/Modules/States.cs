using EntityStates;
using R2API;
using RoR2;
using PaladinMod.States;
using PaladinMod.States.Dash;
using PaladinMod.States.Emotes;
using PaladinMod.States.Quickstep;
using PaladinMod.States.Spell;
using PaladinMod.States.LunarKnight;

namespace PaladinMod.Modules
{
    public static class States
    {
        public static void RegisterStates()
        {
            LoadoutAPI.AddSkill(typeof(PaladinMain));
            LoadoutAPI.AddSkill(typeof(SpawnState));
            LoadoutAPI.AddSkill(typeof(BaseEmote));
            LoadoutAPI.AddSkill(typeof(PraiseTheSun));
            LoadoutAPI.AddSkill(typeof(PointDown));
            LoadoutAPI.AddSkill(typeof(Rest));
            LoadoutAPI.AddSkill(typeof(Drip));

            LoadoutAPI.AddSkill(typeof(Slash));

            LoadoutAPI.AddSkill(typeof(SpinSlashEntry));
            LoadoutAPI.AddSkill(typeof(GroundSweep));
            LoadoutAPI.AddSkill(typeof(AirSlam));
            LoadoutAPI.AddSkill(typeof(AirSlamAlt));

            LoadoutAPI.AddSkill(typeof(ChargeLightningSpear));
            LoadoutAPI.AddSkill(typeof(ThrowLightningSpear));

            LoadoutAPI.AddSkill(typeof(LunarShards));

            LoadoutAPI.AddSkill(typeof(QuickstepSimple));

            LoadoutAPI.AddSkill(typeof(AimHeal));
            LoadoutAPI.AddSkill(typeof(CastHeal));

            LoadoutAPI.AddSkill(typeof(ChannelHealZone));
            LoadoutAPI.AddSkill(typeof(CastChanneledHealZone));

            LoadoutAPI.AddSkill(typeof(ChannelTorpor));
            LoadoutAPI.AddSkill(typeof(CastChanneledTorpor));

            LoadoutAPI.AddSkill(typeof(ChannelWarcry));
            LoadoutAPI.AddSkill(typeof(CastChanneledWarcry));

            LoadoutAPI.AddSkill(typeof(MaceSlam));

            // and now apply custom states to our prefabs- probably shouldn't go here but meh

            EntityStateMachine paladinStateMachine = Prefabs.paladinPrefab.GetComponent<EntityStateMachine>();
            paladinStateMachine.mainStateType = new SerializableEntityStateType(typeof(PaladinMain));
            paladinStateMachine.initialStateType = new SerializableEntityStateType(typeof(SpawnState));

            EntityStateMachine lunarKnightStateMachine = Prefabs.lunarKnightPrefab.GetComponent<EntityStateMachine>();
            lunarKnightStateMachine.mainStateType = new SerializableEntityStateType(typeof(PaladinMain));
            lunarKnightStateMachine.initialStateType = new SerializableEntityStateType(typeof(SpawnState));
        }
    }
}