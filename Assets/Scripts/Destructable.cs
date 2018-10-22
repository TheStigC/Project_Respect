using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Destructable : MonoBehaviour
{

    public float startHealth = 50f;
    private float health;
    private bool isDead;
    public GameObject deathParticle;

    [Header("Unity Stuff")]
    public GameObject healthBarObject;
    public Image healthBarFill;


    void Start()
    {
        health = startHealth;
        if (healthBarObject != null)
        {
            healthBarObject.SetActive(false);
        }
    }


    public void TakeDamage(float amount)
    {
        health -= amount;
        if (healthBarObject != null)
        {
            StartCoroutine(ShowHealtBar());
            healthBarFill.fillAmount = health / startHealth;
        }

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }



    void Die()
    {
        isDead = true;
        SpawnDeathParticle();
        Destroy(gameObject);
    }

    void SpawnDeathParticle()
    {
        GameObject particleClone;
        particleClone = Instantiate(deathParticle, this.transform.position, this.transform.rotation) as GameObject;
        Destroy(particleClone, 3f);
    }

    IEnumerator ShowHealtBar()
    {
        healthBarObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        healthBarObject.SetActive(false);
    }
}
