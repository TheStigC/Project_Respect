using UnityEngine;

namespace Com.Geo.Respect
{
    public class CameraFollowerMultiplayer : MonoBehaviour
    {
        public float smoothTime = 0.3F;
        public Vector3 offset;
        private Vector3 velocity = Vector3.zero;


        // cached transform of the target
        Transform cameraTransform;

        // maintain a flag internally to reconnect if target is lost or camera is switched
        bool isFollowing;


        void LateUpdate()
        {
            if (cameraTransform == null && isFollowing)
            {
                OnStartFollowing();
            }

            if (isFollowing)
            {
                Apply();
            }
        }


        public void Apply()
        {


            // Define a target position above and behind the target transform
            Vector3 desiredPosition = this.transform.position + offset;

            // Smoothly move the camera towards that target position
            cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, desiredPosition, ref velocity, smoothTime);
        }


        public void OnStartFollowing()
        {
            cameraTransform = Camera.main.transform;
            isFollowing = true;
        }
    }
}