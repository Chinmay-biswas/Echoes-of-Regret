using UnityEngine;
using System.Collections;

public class FloatingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform startPoint;         // Starting position of the platform
    public Transform endPoint;           // Ending position of the platform
    public float moveSpeed = 2f;         // Speed of the platform movement
    public bool loop = true;             // Whether the platform loops back and forth
    public float waitTime = 1f;          // Time to wait at the end points

    [Header("Player Interaction")]
    public LayerMask playerLayer;        // Layer to detect the player
    private Transform player;            // Reference to the player
    private Rigidbody playerRb;          // Reference to the player's Rigidbody
    private Vector3 velocity;            // Current velocity of the platform
    private bool movingToEnd = true;     // Tracks the direction of movement
    private bool isWaiting = false;      // Tracks if the platform is currently waiting

    private void FixedUpdate()
    {
        if (!isWaiting)
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        // Determine the target position
        Transform target = movingToEnd ? endPoint : startPoint;

        // Move the platform towards the target position
        Vector3 previousPosition = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.fixedDeltaTime);
        velocity = (transform.position - previousPosition) / Time.fixedDeltaTime;

        // Check if the platform has reached the target
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            if (loop)
            {
                StartCoroutine(WaitAtEndPoint()); // Start waiting before switching direction
            }
        }

        // Carry the player if they're on the platform
        if (player != null && playerRb != null)
        {
            // Apply the platform's velocity to the player
            playerRb.velocity += velocity;
        }
    }

    private IEnumerator WaitAtEndPoint()
    {
        isWaiting = true; // Set waiting flag
        yield return new WaitForSeconds(waitTime); // Wait for the specified time
        movingToEnd = !movingToEnd; // Switch direction
        isWaiting = false; // Reset waiting flag
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player enters the platform's collider
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            player = collision.transform;
            playerRb = collision.rigidbody; // Get the player's Rigidbody
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if the player leaves the platform's collider
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            player = null;
            playerRb = null; // Reset the player's Rigidbody reference
        }
    }
}
