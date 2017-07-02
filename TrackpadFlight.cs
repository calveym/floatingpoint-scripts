using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using Autelia.Serialization;

public class TrackpadFlight : MonoBehaviour {

    VRTK_ControllerEvents events;
    GameObject rig;
    Rigidbody rb;
    GameObject head;
    PlayerScale scale;
    [Tooltip("Warning: not set in editor")]
    public float speedMultiplier; // DO NOT SET IN EDITOR
    [Tooltip("AudioClip used for wind effect")]
    public AudioClip wind;

    float speed;
    bool stop;
    int forward;

    // Use this for initialization
    void Start() {if (Serializer.IsLoading)	return;
        speedMultiplier = 2;
        rig = GameObject.Find("[CameraRig]");
        rb = rig.GetComponent<Rigidbody>();
        scale = GameObject.Find("RightController").GetComponent<PlayerScale>();
        head = GameObject.Find("Camera(head)");
        events = GetComponent<VRTK_ControllerEvents>();
        events.TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouchStart);
        events.TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);
        events.TouchpadPressed += new ControllerInteractionEventHandler(DoRightClick);
    }

    void DoRightClick(object sender, ControllerInteractionEventArgs e)
    {
        U.RightPulse();
    }

    void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        stop = false;
        AudioManager.instance.PlayLoop(wind);

Autelia.Coroutines.CoroutineController.StartCoroutine(this, "Fly");
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
                speed = 4 * speedMultiplier;
                AudioManager.instance.EfxVolume(1f);
            }
            else
            {
                speed = 1 * speedMultiplier;
                AudioManager.instance.EfxVolume(0.5f);
            }
            if (!scale.isCameraSmall) // if regular
            {
                rig.transform.position += transform.forward * Time.deltaTime * forward * speed;
            }
            else // if small
            {
                Vector3 newMoveVector = transform.forward;
                newMoveVector.y = 0;
                newMoveVector.Normalize();
                rig.transform.position += (newMoveVector * speed * forward * Time.deltaTime);
            }
            yield return null;
        }
        if(stop)
        {
            AudioManager.instance.TryStopEfx(wind);
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
