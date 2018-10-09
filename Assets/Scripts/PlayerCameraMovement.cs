using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{
    [Header("General behaviour")]
    public float cameraSpeed;
    public float cursorOffsetFactor;
    public float cursorMaxOffset;
    public GameObject target;

    [Header("Player Magnitude settings")]
    public bool isOffsetByPlayerSpeed;
    public float playerSpeedOffsetFactor;
    public float playerSpeedMaxOffset;
    public float playerSpeedThreshold;

    private Vector3 offsetToTarget;
    private Camera playerCamera;
    private Rigidbody targetRb;

    void Start()
    {
        playerCamera = GetComponent<Camera>();
        offsetToTarget = playerCamera.transform.position - target.transform.position;
        targetRb = target.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 offset = target.transform.position + offsetToTarget;

        if (isOffsetByPlayerSpeed && playerSpeedThreshold < targetRb.velocity.magnitude)
        {
            float currentMagnitude = Mathf.Clamp(targetRb.velocity.magnitude * playerSpeedOffsetFactor, 0f, playerSpeedMaxOffset);
            float radX = Mathf.Deg2Rad * playerCamera.transform.rotation.eulerAngles.x;
            float speedY = Mathf.Sin(radX) * currentMagnitude;
            float speedZ = Mathf.Cos(radX) * currentMagnitude;
            offset.z -= speedZ;
            offset.y += speedY;
        }

        RaycastHit cursorHit;
        Ray cameraRay = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(cameraRay, out cursorHit))
        {
            Vector3 scaledHit = (cursorHit.point - target.transform.position) * cursorOffsetFactor;
            if (scaledHit.magnitude > cursorMaxOffset)
            {
                scaledHit.Normalize();
                scaledHit *= cursorMaxOffset;
            }
            offset += scaledHit;
        }

        playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, offset, cameraSpeed * Time.deltaTime);
    }
}
