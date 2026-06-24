using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuMan : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
        // public string SceneToLoad;
        // public PauseManagement PauseManag;

    void Start()
    {
        // PauseManag = Object.FindAnyObjectByType<PauseManagement>(); // Get the PauseManagement component
    }

    public void LoadSceneByName(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
        Debug.Log("Scene loaded: " + sceneName);
    }

    #region === Quit Game ===

    public void Quit()
    {
        Debug.Log("Quit button pressed\nGame exiting...");
        Application.Quit();


        // stops playback in editor to test out mechanics when called (can be comented out)
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
    #endregion

}
