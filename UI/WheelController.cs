using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class WheelController : MonoBehaviour {

    VRTK_ControllerEvents events;
    DisplayUI displayUI;
    GameObject controller;
    GameObject wheelBase;

    float snapAngle;
    float angleChange; // Used to figure out how much to rotate by
    float angle;

    bool requireUpdate;  // set to true when new angle received
    bool updating;

    void Awake()
    {
        wheelBase = transform.Find("WheelBase").gameObject;
    }

    void Start () {
        controller = transform.parent.gameObject;
        events = GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>();
        displayUI = GetComponent<DisplayUI>();

        events.TouchpadTouchStart += DoTouchpadTouchStart;
        events.TouchpadTouchEnd += DoTouchpadTouchEnd;
	}

    void Update()
    {
        if(updating && requireUpdate)
        {
            wheelBase.transform.Rotate(new Vector3(0f, angleChange, 0f));
            requireUpdate = false;
        }
    }

    void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
    // TouchpadReleased event
    {
        PerformSnap();
        updating = false;
        displayUI.HideUI();
    }

    void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        updating = true;
        displayUI.ShowUI();
    } 

    void PerformSnap()
    {
        Debug.Log("Snap should happen here, but is currently disabled");
        //wheelBase.transform.rotation = Quaternion.Euler(0f, snapAngle, 0f);
    }
	
    public void SendNewAngle(float newAngle, float newSnapAngle)
    {
        angleChange = (newAngle - angle) / 4;
        angle = newAngle;
        snapAngle = newSnapAngle;
        requireUpdate = true;
    }
}
