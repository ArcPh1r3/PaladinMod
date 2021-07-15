namespace PaladinMod.States.Emotes
{
    public class TestPose : BaseEmote
    {
        private bool posed;
        public override void OnEnter()
        {
            this.animString = "TestPose";
            this.animDuration = 1.6f;
            base.OnEnter();
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if (!posed && base.fixedAge > 0.225f) {
                posed = true;

                string effectString = Modules.Effects.GetSkinInfo(base.swordController.skinName).passiveEffectName;
                if (effectString != "") {

                    UnityEngine.ParticleSystem[] nigs = base.childLocator.FindChild(effectString).GetComponentsInChildren<UnityEngine.ParticleSystem>();
                    for (int n = 0; n < nigs.Length; n++) {
                        nigs[n].Play();
                    }
                }
            }
        }
    }
}