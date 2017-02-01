using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;

public class RoadSnap : MonoBehaviour {

    VRTK_InteractableObject interact;
    bool objectUsed;
    RoadGenerator roadGenerator;

	void Start ()
    {
        // Adds listeners for controller grab to both controllers
        GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().AliasGrabOn+=
            new ControllerInteractionEventHandler(DoGrabStart);
        GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>().AliasGrabOn +=
            new ControllerInteractionEventHandler(DoGrabStart);

        // Add listeners for controller release to both controllers
        GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().AliasGrabOff +=
            new ControllerInteractionEventHandler(DoGrabRelease);
        GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>().AliasGrabOff +=
            new ControllerInteractionEventHandler(DoGrabRelease);

        interact = gameObject.GetComponent<VRTK_InteractableObject>();
        roadGenerator = GameObject.Find("Island").GetComponent<RoadGenerator>();
        objectUsed = false;
    }
	
	void DoGrabRelease(object sender, ControllerInteractionEventArgs e)
    // Grab end event listener
    {
        if(objectUsed == true)
        {
            InitiateSnapCheck();
        }
    }

    void DoGrabStart(object sender, ControllerInteractionEventArgs e)
    // Grab start event listener
    {
        if(interact.IsGrabbed() == true)
        {
            objectUsed = true;
        }
    }

    void InitiateSnapCheck()
    // Checks if object can snap
    {
        if(roadGenerator.CheckSurroundingRoads(transform.position))
        {
            Vector3 newPosition;
            roadGenerator.Round(transform.position, out newPosition);
            transform.position = newPosition;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f); // TODO: Change this! Make it round the rotation as well
        }
    }
}
