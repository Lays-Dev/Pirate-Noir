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

    // public void LoadSceneByName()
	// {
	// 	SceneManager.LoadScene(SceneToLoad);
    //     Debug.Log("Scene loaded: " + SceneToLoad);
    //     Time.timeScale = 1f;
    //     PauseManag.GameIsPaused = false;
    // }
}
