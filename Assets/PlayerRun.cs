using UnityEngine;

public class PlayerRun : MonoBehaviour
{
    public float startSpeed = 6f;
    public float maxSpeed = 20f;
    public float speedIncreaseRate = 0.2f;
    public float sideSpeed = 5f;

    private float currentSpeed;

    void Start()
    {
        currentSpeed = startSpeed;
    }

    void Update()
    {
        // Forward auto-run
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        // Side movement
        float horizontal = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontal * sideSpeed * Time.deltaTime);

        // Gradual speed increase
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += speedIncreaseRate * Time.deltaTime;
        }
    }
}
