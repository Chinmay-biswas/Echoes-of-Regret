using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class canvasScript : MonoBehaviour
{
    public enum state {play, success, fail, none};
   
    [SerializeField] GameObject successPanel;
    [SerializeField] GameObject failPanel;
    public state state1 ;
    // Start is called before the first frame update
    void Start()
    {
        state1 = state.play;
    }

    // Update is called once per frame
    void Update()
    {
        if (state1 != state.none)
        {
            
            if (state1 == state.success)
            {
                //load suc panel
                successPanel.SetActive(true);
            }
            else if (state1 == state.fail)
            {
                //load fail
                failPanel.SetActive(true);
            }
        }
        else
        {
            failPanel.SetActive(false);
            successPanel.SetActive(false);
        }
    }
    public void replayfn()
    {
        
    }
}
