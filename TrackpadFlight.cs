using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class TrackpadFlight : MonoBehaviour {

    VRTK_ControllerEvents events;
    GameObject rig;
    GameObject head;
    PlayerScale scale;
    public float speedMultiplier;

    float speed;
    bool stop;
    int forward;

    // Use this for initialization
    void Start() {
        rig = GameObject.Find("[CameraRig]");
        scale = GameObject.Find("RightController").GetComponent<PlayerScale>();
        head = GameObject.Find("Camera(head)");
        events = GetComponent<VRTK_ControllerEvents>();
        events.TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouchStart);
        events.TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);
    }

    void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        stop = false;
        StartCoroutine("Fly");
    }
    
    void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
    {
        stop = true;
    }

    IEnumerator Fly()
    {
        while (!stop)
        {
            SetForward();
            if (events.touchpadPressed)
            {
                speed = 2 * speedMultiplier;
            }
            else
            {
                speed = 1 * speedMultiplier;
            }
            if (!scale.isCameraSmall)
            {
                rig.transform.position += transform.forward * Time.deltaTime * forward * speed;
            }
            else
            {
                float tempHeight = rig.transform.position.y;
                rig.transform.position += head.transform.forward * Time.deltaTime * forward * speed;
                rig.transform.position.y = tempHeight;
            }
            yield return null;
        }
    }

    void SetForward()
    {
        if (events.GetTouchpadAxis().y > 0)
        {
            forward = 1;
        }
        else forward = -1;
    }
}
