using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.Geo.Respect
{
    public class ProjectileMultiplayer : MonoBehaviourPunCallbacks
    {

        public float movementSpeed, damage, pushForce;

        Rigidbody myRigidbody;
        private Vector2 bulletsVelocity;


        void Awake()
        {
            myRigidbody = GetComponent<Rigidbody>();
        }


        void FixedUpdate()
        {
            myRigidbody.velocity = (transform.forward * (movementSpeed * 100) * Time.fixedDeltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            /*
            if (!photonView.IsMine)
            {
                return;
            }
            */
            if (other.gameObject.layer == 9)
            {
                if (other.tag == "HitCollider")
                {
                    if (other.GetComponentInParent<DestructableMultiplayer>() != null)
                    {
                        Debug.Log("GonnaDamageYaFool");
                        other.GetComponentInParent<DestructableMultiplayer>().TakeDamage(damage);
                    }


                    bulletsVelocity = myRigidbody.velocity.normalized;
                    //Push objects when hit
                    if (other.GetComponentInParent<Rigidbody>() != null)
                    {
                        other.GetComponentInParent<Rigidbody>().AddForce(bulletsVelocity * (pushForce * 100) * Time.fixedDeltaTime, ForceMode.Impulse);
                    }
                }
            }

            Destroy(this.gameObject);
        }
    }
}