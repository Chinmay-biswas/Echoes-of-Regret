using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{

    public float pressDepth = 15f; // How far the key moves when pressed
    public float returnSpeed = 5f; // Speed at which the key returns to its original position
    private Vector3 originalPosition; // The original position of the key
    public bool isPressed = false;
    [SerializeField] KeyCode key;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            isPressed = true;
        }
        if (Input.GetKeyUp(key))
        {
            isPressed = false;
        }
        playKey();
    }
    void playKey()
    {
        if (isPressed)
        {
            // Move the key downward when pressed
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition - new Vector3(0, pressDepth, 0), Time.deltaTime * returnSpeed);
        }
        else
        {
            // Move the key back to its original position when released
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * returnSpeed);
        }
    }


}
