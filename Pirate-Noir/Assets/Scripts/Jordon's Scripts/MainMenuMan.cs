using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuMan : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LoadSceneByName(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
        Debug.Log("Scene loaded: " + sceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
