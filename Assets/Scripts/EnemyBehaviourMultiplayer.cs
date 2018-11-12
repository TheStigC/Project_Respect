using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

namespace Com.Geo.Respect
{
    public class EnemyBehaviourMultiplayer : MonoBehaviourPunCallbacks, IPunObservable
    {
        public float speed, stoppingDistance, retreatDistance, attackSpeed, rotationSpeed;
        private Vector3 position;
        private Quaternion lookRotation;
        private float timeBetweenShots;
        public GameObject player;
        public GameObject projectile, firePoint;
        NavMeshAgent agent;


        #region IPunObservable implementation


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(position);
                stream.SendNext(lookRotation);
                stream.SendNext(timeBetweenShots);
            }
            else
            {
                // Network player, receive data
                this.position = (Vector3)stream.ReceiveNext();
                this.lookRotation = (Quaternion)stream.ReceiveNext();
                this.timeBetweenShots = (float)stream.ReceiveNext();
            }
        }

        #endregion




        void Start()
        {

            player = GameObject.FindGameObjectWithTag("Player");
            agent = GetComponent<NavMeshAgent>();
            timeBetweenShots = attackSpeed;

            //Random tal er ikke nødvendigt, det er bare en test om de så finder forskellige mål
            float waitTime = Random.Range(3, 5);
            InvokeRepeating("LookForPlayer", 0.5f, waitTime);
        }


        void Update()
        {
            if (player != null && photonView.IsMine)
            {
                FollowTarget(player.GetComponent<Target>());
            }
            /*  //FIND PLAYER ROTATION AND ROTATE TOWARDS HIM
              Vector3 targetDir = player.position - transform.position;
              float step = rotationSpeed * Time.deltaTime;
              Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
              //RETREAT
              if (Vector3.Distance(transform.position, player.position) <= retreatDistance)
              {
                  transform.position = Vector3.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
                  transform.rotation = Quaternion.LookRotation(newDir);
              }*/


            //SHOOT
            if (timeBetweenShots <= 0)
            {
                GameObject projectileClone;
                projectileClone = PhotonNetwork.InstantiateSceneObject(this.projectile.name, firePoint.transform.position, this.transform.rotation) as GameObject;
                timeBetweenShots = attackSpeed;
                StartCoroutine(DestroyGameObject(projectileClone, 3f));
            }
            else
            {
                timeBetweenShots -= Time.deltaTime;
            }

        }

        void FixedUpdate()
        {
            if (!photonView.IsMine)
            {
                position = this.transform.position;
            }
        }

        void LookForPlayer()
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            else
                CancelInvoke();
        }

        void SetFocus()
        {
            FollowTarget(player.GetComponent<Target>());
            player.GetComponent<Target>().OnFococused(transform);
        }


        public void FollowTarget(Target newTarget)
        {
            agent.SetDestination(player.transform.position);
            FaceTarget();
            agent.stoppingDistance = newTarget.radius * .8f;
            agent.updateRotation = false;
            // player.transform = newTarget.playerTransform;
        }
        void FaceTarget()
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        IEnumerator DestroyGameObject(GameObject objectToDestroy, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            PhotonNetwork.Destroy(objectToDestroy);
        }
    }
}