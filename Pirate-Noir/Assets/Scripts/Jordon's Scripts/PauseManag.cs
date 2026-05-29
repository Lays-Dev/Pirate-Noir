using UnityEngine;
using System.Collections;
using UnityEngine.UI; 
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement; 
using TMPro;
using Unity.Cinemachine;


public class PauseManagement : MonoBehaviour
{
    [Header("Variables")]
    public bool GameIsPaused = false;
    
    #region === Ui Transition Pieces ===
    [Header("Pause Ui Fade in & out")]
    public CanvasGroup canvasGroup; //this will be used to fade in and fade out the pause ui
    public float fadeDuration = 0.3f; // How many seconds the fade takes


    #endregion

    #region === Pause Transformation and Options ===

    public void Pause()
    {
        //transition to turn on opacity and allow the ui to appear for the pause area
        //move the images from the closed position to the open position for the pause area 
        //Check if images are there and make the text and buttons appear
        
        Cursor.lockState = CursorLockMode.None; //unlock cursor so the player can click on the buttons
        Cursor.visible = true;


        StartCoroutine(FadeIn());
        Time.timeScale = 0f;
        //AudioListener.pause = false; (this is for later when we implement audio)
        
        Debug.Log("Game Pause Test");
        GameIsPaused = true;
    }
    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        float startOpacity = canvasGroup.alpha; // Where are we starting from? (will be 0 here)
        float targetOpacity = 1f;               // Desired value

        // Keep looping as long as we haven't reached our target duration
        while (elapsedTime < fadeDuration)
        {
            // Add the time passed since the last frame to see if its passed the cooldown
            elapsedTime += Time.unscaledDeltaTime;

            // Calculate our progress percentage (between 0.0 and 1.0)
            float percentage = elapsedTime / fadeDuration;

            // Set the opacity based on that progress
            canvasGroup.alpha = Mathf.Lerp(startOpacity, targetOpacity, percentage);

            // Wait for the very next frame before continuing the loop
            yield return null;
        }

        // This is to make sure it doesn't do any fancy decimils and is a perfect int at the end of the fade
        canvasGroup.alpha = targetOpacity;
        if (canvasGroup.alpha == 1f)
        {
            canvasGroup.interactable = true;
        }
    }

    public void Resume()
    {
        //Make the text and buttons dissappear
        //move the images from the open position to the closed position for the pause area 
        //transition to turn off opacity and make the ui dissappear for the pause area

        Cursor.lockState = CursorLockMode.Locked; //unlock cursor so the player can click on the buttons
        Cursor.visible = false;

        
        StartCoroutine(FadeOut());
        Time.timeScale = 1f;

        Debug.Log("Game Resume Test");
        GameIsPaused = false;
    }

    private IEnumerator FadeOut()
    {
        canvasGroup.interactable = false;
        float elapsedTime = 0f;
        float startOpacity = canvasGroup.alpha; // Where are we starting from? (will be 0 here)
        float targetOpacity = 0f;               // Desired value

        // Keep looping as long as we haven't reached our target duration
        while (elapsedTime < fadeDuration)
        {
            // Add the time passed since the last frame to see if its passed the cooldown
            elapsedTime += Time.unscaledDeltaTime;

            // Calculate our progress percentage (between 0.0 and 1.0)
            float percentage = elapsedTime / fadeDuration;

            // Set the opacity based on that progress
            canvasGroup.alpha = Mathf.Lerp(startOpacity, targetOpacity, percentage);

            // Wait for the very next frame before continuing the loop
            yield return null;
        };

        // This is to make sure it doesn't do any fancy decimils and is a perfect int at the end of the fade
        canvasGroup.alpha = targetOpacity;
        if (canvasGroup.alpha == 0f)
        {
            canvasGroup.interactable = false;
        }
    }


    #endregion



    #region == Settings Transformation and Options ==

    //make the text and buttons dissappear for the pause menu
    //move the images from the standard position to the updated position for the settings area
    //then make the text and buttons appear for the settings menu




    #endregion
}
