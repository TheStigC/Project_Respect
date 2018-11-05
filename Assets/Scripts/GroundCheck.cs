using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Geo.Respect
{
    public class GroundCheck : MonoBehaviour
    {

        public float distanceToGround = 0.5f;
        PlayerMovement playerMovement;
        PlayerMovementMultiplayer playerMovementMP;


        private void Start()
        {
            playerMovement = GetComponentInParent<PlayerMovement>();
            playerMovementMP = GetComponentInParent<PlayerMovementMultiplayer>();
        }



        private void FixedUpdate()
        {
            if (Physics.Raycast(transform.position, Vector3.down, distanceToGround))
            {

                if (playerMovement != null)
                    playerMovement.isGrounded = true;

                if (playerMovementMP != null)
                    playerMovementMP.isGrounded = true;
            }
            else
            {
                if (playerMovement != null)
                    playerMovement.isGrounded = false;

                if (playerMovementMP != null)
                    playerMovementMP.isGrounded = false;
            }
        }
    }
}