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
        StartCoroutine("DelayedHide");
    }

    void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        updating = true;
        displayUI.ShowUI();
    } 

    void PerformSnap()
    {
        wheelBase.transform.rotation = Quaternion.Euler(0f, Mathf.Round(wheelBase.transform.localEulerAngles.y / 45) * 45, 0f);
    }
	
    public void SendNewAngle(float newAngle)
    {
        angleChange = (newAngle - angle) / 4;
        angle = newAngle;
        requireUpdate = true;
    }

    IEnumerator DelayedHide()
    {
        for(int i = 0; i <= 1; i++)
        {
            if(i == 1)
            {
                Debug.Log("Hiding...");
                displayUI.HideUI();
            }
            yield return new WaitForSeconds(5);
        }
    }
}
