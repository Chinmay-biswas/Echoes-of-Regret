using UnityEngine;

public class ResetObjectActivator : MonoBehaviour
{
    public string objectKey; // Unique identifier for the object to reset

    public void ResetObject()
    {
        // Set the PlayerPrefs value for the specific object to 0
        PlayerPrefs.SetInt(objectKey, 0);
        PlayerPrefs.Save();
        Debug.Log($"Object {objectKey} flag reset to 0");

        // Optionally, log the reset or transition to the main scene after the reset
        Debug.Log("Reset triggered, switch to the main scene to see the effect.");
    }
}
