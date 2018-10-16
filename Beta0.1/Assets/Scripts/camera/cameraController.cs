using UnityEngine;

public class cameraController : MonoBehaviour {

    public Transform target;
    public Vector3 camOffset;
    public float smoothSpeed = 0.1f;

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + camOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
