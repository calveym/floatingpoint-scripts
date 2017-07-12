using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using Autelia.Serialization;

public class PlayerScale : MonoBehaviour {

    public delegate void Action();
    public static Action OnSizeChange;

	public GameObject cameraRig;
    Rigidbody rb;
	public bool isCameraSmall;
	public Camera cameraEye;
	private Vector3 previousCameraPosition;
    private Vector3 previousRigPosition;

    TrackpadFlight trackpadFlight;

	void Start()
    {
        GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>().ButtonOnePressed += new ControllerInteractionEventHandler(DoButtonOnePressed);
        rb = cameraRig.GetComponent<Rigidbody>();
        isCameraSmall = false;
        trackpadFlight = GetComponent<TrackpadFlight>();
    }
    
    void DoButtonOnePressed(object sender, ControllerInteractionEventArgs e)
    {
        if (OnSizeChange != null)
            OnSizeChange();
    	if (isCameraSmall)  // Going big
    	{
            GoBig();
    	}
    	else  // Going small
    	{
            GoSmall();
        }
    }

    void GoSmall()
    {
        previousCameraPosition = cameraEye.transform.position;
        previousRigPosition = cameraRig.transform.position;
        cameraRig.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Vector3 adjustVector = previousCameraPosition - cameraEye.transform.position;
        cameraRig.transform.position += adjustVector;
        cameraRig.transform.position = new Vector3(cameraRig.transform.position.x, 10f, cameraRig.transform.position.z);
        isCameraSmall = !isCameraSmall;
        trackpadFlight.speedMultiplier = 0.2f;
    }

    void GoBig()
    {
        Vector3 oldPosition = cameraEye.transform.position;
        cameraRig.transform.localScale = new Vector3(10f, 10f, 10f);
        Vector3 adjustVector = oldPosition - cameraEye.transform.position;
        cameraRig.transform.position += adjustVector;
        cameraRig.transform.position = new Vector3(cameraRig.transform.position.x, previousRigPosition.y, cameraRig.transform.position.z);
        isCameraSmall = !isCameraSmall;
        trackpadFlight.speedMultiplier = 2f;
    }
}
