using UnityEngine;
using UnityEngine.UI;

public class LightBeamController : MonoBehaviour
{
    public LineRenderer lineRenderer; // For visualizing the beam
    public int maxReflections = 10; // Maximum number of reflections
    public LayerMask reflectionLayers; // Layers to detect mirrors, obstacles, and targets
    private bool isBeamActive = false; // Tracks if the beam is active
    private bool isLightActivated = false; // Tracks if the light has been turned on for the first time
    public int toggleCount = 0; // Tracks the number of toggles
    public int maxToggles = 3; // Maximum number of toggles allowed
    private Target[] allTargets; // List of all target objects in the scene
    private GameManagermirror gameManager; // Reference to the Game Manager
    public Text toggleCountText;

    // Public property to expose beam state
    public bool IsBeamActive => isBeamActive;

    void Start()
    {
        // Cache all target objects in the scene
        allTargets = FindObjectsOfType<Target>();
        gameManager = FindObjectOfType<GameManagermirror>(); // Find the game manager in the scene
    }

    void Update()
    {
        if (toggleCountText != null)
        {
            toggleCountText.text = "Toggle Count: " + (maxToggles - toggleCount);
        }

        HandleBeamToggle(); // Check for toggling the light beam

        if (isBeamActive)
        {
            CastLightBeam(); // Cast the beam if it's active
            CheckTargets(); // Check if all targets are hit
        }
        else
        {
            lineRenderer.positionCount = 0; // Clear the beam when inactive
        }
    }

    private void CastLightBeam()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward; // Initial direction of the light beam

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, origin);

        int reflections = 0;

        while (reflections < maxReflections)
        {
            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity, reflectionLayers))
            {
                Debug.DrawRay(origin, direction * hit.distance, Color.red, 2f); // Debug ray
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

                if (hit.collider.CompareTag("Mirror"))
                {
                    // Reflect the beam if it hits a mirror
                    Vector3 normal = hit.normal;
                    direction = MirrorController.Reflect(direction, normal);
                    origin = hit.point;

                    reflections++;
                }
                else if (hit.collider.CompareTag("Target"))
                {
                    Target target = hit.collider.GetComponent<Target>();
                    if (target != null)
                    {
                        target.OnLightHit(); // Mark the target as hit
                    }
                    break; // Stop the beam after hitting a target
                }
                else
                {
                    // Stop the beam on walls or other obstacles
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                    break;
                }
            }
            else
            {
                // No more collisions, draw the beam to the max distance
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, origin + direction * 100f);
                break;
            }
        }
    }

    
        private void HandleBeamToggle()
    {
        // Detect mouse clicks
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits this light source
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject && toggleCount < maxToggles)
                {
                    isBeamActive = !isBeamActive; // Toggle the beam state
                    if (isBeamActive)
                    {
                        isLightActivated = true; // Mark the light as activated on the first toggle
                        Debug.Log($"Light beam toggled ON. Toggles left: {maxToggles - toggleCount}");
                    }
                    else
                    {
                        ResetAllTargets(); // Reset targets when the light is turned off
                        Debug.Log("Light beam toggled OFF. Resetting targets.");
                    }

                    toggleCount++;

                    // If toggles are exhausted, restart the game
                    if (toggleCount >= maxToggles && !AllTargetsHit())
                    {
                        gameManager.EndLevel(false); // Trigger level failure
                    }
                }
            }
        }
    }

    public void TurnOffLight()
    {
        if (!isBeamActive) return; // Prevent unnecessary calls
        isBeamActive = false;
        lineRenderer.positionCount = 0; // Clear the beam
        ResetAllTargets(); // Reset targets when the light is turned off
        Debug.Log("Light turned off. Resetting targets.");
    }

    private void ResetAllTargets()
    {
        foreach (Target target in allTargets)
        {
            target.ResetTarget(); // Reset each target
        }
    }


    private void CheckTargets()
    {
        if (AllTargetsHit())
        {
            // All targets are hit, proceed to the next level
            Debug.Log("All targets hit! Proceeding to the next level.");
            gameManager.EndLevel(true); // Inform the Game Manager to end the level successfully
        }
    }

    private bool AllTargetsHit()
    {
        foreach (Target target in allTargets)
        {
            if (target.gameObject.activeSelf)
            {
                return false; // A target is still active
            }
        }
        return true; // All targets are hit
    }
}
