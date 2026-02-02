using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    Animator animator;
    CameraFollowAndShake cam;
    CharacterController controller;
    bool canHit = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        cam = Camera.main.GetComponent<CameraFollowAndShake>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!controller.isGrounded) return;

        if (hit.gameObject.CompareTag("obstacle") && canHit)
        {
            animator.SetTrigger("Hit");
            cam.ShakeOnce();
            canHit = false;
            Invoke(nameof(ResetHit), 0.6f);
        }
    }

    void ResetHit()
    {
        canHit = true;
    }
}
