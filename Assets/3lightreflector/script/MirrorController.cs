using UnityEngine;

public class MirrorController : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed at which the mirror rotates
    private bool isSelected = false; // Tracks if this mirror is currently selected
    private bool isClockwise = true; // Tracks rotation direction
    public LightBeamController lightBeamController; // Reference to the LightBeamController

    private float currentRotation = 0f; // Tracks the current rotation of the mirror

    void Start()
    {
        // Find the LightBeamController in the scene
        lightBeamController = FindObjectOfType<LightBeamController>();
    }

    void Update()
    {
        HandleSelection(); // Check for mouse clicks on this mirror
        if (isSelected)
        {
            HandleContinuousRotation(); // Rotate the mirror while the mouse button is held
        }
    }

    private void HandleSelection()
    {
        // Detect mouse click
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            // Cast a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits this mirror
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject) // Ensure it's this specific mirror
                {
                    isSelected = true; // Select this mirror
                    isClockwise = true; // Set rotation to clockwise
                }
            }
        }
        else if (Input.GetMouseButtonDown(1)) // Right-click
        {
            // Cast a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits this mirror
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject) // Ensure it's this specific mirror
                {
                    isSelected = true; // Select this mirror
                    isClockwise = false; // Set rotation to anticlockwise
                }
            }
        }

        // Deselect the mirror when the mouse button is released
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            isSelected = false;
        }
    }

    private void HandleContinuousRotation()
    {
        // Rotate the mirror continuously while the mouse button is held
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            // Adjust rotation direction based on isClockwise
            float direction = isClockwise ? 1 : -1;
            currentRotation += direction * rotationSpeed * Time.deltaTime;

            // Apply the rotation to the mirror
            transform.rotation = Quaternion.Euler(0, currentRotation, 0);

            // Notify the LightBeamController to turn off the light
            if (lightBeamController != null && lightBeamController.IsBeamActive)
            {
                lightBeamController.TurnOffLight();
            }
        }
    }

    public static Vector3 Reflect(Vector3 incoming, Vector3 normal)
    {
        return incoming - 2 * Vector3.Dot(incoming, normal) * normal;
    }
}
