using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class TrackpadFlight : MonoBehaviour {

    VRTK_ControllerEvents events;
    GameObject rig;
    public float sizeSpeed;

    float speed;
    bool stop;
    int forward;

    // Use this for initialization
    void Start() {
        rig = GameObject.Find("[CameraRig]");
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
        while(!stop)
        {
            if(events.touchpadPressed)
            {
                speed = 2;
            } else
            {
                speed = 1;
            }
            SetForward();
            rig.transform.position += transform.forward * Time.deltaTime * forward * speed;
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
