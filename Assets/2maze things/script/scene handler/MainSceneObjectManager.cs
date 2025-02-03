using UnityEngine;

public class MainSceneObjectManager : MonoBehaviour
{
    [System.Serializable]
    public class ObjectActivationPair
    {
        public string activationKey; // Unique key to identify the button
        public GameObject objectToActivate; // The object to activate for this key
    }

    public ObjectActivationPair[] objectsToManage; // Array to manage multiple objects and keys

    private void Update()
    {
        // Loop through all objects in the array
        foreach (ObjectActivationPair pair in objectsToManage)
        {
            // Check if the PlayerPrefs flag for the current key is set
            if (PlayerPrefs.GetInt(pair.activationKey, 0) == 1)
            {
                // Activate the associated object
                pair.objectToActivate.SetActive(true);
            }
            else
            {
                // Ensure the object is inactive if the flag is not set
                pair.objectToActivate.SetActive(false);
            }
        }
    }
}
