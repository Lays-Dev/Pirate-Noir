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
    public bool InSettings = false;
    public string SceneToLoad;
    
    #region === Ui Transition Pieces ===
    [Header("Pause Ui Fade in & out")]
    public CanvasGroup canvasGroup; //this will be used to fade in and fade out the pause ui
    public float fadeDuration = 0.3f; // How many seconds the fade takes
    #endregion

    #region === UI Top & Bottom Pieces ===
    // variable for each height
    [Header("Framing Pieces")]
    public RectTransform TopImage;
    public RectTransform BottomImage;
    public float TOTPH = 223f; //"Target Open Top Pause Height" for the top piece
    public float TOBPH = -223f; //"Target Open Bottom Pause Height" for the bottom piece
    public float TCTPH = 56f; //"Target Closed Top Pause Height" for the top piece
    public float TCBPH = -56f; //"Target Closed Bottom Pause Height" for the bottom piece
    public float TTSH = 300f; //"Target Top Settings Height" for the bottom piece in settings mode
    public float TBSH = 300f; //"Target Bottom Settings Height" for the bottom piece in settings mode

    [Space(10)]
    public GameObject ResumeBut;
    public GameObject OptionsBut;
    public GameObject MainMenuBut;

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
        StartCoroutine(MoveTop());
        StartCoroutine(MoveBottom());
        
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

    private IEnumerator MoveTop()
    {
        float elapsedTime = 0f;
        float startPos = TCTPH; // Where are we starting from in the ui
        float targetPos = InSettings ? TTSH : TOTPH; // Desired value
        // Keep looping as long as we haven't reached our target duration
        while (elapsedTime < fadeDuration)
        {
            // Add the time passed since the last frame to see if its passed the cooldown
            elapsedTime += Time.unscaledDeltaTime;

            // Calculate our progress percentage (between 0.0 and 1.0)
            float percentage = elapsedTime / fadeDuration;

            Vector2 currentPos = TopImage.anchoredPosition; // keep the x position
            float newY = Mathf.Lerp(startPos, targetPos, percentage);
            
            TopImage.anchoredPosition = new Vector2(currentPos.x, newY); // Set the new position

            // Wait for the very next frame before continuing the loop
            yield return null;
        }

        TopImage.anchoredPosition = new Vector2(TopImage.anchoredPosition.x, targetPos);
        
        if (!InSettings)
        {
        ResumeBut.SetActive(true);
        OptionsBut.SetActive(true);
        MainMenuBut.SetActive(true);
        }
    }

    private IEnumerator MoveBottom()
    {
        float elapsedTime = 0f;
        float startPos = TCBPH; // Where are we starting from in the ui
        float targetPos = InSettings ? TBSH : TOBPH; // Desired value
        // Keep looping as long as we haven't reached our target duration
        while (elapsedTime < fadeDuration)
        {
            // Add the time passed since the last frame to see if its passed the cooldown
            elapsedTime += Time.unscaledDeltaTime;

            // Calculate our progress percentage (between 0.0 and 1.0)
            float percentage = elapsedTime / fadeDuration;

            Vector2 currentPos = BottomImage.anchoredPosition; // keep the x position
            float newY = Mathf.Lerp(startPos, targetPos, percentage);
            
            BottomImage.anchoredPosition = new Vector2(currentPos.x, newY); // Set the new position

            // Wait for the very next frame before continuing the loop
            yield return null;
        }

        BottomImage.anchoredPosition = new Vector2(BottomImage.anchoredPosition.x, targetPos);

        // This is to make sure it doesn't do any fancy decimals and is a perfect int at the end of the fade
        
    }

    public void Resume()
    {
        //Make the text and buttons dissappear
        //move the images from the open position to the closed position for the pause area 
        //transition to turn off opacity and make the ui dissappear for the pause area

        Cursor.lockState = CursorLockMode.Locked; //unlock cursor so the player can click on the buttons
        Cursor.visible = false;

        ResumeBut.SetActive(false);
        OptionsBut.SetActive(false);
        MainMenuBut.SetActive(false);
        Time.timeScale = 1f;

        StartCoroutine(CloseTop());
        StartCoroutine(CloseBottom());

        GameIsPaused = false;

        StartCoroutine(FadeOut());
        
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
    private IEnumerator CloseTop()
    {
        float elapsedTime = 0f;
        float startPos = TOTPH; // Where are we starting from in the ui
        float targetPos = InSettings ? TTSH : TCTPH; // Desired value
        // Keep looping as long as we haven't reached our target duration
        while (elapsedTime < fadeDuration)
        {
            // Add the time passed since the last frame to see if its passed the cooldown
            elapsedTime += Time.unscaledDeltaTime;

            // Calculate our progress percentage (between 0.0 and 1.0)
            float percentage = elapsedTime / fadeDuration;

            Vector2 currentPos = TopImage.anchoredPosition; // keep the x position
            float newY = Mathf.Lerp(startPos, targetPos, percentage);
            
            TopImage.anchoredPosition = new Vector2(currentPos.x, newY); // Set the new position

            // Wait for the very next frame before continuing the loop
            yield return null;
        }

        TopImage.anchoredPosition = new Vector2(TopImage.anchoredPosition.x, targetPos);
        
        if (!InSettings)
        {
        ResumeBut.SetActive(true);
        OptionsBut.SetActive(true);
        MainMenuBut.SetActive(true);
        }
    }

    private IEnumerator CloseBottom()
    {
        float elapsedTime = 0f;
        float startPos = TOBPH; // Where are we starting from in the ui
        float targetPos = InSettings ? TBSH : TCBPH; // Desired value
        // Keep looping as long as we haven't reached our target duration
        while (elapsedTime < fadeDuration)
        {
            // Add the time passed since the last frame to see if its passed the cooldown
            elapsedTime += Time.unscaledDeltaTime;

            // Calculate our progress percentage (between 0.0 and 1.0)
            float percentage = elapsedTime / fadeDuration;

            Vector2 currentPos = BottomImage.anchoredPosition; // keep the x position
            float newY = Mathf.Lerp(startPos, targetPos, percentage);
            
            BottomImage.anchoredPosition = new Vector2(currentPos.x, newY); // Set the new position

            // Wait for the very next frame before continuing the loop
            yield return null;
        }

        BottomImage.anchoredPosition = new Vector2(BottomImage.anchoredPosition.x, targetPos);

        // This is to make sure it doesn't do any fancy decimals and is a perfect int at the end of the fade
        
    }

    #endregion



    #region == Settings Transformation and Options ==

    //make the text and buttons dissappear for the pause menu
    //move the images from the standard position to the updated position for the settings area
    //then make the text and buttons appear for the settings menu




    #endregion

    #region === Transition Scenes ===
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LoadSceneByName()
	{
		SceneManager.LoadScene(SceneToLoad);
        Debug.Log("Scene loaded: " + SceneToLoad);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    
    #endregion
}
