using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagermirror : MonoBehaviour
{
    [Header("Game Settings")]
    public int totalTargets; // Total targets in the level
    public Text statusText; // UI Text for status updates (optional)
    public GameObject winPanel; // The win panel to display upon completing the level
    public GameObject losePanel; // The lose panel to display when restarting the level

    private bool levelComplete; // Tracks if the level is complete

    private void Start()
    {
        levelComplete = false;

        // Count all target objects in the scene if not manually assigned
        if (totalTargets == 0)
        {
            totalTargets = GameObject.FindGameObjectsWithTag("Target").Length;
        }

        // Ensure the win & lose panels are initially inactive
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        UpdateStatusText();
    }

    // Call this function whenever a target is destroyed
    public void OnTargetDestroyed()
    {
        if (levelComplete) return;

        totalTargets--;

        UpdateStatusText();

        // Check if all targets are destroyed
        if (totalTargets <= 0)
        {
            EndLevel(true); // Calls EndLevel to activate win panel
        }
    }

    // Ends the level and handles win/lose conditions
    public void EndLevel(bool success)
    {
        if (levelComplete) return; // Prevents multiple calls

        levelComplete = true;

        if (success)
        {
            ActivateWinPanel();
        }
        else
        {
            ActivateLosePanel();
        }
    }

    private void ActivateWinPanel()
    {
        // Show the win panel
        if (winPanel != null) winPanel.SetActive(true);

        // Update status text if available
        if (statusText != null) statusText.text = "You Win!";

        Debug.Log("Level Complete! All targets destroyed.");
    }

    private void ActivateLosePanel()
    {
        // Show the lose panel
        if (losePanel != null) losePanel.SetActive(true);

        // Update status text if available
        if (statusText != null) statusText.text = "You Lose!";

        // Restart level after 3 seconds
    }

    

    private void UpdateStatusText()
    {
        if (statusText != null)
        {
            statusText.text = $"Targets Left: {totalTargets}";
        }
    }
}
