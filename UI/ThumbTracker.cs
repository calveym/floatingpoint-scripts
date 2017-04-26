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
    string direction; // right or left

    float endPosition;  // position at end
    float position;  // Stored position
    public float swipeReq;  // swipe requirement ( out of a total of 2.0)
    float swipe;  // Stores distance info on last swipe. Includes whether it is positive or negative

    bool trackingAngle;  // controls angle tracker coroutine
    bool trackingPosition;  // controls position tracker coroutine

    private void Start()
    // Sets up references
    {
        swipeReq = 0.2f;
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
        endPosition = events.GetTouchpadAxis().x;
        RecalculatePosition();
        SendPosition();
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
        swipe = (endPosition - position);
    }

    void SendPosition()
    // Sends new swipe info to displayUI
    {
        if(swipe > swipeReq)
        {
            displayUI.SendSwipe(1);
            swipe = 0;
        }
        else if(swipe < -swipeReq)
        {
            displayUI.SendSwipe(-1);
            swipe = 0;
        }
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
}
