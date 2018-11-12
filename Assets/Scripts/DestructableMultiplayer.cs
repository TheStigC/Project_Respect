using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Com.Geo.Respect
{
    public class DestructableMultiplayer : MonoBehaviourPunCallbacks, IPunObservable
    {

        public float startHealth = 50f;
        public float health;
        private bool isDead;
        public GameObject deathParticle;

        public GameObject healthBarObject;
        public Image healthBarFill;


        #region IPunObservable implementation


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(health);
                stream.SendNext(healthBarFill);
                //stream.SendNext(this.gameObject.activeSelf);
            }
            else
            {
                // Network player, receive data
                this.health = (float)stream.ReceiveNext();
                this.healthBarFill = (Image)stream.ReceiveNext();
                //this.gameObject.SetActive((bool)stream.ReceiveNext());
            }
        }


        #endregion


        void Start()
        {

            health = startHealth;

            InvokeRepeating("FindHealthBar", 0.5f, 1);
            if (healthBarObject != null)
                healthBarObject.SetActive(false);

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
            if (photonView.IsMine)
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
        }



        void Die()
        {
            isDead = true;
            SpawnDeathParticle();
            StartCoroutine(DestroyGameObject(this.gameObject, 0.25f));
        }

        void SpawnDeathParticle()
        {
            GameObject particleClone;
            particleClone = PhotonNetwork.Instantiate(this.deathParticle.name, this.transform.position, this.transform.rotation) as GameObject;
            StartCoroutine(DestroyGameObject(particleClone, 3f));
        }

        IEnumerator ShowHealtBar()
        {
            healthBarObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            healthBarObject.SetActive(false);
        }

        IEnumerator DestroyGameObject(GameObject objectToDestroy, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            PhotonNetwork.Destroy(objectToDestroy);
        }
    }
}