using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCollisionCheck : MonoBehaviour {

	public GameObject parentBuilding;

	GameObject nearestBuilding;
	int buildingsLayer;

	public void setNearestBuilding(GameObject building) {
		nearestBuilding = building;
	}

	void Start() {
		buildingsLayer = LayerMask.NameToLayer ("Buildings");
		Physics.IgnoreCollision (nearestBuilding.GetComponent<Collider>(), GetComponent<Collider>());
	}

	void FixedUpdate() {
		updateTargetIsBlocked (false);
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject.layer == buildingsLayer) {
			updateTargetIsBlocked (true);
		}
	}

	void updateTargetIsBlocked(bool status) {
		parentBuilding.GetComponent<RoadSnap> ().updateTargetIsBlocked (status);
	}

}
