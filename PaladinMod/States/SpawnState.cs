using RoR2;
using UnityEngine.Networking;
using EntityStates;
using UnityEngine;
using PaladinMod.Misc;

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
            ChildLocator childLocator = base.GetModelChildLocator();

            if (NetworkServer.active) base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, SpawnState.duration * 1.5f);

            if (base.characterBody.skinIndex == PaladinPlugin.claySkinIndex) this.isClay = true;

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
                Debug.LogWarning("n");
                base.PlayAnimation("Body", "Spawn", "Spawn.playbackRate", SpawnState.duration);
                Util.PlaySound(EntityStates.ParentMonster.SpawnState.spawnSoundString, base.gameObject);
                if (childLocator.FindChild("SpawnEffect")) childLocator.FindChild("SpawnEffect").gameObject.SetActive(true);
                Debug.LogWarning("n");

                if (this.modelTransform)
                Debug.LogWarning("n");
                {
                    TemporaryOverlay overlay = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                    overlay.duration = SpawnState.duration * 0.75f;
                    overlay.animateShaderAlpha = true;
                    overlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    overlay.destroyComponentOnEnd = true;
                    overlay.originalMaterial = Resources.Load<Material>("Materials/matHuntressFlashBright");
                    overlay.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
                    Debug.LogWarning("n");

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
            Debug.LogWarning("n");

            if (this.animator)
            {
                this.animator.SetFloat(AnimationParameters.aimWeight, 0f);
            }

            if (this.modelTransform)
            {
                this.swordMat = modelTransform.GetComponent<CharacterModel>().baseRendererInfos[0].defaultMaterial;
            }
            Debug.LogWarning("n");

            if (childLocator)
            {
                childLocator.FindChild("SwordActiveEffect").gameObject.SetActive(false);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (this.animator) this.animator.SetBool("inCombat", true);
            if (this.swordMat) this.swordMat.SetFloat("_EmPower", StaticValues.maxSwordGlow);

            if (base.fixedAge >= SpawnState.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (this.animator)
            {
                this.animator.SetFloat(AnimationParameters.aimWeight, 1f);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}