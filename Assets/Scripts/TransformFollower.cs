using UnityEngine;

public class TransformFollower : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        // Define a target position above and behind the target transform
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
    }
}