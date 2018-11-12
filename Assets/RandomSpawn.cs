using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.Geo.Respect
{
    public class RandomSpawn : MonoBehaviourPunCallbacks, IPunObservable
    {

        public GameObject prefabToSpawn;
        public List<Transform> spawnPoints;
        public int amountToSpawn = 5;



        #region IPunObservable implementation        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(amountToSpawn);

            }
            else
            {
                this.amountToSpawn = (int)stream.ReceiveNext();
            }
        }
        #endregion



        void Start()
        {
            StartCoroutine(SpawnPrefab(amountToSpawn));
        }


        IEnumerator SpawnPrefab(int spawnAmount)
        {
            int i = 0;
            while (i < spawnAmount)
            {
                int randomPosition = Random.Range(0, spawnPoints.Count);
                PhotonNetwork.InstantiateSceneObject(this.prefabToSpawn.name, spawnPoints[randomPosition].transform.position, this.transform.rotation);
                yield return 0; //Wait 1 Frame
                spawnAmount--;
            }
        }
    }
}