using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RoadDeleter : VRTK_InteractableObject {

    VRTK_ControllerEvents events;
	private GameObject controller;
	private RoadGenerator roadGenerator;
    private RaycastHit hit;

	void Start ()
	{
		controller = GameObject.Find("LeftController");
        events = controller.GetComponent<VRTK_ControllerEvents>();
		roadGenerator = GameObject.Find("Island").GetComponent<RoadGenerator>();
	}

	public override void StartUsing (GameObject usingObject)
	{
		base.StartUsing (usingObject);
		Physics.Raycast (controller.transform.position, controller.transform.forward, out RaycastHit hit, 1000.0f);
		if (hit.transform.gameObject.tag == "road")
		{
			roadGenerator.RemoveRoad(roadGenerator.Round(hit.point));
		}
	}
}
