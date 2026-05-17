using UnityEngine; // Core Unity engine functionality
using UnityEngine.InputSystem; // Unity Input System namespace

public class PlayerMovement : MonoBehaviour // Handles player movement, jumping, sprinting, and animation
{
    #region === Movement Settings ===

    [Header("Movement Settings")] // Inspector header for movement settings
    public float MoveSpeed = 7f; // Normal walking speed

    public float SprintSpeed = 12f; // Sprinting movement speed

    public float RotationSpeed = 10f; // Rotation smoothing speed

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

    public float GroundCheckOffset = 0.1f; // Height offset for ground check sphere

    #endregion


    #region === Internal References ===

    [Header("Internal References")] // Inspector header for references
    public Rigidbody RB; // Rigidbody reference

    public Transform CameraTransform; // Main camera transform reference

    #endregion


    #region === Runtime State ===

    [Header("Internal States")] // Inspector header for runtime states

    private Vector2 MoveInput; // Raw movement input from player

    private Vector3 MoveDirection; // Calculated world movement direction

    private float VerticalY; // Vertical velocity value

    public bool IsGrounded; // Whether the player is touching the ground

    public bool IsSprinting; // Whether the sprint input is active

    #endregion


    #region === Animation ===

    [Header("Animation")] // Inspector header for animation settings
    public Animator Anim; // Animator reference

    #endregion


    #region === Unity Lifecycle ===

    void Start() // Called before the first frame update
    {
        RB = GetComponent<Rigidbody>(); // Cache Rigidbody reference

        CameraTransform = Camera.main.transform; // Cache main camera transform

        RB.isKinematic = false; // Allow physics simulation

        RB.freezeRotation = true; // Prevent physics from rotating player

        RB.useGravity = false; // Disable built-in gravity

        RB.collisionDetectionMode = CollisionDetectionMode.Continuous; // Improve collision accuracy
    }

    void Update() // Called once per frame
    {
        UpdateAnimations(); // Update animation parameters
    }

    void FixedUpdate() // Called at a fixed interval for physics updates
    {
        CheckGround(); // Detect whether the player is grounded

        CalculateMoveDirection(); // Convert input into world movement direction

        ApplyGravity(); // Apply custom gravity

        ApplyRotation(); // Rotate player toward movement direction

        ApplyMovement(); // Apply final movement velocity
    }

    #endregion


    #region === Input Handlers ===

    
    // Handles movement input from the Input System.
    
    public void OnMove(InputAction.CallbackContext Context) => MoveInput = Context.ReadValue<Vector2>(); // Read movement input

    
    // Handles sprint input state.
    
    public void OnSprint(InputAction.CallbackContext Context) // Called when sprint input changes
    {
        if (Context.performed) // Sprint button pressed
            IsSprinting = true; // Enable sprinting

        else if (Context.canceled) // Sprint button released
            IsSprinting = false; // Disable sprinting
    }

    
    // Handles jumping input.
    
    public void OnJump(InputAction.CallbackContext Context) // Called when jump input occurs
    {
        if (Context.started && IsGrounded) // Only jump if grounded
        {
            VerticalY = Mathf.Sqrt(JumpForce * -2f * Gravity); // Calculate jump velocity

            if (Anim != null) // Ensure animator exists
                Anim.SetTrigger("Jump"); // Trigger jump animation
        }
    }

    #endregion


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


    #region === Movement Logic ===

    
    // Calculates movement direction relative to the camera.
    
    public void CalculateMoveDirection()
    {
        Vector3 Forward = CameraTransform.forward; // Camera forward direction

        Vector3 Right = CameraTransform.right; // Camera right direction

        Forward.y = 0; // Remove vertical component

        Right.y = 0; // Remove vertical component

        Forward.Normalize(); // Normalize forward vector

        Right.Normalize(); // Normalize right vector

        MoveDirection = (Forward * MoveInput.y + Right * MoveInput.x).normalized; // Calculate final movement direction
    }

    
    // Rotates the player toward movement direction.
    
    public void ApplyRotation()
    {
        if (MoveInput.sqrMagnitude > 0.01f) // Only rotate when moving
        {
            Quaternion TargetRotation = Quaternion.LookRotation(MoveDirection); // Create target rotation

            RB.MoveRotation(Quaternion.Slerp( // Smoothly rotate Rigidbody
                RB.rotation, // Current rotation
                TargetRotation, // Desired rotation
                RotationSpeed * Time.fixedDeltaTime // Rotation speed
            ));
        }
    }

    
    // Applies movement velocity to the Rigidbody.
    
    public void ApplyMovement()
    {
        float CurrentSpeed = IsSprinting ? SprintSpeed : MoveSpeed; // Choose movement speed

        Vector3 HorizontalVelocity = MoveDirection * CurrentSpeed; // Calculate horizontal movement velocity

        RB.linearVelocity = new Vector3( // Apply final velocity
            HorizontalVelocity.x, // Horizontal X velocity
            VerticalY, // Vertical velocity
            HorizontalVelocity.z // Horizontal Z velocity
        );
    }

    #endregion


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


    #region === Debug Visualization ===

    
    // Draws ground check sphere in the Scene view.
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red; // Change color based on grounded state

        Gizmos.DrawWireSphere( // Draw wireframe sphere
            transform.position + Vector3.up * GroundCheckOffset, // Sphere position
            GroundCheckRadius // Sphere radius
        );
    }

    #endregion
}