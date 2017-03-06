using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Cloud : VRTK_InteractableObject {

    bool grabbed;

	// Use this for initialization
	void Start () {
        grabbed = false;
	}
	
	// Update is called once per frame
	public override void Grabbed(GameObject currentGrabbingObject)
    {
        Debug.Log("Grabbed");
    }
}
