using UnityEngine;

public class ResetPlayerPrefs : MonoBehaviour
{
    public void ResetAllPreferences()
    {
        // Clear all PlayerPrefs data
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("All PlayerPrefs have been reset.");
    }
}
