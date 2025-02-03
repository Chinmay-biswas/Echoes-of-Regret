using UnityEngine;
using UnityEngine.UI;

public class ClickToActivatePanel : MonoBehaviour
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
        if (Input.GetMouseButtonDown(0)) // Detect left mouse click
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) // Check if we hit something
            {
                if (hit.transform == transform) // If the clicked object is this one
                {
                    panel.SetActive(true); // Activate the panel
                }
            }
        }
    }
}
