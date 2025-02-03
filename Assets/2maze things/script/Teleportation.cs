using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Teleportation : MonoBehaviour
{
    [System.Serializable]
    public class Portal
    {
        [Tooltip("The location the player will be teleported to.")]
        public Transform teleportLocation;

        [Tooltip("Optional particle effect to play when teleporting.")]
        public ParticleSystem teleportEffect;

        [Tooltip("Optional sound to play when teleporting.")]
        public AudioClip teleportSound;
    }

    [Header("Portal Settings")]
    [Tooltip("Tag to identify the player.")]
    [SerializeField] private string playerTag = "Player";

    [Tooltip("Settings for the portal.")]
    [SerializeField] private Portal portalSettings;

    [Header("Post-Processing Settings")]
    [Tooltip("Post-Processing Profile with desired effects.")]
    [SerializeField] private PostProcessProfile postProcessProfile;

    [Tooltip("Duration of effects during teleportation.")]
    [SerializeField] private float effectDuration = 1.5f;

    [Tooltip("Delay before teleporting the player.")]
    [SerializeField] private float teleportDelay = 0.5f;

    // Post-Processing effects
    private DepthOfField blurEffect;
    private MotionBlur motionBlurEffect;
    private Bloom bloomEffect;
    private Vignette vignetteEffect;
    private ChromaticAberration chromaticAberrationEffect;
    private LensDistortion lensDistortionEffect;

    private bool isTeleporting = false;

    private void Start()
    {
        // Initialize Post-Processing effects from the profile
        if (postProcessProfile != null)
        {
            postProcessProfile.TryGetSettings(out blurEffect);
            postProcessProfile.TryGetSettings(out motionBlurEffect);
            postProcessProfile.TryGetSettings(out bloomEffect);
            postProcessProfile.TryGetSettings(out vignetteEffect);
            postProcessProfile.TryGetSettings(out chromaticAberrationEffect);
            postProcessProfile.TryGetSettings(out lensDistortionEffect);

            // Ensure all effects are initially disabled
            DisableAllEffects();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && !isTeleporting)
        {
            isTeleporting = true;

            // Play teleport sound
            if (portalSettings.teleportSound != null)
            {
                AudioSource.PlayClipAtPoint(portalSettings.teleportSound, transform.position);
            }

            // Play particle effect
            if (portalSettings.teleportEffect != null)
            {
                portalSettings.teleportEffect.Play();
            }

            // Start teleportation sequence with effects
            StartCoroutine(TeleportWithEffects(other));
        }
    }

    private System.Collections.IEnumerator TeleportWithEffects(Collider player)
    {
        // Enable effects
        EnableAllEffects();

        // Wait before teleporting
        yield return new WaitForSeconds(teleportDelay);

        // Teleport the player
        if (portalSettings.teleportLocation != null)
        {
            player.transform.position = portalSettings.teleportLocation.position;
        }

        // Gradually disable effects
        float timer = effectDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            if (blurEffect != null)
                blurEffect.aperture.value = Mathf.Lerp(32f, 0f, 1 - (timer / effectDuration));
            if (vignetteEffect != null)
                vignetteEffect.intensity.value = Mathf.Lerp(0.5f, 0f, 1 - (timer / effectDuration));
            if (chromaticAberrationEffect != null)
                chromaticAberrationEffect.intensity.value = Mathf.Lerp(1f, 0f, 1 - (timer / effectDuration));
            if (lensDistortionEffect != null)
                lensDistortionEffect.intensity.value = Mathf.Lerp(-50f, 0f, 1 - (timer / effectDuration));

            yield return null;
        }

        // Ensure all effects are fully disabled
        DisableAllEffects();

        isTeleporting = false;
    }

    private void EnableAllEffects()
    {
        if (blurEffect != null)
        {
            blurEffect.enabled.value = true;
            blurEffect.aperture.value = 32f;
        }

        if (motionBlurEffect != null)
        {
            motionBlurEffect.enabled.value = true;
        }

        if (bloomEffect != null)
        {
            bloomEffect.enabled.value = true;
            bloomEffect.intensity.value = 10f;
        }

        if (vignetteEffect != null)
        {
            vignetteEffect.enabled.value = true;
            vignetteEffect.intensity.value = 0.5f;
        }

        if (chromaticAberrationEffect != null)
        {
            chromaticAberrationEffect.enabled.value = true;
            chromaticAberrationEffect.intensity.value = 1f;
        }

        if (lensDistortionEffect != null)
        {
            lensDistortionEffect.enabled.value = true;
            lensDistortionEffect.intensity.value = -50f;
        }
    }

    private void DisableAllEffects()
    {
        if (blurEffect != null)
            blurEffect.enabled.value = false;

        if (motionBlurEffect != null)
            motionBlurEffect.enabled.value = false;

        if (bloomEffect != null)
            bloomEffect.enabled.value = false;

        if (vignetteEffect != null)
            vignetteEffect.enabled.value = false;

        if (chromaticAberrationEffect != null)
            chromaticAberrationEffect.enabled.value = false;

        if (lensDistortionEffect != null)
            lensDistortionEffect.enabled.value = false;
    }
}
