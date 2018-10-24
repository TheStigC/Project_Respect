using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float movementSpeed, damage;

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
        if (other.gameObject.layer == 9)
        {
            if (other.tag == "HitCollider")
            {
                if (other.GetComponentInParent<Destructable>() != null)
                {
                    other.GetComponentInParent<Destructable>().TakeDamage(damage);
                }
                Debug.Log("Deal damage to player!");


                bulletsVelocity = myRigidbody.velocity.normalized;
                //Push objects when hit
                if (other.GetComponentInParent<Rigidbody>() != null)
                {
                    other.GetComponentInParent<Rigidbody>().AddForce(bulletsVelocity * (movementSpeed * 30) * Time.fixedDeltaTime, ForceMode.Impulse);
                }
            }
        }

        Destroy(this.gameObject);
    }
}