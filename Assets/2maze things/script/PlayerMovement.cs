using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;           // Speed of the player
    public float jumpForce = 7f;          // Jump force
    public float acceleration = 5f;       // Acceleration for smoother movement
    public float deceleration = 8f;       // Deceleration for smoother stopping

    [Header("Ground Detection")]
    public Transform groundCheck;         // Position to check if the player is grounded
    public float groundDistance = 0.3f;   // Radius of the ground check sphere
    public LayerMask groundLayer;         // Layer mask to define ground objects

    [Header("Air Control")]
    public float airControlFactor = 0.5f; // Control factor while in the air

    [Header("Camera Settings")]
    public Transform cameraTransform;     // Reference to the camera for movement direction

    [Header("Custom Gravity")]
    public Vector3 customGravity = new Vector3(0, -9.81f, 0); // Custom gravity vector

    private Rigidbody rb;                 // Reference to the Rigidbody component
    private Vector3 movementInput;        // Tracks the player's movement input
    private bool isGrounded;              // Tracks if the player is grounded
    private Vector3 currentVelocity;      // For smooth movement calculations

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent Rigidbody from rotating due to physics forces
    }

    void Update()
    {
        HandleInput();
        GroundCheck();

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        ApplyCustomGravity();
        MovePlayer();
    }

    private void HandleInput()
    {
        // Get movement input from player
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calculate movement direction relative to the camera
        if (cameraTransform != null)
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            // Normalize to ensure consistent movement speed
            forward.y = 0f; // Prevent movement in the Y-axis
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            // Combine input with camera directions
            movementInput = (forward * z + right * x).normalized;
        }
        else
        {
            Debug.LogWarning("CameraTransform is not assigned! Player movement will be incorrect.");
            movementInput = Vector3.zero;
        }
    }

    private void GroundCheck()
    {
        // Check if the player is grounded using a sphere cast
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
        }
        else
        {
            Debug.LogWarning("GroundCheck Transform is not assigned. Assign it in the Inspector.");
            isGrounded = false;
        }
    }

    private void MovePlayer()
    {
        if (isGrounded)
        {
            // Smoothly accelerate or decelerate to the target velocity
            Vector3 targetVelocity = movementInput * moveSpeed;
            currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, Time.fixedDeltaTime * acceleration);

            // Apply movement while grounded
            rb.velocity = new Vector3(currentVelocity.x, rb.velocity.y, currentVelocity.z);
        }
        else
        {
            // Allow limited control in the air
            Vector3 airVelocity = movementInput * moveSpeed * airControlFactor;
            rb.velocity = new Vector3(airVelocity.x, rb.velocity.y, airVelocity.z);
        }
    }

    private void Jump()
    {
        // Reset vertical velocity to prevent stacking forces
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Add upward force for jumping
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void ApplyCustomGravity()
    {
        // Apply custom gravity
        rb.AddForce(customGravity * rb.mass, ForceMode.Force);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the ground check sphere in the Scene view
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}
