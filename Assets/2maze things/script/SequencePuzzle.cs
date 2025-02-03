using UnityEngine;

public class SequencePuzzle : MonoBehaviour
{
    [Header("Objects and Lights")]
    public GameObject[] sequenceObjects; // The four objects to touch in sequence
    public GameObject[] lights;          // The eight lights to activate
    public GameObject extraObject;       // The object to activate after the sequence is completed

    private int currentSequenceIndex = 0; // Tracks the current sequence step
    private bool sequenceCompleted = false; // Flag to check if the sequence is completed

    private void Start()
    {
        // Deactivate all lights at the beginning
        ResetLights();

        // Make sure the extra object is inactive
        if (extraObject != null)
        {
            extraObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the sequence is already completed, ignore further interactions
        if (sequenceCompleted) return;

        Debug.Log($"Touched object: {other.gameObject.name}, Expected: {sequenceObjects[currentSequenceIndex].name}");

        // Check if the object touched is the correct one in the sequence
        if (other.gameObject == sequenceObjects[currentSequenceIndex])
        {
            HandleCorrectObjectTouch();
        }
        else if (!IsObjectInSequence(other.gameObject))
        {
            // Reset only if the object is not in the sequence
            HandleWrongObjectTouch();
        }
    }

    private void HandleCorrectObjectTouch()
    {
        Debug.Log($"Correct object touched: {sequenceObjects[currentSequenceIndex].name}");

        // Activate lights for the current step
        ActivateLightsForCurrentStep();
        currentSequenceIndex++;

        // Check if the sequence is complete
        if (currentSequenceIndex >= sequenceObjects.Length)
        {
            Debug.Log("Sequence completed! Activating extra object.");

            // Activate the extra object
            if (extraObject != null)
            {
                extraObject.SetActive(true);
            }

            // Mark the sequence as completed
            sequenceCompleted = true;
        }
    }

    private void HandleWrongObjectTouch()
    {
        Debug.Log("Wrong object touched! Resetting sequence.");

        // Reset the sequence and deactivate all lights
        ResetLights();
        currentSequenceIndex = 0;
    }

    private void ActivateLightsForCurrentStep()
    {
        // Activate two lights for each correct step
        int startIndex = currentSequenceIndex * 2;
        for (int i = startIndex; i < startIndex + 2; i++)
        {
            if (i < lights.Length)
            {
                lights[i].SetActive(true);
            }
        }
    }

    private void ResetLights()
    {
        // Deactivate all lights
        foreach (GameObject light in lights)
        {
            light.SetActive(false);
        }
    }

    private bool IsObjectInSequence(GameObject obj)
    {
        // Check if the object exists in the sequenceObjects array
        foreach (GameObject sequenceObject in sequenceObjects)
        {
            if (obj == sequenceObject)
            {
                return true;
            }
        }
        return false;
    }
}
