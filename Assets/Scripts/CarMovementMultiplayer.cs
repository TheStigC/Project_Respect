using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Photon.Pun;

namespace Com.Geo.Respect
{
    public class CarMovementMultiplayer : MonoBehaviourPunCallbacks, IPunObservable
    {

        public float speed = 5f, rotationSpeed = 450f;

        private Vector3 moveVelocity, moveInput, networkPosition;
        private Quaternion targetRotation, networkRotation;
        private Rigidbody rigidBody;
        public bool isControlledByPlayer = false, firstTimeEnteringCar = true;
        public GameObject spawnPosition, enemyPrefab;



        #region IPunObservable implementation


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data

                stream.SendNext(moveVelocity);
                stream.SendNext(moveInput);
                stream.SendNext(this.rigidBody.position);
                stream.SendNext(this.rigidBody.rotation);
                stream.SendNext(this.rigidBody.velocity);
                stream.SendNext(firstTimeEnteringCar);
                stream.SendNext(isControlledByPlayer);
            }
            else
            {
                // Network player, receive data
                this.moveVelocity = (Vector3)stream.ReceiveNext();
                this.moveInput = (Vector3)stream.ReceiveNext();
                this.firstTimeEnteringCar = (bool)stream.ReceiveNext();
                this.isControlledByPlayer = (bool)stream.ReceiveNext();

                rigidBody.position = (Vector3)stream.ReceiveNext();
                rigidBody.rotation = (Quaternion)stream.ReceiveNext();
                rigidBody.velocity = (Vector3)stream.ReceiveNext();


                networkPosition = (Vector3)stream.ReceiveNext();
                networkRotation = (Quaternion)stream.ReceiveNext();

                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
                // networkPosition += (this.rigidBody.velocity * lag);
            }
        }


        #endregion






        void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
        }



        void Update()
        {
            if (photonView.IsMine)
            {
                if (isControlledByPlayer)
                {
                    ProcessInputs();
                }

            }

            moveInput = new Vector3(CrossPlatformInputManager.GetAxisRaw("Horizontal"), 0, CrossPlatformInputManager.GetAxisRaw("Vertical"));
            moveVelocity = moveInput.normalized * speed;
        }


        private void FixedUpdate()
        {
            if (isControlledByPlayer)
            {
                if (firstTimeEnteringCar)
                {
                    GameObject enemyInstance;
                    enemyInstance = PhotonNetwork.InstantiateSceneObject(enemyPrefab.name, spawnPosition.transform.position, spawnPosition.transform.rotation);
                    firstTimeEnteringCar = false;
                }


                /*
                if (moveInput != Vector3.zero)
                {

                    float translation = CrossPlatformInputManager.GetAxis("Vertical") * speed;
                    float rotation = CrossPlatformInputManager.GetAxis("Horizontal") * rotationSpeed;
                    translation *= Time.deltaTime;
                    rotation *= Time.deltaTime;
                    transform.Translate(0, 0, translation);
                    transform.Rotate(0, rotation, 0);
                }
                */

                if (!photonView.IsMine)
                {
                    rigidBody.position = Vector3.MoveTowards(rigidBody.position, networkPosition, Time.fixedDeltaTime);
                    rigidBody.rotation = Quaternion.RotateTowards(rigidBody.rotation, networkRotation, Time.fixedDeltaTime * 100.0f);
                }
            }
        }

        void ProcessInputs()
        {

            rigidBody.MovePosition(rigidBody.position + moveVelocity * Time.fixedDeltaTime);

            if (moveInput != Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(moveInput);
                transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.fixedDeltaTime);
            }
        }

        /*
        public void OnOwnershipRequest(int playerID)
        {
            Debug.Log("The ID is: " + playerID);
            photonView.TransferOwnership(playerID);
        }
        */
    }
}