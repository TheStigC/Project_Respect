using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTest3 : MonoBehaviour
{
    public Vector2 move;
    public Vector2 velocity; // in metres per second
    public float maxSpeed = 5.0f; // in metres per second
    public float acceleration = 5.0f; // in metres/second/second
    public float brake = 5.0f; // in metres/second/second
    public float turnSpeed = 30.0f; // in degrees/second
    private float speed = 0.0f;    // in metres/second
    Rigidbody rb;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // the horizontal axis controls the turn
        float turn = Input.GetAxis("Horizontal");



        // the vertical axis controls acceleration fwd/back
        float forwards = Input.GetAxis("Vertical");
        if (forwards > 0)
        {
            // accelerate forwards
            speed = speed + acceleration * Time.deltaTime;

            if (speed >= 1)
            {
                // turn the car
                transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
            }

        }
        else if (forwards < 0)
        {
            // accelerate backwards
            speed = speed - acceleration * Time.deltaTime;

            if (speed <= 1)
            {
                // turn the car
                transform.Rotate(0, turn * -turnSpeed * Time.deltaTime, 0);
            }
        }
        else
        {
            // braking
            if (speed > 0)
            {
                speed = speed - brake * Time.deltaTime;
            }
            else
            {
                speed = speed + brake * Time.deltaTime;
            }
        }

        // clamp the speed
        speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
        // compute a vector in the up direction of length speed
        Vector2 velocity = Vector3.right * speed;
        // move the object
        rb.velocity = (velocity * Time.deltaTime);
    }
}