using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyControl : MonoBehaviour
{

    [SerializeField] GameObject lowC;
    [SerializeField] GameObject lowD;
    [SerializeField] GameObject lowE;
    [SerializeField] GameObject lowF;
    [SerializeField] GameObject lowG;
    [SerializeField] GameObject lowA;
    [SerializeField] GameObject lowB;
    public GameObject gameController;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            gameController.GetComponent<PianoGame>().playerInput.Add("C");
            lowC.SetActive(false);
            lowC.SetActive(true);
        }
        //end key
        if (Input.GetKeyDown(KeyCode.W))
        {
            gameController.GetComponent<PianoGame>().playerInput.Add("D");
            lowD.SetActive(false);
            lowD.SetActive(true);
        }
        //end key
        if (Input.GetKeyDown(KeyCode.E))
        {
            gameController.GetComponent<PianoGame>().playerInput.Add("E");
            lowE.SetActive(false);
            lowE.SetActive(true);
        }
        //end key
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameController.GetComponent<PianoGame>().playerInput.Add("F");
            lowF.SetActive(false);
            lowF.SetActive(true);
        }
        //end key
        if (Input.GetKeyDown(KeyCode.T))
        {
            gameController.GetComponent<PianoGame>().playerInput.Add("G");
            lowG.SetActive(false);
            lowG.SetActive(true);
        }
        //end key
        if (Input.GetKeyDown(KeyCode.Y))
        {
            gameController.GetComponent<PianoGame>().playerInput.Add("A");
            lowA.SetActive(false);
            lowA.SetActive(true);
        }
        //end key
        if (Input.GetKeyDown(KeyCode.U))
        {
            gameController.GetComponent<PianoGame>().playerInput.Add("B");
            lowB.SetActive(false);
            lowB.SetActive(true);
        }
        //end key

    }
}