using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;

public class RoadSnap : MonoBehaviour {

	public bool manualUse;
	public bool setBuildingPos;

	VRTK_InteractableObject interact;
	bool objectUsed;
	Renderer rend;
	Collider[] hitColliders;
	GameObject nearestBuilding;
	MeshRenderer targetRend;
	MeshRenderer thisRend;
	Vector3 targetPosition;
	float distanceToMoveX;
	float distanceToMoveZ;
	float spacing;
	float frontTargetPoint;
	float frontThisPoint;
	float pointDifference;
	int buildingLayer = 8;
	int layerMask;

	void Update() {
		if (objectUsed || manualUse) {
			gameObject.layer = 9;
			getNearbyBuildings ();
		} 
		else {
			gameObject.layer = buildingLayer;
		}

		if (setBuildingPos) {
			checkForNearbyBuilding ();
		} 
	}

	void Start ()
	{
		layerMask = 1 << buildingLayer;
		rend = GetComponent<Renderer>();
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
		objectUsed = false;
	}

	void DoGrabRelease(object sender, ControllerInteractionEventArgs e)
	// Grab end event listener
	{
		if (objectUsed == true) {
			checkForNearbyBuilding ();
			objectUsed = false;
		} 
	}

	void DoGrabStart(object sender, ControllerInteractionEventArgs e)
	// Grab start event listener
	{
		if(interact.IsGrabbed() == true) {	
			objectUsed = true;
		}
	}

	void checkForNearbyBuilding() {
		if (nearestBuilding) {
			gameObject.GetComponent<BoxCollider> ().enabled = false;
			setPosition ();
			setRotation ();
			gameObject.GetComponent<BoxCollider> ().enabled = true;
			setBuildingPos = false;
		} else {
			// Debug.Log (nearestBuilding);
			GetComponent<BoxCollider> ().enabled = true;
		}
	}

	void getNearbyBuildings() {

		hitColliders = Physics.OverlapSphere (transform.position, 1.5f, layerMask);
		if (hitColliders.Length == 0) {
			nearestBuilding = null;
		} 
		else {
			foreach (Collider hitcol in hitColliders) {
				if (hitcol.CompareTag ("residential") && hitcol != GetComponent<Collider> ()) {

					setToNearestBuilding (hitcol);

					// Debug.Log ("FOUND HIT: " + nearestBuilding);

					if (Mathf.Abs ((nearestBuilding.transform.position.x - transform.position.x)) < Mathf.Abs ((nearestBuilding.transform.position.z - transform.position.z))) {
						// Debug.Log ("Closer to z");
					}

					if (Mathf.Abs ((nearestBuilding.transform.position.x - transform.position.x)) > Mathf.Abs ((nearestBuilding.transform.position.z - transform.position.z))) {
						// Debug.Log ("Closer to x");
					}

				} else {
					Debug.Log ("Not building: " + hitcol);
				}
			}
		}
	}

	void setToNearestBuilding (Collider hitcol){
		float currentTargetDistance;
		float newTargetDistance;

		if (nearestBuilding != null) {
			currentTargetDistance = Vector3.Distance (transform.position, nearestBuilding.transform.position);
			newTargetDistance = Vector3.Distance (transform.position, hitcol.gameObject.transform.position);

			if (newTargetDistance < currentTargetDistance) {
				nearestBuilding = hitcol.gameObject;
			}
		} 
		else {
			nearestBuilding = hitcol.gameObject;
		}
	}

	void setRotation(){
		// transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

		transform.parent = nearestBuilding.transform;

		float yAngle = Mathf.Round(transform.localEulerAngles.y / 90) * 90;

		// Debug.Log("Current angle: " + transform.eulerAngles.y + "Rounded: " + yAngle);

		transform.localRotation = Quaternion.Euler(0, yAngle, 0);

		transform.parent = null;

	}

	void setPosition() {
		targetRend = nearestBuilding.GetComponent<MeshRenderer>();
		thisRend = gameObject.GetComponent<MeshRenderer> ();

		targetPosition = nearestBuilding.transform.position;
		spacing = 0.1f;

		distanceToMoveX = (targetRend.bounds.size.x / 2) + (thisRend.bounds.size.x / 2) + spacing;
		distanceToMoveZ = (targetRend.bounds.size.z / 2) + (thisRend.bounds.size.z / 2) + spacing;

		frontTargetPoint = nearestBuilding.transform.position.x + (targetRend.bounds.size.x / 2);
		frontThisPoint = transform.position.x -(thisRend.bounds.size.x / 2);
		pointDifference = Mathf.Abs (frontThisPoint) - Mathf.Abs (frontTargetPoint);

		transform.parent = nearestBuilding.transform;

		if (Mathf.Abs((nearestBuilding.transform.position.x - transform.position.x)) > Mathf.Abs((nearestBuilding.transform.position.z - transform.position.z))) {
			snapToX ();
		}


		else if (Mathf.Abs ((nearestBuilding.transform.position.x - transform.position.x)) < Mathf.Abs ((nearestBuilding.transform.position.z - transform.position.z))) {
			snapToZ ();
		}

		transform.parent = null;

		// Debug.Log ("IT RAN: " + transform.position);
		// set z position, and align x axis
		//transform.position = new Vector3(transform.position.x + pointDifference, targetPosition.y, targetPosition.z - distanceToMoveZ);

	}

	void snapToX() {
		if (Mathf.FloorToInt (nearestBuilding.transform.rotation.eulerAngles.y) == 0) {
			Debug.Log("Snap to X: 0 angle");
			transform.localPosition = new Vector3 (transform.localPosition.x, 0, 0);
		} else {
			Debug.Log("Snap to X: +0 angle");
			transform.localPosition = new Vector3 (0, 0, transform.localPosition.z);
		}
	}

	void snapToZ() {
		if (Mathf.FloorToInt(nearestBuilding.transform.rotation.eulerAngles.y) == 0) {
			Debug.Log("Snap to Z: 0 angle");
			transform.localPosition = new Vector3 (0, 0, transform.localPosition.z);
		} else {
			Debug.Log("Snap to Z: +0 angle");
			transform.localPosition = new Vector3 (transform.localPosition.x, 0, 0);
		}
	}
}