using RoR2;
using UnityEngine.Networking;
using EntityStates;
using UnityEngine;

namespace PaladinMod.States
{
    public class SpawnState : BaseState
    {
        public static float duration = 3f;

        private Transform modelTransform;
        private Animator animator;
        private bool isClay;
        private Material swordMat;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();
            this.modelTransform = base.GetModelTransform();
            this.isClay = false;

            if (NetworkServer.active) base.characterBody.AddBuff(BuffIndex.HiddenInvincibility);

            //this shouldn't be hardcoded in but i really just cba
            if (base.characterBody.skinIndex == 3) this.isClay = true;

            if (this.isClay)
            {
                base.PlayAnimation("Body", "SpawnClay", "Spawn.playbackRate", SpawnState.duration);
                Util.PlaySound(EntityStates.ClayBruiserMonster.SpawnState.spawnSoundString, base.gameObject);
                EffectManager.SimpleMuzzleFlash(EntityStates.ClayBruiserMonster.SpawnState.spawnEffectPrefab, base.gameObject, "Base", false);

                if (this.modelTransform)
                {
                    TemporaryOverlay overlay = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                    overlay.duration = SpawnState.duration;
                    overlay.animateShaderAlpha = true;
                    overlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    overlay.destroyComponentOnEnd = true;
                    overlay.originalMaterial = Resources.Load<Material>("Materials/matClayGooDebuff");
                    overlay.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());

                    PrintController printController = this.modelTransform.gameObject.AddComponent<PrintController>();
                    printController.printTime = EntityStates.ClayBruiserMonster.SpawnState.printDuration;
                    printController.enabled = true;
                    printController.startingPrintHeight = 0.3f;
                    printController.maxPrintHeight = 0.3f;
                    printController.startingPrintBias = EntityStates.ClayBruiserMonster.SpawnState.startingPrintBias;
                    printController.maxPrintBias = EntityStates.ClayBruiserMonster.SpawnState.maxPrintBias;
                    printController.disableWhenFinished = true;
                    printController.printCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
                }
            }
            else
            {
                base.PlayAnimation("Body", "Spawn", "Spawn.playbackRate", SpawnState.duration);
                Util.PlaySound(EntityStates.ParentMonster.SpawnState.spawnSoundString, base.gameObject);
                base.GetModelChildLocator().FindChild("SpawnEffect").gameObject.SetActive(true);

                if (this.modelTransform)
                {
                    TemporaryOverlay overlay = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                    overlay.duration = SpawnState.duration * 0.75f;
                    overlay.animateShaderAlpha = true;
                    overlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    overlay.destroyComponentOnEnd = true;
                    overlay.originalMaterial = Resources.Load<Material>("Materials/matHuntressFlashBright");
                    overlay.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());

                    PrintController printController = this.modelTransform.gameObject.AddComponent<PrintController>();
                    printController.printTime = EntityStates.ClayBruiserMonster.SpawnState.printDuration;
                    printController.enabled = true;
                    printController.startingPrintHeight = 0.3f;
                    printController.maxPrintHeight = 0.3f;
                    printController.startingPrintBias = EntityStates.ClayBruiserMonster.SpawnState.startingPrintBias;
                    printController.maxPrintBias = EntityStates.ClayBruiserMonster.SpawnState.maxPrintBias;
                    printController.disableWhenFinished = true;
                    printController.printCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
                }
            }

            if (this.animator)
            {
                this.animator.SetFloat(AnimationParameters.aimWeight, 0f);
            }

            if (base.characterBody)
            {
                Transform modelTransform = base.GetModelTransform();
                if (modelTransform)
                {
                    modelTransform.GetComponent<CharacterModel>().baseRendererInfos[0].defaultMaterial.SetFloat("_EmPower", StaticValues.maxSwordGlow);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (this.animator) this.animator.SetBool("inCombat", true);

            if (base.fixedAge >= SpawnState.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (this.animator)
            {
                this.animator.SetFloat(AnimationParameters.aimWeight, 1f);
            }

            if (NetworkServer.active) base.characterBody.RemoveBuff(BuffIndex.HiddenInvincibility);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}