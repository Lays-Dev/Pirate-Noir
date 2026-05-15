using UnityEngine; // Core Unity functionality
using UnityEngine.InputSystem; // Unity Input System support
using Unity.Cinemachine; // Cinemachine camera system

public class CameraZoom : MonoBehaviour // Handles camera zoom controls
{
    public CinemachineCamera Vcam; // Reference to the Cinemachine camera

    #region === Settings ===

    [Header("Settings")] // Inspector header for zoom settings
    public float ZoomSensitivity = 200f; // How sensitive zooming is

    public float MinRadius = 2.5f; // Minimum zoom distance
    public float MaxRadius = 5f; // Maximum zoom distance

    [Tooltip("How fast the camera catches up. Higher = Snappier")] // Inspector tooltip
    public float LerpSpeed = 20f; // Smooth zoom interpolation speed

    #endregion

    #region === Runtime State ===

    private CinemachineOrbitalFollow OrbitalFollow; // Cached orbital follow component

    private float ScrollDelta; // Current mouse wheel zoom input
    private Vector2 StickInput; // Current gamepad stick input

    private bool ZoomTrigger; // Whether the gamepad zoom modifier is held

    private float TargetRadius; // Desired zoom radius

    #endregion

    #region === Unity Lifecycle ===

    void Start() // Called before the first frame update
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the mouse cursor

        if (Vcam != null) // Ensure the camera exists
        {
            OrbitalFollow = Vcam.GetComponent<CinemachineOrbitalFollow>(); // Get orbital follow component

            if (OrbitalFollow != null) // Ensure the component exists
                TargetRadius = OrbitalFollow.Radius; // Initialize target radius from current camera radius
        }
    }

    void Update() // Called once per frame
    {
        if (OrbitalFollow == null) return; // Stop if the orbital follow component is missing

        float FinalInput = 0; // Final zoom input value for this frame

        // Prioritize stick zoom when modifier button is held
        if (ZoomTrigger) // Check if modifier is active
        {
            FinalInput = StickInput.y; // Use vertical stick input
        }
        else // Otherwise use mouse wheel
        {
            FinalInput = ScrollDelta; // Use mouse scroll input
        }

        // Apply zoom movement
        if (Mathf.Abs(FinalInput) > 0.01f) // Ignore tiny inputs
        {
            TargetRadius -= FinalInput * ZoomSensitivity * Time.unscaledDeltaTime; // Adjust desired radius
        }

        // Clamp and smooth zoom
        TargetRadius = Mathf.Clamp(TargetRadius, MinRadius, MaxRadius); // Prevent zoom from exceeding limits

        OrbitalFollow.Radius = Mathf.Lerp( // Smoothly move toward target radius
            OrbitalFollow.Radius, // Current radius
            TargetRadius, // Desired radius
            Time.unscaledDeltaTime * LerpSpeed // Interpolation speed
        );

        // Reset mouse scroll burst
        ScrollDelta = 0; // Reset scroll input after applying it
    }

    #endregion

    #region === Input Handlers ===

    // Bind this to Scroll Wheel (Value -> Vector2)
    public void OnZoom(InputAction.CallbackContext Context) // Handles mouse wheel zoom
    {
        Vector2 Input = Context.ReadValue<Vector2>(); // Read scroll wheel input

        // Mouse wheel input is burst-based
        if (Input.y > 0) // Scrolling upward
            ScrollDelta = 1f; // Zoom inward
        else if (Input.y < 0) // Scrolling downward
            ScrollDelta = -1f; // Zoom outward
    }

    // Bind this to Right Stick (Value -> Vector2)
    public void OnStickZoom(InputAction.CallbackContext Context) // Handles gamepad stick zoom
    {
        StickInput = Context.ReadValue<Vector2>(); // Read right stick input
    }

    // Bind this to Modifier Button (Button -> Press)
    public void OnGamepadZoom(InputAction.CallbackContext Context) // Handles modifier button state
    {
        if (Context.performed) // Button pressed
            ZoomTrigger = true; // Enable stick zoom mode

        if (Context.canceled) // Button released
            ZoomTrigger = false; // Disable stick zoom mode
    }

    #endregion
}