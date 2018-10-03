using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{

    public float health = 50f;
    private bool isDead;

    void Start()
    {
    }


    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0 && !isDead)
        {
            Die();
        }
    }



    void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }
}
