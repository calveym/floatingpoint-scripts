using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ThumbTracker : MonoBehaviour {

    VRTK_ControllerEvents events;  // VRTK grab event handler
    GameObject controller;  // left controller
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
        displayUI = GetComponent<DisplayUI>();
        spawnManager = GameObject.Find("Managers").GetComponent<SpawnManager>();

        events.TouchpadTouchEnd += StopTracking;
    }

    public void StartTracking()
    {
        if (displayUI.showBuildings)
        {
            trackingPosition = true;
            position = events.GetTouchpadAxis().x;
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
        angleIncrement = 0;
    }

    public void ForceStopTrackingPosition()
    {
        trackingPosition = false;
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
        int selection = displayUI.GetSelection();

        if (swipe > swipeReq)
        {
            U.LeftPulse();
            U.PlayClick();
            if (displayUI.showBuildings && angleIncrement + 4 < spawnManager.GetNumBuildings(selection))
            {
                angleIncrement++;
                spawnManager.SpawnUIBuildings(selection, angleIncrement);
            }
            else if(!displayUI.showBuildings)
            {
                displayUI.SendSwipe(1);
            }
            
            swipe = 0;
        }
        else if(swipe < -swipeReq)
        {
            U.LeftPulse();
            U.PlayClick();
            if (displayUI.showBuildings && angleIncrement >= 1)
            {
                angleIncrement--;
                spawnManager.SpawnUIBuildings(selection, angleIncrement);
            }
            else if (!displayUI.showBuildings)
            {
                displayUI.SendSwipe(-1);
            }
            swipe = 0;
        }
    }

    IEnumerator TrackThumbAngle()
    {
        while(trackingAngle)
        {
            //rawAngle = events.GetTouchpadAxisAngle();
            if (rawAngle != angle)
            {
                //RecalculateAngle();
                //SendAngle();
            }
            yield return null;
        }
    }
}
