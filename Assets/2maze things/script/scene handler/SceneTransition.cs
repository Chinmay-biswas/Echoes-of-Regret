using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonSceneTransition : MonoBehaviour
{
    // Name of the scene to load
    public string sceneName;

    // Delay time before changing the scene
    public float delayTime = 1.0f;

    // Function to call when the button is pressed
    public void OnButtonPress()
    {
        StartCoroutine(LoadSceneAfterDelay());
    }

    // Coroutine to wait and then change the scene
    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayTime); // Wait for the delay
        SceneManager.LoadScene(sceneName); // Load the specified scene
    }
}
