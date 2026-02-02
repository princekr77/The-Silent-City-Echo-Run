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
    private int currentLane = 1;
    private float startX;

    // ================= JUMP & GRAVITY =================
    [Header("Jump")]
    public float jumpForce = 6f;
    public float gravity = -20f;
    private float verticalVelocity;

    // ================= SLIDE =================
    [Header("Slide")]
    public float slideDuration = 0.8f;
    public float slideHeight = 1f;

    private float originalHeight;
    private Vector3 originalCenter;
    private bool isSliding = false;

    // ================= SWIPE =================
    [Header("Swipe Settings")]
    public float swipeThreshold = 50f;
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;

    // ================= REFERENCES =================
    [Header("References")]
    public Animator animator;
    private CharacterController controller;

    private bool isRunning = false;

    // ================= UNITY METHODS =================
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Start()
    {
        startX = transform.position.x;

        originalHeight = controller.height;
        originalCenter = controller.center;

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

        // ================= RUN =================
        if (isRunning)
        {
            move.z = runSpeed;

            if (runSpeed < maxSpeed)
                runSpeed += speedIncreaseRate * Time.deltaTime;
        }

        // ================= LANE =================
        float targetX = startX + (currentLane - 1) * laneDistance;
        move.x = (targetX - transform.position.x) * laneChangeSpeed;

        // ================= GROUND & JUMP =================
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

        // ================= GRAVITY =================
        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        // ================= APPLY MOVE =================
        controller.Move(move * Time.deltaTime);

        // ================= PC INPUT =================
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            ChangeLane(-1);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            ChangeLane(1);

        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !isSliding)
            StartCoroutine(Slide());

        // ================= MOBILE INPUT =================
        HandleSwipeInput();
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
        if (isSliding) return;

        verticalVelocity = jumpForce;
        animator.SetTrigger("Jump");
    }

    IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);

        controller.height = slideHeight;
        controller.center = new Vector3(
            originalCenter.x,
            slideHeight / 2f,
            originalCenter.z
        );

        yield return new WaitForSeconds(slideDuration);

        controller.height = originalHeight;
        controller.center = originalCenter;

        animator.SetBool("isSliding", false);
        isSliding = false;
    }

    // ================= SWIPE =================
    void HandleSwipeInput()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            startTouchPos = touch.position;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            endTouchPos = touch.position;
            Vector2 delta = endTouchPos - startTouchPos;

            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            {
                if (Mathf.Abs(delta.x) > swipeThreshold)
                {
                    if (delta.x > 0) ChangeLane(1);
                    else ChangeLane(-1);
                }
            }
            else
            {
                if (delta.y > swipeThreshold && controller.isGrounded)
                    Jump();
                else if (delta.y < -swipeThreshold && !isSliding)
                    StartCoroutine(Slide());
            }
        }
    }
}
