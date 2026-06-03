using UnityEngine;
using UnityEngine.InputSystem; // Unity Input System namespace

public class CameraMovement : MonoBehaviour
{
    public float SensX = 1f; // Mouse sensitivity for horizontal rotation
    public float SensY = 1f; // Mouse sensitivity for vertical rotation

    public Transform Orientation; // Reference to the player's orientation for camera rotation

    public float MouseX; // Horizontal mouse input
    public float MouseY; // Vertical mouse input

    private float xRotation; // Cumulative vertical rotation value

    public PauseManagement PauseManagement; // Reference to the PauseManagement script

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
        
        PauseManagement = Object.FindAnyObjectByType<PauseManagement>(); // Get the PauseManagement component

    }

    public void OnLook(InputAction.CallbackContext Context)
    {
        // if (PauseManagement != null) (to fix it not being there)
        
            if (PauseManagement != null)
            {
                if (PauseManagement.GameIsPaused) 
                {
                    return;
                }
            }
        
        

        Vector2 LookInput = Context.ReadValue<Vector2>();

        // 1. Calculate the frame's deltas
        float deltaX = LookInput.x * SensX;
        float deltaY = LookInput.y * SensY;

        // 2. Accumulate the vertical rotation (subtract or add depending on your preferred inversion)
        xRotation -= deltaY; 

        // 3. Clamp the CUMULATIVE rotation, not the delta
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        // 4. Apply horizontal rotation to the player body (still using delta is fine here)
        Orientation.Rotate(Vector3.up * deltaX);

        // 5. Directly set the local rotation of the camera using the clamped total
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
