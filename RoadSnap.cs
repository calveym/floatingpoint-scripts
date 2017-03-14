﻿using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;

public class RoadSnap : MonoBehaviour {

	public bool manualUse;
	public bool setBuildingPos;
	public GameObject targetBoxPrefab;
	public GameObject snapCube;
	public bool targetIsBlocked;
	public Material blockedMaterial;

	VRTK_InteractableObject interact;
	bool objectUsed;
	GameObject targetBox = null;
	Renderer rend;
	Collider[] hitColliders;
	GameObject nearestBuilding;
	Vector3 targetBounds;
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
	Vector3 thisSize;

	public Vector3 top;
	public Vector3 bottom;
	public Vector3 left;
	public Vector3 right;

	Bounds thisBounds;

	GameObject objectToPlace;


	public class StringFloat {
		//define all of the values for the class
		public string name;
		public Vector3 value;

		public StringFloat (string Name, Vector3 Value){
			name = Name;
			value = Value;        
		}
	}

	public void updateSnapPoints() {
		right = new Vector3(objectToPlace.transform.position.x - (thisBounds.size.x / 2), objectToPlace.transform.position.y, objectToPlace.transform.position.z);
		left = new Vector3(objectToPlace.transform.position.x + (thisBounds.size.x / 2), objectToPlace.transform.position.y, objectToPlace.transform.position.z);
		top = new Vector3(objectToPlace.transform.position.x, objectToPlace.transform.position.y, objectToPlace.transform.position.z - (thisBounds.size.z / 2));
		bottom = new Vector3(objectToPlace.transform.position.x, objectToPlace.transform.position.y,objectToPlace.transform.position.z + (thisBounds.size.z / 2));
	}


	public void updateTargetIsBlocked(bool status) {
		targetIsBlocked = status;
	}

	public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
		Vector3 dir = point - pivot; // get point direction relative to pivot
		dir = Quaternion.Euler(angles) * dir; // rotate it
		point = dir + pivot; // calculate rotated point
		return point; // return it
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
				objectToPlace = gameObject;
				checkForNearbyBuilding ();
			}
		}
	}

	void Start ()
	{
		objectToPlace = gameObject;
		thisBounds = GetComponent<Renderer> ().bounds;

		thisSize = GetComponent<Renderer> ().bounds.size;
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
				objectToPlace = gameObject;
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
			//gameObject.GetComponent<BoxCollider> ().enabled = false;
			setRotation ();
			setPosition ();
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
			targetBox = (GameObject)Instantiate(targetBoxPrefab, transform.position, transform.rotation);
		} else {
			targetBox = (GameObject)Instantiate(targetBoxPrefab, transform.position, transform.rotation);
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
		objectToPlace = targetBox;
		setRotation();
		setPosition();

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

		objectToPlace.transform.parent = nearestBuilding.transform;

		float yAngle = Mathf.Round(objectToPlace.transform.localEulerAngles.y / 90) * 90;

		objectToPlace.transform.localRotation = Quaternion.Euler(0, yAngle, 0);

		objectToPlace.transform.parent = null;

	}

	StringFloat getClosestTargetSnapPoint() {

		nearestBuilding.GetComponent<RoadSnap> ().updateSnapPoints ();

		Vector3 targetTop = RotatePointAroundPivot (nearestBuilding.GetComponent<RoadSnap> ().top, nearestBuilding.transform.position, nearestBuilding.transform.eulerAngles);
		Vector3 targetBottom = RotatePointAroundPivot (nearestBuilding.GetComponent<RoadSnap> ().bottom, nearestBuilding.transform.position, nearestBuilding.transform.eulerAngles);
		Vector3 targetLeft = RotatePointAroundPivot (nearestBuilding.GetComponent<RoadSnap> ().left, nearestBuilding.transform.position, nearestBuilding.transform.eulerAngles);
		Vector3 targetRight = RotatePointAroundPivot (nearestBuilding.GetComponent<RoadSnap> ().right, nearestBuilding.transform.position, nearestBuilding.transform.eulerAngles);

		/* find the closest snap point to the targetClosestPoint */

		Vector3 targetClosestPoint = nearestBuilding.GetComponent<Renderer> ().bounds.ClosestPoint (objectToPlace.transform.position); // get closest point on target building to this object's position

		List<StringFloat> snapPoints = new List<StringFloat>();

		snapPoints.Add (new StringFloat("top", targetTop));
		snapPoints.Add (new StringFloat("bottom", targetBottom));
		snapPoints.Add (new StringFloat("left", targetLeft));
		snapPoints.Add (new StringFloat("right", targetRight));

		float closestDistance = Vector3.Distance (targetClosestPoint, snapPoints[0].value);
		StringFloat closestTargetSnapPoint = new StringFloat(snapPoints[0].name, snapPoints[0].value);

		for (int i = 1; i < snapPoints.Count; i++) {
			float newDistance = Vector3.Distance (targetClosestPoint, snapPoints[i].value);

			if (newDistance < closestDistance) {
				closestDistance = newDistance;
				closestTargetSnapPoint = snapPoints[i];
			}
		}

		return closestTargetSnapPoint;
	}

	StringFloat getClosestSnapPoint(bool snapX) {

		Vector3 closestTargetSnapPoint = nearestBuilding.transform.InverseTransformPoint(getClosestTargetSnapPoint ().value);

		updateSnapPoints ();

		/* find this closest side/point */

		Vector3 thisTop = RotatePointAroundPivot (top, objectToPlace.transform.position, objectToPlace.transform.eulerAngles);
		Vector3 thisBottom = RotatePointAroundPivot (bottom, objectToPlace.transform.position, objectToPlace.transform.eulerAngles);
		Vector3 thisLeft = RotatePointAroundPivot (left, objectToPlace.transform.position, objectToPlace.transform.eulerAngles);
		Vector3 thisRight = RotatePointAroundPivot (right, objectToPlace.transform.position, objectToPlace.transform.eulerAngles);

		/* find the closest snap point to the targetClosestPoint */

		Vector3 closestPoint = GetComponent<Renderer> ().bounds.ClosestPoint (nearestBuilding.transform.position); // get closest point on target building to this object's position

		List<StringFloat> snapPoints = new List<StringFloat>();

		snapPoints.Add (new StringFloat("top", thisTop));
		snapPoints.Add (new StringFloat("bottom", thisBottom));
		snapPoints.Add (new StringFloat("left", thisLeft));
		snapPoints.Add (new StringFloat("right", thisRight));

		float closestDistance;

		if (snapX) {
			closestDistance = Mathf.Abs (closestTargetSnapPoint.x) - Mathf.Abs (nearestBuilding.transform.InverseTransformPoint (snapPoints [0].value).x);
		} else {
			closestDistance = Mathf.Abs (closestTargetSnapPoint.z) - Mathf.Abs (nearestBuilding.transform.InverseTransformPoint (snapPoints [0].value).z);
		}

		StringFloat closestSnapPoint = new StringFloat(snapPoints[0].name, snapPoints[0].value);

		for (int i = 1; i < snapPoints.Count; i++) {
			float newDistance;

			if (snapX) {
				newDistance = Mathf.Abs (closestTargetSnapPoint.x) - Mathf.Abs (nearestBuilding.transform.InverseTransformPoint (snapPoints [i].value).x);
			} else {
				newDistance = Mathf.Abs (closestTargetSnapPoint.z) - Mathf.Abs (nearestBuilding.transform.InverseTransformPoint (snapPoints [i].value).z);
			}

			if (newDistance > closestDistance) {
				closestDistance = newDistance;
				closestSnapPoint = snapPoints[i];
			}
		}

		return closestSnapPoint;
	}

	void setPosition() {

		StringFloat closestTargetSnapPoint = getClosestTargetSnapPoint ();
		StringFloat closestSnapPointX = getClosestSnapPoint (true);
		StringFloat closestSnapPointZ = getClosestSnapPoint (false);

		objectToPlace.transform.parent = nearestBuilding.transform;

		float snapPositionX = nearestBuilding.transform.InverseTransformPoint (closestTargetSnapPoint.value).x - (nearestBuilding.transform.InverseTransformPoint (closestSnapPointX.value).x - objectToPlace.transform.localPosition.x);
		float snapPositionZ = nearestBuilding.transform.InverseTransformPoint (closestTargetSnapPoint.value).z - (nearestBuilding.transform.InverseTransformPoint (closestSnapPointZ.value).z - objectToPlace.transform.localPosition.z);

		if (closestTargetSnapPoint.name == "right") {
			objectToPlace.transform.localPosition = new Vector3(snapPositionX, 0, 0);
		}

		if (closestTargetSnapPoint.name == "left") {
			objectToPlace.transform.localPosition = new Vector3(snapPositionX, 0, 0);
		}

		if (closestTargetSnapPoint.name == "top") {
			objectToPlace.transform.localPosition = new Vector3(0, 0, snapPositionZ);
		}

		if (closestTargetSnapPoint.name == "bottom") {
			objectToPlace.transform.localPosition = new Vector3(0, 0, snapPositionZ);
		}

	}
}