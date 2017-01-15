using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlayerScale : MonoBehaviour {

	public VRTK_ControllerEvents events;
	public GameObject cameraRig;
	public bool isCameraSmall;
	public Camera cameraEye;
	private Vector3 previousCameraPosition;

	void OnEnable()
    { 
		events.ApplicationMenuPressed += new ControllerInteractionEventHandler(OnApplicationMenuPressed);

    }

    void OnDisable()
    {
		events.ApplicationMenuPressed -= new ControllerInteractionEventHandler(OnApplicationMenuPressed);
    }




    void OnApplicationMenuPressed(object sender, ControllerInteractionEventArgs e)
    {
    	if (isCameraSmall)
    	{
			previousCameraPosition = cameraEye.transform.position;
			cameraRig.transform.localScale = new Vector3(100f, 100f, 100f);
			cameraRig.transform.position -= new Vector3(previousCameraPosition.x, 100f, previousCameraPosition.z);
			isCameraSmall = !isCameraSmall;
    	}
    	else 
    	{
			previousCameraPosition = cameraEye.transform.position;
			cameraRig.transform.localScale = new Vector3(1f, 1f, 1f);
			cameraRig.transform.position += new Vector3(previousCameraPosition.x, 100f, previousCameraPosition.z);
			isCameraSmall = !isCameraSmall;
    	}
    }

}
