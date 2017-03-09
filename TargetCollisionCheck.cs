using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCollisionCheck : MonoBehaviour {
	public bool hasCollided;
	public GameObject parentBuilding;
	public Material blockedMaterial;
	GameObject nearestBuilding;
	Material defaultMaterial;

	void Start() {
		Physics.IgnoreCollision (nearestBuilding.GetComponent<Collider>(), GetComponent<Collider>());

	}

	void Update() {
		//GetComponent<MeshRenderer> ().material = defaultMaterial;
		//gameObject.GetComponent<Renderer> ().material = blockedMaterial;

	}

	void FixedUpdate() {
		updateTargetIsBlocked (false);
	}

	void OnTriggerStay(Collider other) {
		gameObject.GetComponent<Renderer> ().material = blockedMaterial;
		Debug.Log ("hello boss");
		if (other.GetComponent<Collider> ().CompareTag ("residential")) {
			updateTargetIsBlocked (true);
		}
	}
		
	public void setNearestBuilding(GameObject building) {
		nearestBuilding = building;
	}

	void updateTargetIsBlocked(bool status) {
		parentBuilding.GetComponent<RoadSnap> ().updateTargetIsBlocked (status);
	}

}
