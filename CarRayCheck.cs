using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRayCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
            hit.transform.SendMessage("HitByRay");
    }
}
