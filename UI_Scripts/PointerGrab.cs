using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PointerGrab : MonoBehaviour
{
    public VRTK_ControllerEvents events;
    public GameObject controller;
    public bool IsAttached;
    private bool TouchForwardHold;
    private bool TouchBackwardHold;
    private RaycastHit hit;
    private Rigidbody body;
    public GameObject obj;


    void Start()
    {
        body = obj.GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        events.TriggerClicked += new ControllerInteractionEventHandler(OnTriggerClicked);
        events.TouchpadPressed += new ControllerInteractionEventHandler(OnTouchPadDownClick);
        events.TouchpadReleased += new ControllerInteractionEventHandler(OnTouchPadRelease);

    }

    void OnDisable()
    {
        events.TriggerClicked -= new ControllerInteractionEventHandler(OnTriggerClicked);
        events.TouchpadPressed -= new ControllerInteractionEventHandler(OnTouchPadDownClick);
        events.TouchpadReleased -= new ControllerInteractionEventHandler(OnTouchPadRelease);

    }

    private void Update()
    {
        if (this.IsAttached == true)
        {
            controller.GetComponent<VRTK_SimplePointer>().pointerLength = hit.distance;
            controller.GetComponent<VRTK_SimplePointer>().pointerTip.transform.position = hit.point;
            obj.transform.parent = controller.GetComponent<VRTK_SimplePointer>().pointerTip.transform;
            body.isKinematic = true;
            body.useGravity = false;
            body.freezeRotation = true;
            body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            if (TouchForwardHold)
            {
                hit.distance += 0.05f;
                hit.point = gameObject.transform.position;
            }

            if(TouchBackwardHold)
            {
                hit.distance -= 0.05f;
                hit.point = gameObject.transform.position;
            }
        }
        else if (this.IsAttached == false)
        {
            body.useGravity = true;
            body.isKinematic = false;
            body.freezeRotation = false;
            body.collisionDetectionMode = CollisionDetectionMode.Discrete;
            obj.transform.parent = null;
            controller.GetComponent<VRTK_SimplePointer>().pointerLength = 100f;
        }
  
    }

    private void OnTriggerClicked(object sender, ControllerInteractionEventArgs e)
    {
        if (Physics.Raycast(controller.transform.position, controller.transform.forward, out hit, 100))
        {
            if (hit.collider.gameObject == obj)
            {
                this.IsAttached = !IsAttached;
            }
        }

    }

    private void OnTouchPadDownClick(object sender, ControllerInteractionEventArgs e)
    {
        if ((events.GetTouchpadAxisAngle() > 270 && events.GetTouchpadAxisAngle() <= 360) || (events.GetTouchpadAxisAngle() >= 0 && events.GetTouchpadAxisAngle() <= 90))
        {
            TouchForwardHold = true;
        }
        if (events.GetTouchpadAxisAngle() > 90 && events.GetTouchpadAxisAngle() < 270)
        {
            TouchBackwardHold = true;
        }

    }

    private void OnTouchPadRelease(object sender, ControllerInteractionEventArgs e)
    {
        TouchForwardHold = false;
        TouchBackwardHold = false;
    }
}
