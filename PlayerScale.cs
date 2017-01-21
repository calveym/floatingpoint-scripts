using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlayerScale : MonoBehaviour {

	public GameObject cameraRig;
	public bool isCameraSmall;
	public Camera cameraEye;
	private Vector3 previousCameraPosition;

	void Start()
    {
        GetComponent<VRTK_ControllerEvents>().ButtonOnePressed += new ControllerInteractionEventHandler(DoButtonOnePressed);
    }
    
    void DoButtonOnePressed(object sender, ControllerInteractionEventArgs e)
    {
    	if (isCameraSmall)
    	{
			previousCameraPosition = cameraEye.transform.position;
			cameraRig.transform.localScale = new Vector3(10f, 10f, 10f);
			cameraRig.transform.position -= new Vector3(previousCameraPosition.x, 10f, previousCameraPosition.z);
			isCameraSmall = !isCameraSmall;
    	}
    	else 
    	{
            Debug.Log("Off");
			previousCameraPosition = cameraEye.transform.position;
			cameraRig.transform.localScale = new Vector3(1f, 1f, 1f);
			cameraRig.transform.position += new Vector3(previousCameraPosition.x, 10f, previousCameraPosition.z);
			isCameraSmall = !isCameraSmall;
    	}
    }

}
