using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class   camcontrlpiano : MonoBehaviour
{
    [SerializeField] private Transform target; // The object or point the camera will look at
    [SerializeField] private float rotationSpeed = 100f; // Speed of mouse rotation

    private Vector3 fixedPosition; // The fixed position of the camera

    void Start()
    {
        // Store the initial position of the camera to keep it fixed
        fixedPosition = transform.position;

        if (target == null)
        {
            Debug.LogError("Camera target is not assigned! Please assign a target for the camera to look at.");
        }
    }

    void Update()
    {
        RotateCameraAroundTarget();
    }

    private void RotateCameraAroundTarget()
    {
        // Ensure the camera stays at the fixed position
        transform.position = fixedPosition;

        if (target != null)
        {
            // Rotate around the target based on mouse input
            float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float verticalRotation = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            // Rotate the camera around the target
            transform.RotateAround(target.position, Vector3.up, horizontalRotation); // Horizontal rotation
            transform.RotateAround(target.position, transform.right, -verticalRotation); // Vertical rotation

            // Always look at the target
            transform.LookAt(target);
        }
    }
}
