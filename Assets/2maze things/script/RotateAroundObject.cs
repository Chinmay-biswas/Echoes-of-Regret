using UnityEngine;

public class RotateAroundObject : MonoBehaviour
{
    [Header("Rotation Settings")]
    public Transform targetObject;       // The object to rotate around
    public float rotationSpeed = 50f;   // Speed of rotation in degrees per second
    public Vector3 rotationAxis = Vector3.up; // Axis to rotate around

    private void Update()
    {
        if (targetObject != null)
        {
            // Rotate the object around the target object
            transform.RotateAround(targetObject.position, rotationAxis, rotationSpeed * Time.deltaTime);
        }
    }
}
