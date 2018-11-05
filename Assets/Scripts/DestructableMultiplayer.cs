using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Geo.Respect
{
    public class DestructableMultiplayer : MonoBehaviour
    {

        public float startHealth = 50f;
        public float health;
        private bool isDead;
        public GameObject deathParticle;

        public GameObject healthBarObject;
        public Image healthBarFill;


        void Start()
        {
            health = startHealth;

            InvokeRepeating("FindHealthBar", 0.5f, 1);
        }

        void FindHealthBar()
        {
            if (healthBarObject == null)
            {
                healthBarObject = GameObject.Find("PlayerHealthBarBG");

                if (healthBarObject != null)
                {
                    Debug.Log("FUNDET");
                    healthBarFill = healthBarObject.transform.GetChild(0).GetComponent<Image>();
                    Debug.Log(healthBarFill);
                    healthBarObject.SetActive(false);
                }
            }
            else
                CancelInvoke();
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
}