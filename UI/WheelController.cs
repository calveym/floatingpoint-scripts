using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class WheelController : MonoBehaviour {

    VRTK_ControllerEvents events;
    GameObject controller;
    GameObject wheelBase;

    float angle;
    float snapAngle;
    float angleChange; // Used to figure out how much to rotate by

    bool requireUpdate;  // set to true when new angle received
    bool updating;

	void Start () {
        wheelBase = transform.Find("WheelBase").gameObject;
        controller = transform.parent.gameObject;
        events = GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>();

        events.TouchpadTouchStart += DoTouchpadTouchStart;
        events.TouchpadTouchEnd += DoTouchpadTouchEnd;
	}

    void Update()
    {
        if(updating && requireUpdate)
        {
            Debug.Log("Updating to new angle: " + angle);
            wheelBase.transform.Rotate(new Vector3(0f, angleChange));
            requireUpdate = false;
        }
    }

    void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
    // TouchpadReleased event
    {
        PerformSnap();
        updating = false;
    }

    void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        updating = true;
    } 

    void PerformSnap()
    {
        angle = snapAngle;
    }
	
    public void SendNewAngle(float newAngle, float newSnapAngle)
    {
        angleChange = newAngle - angle;
        angle = newAngle;
        snapAngle = newSnapAngle;
        requireUpdate = true;
    }
}
