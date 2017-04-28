using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ThumbTracker : MonoBehaviour {

    VRTK_ControllerEvents events;  // VRTK grab event handler
    GameObject controller;  // left controller
    WheelController wheelController;
    DisplayUI displayUI;
    SpawnManager spawnManager;

    public int angleIncrement;  // Used for spawning buildings, number of ticks from origin point

    float angle;  // old angle, stored for comparison
    float angleChange;  // Amount change from last set angle
    float rawAngle;  // raw angle returned from thumb position
    float totalAngle;  // aggregate angle chamnge
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
        angleIncrement = 0;
        swipeReq = 0.2f;
        controller = GameObject.Find("LeftController");
        events = controller.GetComponent<VRTK_ControllerEvents>();
        wheelController = GetComponent<WheelController>();
        displayUI = GetComponent<DisplayUI>();
        spawnManager = GameObject.Find("Managers").GetComponent<SpawnManager>();

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
            spawnManager.SpawnUIBuildings(displayUI.GetSelection(), angleIncrement);

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
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (trackingPosition)
        {
            endPosition = events.GetTouchpadAxis().x;
            RecalculatePosition();
            SendPosition();
            ForceStopTrackingPosition();
        }
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
        //wheelController.SendNewAngle(angleChange);
        int selection = displayUI.GetSelection();
        if(totalAngle > 45 || totalAngle < -45)
        {
            if (totalAngle < 45 && angleIncrement >= 1)
            {
                angleIncrement--;
                totalAngle = 0;
                // TODO: play noise
            }
            else if (totalAngle > 45 && angleIncrement + 2 < spawnManager.GetNumBuildings(selection))
            {
                angleIncrement++;
                totalAngle = 0;
            }
            spawnManager.SpawnUIBuildings(selection, angleIncrement);
        }
    } 

    void RecalculateAngle()
    {
        // CAUSE: This is why the wheel does not spin a full circle.
        angleChange = (rawAngle - angle);
        totalAngle = totalAngle + angleChange;
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
            Debug.Log("Sending positive swipe");
            displayUI.SendSwipe(1);
            
            swipe = 0;
        }
        else if(swipe < -swipeReq)
        {
            Debug.Log("Sending negative swipe");
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
