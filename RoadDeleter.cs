using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RoadDeleter : VRTK_InteractableObject {

	private GameObject controller;
	private RoadGenerator roadGenerator;
    private RaycastHit hit;

	void Start ()
	{
		controller = GameObject.Find("Controller (left)");
		roadGenerator = GameObject.Find("Island").GetComponent<RoadGenerator>();
	}

	public override void StartUsing (GameObject usingObject)
	{
		base.StartUsing (usingObject);
		Physics.Raycast (controller.transform.position, controller.transform.forward, out hit, 1000.0f);
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
