using UnityEngine;

public class Target : MonoBehaviour
{
    private bool isHit = false; // Tracks if the target was hit this frame

    public void OnLightHit()
    {
        if (isHit) return; // Prevent multiple hits in rapid succession

        Debug.Log($"{gameObject.name} was hit by the light!");
        isHit = true; // Mark as hit
        gameObject.SetActive(false); // Make the target invisible
    }

    public void ResetTarget()
    {
        isHit = false; // Reset hit state
        gameObject.SetActive(true); // Make the target reappear
    }
}
