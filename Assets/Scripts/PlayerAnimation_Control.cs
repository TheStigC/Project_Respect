using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation_Control : MonoBehaviour {

    public float walkSpeed = 2;
    public float runSpeed = 6;
    public float gravity = -12;
    public float jumpHeight = 1;
    [Range(0,1)]
    public float airControlPercent;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
    float vY;

    //Reference for the animatorController
    Animator playerAnim;

    //Reference for the Character Controller
    CharacterController controller;

	// Use this for initialization
	void Start () {
        playerAnim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        //input section
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDirection = input.normalized;
        bool running = Input.GetKey(KeyCode.LeftShift);

        //Calculate direction of the player
        if (inputDirection != Vector2.zero)
        {
            //Target Rotation
            float targetR = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetR, ref turnSmoothVelocity, SmoothOut(turnSmoothTime));
        }

        //Make the player move in the direction that he is facing
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDirection.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, SmoothOut(speedSmoothTime));

        vY += Time.deltaTime * gravity;
        Vector3 v = transform.forward * currentSpeed + Vector3.up * vY;

        //Move controller
        controller.Move(v * Time.deltaTime);

        //Wanting the character to stop moving when running into stuff
        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

        //Resetting vY
        if (controller.isGrounded)
        {
            vY = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        //animator
        float speedPercentAnimator = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
        playerAnim.SetFloat("SpeedPercent", speedPercentAnimator, speedSmoothTime, Time.deltaTime);
    }

    void Jump()
    {
        if (controller.isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            vY = jumpVelocity;
        }
    }

    float SmoothOut(float SmoothTime)
    {
        if (controller.isGrounded)
        {
            return SmoothTime;
        }

        if(airControlPercent == 0)
        {
            return float.MaxValue;
        }
        return SmoothTime / airControlPercent;
    }
}
