using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Vehicles.Car;
using Photon.Pun;

namespace Com.Geo.Respect
{
    public class PlayerMovementMultiplayer : MonoBehaviourPunCallbacks, IPunObservable
    {

        public float speed = 5f, rotationSpeed = 450f, jumpForce = 2.0f, health;

        private Vector3 moveVelocity, moveInput, jump, networkPosition;
        private Quaternion targetRotation, networkRotation;
        private Rigidbody rigidBody;
        public bool canMove = true, isInCar = false, isGrounded = true, isFiring;
        public GameObject playerGraphics, interactingObject;
        public Camera gameCam;
        TransformFollower transformFollower;
        WeaponManagerMultiplayer weaponManagerMP;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;




        #region IPunObservable implementation


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                //   stream.SendNext(speed);
                //   stream.SendNext(rotationSpeed);
                //   stream.SendNext(jumpForce);
                stream.SendNext(health);
                stream.SendNext(moveVelocity);
                stream.SendNext(moveInput);
                //  stream.SendNext(jump);
                stream.SendNext(canMove);
                stream.SendNext(isFiring);
                stream.SendNext(this.rigidBody.position);
                stream.SendNext(this.rigidBody.rotation);
                stream.SendNext(this.rigidBody.velocity);
                stream.SendNext(this.rigidBody.isKinematic);
                stream.SendNext(canMove);
                stream.SendNext(isInCar);
            }
            else
            {
                // Network player, receive data
                // this.speed = (float)stream.ReceiveNext();
                // this.rotationSpeed = (float)stream.ReceiveNext();
                // this.jumpForce = (float)stream.ReceiveNext();
                this.health = (float)stream.ReceiveNext();
                this.moveVelocity = (Vector3)stream.ReceiveNext();
                this.moveInput = (Vector3)stream.ReceiveNext();
                // this.jump = (Vector3)stream.ReceiveNext();
                this.isFiring = (bool)stream.ReceiveNext();
                // rigidBody.position = (Vector3)stream.ReceiveNext();
                // rigidBody.rotation = (Quaternion)stream.ReceiveNext();
                // rigidBody.velocity = (Vector3)stream.ReceiveNext();
                this.rigidBody.isKinematic = (bool)stream.ReceiveNext();
                this.canMove = (bool)stream.ReceiveNext();
                this.isInCar = (bool)stream.ReceiveNext();


                networkPosition = (Vector3)stream.ReceiveNext();
                networkRotation = (Quaternion)stream.ReceiveNext();

                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
                // networkPosition += (this.rigidBody.velocity * lag);
            }
        }


        #endregion


        void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                PlayerMovementMultiplayer.LocalPlayerInstance = this.gameObject;
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }




        void Start()
        {

            jump = new Vector3(0.0f, 2.0f, 0.0f);
            gameCam = Camera.main;
            transformFollower = gameCam.GetComponent<TransformFollower>();

            if (photonView.IsMine)
            {
                weaponManagerMP = LocalPlayerInstance.GetComponentInChildren<WeaponManagerMultiplayer>();
                transformFollower.SetTarget(this.gameObject);
            }


#if UNITY_5_4_OR_NEWER
            // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
            {
                this.CalledOnLevelWasLoaded(scene.buildIndex);
            };
#endif
        }

        void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }
        }

        void Update()
        {
            if (photonView.IsMine)
            {
                ProcessInputs();
            }

            moveInput = new Vector3(CrossPlatformInputManager.GetAxisRaw("Horizontal"), 0, CrossPlatformInputManager.GetAxisRaw("Vertical"));
            moveVelocity = moveInput.normalized * speed;
        }

        void FixedUpdate()
        {
            if (!photonView.IsMine)
            {
                rigidBody.position = Vector3.MoveTowards(rigidBody.position, networkPosition, Time.fixedDeltaTime);
                rigidBody.rotation = Quaternion.RotateTowards(rigidBody.rotation, networkRotation, Time.fixedDeltaTime * 100.0f);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (photonView.IsMine)
            {
                //ENTER/LEAVE CAR
                if (other.tag == "Car")
                {
                    if (!isInCar)
                    {
                        interactingObject = other.gameObject;
                    }

                    //IF WE WANT TO ENTER CAR
                    if (canMove && !isInCar)
                    {
                        if (CrossPlatformInputManager.GetButtonDown("Use1"))
                        {
                            //Activate Car controls
                            StartCoroutine(InteractionCooldown());
                            other.GetComponent<CarMovementMultiplayer>().isControlledByPlayer = true;

                            //Deactivate Playermovement and set car as parent
                            rigidBody.isKinematic = true;
                            //this.GetComponent<PlayerMovement>().enabled = false;
                            //this.transform.SetParent(other.transform);

                            //Deactivate Playervisuals (and shooting)
                            playerGraphics.SetActive(false);

                            //Update camera target
                            transformFollower.SetTarget(interactingObject);

                            gameCam.fieldOfView = 100;

                            if (other.GetComponent<EnemyPatrol>() != null)
                            {
                                other.GetComponent<EnemyPatrol>().enabled = false;
                            }

                            other.GetComponent<PhotonView>().RequestOwnership();
                            Debug.Log("Just ENTERED the car!");
                        }
                    }
                }


            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (photonView.IsMine)
            {
                //ENTER/LEAVE CAR
                if (other.tag == "Car" && !isInCar)
                {
                    interactingObject = null;
                }
            }
        }

        IEnumerator InteractionCooldown()
        {
            canMove = true;
            isInCar = false;
            yield return new WaitForSeconds(0.15f);
            canMove = false;
            isInCar = true;
        }


        IEnumerator ExitCar(GameObject objectToExit)
        {
            //Turn off Car controls
            //      objectToExit.GetComponent<CarController>().isControlledByPlayer = false;
            objectToExit.GetComponent<CarMovementMultiplayer>().isControlledByPlayer = false;
            //      objectToExit.GetComponent<CarUserControl>().enabled = false;
            objectToExit.tag = "Car";

            //Change Camera back to normal
            Camera.main.fieldOfView = 60;

            //this.GetComponent<PlayerMovement>().enabled = false;
            //this.transform.SetParent(null);
            this.transform.position = objectToExit.GetComponent<CarMovementMultiplayer>().spawnPosition.transform.position;

            //Reactivate Playervisuals (and shooting)
            playerGraphics.SetActive(true);


            //Update camera target
            transformFollower.SetTarget(this.gameObject);

            yield return new WaitForSeconds(0.2f);

            //Reactivate Playermovement and set car as parent
            this.isInCar = false;
            this.canMove = true;
            rigidBody.isKinematic = false;



            Debug.Log("Just EXITED the car!");

        }

        void ProcessInputs()
        {


            if (CrossPlatformInputManager.GetButtonDown("Fire1"))
            {
                if (!isFiring)
                {

                    isFiring = true;
                    if (photonView.IsMine)
                    {
                        weaponManagerMP.ShootWeapon();
                    }
                }
            }
            if (CrossPlatformInputManager.GetButtonUp("Fire1"))
            {
                if (isFiring)
                {
                    isFiring = false;
                }
            }



            if (CrossPlatformInputManager.GetButtonDown("Jump") && isGrounded)
            {
                rigidBody.AddForce(jump * jumpForce, ForceMode.Impulse);
            }

            //Update movement
            if (canMove)
            {
                rigidBody.MovePosition(rigidBody.position + moveVelocity * Time.fixedDeltaTime);

                if (moveInput != Vector3.zero)
                {
                    targetRotation = Quaternion.LookRotation(moveInput);
                    transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.fixedDeltaTime);
                }
            }


            if (CrossPlatformInputManager.GetButtonDown("Use1"))
            {
                if (!canMove && isInCar)
                {
                    StartCoroutine(ExitCar(interactingObject));
                }
            }
        }
    }
}
