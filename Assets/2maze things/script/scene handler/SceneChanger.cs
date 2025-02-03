using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneChanger : MonoBehaviour
{
    public string targetSceneName; // Name of the target scene

    public float sceneChangeDelay = 2f; // Delay before changing to the target scene

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Start the coroutine to change the scene after the delay
            StartCoroutine(ChangeSceneAfterDelay());
        }
    }

    private System.Collections.IEnumerator ChangeSceneAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(sceneChangeDelay);
        // Load the target scene
        SceneManager.LoadScene(targetSceneName);
    }
}
