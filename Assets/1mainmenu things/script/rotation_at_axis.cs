using UnityEngine;

public class   rotate_at_axis : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of rotation in degrees per second
    public Vector3 rotationAxis = Vector3.up; // Axis to rotate around, default is the Y-axis

    void Update()
    {
        // Rotate the object around the specified axis at the specified speed
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
