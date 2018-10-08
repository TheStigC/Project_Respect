using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                if (!other.GetComponentInParent<PlayerMovement>().isInCar)
                {
                    other.gameObject.transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }
}
