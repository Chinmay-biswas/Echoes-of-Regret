using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton pattern
    private Vector3 playerPosition; // To store player's position
    private string previousSceneName;
    private bool isReturning;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist GameManager across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerState(Vector3 position, string sceneName)
    {
        playerPosition = position;
        previousSceneName = sceneName;
    }

    public void LoadNextScene(string nextSceneName)
    {
        isReturning = false; // Not returning yet
        SceneManager.LoadScene(nextSceneName);
    }

    public void ReturnToPreviousScene()
    {
        isReturning = true; // Mark as returning
        SceneManager.LoadScene(previousSceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isReturning && scene.name == previousSceneName)
        {
            // Restore player position
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = playerPosition;
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
