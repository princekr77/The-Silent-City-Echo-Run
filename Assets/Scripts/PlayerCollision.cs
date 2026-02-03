using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    Animator animator;
    CharacterController controller;

    public CameraFollowAndShake cam;

    bool isDead = false; // prevents multiple hits

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isDead) return;

        if (hit.gameObject.CompareTag("obstacle"))
        {
            isDead = true;

            // Play hit animation
            animator.SetTrigger("Hit");

            // Shake camera once
            if (cam != null)
                cam.ShakeForTime();

            // Stop player instantly
            controller.enabled = false;

            // Call game over (optional delay)
            Invoke(nameof(GameOver), 1.5f);
        }
    }

    void GameOver()
    {
        Debug.Log("GAME OVER");
        // Add Game Over UI here later
    }
}
