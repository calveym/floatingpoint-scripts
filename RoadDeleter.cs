using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RoadDeleter : VRTK_InteractableObject {

	private GameObject controller;
	private RoadGenerator roadGenerator;
	private RaycastHit hit;
	private Vector3 newPosition;
	private Vector3 gridSize = new Vector3(1f, 0.01f, 1f);

	void Start ()
	{
		controller = GameObject.Find("Controller (left)");
	}

	protected override void Update () {
		base.Update();
		roadGenerator = GameObject.Find("Island").GetComponent<RoadGenerator>();
	}

	public override void StartUsing (GameObject usingObject) {
		base.StartUsing (usingObject);
		if (Physics.Raycast (controller.transform.position, controller.transform.forward, out hit, 1000.0f)) {
			// round(hit.point, out newPosition);
			if(hit.transform.gameObject.tag == "road") {
				roadGenerator.removeRoad(hit.point);
				roadGenerator.destroy();
				roadGenerator.reDrawRoads();
			}
		}
	}

	private void round (Vector3 point, out Vector3 newPosition) {
		newPosition = new Vector3(Mathf.Round(point.x) * gridSize.x,
			10.01f,
			Mathf.Round(point.z) * gridSize.z);
	}
}
