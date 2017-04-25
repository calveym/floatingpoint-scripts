using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class WheelController : MonoBehaviour {

    VRTK_ControllerEvents events;
    DisplayUI displayUI;
    GameObject controller;
    GameObject wheelBase;

    Vector3 startAngle;
    Vector3 endAngle;
    float translationTime;
    float timer;

    float snapAngle;
    float angleChange; // Used to figure out how much to rotate by

    bool requireUpdate;  // set to true when new angle received
    bool updating;
    bool stopHide;

    void Awake()
    {
        wheelBase = transform.Find("WheelBase").gameObject;
    }

    void Start () {
        translationTime = 0.5f;

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
        stopHide = false;
        PerformSnap();
        StartCoroutine("DelayedHide");
    }

    void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        updating = true;
        stopHide = true;
        displayUI.ShowUI();
    } 

    void PerformSnap()
    {
        StartCoroutine("Snap");
        //wheelBase.transform.localRotation = Quaternion.Euler(0f, Mathf.Round(wheelBase.transform.localEulerAngles.y / 45) * 45, 0f);
    }

    float SnapAngle()
    {
        return Mathf.Round(wheelBase.transform.localEulerAngles.y / 45) * 45;
    }
	
    public void SendNewAngle(float newAngleChange)
    {
        angleChange = newAngleChange;
        requireUpdate = true;
    }

    IEnumerator Snap()
    {
        startAngle = wheelBase.transform.localEulerAngles;
        endAngle = new Vector3(startAngle.x, SnapAngle(), startAngle.z);
        timer = 0;
        while (timer <= 1)
        {
            Vector3 tempAngle = Vector3.Slerp(startAngle, endAngle, timer);
            wheelBase.transform.localEulerAngles = tempAngle;
            timer += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator DelayedHide()
    {
        yield return new WaitForSeconds(5);
        if (!stopHide)
        {
            displayUI.HideUI();
            updating = false;
        }
    }
}
