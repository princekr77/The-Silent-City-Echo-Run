using UnityEngine;
using System.Collections;

public class CameraFollowAndShake : MonoBehaviour
{
    [Header("Follow")]
    public Transform player;
    public Vector3 offset = new Vector3(0, 4, -8);
    public float smoothTime = 0.15f;

    [Header("Shake")]
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.15f;

    Vector3 velocity;
    Vector3 shakeOffset;
    Coroutine shakeRoutine;

    void LateUpdate()
    {
        if (!player) return;

        Vector3 basePos = new Vector3(
            player.position.x + offset.x,
            offset.y,
            player.position.z + offset.z
        );

        Vector3 smoothPos = Vector3.SmoothDamp(
            transform.position,
            basePos,
            ref velocity,
            smoothTime
        );

        transform.position = smoothPos + shakeOffset;
    }

    public void ShakeOnce()
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            shakeOffset = new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        shakeOffset = Vector3.zero;
    }
}
