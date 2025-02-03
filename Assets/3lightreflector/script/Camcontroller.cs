using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Pan Settings")]
    [SerializeField] private float panSpeed = 20f; // Speed of camera panning
    [SerializeField] private float dragSpeed = 10f; // Speed of panning with middle mouse button

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 5f; // Speed of zooming with mouse wheel
    [SerializeField] private float minZoom = 5f; // Minimum zoom level
    [SerializeField] private float maxZoom = 20f; // Maximum zoom level

    [Header("View Settings")]
    [SerializeField] private Vector3 isometricPosition = new Vector3(10, 10, 10); // Default isometric position
    [SerializeField] private Vector3 isometricRotation = new Vector3(30, 45, 0); // Default isometric rotation

    private bool isIsometricView = false; // Tracks whether the camera is in isometric view
    private Vector3 dragOrigin; // For dragging camera with middle mouse button

    void Update()
    {
        HandlePan();
        HandleZoom();
        HandleViewToggle();
    }

    private void HandlePan()
    {
        // Pan the camera with WASD or arrow keys
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.Translate(direction * panSpeed * Time.deltaTime, Space.World);

        // Pan the camera by dragging with the middle mouse button
        if (Input.GetMouseButtonDown(2)) // Middle mouse button pressed
        {
            dragOrigin = Input.mousePosition;
        }

        if (Input.GetMouseButton(2)) // Middle mouse button held
        {
            Vector3 difference = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            transform.Translate(-difference.x * dragSpeed, 0, -difference.y * dragSpeed, Space.World);
            dragOrigin = Input.mousePosition;
        }
    }

    private void HandleZoom()
    {
        // Zoom the camera with the mouse scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            if (Camera.main.orthographic)
            {
                // Adjust orthographic size for orthographic view
                Camera.main.orthographicSize -= scroll * zoomSpeed;
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
            }
            else
            {
                // Adjust position for perspective view
                Vector3 position = transform.position;
                position.y -= scroll * zoomSpeed;
                position.y = Mathf.Clamp(position.y, minZoom, maxZoom);
                transform.position = position;
            }
        }
    }

    private void HandleViewToggle()
    {
        // Toggle between isometric and perspective views
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Press "1" to toggle
        {
            ToggleIsometricView();
        }
    }

    private void ToggleIsometricView()
    {
        isIsometricView = !isIsometricView;

        if (isIsometricView)
        {
            // Set up isometric view
            transform.position = isometricPosition;
            transform.rotation = Quaternion.Euler(isometricRotation);
            Camera.main.orthographic = true; // Use orthographic projection for isometric
        }
        else
        {
            // Reset to perspective view (or any custom default position)
            Camera.main.orthographic = false; // Switch to perspective projection
        }
    }
}
