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


    #region === Player Movement ===]
    public PlayerMovement Movement; // Reference to the PlayerMovement component ~F
    #endregion

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
        
        Movement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>(); // Get the PlayerMovement component from the player Gameobject ~F

    }

    public void OnLook(InputAction.CallbackContext Context)
    {
        
        
        if (!Movement.CanMove) // Check if the player can move
        {
            return; // If the player cannot move, exit the method
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
