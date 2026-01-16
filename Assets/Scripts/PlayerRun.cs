using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // ================= FORWARD MOVEMENT =================
    [Header("Forward Movement")]
    public float runSpeed = 6f;
    public float speedIncreaseRate = 0.2f;
    public float maxSpeed = 15f;

    // ================= LANE MOVEMENT =================
    [Header("Lane Movement")]
    public float laneDistance = 3f;
    public float laneChangeSpeed = 10f;
    private int currentLane = 1; // 0 = left, 1 = middle, 2 = right
    private float startX;

    // ================= JUMP & GRAVITY =================
    [Header("Jump")]
    public float jumpForce = 6f;
    public float gravity = -20f;
    private float verticalVelocity;

    // ================= REFERENCES =================
    [Header("References")]
    public Animator animator;
    private CharacterController controller;

    private bool isRunning = false;

    // ================= UNITY METHODS =================
    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        startX = transform.position.x;

        // Start in Idle
        isRunning = false;
        animator.SetBool("isRunning", false);

        StartCoroutine(StartRunningAfterDelay());
    }

    IEnumerator StartRunningAfterDelay()
    {
        yield return new WaitForSeconds(0.2f);
        StartRunning();
    }

    void Update()
    {
        Vector3 move = Vector3.zero;

        // ===== RUN STATE =====
        if (isRunning)
        {
            move.z = runSpeed;

            if (runSpeed < maxSpeed)
                runSpeed += speedIncreaseRate * Time.deltaTime;
        }

        // ===== LANE MOVEMENT =====
        float targetX = startX + (currentLane - 1) * laneDistance;
        float diffX = targetX - transform.position.x;
        move.x = diffX * laneChangeSpeed;

        // ===== GROUND CHECK =====
        if (controller.isGrounded)
        {
            animator.SetBool("isGrounded", true);

            if (verticalVelocity < 0)
                verticalVelocity = -2f;

            if (Input.GetKeyDown(KeyCode.Space))
                Jump();
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }

        // ===== GRAVITY =====
        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        // ===== APPLY MOVEMENT =====
        controller.Move(move * Time.deltaTime);

        // ===== LANE INPUT =====
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            ChangeLane(-1);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            ChangeLane(1);
    }

    // ================= METHODS =================

    void StartRunning()
    {
        isRunning = true;
        animator.SetBool("isRunning", true);
    }

    void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, 0, 2);
    }

    void Jump()
    {
        verticalVelocity = jumpForce;
        animator.SetTrigger("Jump");
    }
}
