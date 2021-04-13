using EntityStates.GrandParent;
using PaladinMod.Misc;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.States.Spell
{
    public class CastCruelSun : BaseCastChanneledSpellState
    {
        public static float sunPrefabDiameter = 10f;

        private GameObject sunInstance;
        public Vector3? sunSpawnPosition;
        private PaladinSwordController swordController;

        public override void OnEnter()
        {
            this.baseDuration = 15f;
            this.overrideDuration = 15f;
            this.muzzleflashEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/ExplosionSolarFlare");
            this.projectilePrefab = null;
            this.castSoundString = Modules.Sounds.CastTorpor;
            this.swordController = base.gameObject.GetComponent<PaladinSwordController>();
            
            base.OnEnter();

            if (NetworkServer.active)
            {
                Vector3? vector = this.sunSpawnPosition;

                float oldMinDistance = ChannelSun.sunPlacementMinDistance;
                float oldIdealAltitude = ChannelSun.sunPlacementIdealAltitudeBonus;

                ChannelSun.sunPlacementMinDistance = 0f;
                ChannelSun.sunPlacementIdealAltitudeBonus = 0f;

                this.sunSpawnPosition = this.swordController.sunPosition + Vector3.up;//((vector != null) ? vector : ChannelSun.FindSunSpawnPosition(this.spellPosition));
                if (this.sunSpawnPosition != null)
                {
                    this.sunInstance = this.CreateSun(this.sunSpawnPosition.Value);
                }

                ChannelSun.sunPlacementMinDistance = oldMinDistance;
                ChannelSun.sunPlacementIdealAltitudeBonus = oldIdealAltitude;
            }
        }

        public override void OnExit()
        {
            if (NetworkServer.active && this.sunInstance)
            {
                this.sunInstance.GetComponent<GenericOwnership>().ownerObject = null;
                this.sunInstance = null;
            }

            base.OnExit();
        }

        private GameObject CreateSun(Vector3 sunSpawnPosition)
        {
            GameObject sun = UnityEngine.Object.Instantiate<GameObject>(ChannelSun.sunPrefab, sunSpawnPosition, Quaternion.identity);
            sun.GetComponent<GenericOwnership>().ownerObject = base.gameObject;
            NetworkServer.Spawn(sun);
            return sun;
        }
    }
}