using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;
using System.Linq;

public class RoadSnap : MonoBehaviour {

	public bool manualUse;
	public bool setBuildingPos;
	public GameObject targetBoxPrefab;
	public GameObject snapCube;
	public GameObject invisibleCube;
	public bool targetIsBlocked;
	public Material blockedMaterial;
	public float spacing = 1;

	VRTK_InteractableObject interact;
	bool objectUsed;
	GameObject targetBox = null;
	Collider[] hitColliders;
	GameObject nearestBuilding;
	Vector3 targetBounds;
	Vector3 targetPosition;
	Vector3 targetBoxPosition;
	float distanceToMoveX;
	float distanceToMoveZ;
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

	StringFloat closestTargetSnapPoint;
	StringFloat closestSnapPoint;

	Dictionary<string, Vector3> originalSnapPoints;

	public class StringVector3 {
		//define all of the values for the class
		public string name;
		public Vector3 value;

		public StringVector3 (string Name, Vector3 Value){
			name = Name;
			value = Value;        
		}

	}

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

		//GameObject clone = Instantiate (gameObject, transform.position, Quaternion.Euler(0,0,0));

		/* middle of each side */
		right = new Vector3(GetComponent<BoxCollider>().bounds.center.x - (thisBounds.size.x / 2), objectToPlace.transform.position.y, GetComponent<BoxCollider>().bounds.center.z);
		left = new Vector3(GetComponent<BoxCollider>().bounds.center.x  + (thisBounds.size.x / 2), objectToPlace.transform.position.y, GetComponent<BoxCollider>().bounds.center.z);
		top = new Vector3(GetComponent<BoxCollider>().bounds.center.x, objectToPlace.transform.position.y, GetComponent<BoxCollider>().bounds.center.z - (thisBounds.size.z / 2));
		bottom = new Vector3(GetComponent<BoxCollider>().bounds.center.x, objectToPlace.transform.position.y,GetComponent<BoxCollider>().bounds.center.z + (thisBounds.size.z / 2));

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
		thisBounds = GetComponent<Collider> ().bounds;

		thisSize = GetComponent<Collider> ().bounds.size;
		layerMask = 1 << buildingLayer;

		updateSnapPoints ();
		originalSnapPoints = new Dictionary<string, Vector3>();

		originalSnapPoints.Add ("center", new Vector3(thisBounds.center.x, 0, thisBounds.center.z));
		originalSnapPoints.Add ("top", new Vector3(top.x, 0, top.z));
		originalSnapPoints.Add ("right", new Vector3(right.x, 0, right.z));
		originalSnapPoints.Add ("left", new Vector3(left.x, 0, left.z));
		originalSnapPoints.Add ("bottom", new Vector3(bottom.x, 0, bottom.z));
		originalSnapPoints.Add ("bottomRight", new Vector3(bottomRight.x, 0, bottomRight.z));
		originalSnapPoints.Add ("bottomLeft", new Vector3(bottomLeft.x, 0, bottomLeft.z));
		originalSnapPoints.Add ("topRight", new Vector3(topRight.x, 0, topRight.z));
		originalSnapPoints.Add ("topLeft", new Vector3(topLeft.x, 0, topLeft.z));

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

		hitColliders = Physics.OverlapSphere (transform.position, 1.5f, layerMask); //  create a sphere to detect nearby objects

		if (hitColliders.Length == 0) {
			nearestBuilding = null;
			Destroy (targetBox);
		} 
		else {
			foreach (Collider hitcol in hitColliders) {
				if (hitcol.gameObject.layer == buildingLayer && hitcol != GetComponent<Collider> ()) {

					nearestBuilding = getNearestBuilding (hitcol); // set the nearest building to the closest one

					drawTargetBox (); // draw a target box and use setPosition() to show where the actual object would snap

				} else {
					Debug.Log ("Not building: " + hitcol);
				}
			}
		}
	}

	void drawTargetBox (){

		/* Creates a target box to show where the actual object will snap */ 

		if (targetBox) {

			// if it already exists, destroy it and create a new one

			Destroy (targetBox);
			targetBox = (GameObject)Instantiate(targetBoxPrefab, transform.position, nearestBuilding.transform.rotation);
		} else {
			targetBox = (GameObject)Instantiate(targetBoxPrefab, transform.position, nearestBuilding.transform.rotation);
		}

		if (targetIsBlocked) {
			targetBox.GetComponent<Renderer> ().material = blockedMaterial;
		}

		targetBox.GetComponent<TargetCollisionCheck> ().parentBuilding = gameObject; // tell the targetBox that this object is it's parent (uses it for calling updateTargetIsBlocked on this object)

		Vector3 thisSize = GetComponent<BoxCollider>().size;
		targetBox.transform.localScale = new Vector3 (thisSize.x / 10, 0.05f, thisSize.z / 10); // divides width and depth by 10 due to objects have x10 scale

		targetBox.GetComponent<TargetCollisionCheck>().setNearestBuilding(nearestBuilding); // give target box nearest building for collision checking

		objectToPlace = targetBox; // set rotation and position with use the targetBox instead of this object

		setRotation();
		setPosition();

		targetBox.transform.position = new Vector3(targetBox.transform.position.x, 10.025f, targetBox.transform.position.z); // adjust vertical axis as it is too low by default

		objectToPlace = gameObject; // reset objectToPlace to this object
	}

	GameObject getNearestBuilding (Collider hitcol){

		/* Compares the distance of the current nearestBuilding with the hitcol.gameObject passed in */

		if (nearestBuilding) {
			float currentTargetDistance = Vector3.Distance (transform.position, nearestBuilding.transform.position);
			float newTargetDistance = Vector3.Distance (transform.position, hitcol.gameObject.transform.position);

			if (newTargetDistance < currentTargetDistance) {
				return hitcol.gameObject;
			} else {
				return nearestBuilding;
			}
		} 
		else {
			return hitcol.gameObject;
		}
	}

	void setRotation(){

		/* Sets to the rotation of the current object to face the nearestBuilding to snap to */

		objectToPlace.transform.parent = nearestBuilding.transform;

		float yAngle = Mathf.Round(objectToPlace.transform.localEulerAngles.y / 90) * 90;

		objectToPlace.transform.localRotation = Quaternion.Euler(0, yAngle, 0);

		objectToPlace.transform.parent = null;

	}

	string getNearestAxis() {

		/* Determines if the nearest axis is x or z based on the closest target snap points */

		if (closestTargetSnapPoint.name == "left" || closestTargetSnapPoint.name == "right") {
			return "x";
		} else {
			return "z";
		}

	}


	List<StringFloat> getSnapPoints(GameObject currentObject, bool useCornerSnaps) {

		RoadSnap targetBuildingScript;

		if (currentObject == nearestBuilding) {
			targetBuildingScript = currentObject.GetComponent<RoadSnap> ();
			targetBuildingScript.updateSnapPoints (); // Update snap points to current position
		} else {
			updateSnapPoints ();
			targetBuildingScript = gameObject.GetComponent<RoadSnap> ();
		}

		List<StringFloat> snapPoints = new List<StringFloat>();

		Vector3 targetTop = RotatePointAroundPivot (targetBuildingScript.top, currentObject.transform.position, currentObject.transform.eulerAngles);
		Vector3 targetBottom = RotatePointAroundPivot (targetBuildingScript.bottom, currentObject.transform.position, currentObject.transform.eulerAngles);
		Vector3 targetLeft = RotatePointAroundPivot (targetBuildingScript.left, currentObject.transform.position, currentObject.transform.eulerAngles);
		Vector3 targetRight = RotatePointAroundPivot (targetBuildingScript.right, currentObject.transform.position, currentObject.transform.eulerAngles);

		/*
		GameObject g1 = (GameObject)Instantiate(snapCube, targetTop, transform.rotation);
		GameObject g2 = (GameObject)Instantiate(snapCube, targetBottom, transform.rotation);
		GameObject g3 = (GameObject)Instantiate(snapCube, targetLeft, transform.rotation);
		GameObject g4 = (GameObject)Instantiate(snapCube, targetRight, transform.rotation);
*/

		snapPoints.Add (new StringFloat("top", targetTop));
		snapPoints.Add (new StringFloat("bottom", targetBottom));
		snapPoints.Add (new StringFloat("left", targetLeft));
		snapPoints.Add (new StringFloat("right", targetRight));

		if (useCornerSnaps) {
			Vector3 targetTopLeft = RotatePointAroundPivot (targetBuildingScript.topLeft, currentObject.transform.position, currentObject.transform.eulerAngles);
			Vector3 targetBottomLeft = RotatePointAroundPivot (targetBuildingScript.bottomLeft, currentObject.transform.position, currentObject.transform.eulerAngles);
			Vector3 targetTopRight = RotatePointAroundPivot (targetBuildingScript.topRight, currentObject.transform.position, currentObject.transform.eulerAngles);
			Vector3 targetBottomRight = RotatePointAroundPivot (targetBuildingScript.bottomRight, currentObject.transform.position, currentObject.transform.eulerAngles);

			snapPoints.Add (new StringFloat("topLeft", targetTopLeft));
			snapPoints.Add (new StringFloat("bottomLeft", targetBottomLeft));
			snapPoints.Add (new StringFloat("topRight", targetTopRight));
			snapPoints.Add (new StringFloat("bottomRight", targetBottomRight));
		}

		return snapPoints;
	}




	StringFloat getClosestTargetSnapPoint(bool useCornerSnaps) {

		Vector3 targetClosestPoint = nearestBuilding.GetComponent<BoxCollider> ().bounds.ClosestPoint (objectToPlace.transform.position); // get closest point on target building to this object's position

		List<StringFloat> snapPoints = getSnapPoints(nearestBuilding, useCornerSnaps);

		/* find the closest snap point to the targetClosestPoint */

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

	float getClosestDistance(Vector3 point) {

		string nearestSide = getClosestTargetSide ();

		/* If nearestAxis is "x" then use that to calculate the distance of the two points, else use the z axis 
		 
		   transform.InverseTransformPoint(Vector3) gets the transform.position of the vector in local space, as if it were a child object to it */

		string[] normalSnapPoints = { "bottom", "top", "right", "left" };

		if (normalSnapPoints.Contains(closestTargetSnapPoint.name)) { 

			return Vector3.Distance (closestTargetSnapPoint.value, point);

		} else {
			if (nearestSide == "right" || nearestSide == "left") {
				return Mathf.Abs (nearestBuilding.transform.InverseTransformPoint (point).x);
			} else {
				return Mathf.Abs (nearestBuilding.transform.InverseTransformPoint (point).z);
			}
		}

	}

	StringFloat getClosestSnapPoint(bool useCornerSnaps) {

		//Vector3 closestTargetPointRelative = nearestBuilding.transform.InverseTransformPoint (closestTargetSnapPoint.value);
		//Vector3 closestPoint = nearestBuilding.GetComponent<Renderer> ().bounds.ClosestPoint (transform.position); // get closest point on target building to this object's position

		List<StringFloat> snapPoints = getSnapPoints(gameObject, useCornerSnaps);

		/* find the closest snap point to the targetClosestPoint */

		//float closestDistance = Vector3.Distance (closestPoint, snapPoints [0].value);
		float closestDistance = getClosestDistance (snapPoints[0].value);

		StringFloat closestSnapPoint = new StringFloat(snapPoints[0].name, snapPoints[0].value);

		for (int i = 1; i < snapPoints.Count; i++) {
			float newDistance = getClosestDistance (snapPoints[i].value);

			if (newDistance < closestDistance) {
				closestDistance = newDistance;
				closestSnapPoint = snapPoints[i];
			}
		}

		//GameObject g1 = (GameObject)Instantiate(snapCube, closestSnapPoint.value, transform.rotation);

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

	/*

	void shouldUseCornerSnapPoints() {
		
		float targetSideLength = getSideLength(closestTargetSnapPoint, nearestBuilding);
		float closestSideLength = getSideLength(closestSnapPoint, gameObject);

		if (Mathf.Round(targetSideLength) != Mathf.Round(closestSideLength)) {
			useCornerSnapPoints = true;
		} else {
			useCornerSnapPoints = false;
		}

	}

*/

	float getSnapPosition() {

		string nearestAxis = getNearestAxis ();

		/* Calculate the position to move relative to the nearest building */

		if (targetSide == "right" || targetSide == "left") {
			return nearestBuilding.transform.InverseTransformPoint (closestTargetSnapPoint.value).x - (nearestBuilding.transform.InverseTransformPoint (closestSnapPoint.value).x - objectToPlace.transform.localPosition.x);
		} else {
			return nearestBuilding.transform.InverseTransformPoint (closestTargetSnapPoint.value).z - (nearestBuilding.transform.InverseTransformPoint (closestSnapPoint.value).z - objectToPlace.transform.localPosition.z);
		}

	}

	StringFloat[] getClosestCorners () {

		Dictionary<string, StringFloat[]> snapPointsList = new Dictionary<string, StringFloat[]>();

		Vector3 thisTopLeft = RotatePointAroundPivot (topLeft, transform.position, transform.eulerAngles);
		Vector3 thisBottomLeft = RotatePointAroundPivot (bottomLeft, transform.position, transform.eulerAngles);
		Vector3 thisTopRight = RotatePointAroundPivot (topRight, transform.position, transform.eulerAngles);
		Vector3 thisBottomRight = RotatePointAroundPivot (bottomRight, transform.position, transform.eulerAngles);
		/*
		GameObject g1 = (GameObject)Instantiate (snapCube, thisTopLeft, transform.rotation);
		GameObject g2 = (GameObject)Instantiate (snapCube, thisTopRight, transform.rotation);
		GameObject g3 = (GameObject)Instantiate (snapCube, thisTopRight, transform.rotation);
		GameObject g4 = (GameObject)Instantiate (snapCube, thisBottomRight, transform.rotation);
*/
		snapPointsList.Add ("top", new StringFloat[] { new StringFloat("topLeft", thisTopLeft), new StringFloat("topRight", thisTopRight) });
		snapPointsList.Add ("bottom", new StringFloat[] { new StringFloat("bottomLeft", thisBottomLeft), new StringFloat("bottomRight", thisBottomRight) });
		snapPointsList.Add ("right", new StringFloat[] { new StringFloat("topRight", thisTopRight), new StringFloat("bottomRight", thisBottomRight) });
		snapPointsList.Add ("left", new StringFloat[] { new StringFloat("topLeft", thisTopLeft), new StringFloat("bottomLeft", thisBottomLeft) });

		return snapPointsList [closestSnapPoint.name];

	}

	float appendAxis (StringFloat point, string axis) {
		if (axis == "z") {
			return point.value.z;
		} else {
			return point.value.x;
		}
	}

	string getClosestTargetSide() {

		List<StringFloat> snapPoints = new List<StringFloat>();
		RoadSnap targetBuildingScript = nearestBuilding.GetComponent<RoadSnap> ();

		Vector3 targetTopLeft = RotatePointAroundPivot (targetBuildingScript.topLeft, nearestBuilding.transform.position, nearestBuilding.transform.eulerAngles);
		Vector3 targetBottomLeft = RotatePointAroundPivot (targetBuildingScript.bottomLeft, nearestBuilding.transform.position, nearestBuilding.transform.eulerAngles);
		Vector3 targetTopRight = RotatePointAroundPivot (targetBuildingScript.topRight, nearestBuilding.transform.position, nearestBuilding.transform.eulerAngles);
		Vector3 targetBottomRight = RotatePointAroundPivot (targetBuildingScript.bottomRight, nearestBuilding.transform.position, nearestBuilding.transform.eulerAngles);

		snapPoints.Add (new StringFloat("topLeft", targetTopLeft));
		snapPoints.Add (new StringFloat("bottomLeft", targetBottomLeft));
		snapPoints.Add (new StringFloat("topRight", targetTopRight));
		snapPoints.Add (new StringFloat("bottomRight", targetBottomRight));

		GameObject g1 = (GameObject)Instantiate (invisibleCube, targetBottomRight, nearestBuilding.transform.rotation);
		GameObject g2 = (GameObject)Instantiate (invisibleCube, targetBottomLeft, nearestBuilding.transform.rotation);

		Vector3 relativePosRight = g1.transform.InverseTransformPoint(transform.position);
		Vector3 relativePosLeft = g2.transform.InverseTransformPoint(transform.position);

		Destroy (g1);
		Destroy (g2);

		if (relativePosRight.x < 0) {
			return "right";		
		} else if (relativePosLeft.x > 0) {
			return "left";
		} else if (nearestBuilding.transform.InverseTransformPoint (transform.position).z < 0) {
			return "top";		
		} else {
			return "bottom";		
		}

	}

	string targetSide;

	StringFloat getThisCornerToSnap() {

		/* Returns the correct corner on this object to snap to by comparing the two corner snap points positions */

		targetSide = getClosestTargetSide();
		StringFloat[] closestCorners = getClosestCorners ();

		if ((targetSide == "right" || targetSide == "left") && (closestTargetSnapPoint.name == "topLeft" || closestTargetSnapPoint.name == "topRight")) {
			// compare this sides snaps using highest z

			if (nearestBuilding.transform.InverseTransformPoint(closestCorners [0].value).z > nearestBuilding.transform.InverseTransformPoint(closestCorners [1].value).z) {
				return closestCorners [1];
			} else {
				return closestCorners [0];
			}
		}

		if ((targetSide == "right" || targetSide == "left") && (closestTargetSnapPoint.name == "bottomLeft" || closestTargetSnapPoint.name == "bottomRight")) {
			// compare this sides snaps using lowest z

			if (nearestBuilding.transform.InverseTransformPoint(closestCorners [0].value).z > nearestBuilding.transform.InverseTransformPoint(closestCorners [1].value).z) {
				return closestCorners [0];
			} else {
				return closestCorners [1];
			}
		}

		if ((targetSide == "top" || targetSide == "bottom") && (closestTargetSnapPoint.name == "topRight" || closestTargetSnapPoint.name == "bottomRight")) {
			// compare this sides snaps using highest x

			if (nearestBuilding.transform.InverseTransformPoint(closestCorners [0].value).x > nearestBuilding.transform.InverseTransformPoint(closestCorners [1].value).x) {
				return closestCorners [1];
			} else {
				return closestCorners [0];
			}

		}

		if ((targetSide == "top" || targetSide == "bottom") && (closestTargetSnapPoint.name == "topLeft" || closestTargetSnapPoint.name == "bottomLeft")) {
			// compare this sides snaps using lowest x

			if (nearestBuilding.transform.InverseTransformPoint(closestCorners [0].value).x > nearestBuilding.transform.InverseTransformPoint(closestCorners [1].value).x) {
				return closestCorners [0];
			} else {
				return closestCorners [1];
			}
		} else {
			return new StringFloat("null", Vector3.zero);
		}
	}

	void setPosition() {

		/* This sets the position of the objectToPlace (the target box or the building itself) */

		closestTargetSnapPoint = getClosestTargetSnapPoint (true); // get the closest target snap point
		closestSnapPoint = getClosestSnapPoint (false); // get the closest snap point on this object

		string[] cornerSnaps = { "topLeft", "topRight", "bottomLeft", "bottomRight" };

		//Debug.Log ("closest target: " + closestTargetSnapPoint.name + ", closestPoint: " + closestSnapPoint.name);

		objectToPlace.transform.position = cornerSnaps.Contains (closestTargetSnapPoint.name) ? getCornerPosition () : getSidePosition ();

	}

	Vector3 generateNewPosition(string type, string position, Vector3 buildingLength) {

		/* This returns a Vector3 applying the correct spacing depending on the side, and subtracting the buildingLength */

		Vector3 distance = Vector3.zero;

		switch (position)
		{
		case "right":
			distance = new Vector3 (spacing, 0, 0); 
			break;
		case "left":
			distance = new Vector3 (-spacing, 0, 0);
			break;
		case "bottom":
			distance = new Vector3 (0, 0, -spacing);
			break;
		case "top":
			distance = new Vector3 (0, 0, spacing); 
			break;
		}

		/* InverseTransformPoint() gets the position relative to the called object (as if it is a parent), TransformPoint() gets the Vector3 passed in as a world position 

		  It starts with the closestTargetSnapPoint, subtracts the distance that is set globally, and then subtracts or adds the buildingLength. Snapping to the left or bottom sides are the only cases where the length is added */

		if (type == "side" && (position == "left" || position == "bottom")) {
			return nearestBuilding.transform.TransformPoint ((nearestBuilding.transform.InverseTransformPoint (closestTargetSnapPoint.value) - distance + buildingLength));
		} else {
			return nearestBuilding.transform.TransformPoint ((nearestBuilding.transform.InverseTransformPoint (closestTargetSnapPoint.value) - distance - buildingLength));
		}

	}

	Vector3 getCornerPosition() {

		/* This returns a Vector3 which is the position for snapping to a corner */

		StringFloat thisCornerToSnap = getThisCornerToSnap ();

		Vector3 buildingLength = (originalSnapPoints [thisCornerToSnap.name] - originalSnapPoints ["center"]) * 10; // the distance between the closest point and the center, multiplied by 10 due local/world difference

		float newZAxis = 0;
		float newXAxis = 0;

		if (targetSide == "right" || targetSide == "left") {
			newZAxis = closestTargetSnapPoint.name == "topRight" || closestTargetSnapPoint.name == "topLeft" ? -(Mathf.Abs (buildingLength.z)) : Mathf.Abs (buildingLength.z); // the z-axis will be positive or negative depending on the corner it is snapping to
			newXAxis = targetSide == "right" ? Mathf.Abs (buildingLength.x) : -(Mathf.Abs (buildingLength.x)); // the x-axis will be positive if "right" or negative if "left"
		} else {
			newXAxis = closestTargetSnapPoint.name == "bottomRight" || closestTargetSnapPoint.name == "topRight" ? -(Mathf.Abs (buildingLength.x)) : Mathf.Abs (buildingLength.x); // the x-axis will be positive or negative depending on the corner it is snapping to
			newZAxis = targetSide == "bottom" ? -(Mathf.Abs (buildingLength.z)) : Mathf.Abs (buildingLength.z); // the z-axis will be negative if "bottom" or positive if "top"
		}

		buildingLength = new Vector3 (newXAxis, 0, newZAxis); // this it the distance between the snap point and the center point, which is the distance to subtract or add to get the final position

		return generateNewPosition ("corner", targetSide, buildingLength);

	}

	Vector3 getSidePosition() {

		/* This returns a Vector3 which is the position for snapping to a side */

		Vector3 buildingLength = Vector3.zero;

		if (closestTargetSnapPoint.name == "right" || closestTargetSnapPoint.name == "left") {
			buildingLength = new Vector3 (Vector3.Distance(originalSnapPoints [closestSnapPoint.name], originalSnapPoints ["center"]) * 10, 0, 0); // the distance between the closest point and the center, multiplied by 10 due local/world difference
		} else {
			buildingLength = new Vector3 (0, 0, Vector3.Distance(originalSnapPoints [closestSnapPoint.name], originalSnapPoints ["center"]) * 10); // the distance between the closest point and the center, multiplied by 10 due local/world difference
		}

		return generateNewPosition ("side", closestTargetSnapPoint.name, buildingLength);

	}
}
