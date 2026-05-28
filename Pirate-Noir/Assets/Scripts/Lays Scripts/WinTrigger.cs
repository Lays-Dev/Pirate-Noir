using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{

    [SerializeField] private GameObject winScreen;
    
    [SerializeField] private PlayerInput playerInput;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Show UI
            winScreen.SetActive(true);
            playerInput.enabled = false;
            // Unlock mouse
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Pause game
            Time.timeScale = 0f;
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}