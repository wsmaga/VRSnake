using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Events;
using System.Threading;
using System;

public class MenuButton : MonoBehaviour
{
    public Image circleIndicator;
    public UnityEvent buttonClickEvent;
    public float clickTime = 1;
    bool status = false;
    public float gazeTimer = 0;

    private void Start()
    {
        Debug.Log("BUTTON STARTED");
    }

    // Update is called once per frame
    void Update()
    {
        if (status)
        {
            gazeTimer += Time.deltaTime;
            circleIndicator.fillAmount = gazeTimer / clickTime;
        }

        if(gazeTimer > clickTime)
        {
            status = false;
            circleIndicator.fillAmount = 0;
            gazeTimer = 0;
            Debug.Log("BUTTON CLICKED");
            buttonClickEvent.Invoke();
        }
    }

    public void GazeOn()
    {
        Debug.Log("GAZE ON");
        status = true;
    }

    public void GazeOff()
    {
        Debug.Log("GAZE OFF");
        status = false;
        gazeTimer = 0;
        circleIndicator.fillAmount = 0;
    }
}
