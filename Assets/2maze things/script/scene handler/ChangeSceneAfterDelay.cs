using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneAfterDelay : MonoBehaviour
{
    public string targetSceneName; // Name of the scene to load
    public float delay = 5f; // Time in seconds before changing the scene

    private void Start()
    {
        // Start the scene change after the specified delay
        Invoke("ChangeScene", delay);
    }

    private void ChangeScene()
    {
        // Load the target scene
        SceneManager.LoadScene(targetSceneName);
    }
}
