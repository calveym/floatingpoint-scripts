using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class WorldTilt : MonoBehaviour {

    bool rightIsGrabbed;
    bool leftIsGrabbed;
    GameObject rightController;
    GameObject leftController;
    GameObject headset;
    GameObject island;
    float initialPosition;
    float tilt;
    bool tilting;

	void Start () {
        rightController = GameObject.Find("Controller (right)");
        leftController = GameObject.Find("Controller (left)");
        island = GameObject.Find("Island");

        headset = GameObject.Find("Camera (head)");
        tilt = GameObject.Find("[CameraRig]").transform.rotation.z;

        // GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().AliasGrabOn +=
        //    new ControllerInteractionEventHandler(RightControllerGrab);
        // GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>().AliasGrabOn +=
        //    new ControllerInteractionEventHandler(LeftControllerGrab);
        // GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>().AliasGrabOff +=
        //    new ControllerInteractionEventHandler(TryEndWorldTilt);
        // GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>().AliasGrabOff +=
        //    new ControllerInteractionEventHandler(TryEndWorldTilt);
    }

    private void Update()
    {
        if ( !rightController || !leftController )
        {
            rightController = GameObject.Find("Controller (right)");
            leftController = GameObject.Find("Controller (left)");
        }

        if(tilting)
        {
            Debug.Log("GetControllerPositions" + GetControllerPositions().ToString());
            Debug.Log("initialPosition: " + (initialPosition));
            leftController.transform.parent = null;
            rightController.transform.parent = null;
            GameObject.Find("[CameraRig]").transform.rotation = CalculateTilt();
            leftController.transform.parent = GameObject.Find("[CameraRig]").transform;
            rightController.transform.parent = GameObject.Find("[CameraRig]").transform;
        }
    }

    public Quaternion CalculateTilt()
    {
        return (Quaternion.Euler((GetControllerPositions() - initialPosition) * 9, 0f, 0f));
    }

    void RightControllerGrab(object sender, ControllerInteractionEventArgs e)
    {
        rightIsGrabbed = true;
        TryTiltWorld();
    }

    void LeftControllerGrab(object sender, ControllerInteractionEventArgs e)
    {
        leftIsGrabbed = true;
        TryTiltWorld();
    }

    void TryTiltWorld()
    {
        if(leftIsGrabbed == true && rightIsGrabbed == true)
        {
            StartWorldTilt();
        }
    }

    void StartWorldTilt()
    {
        initialPosition = GetControllerPositions();
        tilting = true;
    }

    void TryEndWorldTilt(object sender, ControllerInteractionEventArgs e)
    {
        leftIsGrabbed = false;
        rightIsGrabbed = false;
        if(tilting == true)
        {
            tilting = false;
        }
    }

    public float GetControllerPositions()
    {
        return (leftController.transform.position.y + rightController.transform.position.y) / 2;
    }
}
