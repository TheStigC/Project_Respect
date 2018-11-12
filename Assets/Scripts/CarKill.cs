using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Geo.Respect
{
    public class CarKill : MonoBehaviour
    {

        Rigidbody myRigibody;

        void Start()
        {
            myRigibody = GetComponentInParent<Rigidbody>();
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.tag != "HitCollider")
            {
                return;
            }
            else
            {
                if (myRigibody.IsSleeping() == false)
                {
                    if (other.GetComponentInParent<PlayerMovement>())
                    {
                        if (!other.GetComponentInParent<PlayerMovement>().isInCar)
                        {
                            other.gameObject.transform.parent.gameObject.SetActive(false);
                        }
                    }
                    else if (other.transform.parent.gameObject.tag == "AI")
                    {
                        other.gameObject.transform.parent.gameObject.SetActive(false);
                    }
                    else if (other.GetComponentInParent<PlayerMovementMultiplayer>())
                    {
                        if (!other.GetComponentInParent<PlayerMovementMultiplayer>().isInCar)
                        {
                            other.gameObject.transform.parent.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}