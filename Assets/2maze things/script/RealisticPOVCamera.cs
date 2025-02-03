using UnityEngine;

public class RealisticPOVCamera : MonoBehaviour
{
    [Header("Mouse Look Settings")]
    public float sensitivity = 100f; // Default sensitivity
    public float smoothTime = 0.1f;  // For smoothing camera movement
    private float originalSensitivity; // Store original sensitivity

    [Header("Head Bobbing Settings")]
    public bool enableHeadBobbing = true;
    public float bobbingSpeed = 14f; // Speed of head bobbing
    public float bobbingAmount = 0.05f; // Amount of head bobbing

    [Header("Tilt Settings")]
    public bool enableTilt = true;
    public float tiltAmount = 5f; // How much the camera tilts when strafing
    public float tiltSpeed = 5f; // Speed at which the tilt is applied and removed

    [Header("Player Settings")]
    public Transform playerBody; // Reference to the player body
    public float verticalClampAngle = 90f; // Clamp vertical rotation

    private float xRotation = 0f; // Tracks camera's vertical rotation
    private Vector3 originalCameraPosition; // For head bobbing
    private Vector3 currentVelocity; // For smoothing
    private float targetTilt = 0f; // Target tilt angle
    private float currentTilt = 0f; // Current tilt angle

    private void Start()
    {
        // Ensure the cursor is always visible and unlocked
        Cursor.lockState = CursorLockMode.None; // Do not lock the cursor
        Cursor.visible = true; // Make sure the cursor is visible

        originalCameraPosition = transform.localPosition; // Store the original position
        originalSensitivity = sensitivity; // Save the original sensitivity
    }

    private void Update()
    {
        // Handle mouse look only if the cursor is not interacting with UI
        HandleMouseLook();

        if (enableHeadBobbing) HandleHeadBobbing();
        if (enableTilt) HandleCameraTilt();
    }

    private void HandleMouseLook()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // Update vertical rotation and clamp
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalClampAngle, verticalClampAngle);

        // Smoothly rotate camera
        Vector3 targetRotation = new Vector3(xRotation, 0f, 0f);
        transform.localRotation = Quaternion.Euler(Vector3.SmoothDamp(transform.localRotation.eulerAngles, targetRotation, ref currentVelocity, smoothTime));

        // Rotate the player body horizontally
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void HandleHeadBobbing()
    {
        // Simulate head bobbing based on movement
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            float wave = Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
            transform.localPosition = new Vector3(originalCameraPosition.x, originalCameraPosition.y + wave, originalCameraPosition.z);
        }
        else
        {
            // Reset to the original position when not moving
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalCameraPosition, Time.deltaTime * bobbingSpeed);
        }
    }

    private void HandleCameraTilt()
    {
        // Get horizontal input (A/D or Left/Right arrow keys)
        float horizontalInput = Input.GetAxis("Horizontal");

        // Determine the target tilt angle
        targetTilt = -horizontalInput * tiltAmount;

        // Smoothly interpolate to the target tilt
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);

        // Apply tilt to the camera
        transform.localRotation = Quaternion.Euler(xRotation, playerBody.rotation.eulerAngles.y, currentTilt);
    }
}
