using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
#if UNITY_EDITOR
            // If the application is running in the Unity Editor, stop the editor play mode.
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // If the application is running outside the Unity Editor, quit the application normally.
        Application.Quit();
#endif
    }
}