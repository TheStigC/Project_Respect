using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Geo.Respect
{
    public class GroundCheck : MonoBehaviour
    {
        PlayerMovement player;


        void Start()
        {
            player = FindObjectOfType<PlayerMovement>();
        }



        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                player.isGrounded = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                player.isGrounded = false;
            }
        }
    }
}