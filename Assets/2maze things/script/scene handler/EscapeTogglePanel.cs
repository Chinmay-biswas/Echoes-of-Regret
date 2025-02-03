using UnityEngine;
using UnityEngine.UI;

public class EscapeTogglePanel : MonoBehaviour
{
    public GameObject panel; // Assign the UI panel in the Inspector

    void Start()
    {
        if (panel != null)
        {
            panel.SetActive(false); // Ensure the panel starts as inactive
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Check if Escape key is pressed
        {
            if (panel != null)
            {
                panel.SetActive(!panel.activeSelf); // Toggle panel visibility
            }
        }
    }
}
