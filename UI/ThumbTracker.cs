using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ThumbTracker : MonoBehaviour {

    VRTK_ControllerEvents events;  // VRTK grab event handler
    GameObject controller;  // left controller
    WheelController wheelController;
    DisplayUI displayUI;

    float angle;  // old angle, stored for comparison
    float angleChange;  // Amount change from last set angle
    float rawAngle;  // raw angle returned from thumb position

    float rawPosition;  // Tracked position, raw state here
    float position;  // Stored position
    public float swipeReq;  // swipe requirement ( out of a total of 2.0)
    float swipe;

    string direction; // right or left

    bool trackingAngle;  // controls angle tracker coroutine
    bool trackingPosition;  // controls position tracker coroutine

    private void Start()
    // Sets up references
    {
        controller = GameObject.Find("LeftController");
        events = controller.GetComponent<VRTK_ControllerEvents>();
        wheelController = GetComponent<WheelController>();

        displayUI = GetComponent<DisplayUI>();
        events.TouchpadTouchStart += DoStartTracking;
        events.TouchpadTouchEnd += StopTracking;
    }

    void DoStartTracking(object sender, ControllerInteractionEventArgs e)
    // Starts tracking coroutine
    {
        StartTracking();
    }

    public void StartTracking()
    {
        if (displayUI.showBuildings)
        {
            trackingAngle = true;
            angle = events.GetTouchpadAxisAngle();

            StartCoroutine("TrackThumbAngle");
        }
        else if(!displayUI.showBuildings)
        {
            trackingPosition = true;
            position = events.GetTouchpadAxis().x;

            StartCoroutine("TrackThumbPosition");
        }
    }

    void StopTracking(object sender, ControllerInteractionEventArgs e)
    // Stops tracking coroutine
    {
        ForceStopTrackingAngle();
        ForceStopTrackingPosition();
    }

    public void ForceStopTrackingAngle()
    {
        trackingAngle = false;
    }

    public void ForceStopTrackingPosition()
    {
        trackingPosition = false;
    }

    void SendAngle()
    // Sends position to Wheel Controller
    {
        wheelController.SendNewAngle(angleChange);
    } 

    void RecalculateAngle()
    {
        // CAUSE: This is why the wheel does not spin a full circle.
        angleChange = (rawAngle - angle);
        angle = rawAngle;
    }

    void RecalculateDirection()
    {
        if(angleChange > 0)
        {
            direction = "right";
        }
        else if(angleChange < 0)
        {
            direction = "left";
        }
    }

    void RecalculatePosition()
    {
        swipe += (rawPosition - position);
        position = rawPosition;
    }

    void SendPosition()
    // Sends new swipe info to displayUI
    {
        displayUI.SendSwipe(swipe);
        swipe = 0;
    }

    IEnumerator TrackThumbAngle()
    {
        while(trackingAngle)
        {
            rawAngle = events.GetTouchpadAxisAngle();
            if (rawAngle != angle)
            {
                RecalculateAngle();
                SendAngle();
            }
            yield return null;
        }
    }

    IEnumerator TrackThumbPosition()
    {
        while(trackingPosition)
        {
            rawPosition = events.GetTouchpadAxis().x;
            if (rawPosition != position)
            {
                RecalculatePosition();
                if(swipe >= 0.5)
                {
                    Debug.Log("Swipe right");
                    SendPosition();
                    yield return new WaitForSeconds(0.2f);
                }
                else if(swipe <= -0.5)
                {
                    Debug.Log("Swipe left");
                    SendPosition();
                    yield return new WaitForSeconds(0.2f);
                }
            }
            yield return null;
        }
    }
}
