using UnityEngine;

public class flicker : MonoBehaviour
{
    public Light lightSource;          // Reference to the Light component
    public float minIntensity = 0.5f;  // Minimum intensity for flicker
    public float maxIntensity = 2f;    // Maximum intensity for flicker
    public float flickerSpeed = 0.1f;  // Speed of flickering (lower value = faster flicker)
    private float targetIntensity;     // Target intensity the light will flicker towards
    private float smoothSpeed = 2f;    // How quickly the light intensity changes

    void Start()
    {
        if (lightSource == null)
        {
            lightSource = GetComponent<Light>();  // Get the Light component if not set
        }

        // Initial intensity
        targetIntensity = Random.Range(minIntensity, maxIntensity);
    }

    void Update()
    {
        // Randomly change the intensity over time
        if (lightSource != null)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            lightSource.intensity = Mathf.Lerp(lightSource.intensity, targetIntensity, Time.deltaTime * smoothSpeed);
        }
    }
}
