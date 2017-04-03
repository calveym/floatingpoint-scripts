using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlayerScale : MonoBehaviour {

	public GameObject cameraRig;
    Rigidbody rb;
	public bool isCameraSmall;
	public Camera cameraEye;
	private Vector3 previousCameraPosition;

	void Start()
    {
        GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>().ButtonOnePressed += new ControllerInteractionEventHandler(DoButtonOnePressed);
        rb = cameraRig.GetComponent<Rigidbody>();
    }
    
    void DoButtonOnePressed(object sender, ControllerInteractionEventArgs e)
    {
    	if (isCameraSmall)  // Going big
    	{
			//previousCameraPosition = cameraEye.transform.position;
            rb.isKinematic = true;
            rb.useGravity = false;
			cameraRig.transform.localScale = new Vector3(10f, 10f, 10f);
			//cameraRig.transform.position -= new Vector3(previousCameraPosition.x, 10f, previousCameraPosition.z);
			isCameraSmall = !isCameraSmall;
            GetComponent<TrackpadFlight>().speedMultiplier = 2f;
    	}
    	else  // Going small
    	{
            rb.isKinematic = false;
            rb.useGravity = true;
			cameraRig.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
			//cameraRig.transform.position += new Vector3(previousCameraPosition.x, 10f, previousCameraPosition.z);
			isCameraSmall = !isCameraSmall;
            GetComponent<TrackpadFlight>().speedMultiplier = 0.2f;
        }
    }

}
