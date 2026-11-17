using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8.0f;
    public float acceleration = 50.0f;
    public float deceleration = 60.0f;
    public float jumpForce = 12.0f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundMask;

    [Header("Audio Settings")]
    [Tooltip("The sound file that plays when the player jumps.")]
    public AudioClip jumpSound;
    [Tooltip("The sound file that plays when the player walks on the ground.")]
    public AudioClip walkSound;
    [Tooltip("Time in seconds between each footstep sound.")]
    public float stepInterval = 0.35f;

    private AudioSource audioSource;
    private float stepTimer;

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;
    private bool isFacingRight = true;
    private float currentHorizontalVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        rb.freezeRotation = true; 
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        // 1. Check if the player is touching the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);

        // 2. Handle Sprite Flipping based on current movement direction
        if (horizontalInput > 0 && !isFacingRight) Flip();
        else if (horizontalInput < 0 && isFacingRight) Flip();

        // 3. Handle Walking Sound (Only when grounded AND moving)
        HandleFootsteps();
    }

    private void HandleFootsteps()
    {
        if (isGrounded && Mathf.Abs(horizontalInput) > 0.1f)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                if (audioSource != null && walkSound != null)
                {
                    // Slight pitch variation so footsteps don't sound robotic
                    audioSource.pitch = Random.Range(0.9f, 1.1f);
                    audioSource.PlayOneShot(walkSound);
                }
                stepTimer = stepInterval; // Reset the timer for the next step
            }
        }
        else
        {
            // Reset timer to 0 when stopped or in mid-air so the first step plays immediately upon landing/moving
            stepTimer = 0f;
        }
    }

    void FixedUpdate()
    {
        float targetHorizontalVelocity = horizontalInput * moveSpeed;
        float speedRate = (horizontalInput != 0) ? acceleration : deceleration;

        currentHorizontalVelocity = Mathf.MoveTowards(
            currentHorizontalVelocity, 
            targetHorizontalVelocity, 
            speedRate * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector2(currentHorizontalVelocity, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        if (audioSource != null && jumpSound != null)
        {
            audioSource.pitch = 1f; // Reset pitch to normal for the jump sound
            audioSource.PlayOneShot(jumpSound);
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }

    public void OnMove(InputValue value)
    {
        Vector2 moveVector = value.Get<Vector2>();
        horizontalInput = moveVector.x;
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            Jump();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
