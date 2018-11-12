using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.Geo.Respect
{
    public class CarKillMultiplayer : MonoBehaviour
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

                    if (other.transform.parent.gameObject.tag == "AI")
                    {
                        if (PhotonNetwork.OfflineMode == true)
                            Destroy(other.gameObject.transform.parent.gameObject);
                        else
                            StartCoroutine(DestroyGameObject(other.gameObject.transform.parent.gameObject, 0.25f));
                    }

                    else if (other.GetComponentInParent<PlayerMovementMultiplayer>())
                    {
                        if (PhotonNetwork.OfflineMode == true)
                            Destroy(other.gameObject.transform.parent.gameObject);

                        else if (!other.GetComponentInParent<PlayerMovementMultiplayer>().isInCar)
                        {
                            StartCoroutine(DestroyGameObject(other.gameObject.transform.parent.gameObject, 0.25f));
                        }
                    }
                }
            }
        }


        IEnumerator DestroyGameObject(GameObject objectToDestroy, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            PhotonNetwork.Destroy(objectToDestroy);
        }
    }
}