using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [Header("Player Settings")]
    public Transform player;                 // Reference to the player
    public float respawnYThreshold = -10f;   // Y-position threshold for respawn

    [Header("Checkpoints")]
    public Transform[] checkpoints;         // Array of checkpoints
    private int currentCheckpointIndex = 0; // Index of the last activated checkpoint

    private void Update()
    {
        // Check if the player's Y position is below the threshold
        if (player.position.y < respawnYThreshold)
        {
            RespawnPlayer();
        }
    }

    public void SetCheckpoint(int checkpointIndex)
    {
        if (checkpointIndex >= 0 && checkpointIndex < checkpoints.Length)
        {
            currentCheckpointIndex = checkpointIndex; // Update the current checkpoint
            Debug.Log($"Checkpoint {checkpointIndex} activated!");
        }
        else
        {
            Debug.LogWarning("Invalid checkpoint index!");
        }
    }

    private void RespawnPlayer()
    {
        // Respawn the player at the last activated checkpoint
        Transform checkpoint = checkpoints[currentCheckpointIndex];
        player.position = checkpoint.position;
        player.rotation = checkpoint.rotation;

        // Optional: Reset player's velocity if using Rigidbody
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.velocity = Vector3.zero;
        }

        Debug.Log("Player respawned at the last checkpoint!");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters a trigger collider
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < checkpoints.Length; i++)
            {
                if (other.transform == checkpoints[i]) // Check if the collider matches a checkpoint
                {
                    SetCheckpoint(i); // Activate the checkpoint
                    break;
                }
            }
        }
    }
}
