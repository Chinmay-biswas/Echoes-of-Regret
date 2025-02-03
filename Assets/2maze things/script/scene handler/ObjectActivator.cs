using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    public string activationKey; // Unique key to identify the object

    public void ActivateObject()
    {
        // Set the PlayerPrefs flag for the specified key
        PlayerPrefs.SetInt(activationKey, 1);
        PlayerPrefs.Save();
        Debug.Log($"ActivateObject flag set for key: {activationKey}");
    }

    public void DeactivateObject()
    {
        // Reset the PlayerPrefs flag for the specified key
        PlayerPrefs.SetInt(activationKey, 0);
        PlayerPrefs.Save();
        Debug.Log($"ActivateObject flag reset for key: {activationKey}");
    }
}
