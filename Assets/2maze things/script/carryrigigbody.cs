using System.Collections.Generic;
using UnityEngine;

public class carryrigigbody : MonoBehaviour
{
    public List<Rigidbody> rigidbodies = new List<Rigidbody>(); // List to store Rigidbody objects on the platform
    private Vector3 lastPosition; // Last position of the platform
    private Vector3 lastEulerAngles; // Last rotation of the platform
    private Transform _transform; // Reference to the platform's transform

    public bool useTriggerAsSensor = false; // Optional: Use trigger as a sensor instead of collision

    void Start()
    {
        // Initialize platform transform and its position/rotation tracking
        _transform = transform;
        lastPosition = _transform.position;
        lastEulerAngles = _transform.eulerAngles;
    }

    void LateUpdate()
    {
        if (rigidbodies.Count > 0)
        {
            // Calculate platform movement and angular velocity
            Vector3 velocity = _transform.position - lastPosition;
            Vector3 angularVelocity = _transform.eulerAngles - lastEulerAngles;

            for (int i = 0; i < rigidbodies.Count; i++)
            {
                Rigidbody rb = rigidbodies[i];
                if (rb != null)
                {
                    // Move rigidbody with platform's movement
                    rb.transform.Translate(velocity, _transform);

                    // Rotate rigidbody with platform's rotation
                    RotateRigidbody(rb, angularVelocity.y);
                }
            }
        }

        // Update the platform's last position and rotation
        lastPosition = _transform.position;
        lastEulerAngles = _transform.eulerAngles;
    }

    private void RotateRigidbody(Rigidbody rb, float angularVelocityY)
    {
        // Rotate the Rigidbody around the Y-axis
        rb.transform.RotateAround(_transform.position, Vector3.up, angularVelocityY);
    }

    private void OnCollisionEnter(Collision c)
    {
        if (useTriggerAsSensor) return;

        // Add Rigidbody to the list if it's not already there
        Rigidbody rb = c.collider.GetComponent<Rigidbody>();
        if (rb != null && !rigidbodies.Contains(rb))
        {
            rigidbodies.Add(rb);
        }
    }

    private void OnCollisionExit(Collision c)
    {
        if (useTriggerAsSensor) return;

        // Remove Rigidbody from the list when it leaves the platform
        Rigidbody rb = c.collider.GetComponent<Rigidbody>();
        if (rb != null && rigidbodies.Contains(rb))
        {
            rigidbodies.Remove(rb);
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if (!useTriggerAsSensor) return;

        // Add Rigidbody to the list if using trigger as sensor
        Rigidbody rb = c.GetComponent<Rigidbody>();
        if (rb != null && !rigidbodies.Contains(rb))
        {
            rigidbodies.Add(rb);
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (!useTriggerAsSensor) return;

        // Remove Rigidbody from the list if using trigger as sensor
        Rigidbody rb = c.GetComponent<Rigidbody>();
        if (rb != null && rigidbodies.Contains(rb))
        {
            rigidbodies.Remove(rb);
        }
    }
}
