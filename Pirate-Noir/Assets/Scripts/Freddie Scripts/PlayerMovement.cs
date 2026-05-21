using UnityEngine;
using UnityEngine.InputSystem; // Unity Input System namespace

public class PlayerMovement : MonoBehaviour
{
    #region === Movement Settings ===

    [Header("Movement Settings")] // Inspector header for movement settings
    public Rigidbody RB; // Rigidbody reference for physics-based movement
    public bool IsGrounded; // Whether the player is currently grounded
    public bool IsSprinting; // Whether the player is currently sprinting
    
    public float MoveSpeed = 7f; // Normal walking speed
    public float SprintSpeed = 12f; // Sprinting movement speed
    public Transform Orientation; // Reference to the player's orientation for movement direction

    public Vector2 MoveInput; // Raw movement input from player
    public float VerticalY; // Vertical velocity value
    Vector3 MoveDirection; // Calculated movement direction

    #endregion

    #region === Jump Settings ===
    [Header("Jump Settings")] // Inspector header for jumping settings
    public float JumpForce = 3f; // Strength of jump force
    public float Gravity = -25f; // Custom gravity applied manually
    #endregion

    #region === Ground Detection ===
    [Header("Ground Detection")] // Inspector header for ground detection settings
    public LayerMask GroundLayer; // Layer mask used to identify ground objects
    public float GroundCheckRadius = 0.3f; // Radius of ground check sphere
    public float GroundCheckOffset = 0.1f; // Height offset for ground check
    #endregion

    #region === Interaction Settings ===
    // Groups interaction-related variables in the Inspector
    [Header("Interaction Settings")]
    public float InteractRange = 3f;
    public LayerMask InteractableLayer; // Assign this in Inspector!
    #endregion

    #region === Animation ===

    [Header("Animation")] // Inspector header for animation settings
    public Animator Anim; // Animator reference

    #endregion

    private void Start()
    {
        RB = GetComponent<Rigidbody>(); // Get the Rigidbody component

        RB.freezeRotation = true; // Prevent the Rigidbody from rotating due to physics

        RB.useGravity = false; // Disable built-in gravity

        RB.collisionDetectionMode = CollisionDetectionMode.Continuous; // Set collision detection mode for better accuracy
    }

    public void OnMove(InputAction.CallbackContext Context)
    {
        MoveInput = Context.ReadValue<Vector2>(); // Read the movement input as a Vector2 (x for horizontal, y for vertical)
    }

    public void OnSprint(InputAction.CallbackContext Context) // Called when sprint input changes
    {
        if (Context.performed) // Sprint button pressed
            IsSprinting = true; // Enable sprinting

        else if (Context.canceled) // Sprint button released
            IsSprinting = false; // Disable sprinting
    }

    public void OnJump(InputAction.CallbackContext Context) // Called when jump input occurs
    {
        Debug.Log("Jump input received"); // Log jump input for debugging
        if (Context.started && IsGrounded) // Only jump if grounded
        {
            VerticalY = Mathf.Sqrt(JumpForce * -2f * Gravity); // Calculate jump velocity

            if (Anim != null) // Ensure animator exists
            {
                Anim.SetTrigger("Jump"); // Trigger jump animation
            }
        }
    }

    public void Movement()
    {
        float CurrentSpeed = IsSprinting ? SprintSpeed : MoveSpeed; // Choose movement speed
        MoveDirection = transform.forward * MoveInput.y + transform.right * MoveInput.x; // Calculate movement direction based on orientation and input
        Vector3 HorizontalVelocity = MoveDirection * CurrentSpeed; // Calculate horizontal movement velocity
        RB.linearVelocity = new Vector3(HorizontalVelocity.x, VerticalY, HorizontalVelocity.z); // Apply horizontal velocity while preserving vertical velocity
        
    }

    #region === Ground Detection ===

    // Checks whether the player is currently grounded.
    
    private void CheckGround()
    {
        // Create sphere position slightly above feet
        Vector3 SpherePosition = transform.position + Vector3.up * GroundCheckOffset; // Offset sphere upward slightly

        // Perform sphere collision check
        IsGrounded = Physics.CheckSphere(SpherePosition, GroundCheckRadius, GroundLayer); // Detect ground collision
    }

    #endregion

    #region === Gravity ===

    
    // Applies custom gravity to the player.
    
    private void ApplyGravity()
    {
        if (IsGrounded && VerticalY < 0) // Player grounded while falling
        {
            VerticalY = -1f; // Keep slight downward force to remain grounded
        }
        else // Player airborne
        {
            VerticalY += Gravity * Time.fixedDeltaTime; // Apply gravity over time
        }
    }

    #endregion

    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Interact button pressed, checking for interactables...");
        
        Ray Ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward); // Create a ray from the camera's position forward
        Debug.DrawRay(Ray.origin, Ray.direction * InteractRange, Color.red, 2f); // Draw the ray in the scene view for debugging purposes

        // Added QueryTriggerInteraction.Collide to include Triggers in the raycast
        if (Physics.Raycast(Ray, out RaycastHit hit, InteractRange, InteractableLayer, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.TryGetComponent(out IInteractable Interactable))
            {
                Interactable.Interact(); // Call the Interact method on the interactable object
                Debug.Log($"Interacted with {hit.collider.name}"); // Log the name of the interacted object for debugging
            }
        }

    }

    #region === Animation Logic ===

    
    // Updates animator parameters based on player state.
    
    private void UpdateAnimations()
    {
        if (Anim == null) return; // Stop if no animator assigned

        float CurrentAnimSpeed = MoveInput.magnitude; // Base movement animation speed

        if (IsSprinting && CurrentAnimSpeed > 0) // Check if sprinting while moving
            CurrentAnimSpeed = 2f; // Increase animation speed for sprinting

        Anim.SetFloat("Speed", CurrentAnimSpeed, 0.1f, Time.deltaTime); // Smoothly update movement speed parameter

        Anim.SetBool("IsGrounded", IsGrounded); // Update grounded state parameter

        Anim.SetFloat("VerticalVelocity", VerticalY); // Update vertical velocity parameter
    }

    #endregion

    private void FixedUpdate()
    {
        Movement(); // Call the movement method in FixedUpdate for consistent physics updates
        CheckGround(); // Check if the player is grounded
        ApplyGravity(); // Apply custom gravity to the player
        UpdateAnimations(); // Update animator parameters based on current state
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red; // Change color based on grounded state

        Gizmos.DrawWireSphere( // Draw wireframe sphere
            transform.position + Vector3.up * GroundCheckOffset, // Sphere position
            GroundCheckRadius // Sphere radius
        );
    }

}