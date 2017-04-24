using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ThumbTracker : MonoBehaviour {

    VRTK_ControllerEvents events;  // VRTK grab event handler
    GameObject controller;  // left controller
    WheelController wheelController;

    public float angle;

    float rawAngle;
    float oldAngle;
    bool tracking;

    private void Start()
    // Sets up references
    {
        controller = GameObject.Find("LeftController");
        events = controller.GetComponent<VRTK_ControllerEvents>();
        wheelController = GetComponent<WheelController>();

        events.TouchpadTouchStart += StartTracking;
        events.TouchpadTouchEnd += StopTracking;
    }

    void StartTracking(object sender, ControllerInteractionEventArgs e)
    // Starts tracking coroutine
    {
        tracking = true;
        oldAngle = events.GetTouchpadAxisAngle() + 0.1f;
        StartCoroutine("TrackThumb");
    }

    void StopTracking(object sender, ControllerInteractionEventArgs e)
    // Stops tracking coroutine
    {
        tracking = false;
    }

    void SendAngle()
    // Sends position to Wheel Controller
    {
        wheelController.SendNewAngle(angle);
    }

    void RecalculateAngle()
    {
        angle = rawAngle;
        oldAngle = rawAngle;
    }

    IEnumerator TrackThumb()
    {
        while(tracking)
        {
            rawAngle = events.GetTouchpadAxisAngle();
            if (rawAngle != oldAngle)
            {
                RecalculateAngle();
                SendAngle();
            }
            yield return null;
        }
    }
}
