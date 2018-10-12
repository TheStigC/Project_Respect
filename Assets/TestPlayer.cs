using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class TestPlayer : MonoBehaviour
{
    private Rigidbody rb;
    public float hInput = 0.0f;
    public float vInput = 0.0f;

    public float speed = 200.0f;
    public float turnRate = 3.0f;

    Quaternion targetRotation;

    private Transform myTransform;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetRotation = Quaternion.identity;

        myTransform = transform;
    }


    void Update()
    {
        hInput = CrossPlatformInputManager.GetAxis("Horizontal");
        vInput = CrossPlatformInputManager.GetAxis("Vertical");
    }


    void FixedUpdate()
    {
        Rotation(hInput, vInput);
        Movement(hInput, vInput);
    }

    void Rotation(float horizontal, float vertical)
    {
        float angle = Mathf.Atan2(hInput, vInput) * Mathf.Rad2Deg;
        float step = turnRate * Time.deltaTime;

        if (horizontal != 0 || vertical != 0)
        {
            targetRotation.eulerAngles = new Vector3(0, angle, 0);

            myTransform.rotation = Quaternion.RotateTowards(myTransform.rotation, targetRotation, step);
        }
    }

    void Movement(float horizontal, float vertical)
    {
        if (horizontal != 0.0f || vertical != 0.0f)
        {
            rb.AddForce(myTransform.forward * speed * Time.fixedDeltaTime);
        }
    }
}