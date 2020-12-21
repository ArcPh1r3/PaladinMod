using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PaladinMod.Misc
{
    public class PaladinGroundController : NetworkBehaviour
    {
        public float lifetime = 4f;
        private float pushForce;
        public CharacterBody body;
        private float mass;
        private Rigidbody rb;
        private CharacterMotor motor;
        private DamageInfo info;
        private float stopwatch;

        private void Start()
        {
            if (body)
            {
                if (body.characterMotor)
                {
                    motor = body.characterMotor;
                    mass = motor.mass;
                }
                else if (body.rigidbody)
                {
                    rb = body.rigidbody;
                    mass = rb.mass;
                }

                stopwatch = 0;
                lifetime = 5f;

                if (mass < 50f) mass = 50f;
                pushForce =  50f * mass;

                info = new DamageInfo
                {
                    attacker = null,
                    inflictor = null,
                    damage = 0,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    crit = false,
                    dotIndex = DotController.DotIndex.None,
                    force = Vector3.down * pushForce * Time.fixedDeltaTime,
                    position = base.transform.position,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 0
                };
            }
        }

        private void Knockdown()
        {
            if (NetworkServer.active)
            {
                info = new DamageInfo
                {
                    attacker = null,
                    inflictor = null,
                    damage = 0,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    crit = false,
                    dotIndex = DotController.DotIndex.None,
                    force = Vector3.down * pushForce * Time.fixedDeltaTime,
                    position = base.transform.position,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 0
                };

                if (motor)
                {
                    body.healthComponent.TakeDamageForce(info);
                }
                else if (rb)
                {
                    body.healthComponent.TakeDamageForce(info);
                }
            }
        }

        private void FixedUpdate()
        {
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= lifetime)
            {
                Destroy(this);
                return;
            }

            Knockdown();
        }
    }
}
