using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;

public class RoadSnap : MonoBehaviour {

	public bool manualUse;
	public bool setBuildingPos;
	public GameObject targetBoxPrefab;
	public bool targetIsBlocked;
	public Material blockedMaterial;

	VRTK_InteractableObject interact;
	bool objectUsed;
	GameObject targetBox = null;
	Renderer rend;
	Collider[] hitColliders;
	GameObject nearestBuilding;
	Vector3 targetBounds;
	Vector3 thisBounds;
	Vector3 targetPosition;
	Vector3 targetBoxPosition;
	float distanceToMoveX;
	float distanceToMoveZ;
	float spacing;
	float frontTargetPoint;
	float frontThisPoint;
	float pointDifference;
	int buildingLayer = 8;
	int layerMask;

	public void updateTargetIsBlocked(bool status) {
		targetIsBlocked = status;
	}

	void Update() {
		if (objectUsed || manualUse) {
			gameObject.layer = 9;
			getNearbyBuildings ();
		} 
		else {
			gameObject.layer = buildingLayer;
		}

		if (setBuildingPos) {
			if (!targetIsBlocked) {
				checkForNearbyBuilding ();
			}
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
			if (!targetIsBlocked) {
				checkForNearbyBuilding ();
			}
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
			Destroy (targetBox);
		} else {
			// Debug.Log (nearestBuilding);
			GetComponent<BoxCollider> ().enabled = true;
		}
	}

	void getNearbyBuildings() {

		hitColliders = Physics.OverlapSphere (transform.position, 1.5f, layerMask);

		if (hitColliders.Length == 0) {
			nearestBuilding = null;
			Destroy (targetBox);
		} 
		else {
			foreach (Collider hitcol in hitColliders) {
				if (hitcol.CompareTag ("residential") && hitcol != GetComponent<Collider> ()) {

					setToNearestBuilding (hitcol);

					drawTargetBox ();

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

	void drawTargetBox (){

		// create box
		if (targetBox != null) {
			Destroy (targetBox);
			targetBox = (GameObject)Instantiate(targetBoxPrefab, transform.position, nearestBuilding.transform.rotation);
		} else {
			targetBox = (GameObject)Instantiate(targetBoxPrefab, transform.position, nearestBuilding.transform.rotation);
		}

		if (targetIsBlocked) {
			targetBox.GetComponent<Renderer> ().material = blockedMaterial;
		}

		targetBox.GetComponent<TargetCollisionCheck> ().parentBuilding = gameObject;

		// set size
		Vector3 sizeCalculated = GetComponent<BoxCollider>().size;
		targetBox.transform.localScale = new Vector3 (sizeCalculated.x / 10, 0.05f, sizeCalculated.z / 10);

		// give target box nearest building for collision

		targetBox.GetComponent<TargetCollisionCheck>().setNearestBuilding(nearestBuilding);

		// place it next to building
		setPosition(targetBox);

		// adjust vertical axis
		targetBox.transform.position = new Vector3(targetBox.transform.position.x, 10.025f, targetBox.transform.position.z);

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
		setPosition(gameObject);
	}

	void setPosition(GameObject objectToPlace) {
		targetBounds = nearestBuilding.GetComponent<MeshFilter> ().mesh.bounds.extents;
		thisBounds = objectToPlace.GetComponent<MeshFilter> ().mesh.bounds.extents;
		targetPosition = nearestBuilding.transform.position;
		spacing = 0.1f;

		if (objectToPlace != gameObject) {
			thisBounds = thisBounds * 10;
		}

		distanceToMoveX = targetBounds.x + thisBounds.x;
		distanceToMoveZ = targetBounds.z + thisBounds.z;

		// frontTargetPoint = nearestBuilding.transform.position.x + (targetRend.bounds.size.x / 2);
		// frontThisPoint = transform.position.x -(thisRend.bounds.size.x / 2);
		// pointDifference = Mathf.Abs (frontThisPoint) - Mathf.Abs (frontTargetPoint);

		objectToPlace.transform.parent = nearestBuilding.transform;

		if (Mathf.Abs((nearestBuilding.transform.position.x - transform.position.x)) > Mathf.Abs((nearestBuilding.transform.position.z - transform.position.z))) {
			snapToX (objectToPlace);
		}


		else if (Mathf.Abs ((nearestBuilding.transform.position.x - transform.position.x)) < Mathf.Abs ((nearestBuilding.transform.position.z - transform.position.z))) {
			snapToZ (objectToPlace);
		}

		objectToPlace.transform.parent = null;

		// Debug.Log ("IT RAN: " + transform.position);
		// set z position, and align x axis
		//transform.position = new Vector3(transform.position.x + pointDifference, targetPosition.y, targetPosition.z - distanceToMoveZ);

	}

	void snapToX(GameObject objectToPlace) {
		if (Mathf.FloorToInt (nearestBuilding.transform.rotation.eulerAngles.y) == 0) {
			// Debug.Log("Snap to X: 0 angle, " + objectToPlace);

			objectToPlace.transform.localPosition = new Vector3 (Mathf.Sign(objectToPlace.transform.localPosition.x) * distanceToMoveX, 0, 0);
		} else {
			// Debug.Log("Snap to X: +0 angle," + objectToPlace);
			objectToPlace.transform.localPosition = new Vector3 (0, 0, Mathf.Sign(objectToPlace.transform.localPosition.z) * distanceToMoveZ);
		}
	}

	void snapToZ(GameObject objectToPlace) {
		if (Mathf.FloorToInt(nearestBuilding.transform.rotation.eulerAngles.y) == 0) {
			// Debug.Log("Snap to Z: 0 angle, " + objectToPlace);
			objectToPlace.transform.localPosition = new Vector3 (0, 0, Mathf.Sign(objectToPlace.transform.localPosition.z) * distanceToMoveZ);
		} else {
			// Debug.Log("Snap to Z: +0 angle, " + objectToPlace);
			objectToPlace.transform.localPosition = new Vector3 (Mathf.Sign(objectToPlace.transform.localPosition.x) * distanceToMoveX, 0, 0);
		}
	}
}