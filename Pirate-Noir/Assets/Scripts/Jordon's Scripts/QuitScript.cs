using UnityEngine;

public class QuitScript : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("Quit button pressed\nGame exiting...");
        Application.Quit();


        // stops playback in editor to test out mechanics when called (can be comented out)
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
