using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Forward Movement")]
    public float runSpeed = 6f;
    public float speedIncreaseRate = 0.2f;
    public float maxSpeed = 15f;

    [Header("Lane Movement")]
    public float laneDistance = 3f;
    public float laneChangeSpeed = 8f;
    private int currentLane = 1;
    private float startX;

    [Header("Jump & Gravity")]
    public float jumpForce = 6f;
    public float gravity = -25f;
    private float verticalVelocity;

    [Header("Slide")]
    public float slideDuration = 0.8f;
    public float slideHeight = 1f;
    private float originalHeight;
    private Vector3 originalCenter;
    private bool isSliding;

    [Header("References")]
    public Animator animator;

    private CharacterController controller;
    private bool isRunning;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (!animator) animator = GetComponent<Animator>();
    }

    void Start()
    {
        startX = transform.position.x;
        originalHeight = controller.height;
        originalCenter = controller.center;

        StartCoroutine(StartRunningAfterDelay());
    }

    IEnumerator StartRunningAfterDelay()
    {
        yield return new WaitForSeconds(0.2f);
        isRunning = true;
        animator.SetBool("isRunning", true);
    }

    void Update()
    {
        Vector3 move = Vector3.zero;

        // -------- FORWARD --------
        if (isRunning)
        {
            move.z = runSpeed * Time.deltaTime;
            if (runSpeed < maxSpeed)
                runSpeed += speedIncreaseRate * Time.deltaTime;
        }

        // -------- LANE MOVEMENT (FIXED) --------
        float targetX = startX + (currentLane - 1) * laneDistance;
        float diffX = targetX - transform.position.x;

        move.x = diffX * laneChangeSpeed * Time.deltaTime;

        if (Mathf.Abs(diffX) < 0.02f)
            move.x = 0f;

        // -------- GROUND / JUMP --------
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -2f;

            if (Input.GetKeyDown(KeyCode.Space))
                Jump();
        }

        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity * Time.deltaTime;

        controller.Move(move);

        // -------- INPUT --------
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            ChangeLane(-1);

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            ChangeLane(1);

        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && !isSliding)
            StartCoroutine(Slide());
    }

    void ChangeLane(int dir)
    {
        currentLane = Mathf.Clamp(currentLane + dir, 0, 2);
    }

    void Jump()
    {
        if (isSliding) return;
        verticalVelocity = jumpForce;
        animator.SetTrigger("Jump");
    }

    IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);

        controller.height = slideHeight;
        controller.center = new Vector3(originalCenter.x, slideHeight / 2f, originalCenter.z);

        yield return new WaitForSeconds(slideDuration);

        controller.height = originalHeight;
        controller.center = originalCenter;

        animator.SetBool("isSliding", false);
        isSliding = false;
    }
}
