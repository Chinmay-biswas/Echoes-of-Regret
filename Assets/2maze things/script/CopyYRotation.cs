using UnityEngine;

public class CopyYRotation : MonoBehaviour
{
    public Transform targetObject; // The object to copy Y rotation from

    void Update()
    {
        if (targetObject != null)
        {
            // Get the current rotation of the target object
            float targetYRotation = targetObject.eulerAngles.y;

            // Apply only the Y rotation while keeping X and Z unchanged
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, targetYRotation, transform.eulerAngles.z);
        }
    }
}
