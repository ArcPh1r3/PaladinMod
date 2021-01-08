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
            if (this.body)
            {
                if (this.body.characterMotor)
                {
                    this.motor = this.body.characterMotor;
                    this.mass = this.motor.mass;
                }
                else if (this.body.rigidbody)
                {
                    this.rb = this.body.rigidbody;
                    this.mass = this.rb.mass;
                }

                this.stopwatch = 0;
                this.lifetime = 5f;

                if (this.mass < 50f) this.mass = 50f;
                this.pushForce =  50f * this.mass;

                this.info = new DamageInfo
                {
                    attacker = null,
                    inflictor = null,
                    damage = 1,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    crit = false,
                    dotIndex = DotController.DotIndex.None,
                    force = Vector3.down * this.pushForce * Time.fixedDeltaTime,
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
                this.info = new DamageInfo
                {
                    attacker = null,
                    inflictor = null,
                    damage = 0,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    crit = false,
                    dotIndex = DotController.DotIndex.None,
                    force = Vector3.down * this.pushForce * Time.fixedDeltaTime,
                    position = base.transform.position,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 0
                };

                if (this.motor)
                {
                    this.body.healthComponent.TakeDamageForce(this.info);
                }
                else if (this.rb)
                {
                    this.body.healthComponent.TakeDamageForce(this.info);
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
