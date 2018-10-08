using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    PlayerMovement player;


    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }



    private void OnCollisionEnter(Collision collision)
    {
        player.isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        player.isGrounded = false;
    }
}
