using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RoadDeleter : VRTK_InteractableObject {

	private GameObject controller;
	RoadPositionList roadList;
	private List<Vector3> roadPositions;
	private RoadGenerator roadGenerator;
	private RaycastHit hit;
	private Vector3 newPosition;
	private Vector3 gridSize = new Vector3(10f, -0.4f, 10f);



	protected override void Update () {
		base.Update();
		roadGenerator = GameObject.FindGameObjectsWithTag("table")[0].GetComponent<RoadGenerator>();
		roadPositions = GameObject.FindGameObjectsWithTag("table")[0].GetComponent<RoadGenerator>().roadPositions;
	}

	public override void StartUsing (GameObject usingObject) {
		base.StartUsing (usingObject);
		controller = GameObject.FindGameObjectsWithTag("controller-left")[0];
		if (Physics.Raycast (controller.transform.position, controller.transform.forward, out hit, 1000.0f)) {
			round(hit.point, out newPosition);
			Debug.DrawLine(controller.transform.position, hit.point);
			if(hit.transform.gameObject.tag == "road") {
				roadGenerator.removeRoad(newPosition);
				roadGenerator.destroy();
				roadGenerator.reDrawRoads();
			}
		}
	}

	private void round (Vector3 point, out Vector3 newPosition) {
		newPosition = new Vector3(Mathf.Round(point.x * 0.1f) * gridSize.x,
			100.1f,
			Mathf.Round(point.z * 0.1f) * gridSize.z);
	}
}
