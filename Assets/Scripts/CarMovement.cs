﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CarMovement : MonoBehaviour
{

    public float speed = 5f, rotationSpeed = 450f;

    private Vector3 moveVelocity, moveInput;
    private Quaternion targetRotation;
    private Rigidbody rigidBody;
    public bool isControlledByPlayer = false;
    public GameObject spawnPosition;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }



    void Update()
    {

        moveInput = new Vector3(CrossPlatformInputManager.GetAxisRaw("Horizontal"), 0, CrossPlatformInputManager.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;
    }


    private void FixedUpdate()
    {
        if (isControlledByPlayer)
        {
            //rigidBody.MovePosition(rigidBody.position + moveVelocity * Time.fixedDeltaTime);

            if (moveInput != Vector3.zero)
            {

                float translation = CrossPlatformInputManager.GetAxis("Vertical") * speed;
                float rotation = CrossPlatformInputManager.GetAxis("Horizontal") * rotationSpeed;
                translation *= Time.deltaTime;
                rotation *= Time.deltaTime;
                transform.Translate(0, 0, translation);
                transform.Rotate(0, rotation, 0);
            }
        }
    }
}
