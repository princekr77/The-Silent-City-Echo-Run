using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 4, -8);
    public float smoothTime = 0.15f;

    private Vector3 velocity;

    void LateUpdate()
    {
        Vector3 targetPos = new Vector3(
            player.position.x + offset.x,
            offset.y,                    // LOCK Y
            player.position.z + offset.z
        );

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothTime
        );
    }
}
