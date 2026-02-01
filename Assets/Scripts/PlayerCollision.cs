using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Optional: check tag
        if (hit.gameObject.CompareTag("obstacle"))
        {
            animator.SetTrigger("Hit");
        }
    }
}
