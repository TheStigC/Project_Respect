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


    void OnTriggerEnter (Collider other)
    {
        if (myRigibody.IsSleeping()==false)
        {
            if (other.transform.GetChild(1).tag == "HitCollider" && !other.GetComponent<PlayerMovement>().isInCar)
            {
                Debug.Log("Lol");
                other.gameObject.SetActive(false);
            }
        }
    }
}
