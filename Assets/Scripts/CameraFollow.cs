using UnityEngine;
using System.Collections;

public class CameraFollowAndShake : MonoBehaviour
{
    [Header("Follow")]
    public Transform player;
    public Vector3 offset = new Vector3(0, 4, -8);
    public float smoothTime = 0.15f;

    [Header("Shake")]
    public float shakeDuration = 2f;
    public float shakeMagnitude = 0.35f;

    Vector3 velocity;
    Vector3 shakeOffset;

    bool hasShaken = false; // ensures shake happens only once

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

    public void ShakeForTime()
    {
        if (hasShaken) return; // 🔥 never shake again

        hasShaken = true;
        StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        float timer = 0f;

        while (timer < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            shakeOffset = new Vector3(x, y, 0f);

            timer += Time.deltaTime;
            yield return null;
        }

        // STOP SHAKE
        shakeOffset = Vector3.zero;
    }
}
