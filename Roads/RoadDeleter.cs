using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using Autelia.Serialization;

public class RoadDeleter : VRTK_InteractableObject {

    VRTK_ControllerEvents events;
	private GameObject controller;
	private RoadGenerator roadGenerator;
    private RaycastHit hit;

	void Start ()
	{if (Serializer.IsDeserializing)	return;if (Serializer.IsLoading)	return;
		controller = GameObject.Find("LeftController");
        events = controller.GetComponent<VRTK_ControllerEvents>();
		roadGenerator = GameObject.Find("Island").GetComponent<RoadGenerator>();
    }

	public override void StartUsing (GameObject usingObject)
	{
        base.StartUsing (usingObject);
        if(controller == null)
        { 
            controller = ReferenceManager.instance.leftController;
        }
        if(roadGenerator == null)
        {
            roadGenerator = ReferenceManager.instance.roadGenerator;
        }
		Physics.Raycast (controller.transform.position, controller.transform.forward, out hit, 100.0f);
		if(hit.transform.gameObject.tag == "road") {
            Vector3 rounded = Round(hit.point);
			roadGenerator.RemoveRoad(rounded);
            roadGenerator.RedrawLocalRoads(rounded);
		}
	}

	public Vector3 Round (Vector3 point)
    {
		Vector3 newPosition = new Vector3(Mathf.Round(point.x),
			10.01f,
			Mathf.Round(point.z));
		return newPosition;
	}
}
