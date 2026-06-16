using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class WinEndManag : MonoBehaviour
{
    public bool EndGameState = false;

    public CanvasGroup WinGroup; //this will be used to fade in the Win screen ui
    public CanvasGroup LoseGroup; //this will be used to fade in the Lose Screen ui
    public GameObject loseUI;
    public float LosefadeDuration = 1f; // How many seconds the fade takes
    public float WinfadeDuration = 2.5f; // How many seconds the fade takes
    public PlayerStats playerStats;
    public PauseManagement PauseManagement;
    public StaminaBarUI stambar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = Object.FindAnyObjectByType<PlayerStats>(); // reference to the player's stats script to modify health

        PauseManagement = Object.FindAnyObjectByType<PauseManagement>(); // reference to the player's stats script to modify health

        stambar = Object.FindAnyObjectByType<StaminaBarUI>(); // reference to the player's stats script to modify health


        GameObject loseUI = GameObject.Find("LoseUI"); // Get the Top Image Rect component from the gameobject Top

        GameObject TopImg = GameObject.Find("Top"); // Get the Top Image Rect component from the gameobject Top

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerStats.CurrentHealth <= 0)
        {
            LoseGameFunc();
            
        }
    }

    public void WinGameFunc()
    {
        EndGameState = true;
        PauseManagement.HealthcanvasGroup.alpha = 0;
        stambar.vis.alpha = 0;
        Cursor.lockState = CursorLockMode.None; //unlock cursor so the player can click on the buttons
        Cursor.visible = true;


        StartCoroutine(FadeInWin());
        Time.timeScale = 0f;

        Debug.Log("You win!");


        
    }

    public void LoseGameFunc()
    {
        EndGameState = true;
        PauseManagement.HealthcanvasGroup.alpha = 0;
        stambar.vis.alpha = 0;

        Cursor.lockState = CursorLockMode.None; //unlock cursor so the player can click on the buttons
        Cursor.visible = true;

        StartCoroutine(FadeInLose());
        Time.timeScale = 0f;

        Debug.Log("You lose!");
    }

    public IEnumerator FadeInWin()
    {
        float elapsedTime = 0f;
        float startOpacity = WinGroup.alpha; // Where are we starting from? (will be 0 here)
        float targetOpacity = 1f;               // Desired value

        // Keep looping as long as we haven't reached our target duration
        
        while (elapsedTime < WinfadeDuration)
        {
            // Add the time passed since the last frame to see if its passed the cooldown
            elapsedTime += Time.unscaledDeltaTime;

            // Calculate our progress percentage (between 0.0 and 1.0)
            float percentage = elapsedTime / WinfadeDuration;

            // Set the opacity based on that progress
            WinGroup.alpha = Mathf.Lerp(startOpacity, targetOpacity, percentage);

            // Wait for the very next frame before continuing the loop
            yield return null;
        }

        // This is to make sure it doesn't do any fancy decimils and is a perfect int at the end of the fade
        WinGroup.alpha = targetOpacity;
        if (WinGroup.alpha == 1f)
        {
            WinGroup.interactable = true;
            WinGroup.blocksRaycasts = true;
        }

    }

    public IEnumerator FadeInLose()
    {
        float elapsedTime = 0f;
        float startOpacity = LoseGroup.alpha; // Where are we starting from? (will be 0 here)
        float targetOpacity = 1f;               // Desired value

        // Keep looping as long as we haven't reached our target duration
        
        while (elapsedTime < LosefadeDuration)
        {
            // Add the time passed since the last frame to see if its passed the cooldown
            elapsedTime += Time.unscaledDeltaTime;

            // Calculate our progress percentage (between 0.0 and 1.0)
            float percentage = elapsedTime / LosefadeDuration;

            // Set the opacity based on that progress
            LoseGroup.alpha = Mathf.Lerp(startOpacity, targetOpacity, percentage);

            // Wait for the very next frame before continuing the loop
            yield return null;
        }

        // This is to make sure it doesn't do any fancy decimils and is a perfect int at the end of the fade
        LoseGroup.alpha = targetOpacity;
        if (LoseGroup.alpha == 1f)
        {
            LoseGroup.interactable = true;
            loseUI.SetActive(true);
            LoseGroup.blocksRaycasts = true;
        }

    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();

        // 3. Load it again using either its name or its build index
        SceneManager.LoadScene(currentScene.name);
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

}
