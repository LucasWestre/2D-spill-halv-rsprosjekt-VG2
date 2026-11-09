using UnityEngine;
using UnityEngine.InputSystem; // Uses the New Input System cleanly

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))] // Automatically ensures an AudioSource is attached
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8.0f;
    public float acceleration = 50.0f; // Higher values = faster takeoff
    public float deceleration = 60.0f; // Higher values = quicker stops
    public float jumpForce = 12.0f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundMask;

    [Header("Audio Settings")]
    [Tooltip("The sound file that plays when the player jumps.")]
    public AudioClip jumpSound;
    private AudioSource audioSource;

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;
    private bool isFacingRight = true;
    private float currentHorizontalVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); // Cache the AudioSource component
        
        // Configure standard sound settings automatically
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        // Prevents the capsule from tipping over sideways when running into walls
        rb.freezeRotation = true; 
        
        // Ensures perfect midair/ground tracking smoothness
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        // 1. Check if the player is touching the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);

        // 2. Handle Sprite Flipping based on current movement direction
        if (horizontalInput > 0 && !isFacingRight) Flip();
        else if (horizontalInput < 0 && isFacingRight) Flip();
    }

    void FixedUpdate()
    {
        // 3. Calculate target velocity based on input direction (cleared of sticky legacy code)
        float targetHorizontalVelocity = horizontalInput * moveSpeed;

        // 4. Choose between acceleration or braking rates
        float speedRate = (horizontalInput != 0) ? acceleration : deceleration;

        // 5. Smoothly step the current velocity toward the target velocity over time
        currentHorizontalVelocity = Mathf.MoveTowards(
            currentHorizontalVelocity, 
            targetHorizontalVelocity, 
            speedRate * Time.fixedDeltaTime
        );

        // 6. Apply the changes safely to the Rigidbody2D
        rb.linearVelocity = new Vector2(currentHorizontalVelocity, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        // TRIGGER THE JUMP AUDIO HERE
        if (audioSource != null && jumpSound != null)
        {
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

    // NEW INPUT SYSTEM CALLBACKS (Triggers automatically via Player Input Component)
    // When you let go of a key, the New Input System automatically sends a 0 here.
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
