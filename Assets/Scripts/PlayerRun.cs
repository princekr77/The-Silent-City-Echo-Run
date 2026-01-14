using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    public float speed = 6f;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
