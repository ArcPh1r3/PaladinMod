using PaladinMod.Modules;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using VRAPI;

namespace PaladinMod.Misc
{
    public class PaladinVRController : MonoBehaviour
    {
        private PaladinSwordController swordController;

        void OnEnable()
        {
            if (PaladinPlugin.VRInstalled)
            {
                SubscribeToHandPairEvent();
            }
        }

        void OnDisable()
        {
            if (PaladinPlugin.VRInstalled)
            {
                UnsubscribeToHandPairEvent();
            }
        }

        private void SubscribeToHandPairEvent()
        {
            MotionControls.onHandPairSet += OnHandPairSet;
            On.EntityStates.EntityState.GetModelChildLocator += EditChildLocator;
            On.EntityStates.BaseState.GetAimRay += EditAimRay;
        }

        private void UnsubscribeToHandPairEvent()
        {
            MotionControls.onHandPairSet -= OnHandPairSet;
            On.EntityStates.EntityState.GetModelChildLocator -= EditChildLocator;
            On.EntityStates.BaseState.GetAimRay -= EditAimRay;
        }

        // replace ChildLocator for all the VFX
        private ChildLocator EditChildLocator(On.EntityStates.EntityState.orig_GetModelChildLocator orig, EntityStates.EntityState self)
        {
            if (PaladinPlugin.IsLocalVRPlayer(self.characterBody) && self.characterBody.name.Contains("RobPaladinBody"))
            {
                if (self is States.PaladinMain || self is States.Spell.ChannelHealZone|| self is States.Spell.ChannelTorpor || self is States.Spell.ChannelWarcry 
                    || self is States.Spell.ScepterChannelHealZone || self is States.Spell.ScepterChannelTorpor || self is States.Spell.ScepterChannelWarcry)

                    return MotionControls.dominantHand.transform.GetComponentInChildren<ChildLocator>();

                else if (self is States.ChargeLightningSpear || self is States.Spell.ChannelSmallHeal
                    || self is States.Spell.ChannelCruelSun || self is States.Spell.ScepterChannelCruelSun 
                    || self is States.Spell.ChannelCruelSunOld || self is States.Spell.ScepterChannelCruelSunOld)

                    return MotionControls.nonDominantHand.transform.GetComponentInChildren<ChildLocator>();
            }
            return orig(self);
        }

        private Ray EditAimRay(On.EntityStates.BaseState.orig_GetAimRay orig, EntityStates.BaseState self)
        {
            if (PaladinPlugin.IsLocalVRPlayer(self.characterBody) && self.characterBody.name.Contains("RobPaladinBody"))
            {
                if (self is States.LunarShards || self is States.Spell.ChannelSmallHeal || self is States.ThrowLightningSpear 
                    || self is States.Spell.ChannelCruelSun || self is States.Spell.ChannelCruelSunOld)
                    return MotionControls.nonDominantHand.aimRay;
            }
            return orig(self);
        }

        private void OnHandPairSet(CharacterBody body)
        {
            if (!body.name.Contains("RobPaladinBody") || GetComponent<CharacterBody>() != body) return;

            // Skin not loaded yet, wait a bit
            StartCoroutine(SetVRHands(body));
        }

        private IEnumerator<WaitForSeconds> SetVRHands(CharacterBody body)
        {
            yield return new WaitForSeconds(0.5f);

            body.aimOriginTransform = MotionControls.dominantHand.muzzle;

            ChildLocator vrWeaponChildLocator = MotionControls.dominantHand.transform.GetComponentInChildren<ChildLocator>();

            if (vrWeaponChildLocator)
            {
                PaladinPlugin.ReplaceVFXMaterials(vrWeaponChildLocator);

                swordController = base.GetComponent<PaladinSwordController>();
                swordController.lightningEffect = vrWeaponChildLocator.FindChild("SwordLightningEffect").GetComponentInChildren<ParticleSystem>();

                List<GameObject> allVRWeapons = new List<GameObject>()
                {
                    vrWeaponChildLocator.FindChild("PaladinHand").gameObject,
                    vrWeaponChildLocator.FindChild("ClayPaladinHand").gameObject,
                    vrWeaponChildLocator.FindChild("PoisonHand").gameObject,
                    vrWeaponChildLocator.FindChild("PaladinGMSexyHand").gameObject,
                    vrWeaponChildLocator.FindChild("LunarHand").gameObject,
                    vrWeaponChildLocator.FindChild("SpecterHand").gameObject,
                    vrWeaponChildLocator.FindChild("DripHand").gameObject,
                    vrWeaponChildLocator.FindChild("LunarKnightHand").gameObject,
                    vrWeaponChildLocator.FindChild("PaladinGMLegacyHand").gameObject,
                    vrWeaponChildLocator.FindChild("PoisonGMLegacyHand").gameObject
                };
                List<GameObject> allVROffHands = new List<GameObject>()
                {
                    vrWeaponChildLocator.FindChild("PaladinOffHand").gameObject,
                    vrWeaponChildLocator.FindChild("ClayPaladinOffHand").gameObject,
                    vrWeaponChildLocator.FindChild("PoisonOffHand").gameObject,
                    vrWeaponChildLocator.FindChild("PaladinGMSexyOffHand").gameObject,
                    vrWeaponChildLocator.FindChild("LunarOffHand").gameObject,
                    vrWeaponChildLocator.FindChild("SpecterOffHand").gameObject,
                    vrWeaponChildLocator.FindChild("BareOffHand").gameObject
                };

                foreach (GameObject weaponObject in allVRWeapons)
                {
                    weaponObject.SetActive(true);
                    weaponObject.SetActive(false);
                    weaponObject.transform.Find("TrailStart")?.gameObject.AddComponent<WeaponTrail>();
                }

                foreach (GameObject handObject in allVROffHands)
                {
                    handObject.SetActive(true);
                    handObject.SetActive(false);
                }

                if (Skins.isPaladinCurrentSkin(body, Skins.PaladinSkin.MASTERY))
                {
                    allVRWeapons[4].SetActive(true);
                    allVROffHands[4].SetActive(true);
                }
                else if (Skins.isPaladinCurrentSkin(body, Skins.PaladinSkin.GRANDMASTERY))
                {
                    allVRWeapons[3].SetActive(true);
                    allVROffHands[3].SetActive(true);
                }
                else if (Skins.isPaladinCurrentSkin(body, Skins.PaladinSkin.POISON))
                {
                    allVRWeapons[2].SetActive(true);
                    allVROffHands[2].SetActive(true);
                }
                else if (Skins.isPaladinCurrentSkin(body, Skins.PaladinSkin.CLAY))
                {
                    allVRWeapons[1].SetActive(true);
                    allVROffHands[1].SetActive(true);
                }
                else if (Skins.isPaladinCurrentSkin(body, Skins.PaladinSkin.SPECTER))
                {
                    allVRWeapons[5].SetActive(true);
                    allVROffHands[5].SetActive(true);
                }
                else if (Skins.isPaladinCurrentSkin(body, Skins.PaladinSkin.DRIP))
                {
                    allVRWeapons[6].SetActive(true);
                    allVROffHands[6].SetActive(true);
                }
                else if (Skins.isPaladinCurrentSkin(body, Skins.PaladinSkin.LUNARKNIGHT))
                {
                    allVRWeapons[7].SetActive(true);
                    allVROffHands[6].SetActive(true);
                }
                else if (Skins.isPaladinCurrentSkin(body, Skins.PaladinSkin.TYPHOONLEGACY))
                {
                    allVRWeapons[8].SetActive(true);
                    allVROffHands[6].SetActive(true);
                }
                else if (Skins.isPaladinCurrentSkin(body, Skins.PaladinSkin.POISONLEGACY))
                {
                    allVRWeapons[9].SetActive(true);
                    allVROffHands[6].SetActive(true);
                }
                else
                {
                    allVRWeapons[0].SetActive(true);
                    allVROffHands[0].SetActive(true);
                }
            }
        }

    }
}