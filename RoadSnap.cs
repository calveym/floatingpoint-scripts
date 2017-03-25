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

	public Vector3 topRight;
	public Vector3 bottomRight;
	public Vector3 topLeft;
	public Vector3 bottomLeft;

	Bounds thisBounds;

	GameObject objectToPlace;

	bool useCornerSnapPoints;
	GameObject objectWithLargerSide;

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

		/* middle of each side */

		right = new Vector3(objectToPlace.transform.position.x - (thisBounds.size.x / 2), objectToPlace.transform.position.y, objectToPlace.transform.position.z);
		left = new Vector3(objectToPlace.transform.position.x + (thisBounds.size.x / 2), objectToPlace.transform.position.y, objectToPlace.transform.position.z);
		top = new Vector3(objectToPlace.transform.position.x, objectToPlace.transform.position.y, objectToPlace.transform.position.z - (thisBounds.size.z / 2));
		bottom = new Vector3(objectToPlace.transform.position.x, objectToPlace.transform.position.y,objectToPlace.transform.position.z + (thisBounds.size.z / 2));

		/* corners */

		topRight = new Vector3 (right.x, objectToPlace.transform.position.y, top.z);
		topLeft = new Vector3 (left.x, objectToPlace.transform.position.y, top.z); 
		bottomRight = new Vector3 (right.x, objectToPlace.transform.position.y, bottom.z);
		bottomLeft = new Vector3 (left.x, objectToPlace.transform.position.y, bottom.z);

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
			/*
			GameObject g1 = (GameObject)Instantiate(snapCube, topRight, transform.rotation);
			GameObject g2 = (GameObject)Instantiate(snapCube, topLeft, transform.rotation);
			GameObject g3 = (GameObject)Instantiate(snapCube, bottomRight, transform.rotation);
			GameObject g4 = (GameObject)Instantiate(snapCube, bottomLeft, transform.rotation);

			GameObject g5 = (GameObject)Instantiate(snapCube, right, transform.rotation);
			GameObject g6 = (GameObject)Instantiate(snapCube, left, transform.rotation);
			GameObject g7 = (GameObject)Instantiate(snapCube, bottom, transform.rotation);
			GameObject g8 = (GameObject)Instantiate(snapCube, top, transform.rotation);
			*/

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
				if (hitcol.gameObject.layer == buildingLayer && hitcol != GetComponent<Collider> ()) {

					setToNearestBuilding (hitcol);

					//shouldUseCornerSnapPoints ();

					//Debug.Log (useCornerSnapPoints);

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

		// place it next to building and rotate side to face it
		objectToPlace = targetBox;
		setRotation();
		setPosition();

		// adjust vertical axis
		targetBox.transform.position = new Vector3(targetBox.transform.position.x, 10.025f, targetBox.transform.position.z);

		objectToPlace = gameObject;
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

	string getNearestAxis() {

		/* Determines if the nearest axis is x or z based on the closest target snap points */

		StringFloat closestTargetSnapPoint = getClosestTargetSnapPoint ();

		if (closestTargetSnapPoint.name == "left" || closestTargetSnapPoint.name == "right") {
			return "x";
		} else {
			return "z";
		}

	}

	float getClosestDistance(Vector3 target, Vector3 current) {

		string nearestAxis = getNearestAxis ();

		/* If nearestAxis is "x" then use that to calculate the distance of the two points, else use the z axis 
		 
		   transform.InverseTransformPoint(Vector3) gets the transform.position of the vector in local space, as if it were a child object to it */

		if (nearestAxis == "x") {
			return Mathf.Abs (target.x) - Mathf.Abs (nearestBuilding.transform.InverseTransformPoint (current).x);
		} else {
			return Mathf.Abs (target.z) - Mathf.Abs (nearestBuilding.transform.InverseTransformPoint (current).z);
		}

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

	StringFloat getClosestSnapPoint() {

		Vector3 closestTargetSnapPoint = nearestBuilding.transform.InverseTransformPoint(getClosestTargetSnapPoint ().value);

		updateSnapPoints ();

		/* find this closest side/point */

		Vector3 thisTop = RotatePointAroundPivot (top, objectToPlace.transform.position, objectToPlace.transform.eulerAngles);
		Vector3 thisBottom = RotatePointAroundPivot (bottom, objectToPlace.transform.position, objectToPlace.transform.eulerAngles);
		Vector3 thisLeft = RotatePointAroundPivot (left, objectToPlace.transform.position, objectToPlace.transform.eulerAngles);
		Vector3 thisRight = RotatePointAroundPivot (right, objectToPlace.transform.position, objectToPlace.transform.eulerAngles);

		Vector3 thisTopLeft = RotatePointAroundPivot (topLeft, objectToPlace.transform.position, objectToPlace.transform.eulerAngles);
		Vector3 thisBottomLeft = RotatePointAroundPivot (bottomLeft, objectToPlace.transform.position, objectToPlace.transform.eulerAngles);
		Vector3 thisTopRight = RotatePointAroundPivot (topRight, objectToPlace.transform.position, objectToPlace.transform.eulerAngles);
		Vector3 thisBottomRight = RotatePointAroundPivot (bottomRight, objectToPlace.transform.position, objectToPlace.transform.eulerAngles);

		/* find the closest snap point to the targetClosestPoint */

		Vector3 closestPoint = GetComponent<Renderer> ().bounds.ClosestPoint (nearestBuilding.transform.position); // get closest point on target building to this object's position

		List<StringFloat> snapPoints = new List<StringFloat>();

		snapPoints.Add (new StringFloat("top", thisTop));
		snapPoints.Add (new StringFloat("bottom", thisBottom));
		snapPoints.Add (new StringFloat("left", thisLeft));
		snapPoints.Add (new StringFloat("right", thisRight));

		/* if (useCornerSnapPoints && objectWithLargerSide == gameObject) {
			snapPoints.Add (new StringFloat("topLeft", thisTopLeft));
			snapPoints.Add (new StringFloat("bottomLeft", thisBottomLeft));
			snapPoints.Add (new StringFloat("topRight", thisTopRight));
			snapPoints.Add (new StringFloat("bottomRight", thisBottomRight));
		} */

		float closestDistance = getClosestDistance (closestTargetSnapPoint, snapPoints [0].value);

		StringFloat closestSnapPoint = new StringFloat(snapPoints[0].name, snapPoints[0].value);

		for (int i = 1; i < snapPoints.Count; i++) {
			float newDistance = getClosestDistance (closestTargetSnapPoint, snapPoints [i].value);

			if (newDistance > closestDistance) {
				closestDistance = newDistance;
				closestSnapPoint = snapPoints[i];
			}
		}

		return closestSnapPoint;
	}

	float getSideLength(StringFloat snapPoint, GameObject currentObject) {

		RoadSnap objectScript = currentObject.GetComponent<RoadSnap>();

		if (snapPoint.name == "left") {
			return Vector3.Distance (objectScript.topLeft, objectScript.bottomLeft);
		} else if (snapPoint.name == "right") {
			return Vector3.Distance (objectScript.topRight, objectScript.bottomRight);
		} else if (snapPoint.name == "top") {
			return Vector3.Distance (objectScript.topLeft, objectScript.topRight);
		} else if (snapPoint.name == "bottom") {
			return Vector3.Distance (objectScript.bottomLeft, objectScript.bottomRight);
		} else {
			return 0;
		}

	}

	void shouldUseCornerSnapPoints() {

		StringFloat closestSnapPoint = getClosestSnapPoint ();
		StringFloat closestTargetSnapPoint = getClosestTargetSnapPoint ();
		float targetSideLength = getSideLength(closestTargetSnapPoint, nearestBuilding);
		float closestSideLength = getSideLength(closestSnapPoint, gameObject);

		if (Mathf.Round(targetSideLength) != Mathf.Round(closestSideLength)) {
			useCornerSnapPoints = true;

			if (targetSideLength > closestSideLength) {
				objectWithLargerSide = nearestBuilding;
			} else {
				objectWithLargerSide = gameObject;
			}

		} else {
			useCornerSnapPoints = false;
		}

	}

	float getSnapPosition(Vector3 closestTarget, Vector3 closestThis) {

		string nearestAxis = getNearestAxis ();

		/* Calculate the position to move relative to the nearest building */

		if (nearestAxis == "x") {
			return nearestBuilding.transform.InverseTransformPoint (closestTarget).x - (nearestBuilding.transform.InverseTransformPoint (closestThis).x - objectToPlace.transform.localPosition.x);
		} else {
			return nearestBuilding.transform.InverseTransformPoint (closestTarget).z - (nearestBuilding.transform.InverseTransformPoint (closestThis).z - objectToPlace.transform.localPosition.z);
		}

	}

	void setPosition() {

		StringFloat closestTargetSnapPoint = getClosestTargetSnapPoint ();
		StringFloat closestSnapPoint = getClosestSnapPoint ();

		objectToPlace.transform.parent = nearestBuilding.transform;

		float snapPosition = getSnapPosition (closestTargetSnapPoint.value, closestSnapPoint.value);

		Debug.Log ("closer to: " + getNearestAxis ());
		Debug.Log ("nearest snap: " + closestTargetSnapPoint.name);

		if (closestTargetSnapPoint.name == "right") {
			objectToPlace.transform.localPosition = new Vector3(snapPosition, 0, 0);
		}

		if (closestTargetSnapPoint.name == "left") {
			objectToPlace.transform.localPosition = new Vector3(snapPosition, 0, 0);
		}

		if (closestTargetSnapPoint.name == "top") {
			objectToPlace.transform.localPosition = new Vector3(0, 0, snapPosition);
		}

		if (closestTargetSnapPoint.name == "bottom") {
			objectToPlace.transform.localPosition = new Vector3(0, 0, snapPosition);
		}

		objectToPlace.transform.parent = null;

	}
}
