using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneChanger : MonoBehaviour
{
    public string nextSceneName; // Name of the next scene to load
    public float delay = 3f; // Delay before changing the scene (default: 3 seconds)

    private void Start()
    {
        // Start the scene change after the specified delay
        Invoke("ChangeScene", delay);
    }

    private void ChangeScene()
    {
        // Load the next scene by name
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next scene name is not set!");
        }
    }
}
