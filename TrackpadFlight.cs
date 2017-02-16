using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class TrackpadFlight : MonoBehaviour {

    VRTK_ControllerEvents events;
    GameObject rig;
    public float walkSpeed;

	// Use this for initialization
	void Start () {
        rig = GameObject.Find("[CameraRig]");
        events = GetComponent<VRTK_ControllerEvents>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(events.touchpadPressed)
        {
            rig.transform.position += transform.forward * walkSpeed;
        }
	}
}
