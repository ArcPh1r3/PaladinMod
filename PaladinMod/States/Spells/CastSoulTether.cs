using PaladinMod.Misc;
using PaladinMod.Modules;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.States.Spell
{
    public class CastSoulTether : BaseCastChanneledSpellStateTargeted
    {
        public override void OnEnter()
        {
            baseDuration = 0.2f;
            base.OnEnter();
            if (!NetworkServer.active)
            {
                return;
            }

            TeamMask teamFilter = TeamMask.GetUnprotectedTeams(base.teamComponent.teamIndex);
            bool isEnemy = teamFilter.HasTeam(trackingTarget.teamIndex);

            if (isEnemy)
            {
                var soulTetherAttachmentAlly = UnityEngine.Object.Instantiate<GameObject>(Asset.SoulTetherAttachmentEnemy);
                soulTetherAttachmentAlly.GetComponent<NetworkedBodyAttachment>().AttachToGameObjectAndSpawn(trackingTarget.healthComponent.gameObject, null);
                soulTetherAttachmentAlly.GetComponent<PaladinSoulTetherBodyAttachmentBase>().InitServer(trackingTarget.healthComponent.body, characterBody);
            }
            else
            {
                var soulTetherAttachmentAlly = UnityEngine.Object.Instantiate<GameObject>(Asset.SoulTetherAttachmentAlly);
                soulTetherAttachmentAlly.GetComponent<NetworkedBodyAttachment>().AttachToGameObjectAndSpawn(gameObject, null);
                soulTetherAttachmentAlly.GetComponent<PaladinSoulTetherBodyAttachmentBase>().InitServer(characterBody, trackingTarget.healthComponent.body);

                var soulTetherAttachmentAlly2 = UnityEngine.Object.Instantiate<GameObject>(Asset.SoulTetherAttachmentAlly);
                soulTetherAttachmentAlly2.GetComponent<NetworkedBodyAttachment>().AttachToGameObjectAndSpawn(trackingTarget.healthComponent.gameObject, null);
                soulTetherAttachmentAlly2.GetComponent<PaladinSoulTetherBodyAttachmentBase>().InitServer(trackingTarget.healthComponent.body, characterBody);
            }
        }
    }
}