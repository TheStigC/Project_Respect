﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 5f, rotationSpeed = 450f, jumpForce = 2.0f;

    private Vector3 moveVelocity, moveInput, jump;
    private Quaternion targetRotation;
    private Rigidbody rigidBody;
    public bool canMove = true, isInCar = false, isGrounded = true;
    public GameObject playerGraphics, interactingObject;
    private Camera gameCam;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        gameCam = Camera.main;
        jump = new Vector3(0.0f, 2.0f, 0.0f);
    }



    void Update()
    {
        
        moveInput = new Vector3(CrossPlatformInputManager.GetAxisRaw("Horizontal"), 0, CrossPlatformInputManager.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            
            rigidBody.AddForce(jump * jumpForce, ForceMode.Impulse);
        }
    }


    private void FixedUpdate()
    {
        //Update movement
        if (canMove)
        {
            rigidBody.MovePosition(rigidBody.position + moveVelocity * Time.fixedDeltaTime);

            if (moveInput != Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(moveInput);
                transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
            }
        }
        else if (!canMove && isInCar)
        {
            this.transform.position = interactingObject.transform.position;

            if (CrossPlatformInputManager.GetButtonDown("Use1"))
            {
                StartCoroutine(ExitCar(interactingObject));
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {

        //ENTER/LEAVE CAR
        if (other.tag == "Car")
        {
            interactingObject = other.gameObject;

            //IF WE WANT TO ENTER CAR
            if (canMove && !isInCar)
            {
                if (CrossPlatformInputManager.GetButtonDown("Use1"))
                {
                    //Activate Car controls
                    other.GetComponent<CarMovement>().isControlledByPlayer = true;
                    other.tag = "Player";

                    //Deactivate Playermovement and set car as parent
                    canMove = false;
                    isInCar = true;
                    rigidBody.isKinematic = true;
                    //this.GetComponent<PlayerMovement>().enabled = false;
                    this.transform.SetParent(other.transform);

                    //Deactivate Playervisuals (and shooting)
                    playerGraphics.SetActive(false);

                    //Update camera target
                    gameCam.GetComponent<TransformFollower>().target = other.transform;

                    if (other.GetComponent<EnemyPatrol>()!=null)
                    {
                        other.GetComponent<EnemyPatrol>().enabled = false;
                    }

                    Debug.Log("Just ENTERED the car!");
                }
            }
        }
    }


    IEnumerator ExitCar(GameObject objectToExit)
    {
        //Turn off Car controls
        objectToExit.GetComponent<CarMovement>().isControlledByPlayer = false;
        objectToExit.tag = "Car";


        //this.GetComponent<PlayerMovement>().enabled = false;
        this.transform.SetParent(null);
        this.transform.position = objectToExit.GetComponent<CarMovement>().spawnPosition.transform.position;

        //Deactivate Playervisuals (and shooting)
        playerGraphics.SetActive(true);

        canMove = true;

        //Update camera target
        gameCam.GetComponent<TransformFollower>().target = this.transform;

        yield return new WaitForSeconds(0.2f);

        //Deactivate Playermovement and set car as parent

        isInCar = false;
        rigidBody.isKinematic = false;

        Debug.Log("Just EXITED the car!");

    }
}
