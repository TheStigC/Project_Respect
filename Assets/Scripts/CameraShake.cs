using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{

    public Camera playerCamera;
    private Transform cameraStartPosition;
    private Vector3 camPos;

    float shakeAmount = 0;

    void Awake()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    void Start()
    {
        cameraStartPosition = playerCamera.transform;
    }

    public void Shake(float amt, float length, Camera thePlayersCamera)
    {
        if (thePlayersCamera != null)
        {
            playerCamera = thePlayersCamera;

            shakeAmount = amt;
            InvokeRepeating("DoShake", 0, 0.01f);
            Invoke("StopShake", length);
        }
    }

    void DoShake()
    {
        if (shakeAmount > 0)
        {
            camPos = playerCamera.transform.position;

            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetZ = Random.value * shakeAmount * 2 - shakeAmount;
            camPos.x += offsetX;
            camPos.z += offsetZ;

            playerCamera.transform.position = camPos;
        }
    }

    void StopShake()
    {
        CancelInvoke("DoShake");
        playerCamera.transform.position = camPos;
    }

}
