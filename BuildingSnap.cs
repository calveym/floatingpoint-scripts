using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;
using System.Linq;

public class BuildingSnap : MonoBehaviour {

	public bool manualUse, setBuildingPos, targetIsBlocked;
	public GameObject targetBoxPrefab, snapCube, invisibleCube;
	public Vector3 top,bottom, left, right, topRight, bottomRight, topLeft, bottomLeft;
 	public Material blockedMaterial;
	public float spacing = 1;

	VRTK_InteractableObject interact;
	bool objectUsed = false;
	GameObject targetBox = null, nearestBuilding, objectToPlace;
	Collider[] hitColliders;
	Vector3 targetBounds, targetPosition, targetBoxPosition, thisSize;

	int buildingLayer;

	Bounds originalBounds;

	StringVector3 closestTargetSnapPoint, closestSnapPoint;

	Dictionary<string, Vector3> originalSnapPoints;

	public class StringVector3 {
		public string name;
		public Vector3 value;

		public StringVector3 (string Name, Vector3 Value){
			name = Name;
			value = Value;        
		}
	}

	public void updateTargetIsBlocked(bool status) {
		targetIsBlocked = status;
	}
		
	public void updateSnapPoints() {

		/* Updates the snapPoints to the current object's position */

		Vector3 center = GetComponent<BoxCollider> ().bounds.center;
		float yAxis = objectToPlace.transform.position.y, halfWidth = (originalBounds.size.x / 2), halfDepth = (originalBounds.size.z / 2);

		right = new Vector3(center.x - halfWidth, yAxis, center.z);
		left = new Vector3(center.x  + halfWidth, yAxis, center.z);
		top = new Vector3(center.x, yAxis, center.z - halfDepth);
		bottom = new Vector3(center.x, yAxis,center.z + halfDepth);

		topRight = new Vector3 (right.x, yAxis, top.z);
		topLeft = new Vector3 (left.x, yAxis, top.z); 
		bottomRight = new Vector3 (right.x, yAxis, bottom.z);
		bottomLeft = new Vector3 (left.x, yAxis, bottom.z);

	}
		
	public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {

		/* Used to rotate the points around the objects rotations, otherwise they would be when it had 0 rotation */
		
		Vector3 dir = point - pivot; // get point direction relative to pivot
		dir = Quaternion.Euler(angles) * dir; // rotate it
		point = dir + pivot; // calculate rotated point
		return point; // return it

	}

	void DoGrabRelease(object sender, ControllerInteractionEventArgs e) {
		if (objectUsed == true) {
			if (!targetIsBlocked) {
				objectToPlace = gameObject;
				placeObject ();
			}
			objectUsed = false;
		} 
	}

	void DoGrabStart(object sender, ControllerInteractionEventArgs e) {
		if(interact.IsGrabbed() == true) {	
			objectUsed = true;
		}
	}

	void Awake() {
		buildingLayer = LayerMask.NameToLayer("Buildings");
	}

	void Start () {
		objectToPlace = gameObject; // Either the current object or the targetBox
		originalBounds = GetComponent<Collider> ().bounds;
		thisSize = GetComponent<Collider> ().bounds.size;

		updateSnapPoints ();
		setOriginalSnapPoints ();
		bindVRTKEvents ();

		interact = gameObject.GetComponent<VRTK_InteractableObject>();
		objectUsed = false;
	}


	void Update() {

		if (objectUsed || manualUse) {
			gameObject.layer = 30; // won't conflict with other buildings now;
			getNearbyBuildings ();
		} 
		else {
			gameObject.layer = buildingLayer;
		}

		if (setBuildingPos) {
			// this is for manual debugging
			if (!targetIsBlocked) {
				objectToPlace = gameObject;
				placeObject ();
			}
		}
	}

	void setOriginalSnapPoints() {
		originalSnapPoints = new Dictionary<string, Vector3>()
		{
			{ "center", new Vector3(originalBounds.center.x, 0, originalBounds.center.z) },
			{ "top", new Vector3(top.x, 0, top.z) },
			{ "right", new Vector3(right.x, 0, right.z) },
			{ "left", new Vector3(left.x, 0, left.z) },
			{ "bottom", new Vector3(bottom.x, 0, bottom.z) },
			{ "bottomRight", new Vector3(bottomRight.x, 0, bottomRight.z) },
			{ "bottomLeft", new Vector3(bottomLeft.x, 0, bottomLeft.z) },
			{ "topRight", new Vector3(topRight.x, 0, topRight.z) },
			{ "topLeft", new Vector3(topLeft.x, 0, topLeft.z) }
		};
	}

	void bindVRTKEvents() {
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

	}
		
	void placeObject() {
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

		hitColliders = Physics.OverlapSphere (transform.position, 1.5f, 1 << buildingLayer); //  create a sphere to detect nearby objects

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
			targetBox = (GameObject)Instantiate(targetBoxPrefab, transform.position, transform.rotation);
		} else {
			targetBox = (GameObject)Instantiate(targetBoxPrefab, transform.position, transform.rotation);
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


	List<StringVector3> getSnapPoints(GameObject currentObject, bool useCornerSnaps) {

		BuildingSnap targetBuildingScript;

		if (currentObject == nearestBuilding) {
			targetBuildingScript = currentObject.GetComponent<BuildingSnap> ();
			targetBuildingScript.updateSnapPoints (); // Update snap points to current position
		} else {
			updateSnapPoints ();
			targetBuildingScript = gameObject.GetComponent<BuildingSnap> ();
		}

		List<StringVector3> snapPoints = new List<StringVector3>();

		Vector3 targetTop = RotatePointAroundPivot (targetBuildingScript.top, currentObject.transform.position, currentObject.transform.eulerAngles);
		Vector3 targetBottom = RotatePointAroundPivot (targetBuildingScript.bottom, currentObject.transform.position, currentObject.transform.eulerAngles);
		Vector3 targetLeft = RotatePointAroundPivot (targetBuildingScript.left, currentObject.transform.position, currentObject.transform.eulerAngles);
		Vector3 targetRight = RotatePointAroundPivot (targetBuildingScript.right, currentObject.transform.position, currentObject.transform.eulerAngles);

		snapPoints.Add (new StringVector3("top", targetTop));
		snapPoints.Add (new StringVector3("bottom", targetBottom));
		snapPoints.Add (new StringVector3("left", targetLeft));
		snapPoints.Add (new StringVector3("right", targetRight));

		if (useCornerSnaps) {
			Vector3 targetTopLeft = RotatePointAroundPivot (targetBuildingScript.topLeft, currentObject.transform.position, currentObject.transform.eulerAngles);
			Vector3 targetBottomLeft = RotatePointAroundPivot (targetBuildingScript.bottomLeft, currentObject.transform.position, currentObject.transform.eulerAngles);
			Vector3 targetTopRight = RotatePointAroundPivot (targetBuildingScript.topRight, currentObject.transform.position, currentObject.transform.eulerAngles);
			Vector3 targetBottomRight = RotatePointAroundPivot (targetBuildingScript.bottomRight, currentObject.transform.position, currentObject.transform.eulerAngles);

			snapPoints.Add (new StringVector3("topLeft", targetTopLeft));
			snapPoints.Add (new StringVector3("bottomLeft", targetBottomLeft));
			snapPoints.Add (new StringVector3("topRight", targetTopRight));
			snapPoints.Add (new StringVector3("bottomRight", targetBottomRight));
		}

		return snapPoints;
	}




	StringVector3 getClosestTargetSnapPoint(bool useCornerSnaps) {

		Vector3 targetClosestPoint = nearestBuilding.GetComponent<BoxCollider> ().bounds.ClosestPoint (objectToPlace.transform.position); // get closest point on target building to this object's position

		List<StringVector3> snapPoints = getSnapPoints(nearestBuilding, useCornerSnaps);

		/* find the closest snap point to the targetClosestPoint */

		float closestDistance = Vector3.Distance (targetClosestPoint, snapPoints[0].value);
		StringVector3 closestTargetSnapPoint = new StringVector3(snapPoints[0].name, snapPoints[0].value);

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

	StringVector3 getClosestSnapPoint(bool useCornerSnaps) {

		List<StringVector3> snapPoints = getSnapPoints(gameObject, useCornerSnaps);

		/* find the closest snap point to the targetClosestPoint */

		//float closestDistance = Vector3.Distance (closestPoint, snapPoints [0].value);
		float closestDistance = getClosestDistance (snapPoints[0].value);

		StringVector3 closestSnapPoint = new StringVector3(snapPoints[0].name, snapPoints[0].value);

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

	float getSideLength(StringVector3 snapPoint, GameObject currentObject) {

		BuildingSnap objectScript = currentObject.GetComponent<BuildingSnap>();

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

	float getSnapPosition() {

		string nearestAxis = getNearestAxis ();

		/* Calculate the position to move relative to the nearest building */

		if (targetSide == "right" || targetSide == "left") {
			return nearestBuilding.transform.InverseTransformPoint (closestTargetSnapPoint.value).x - (nearestBuilding.transform.InverseTransformPoint (closestSnapPoint.value).x - objectToPlace.transform.localPosition.x);
		} else {
			return nearestBuilding.transform.InverseTransformPoint (closestTargetSnapPoint.value).z - (nearestBuilding.transform.InverseTransformPoint (closestSnapPoint.value).z - objectToPlace.transform.localPosition.z);
		}

	}

	StringVector3[] getClosestCorners () {

		Dictionary<string, StringVector3[]> snapPointsList = new Dictionary<string, StringVector3[]>();

		Vector3 thisTopLeft = RotatePointAroundPivot (topLeft, transform.position, transform.eulerAngles);
		Vector3 thisBottomLeft = RotatePointAroundPivot (bottomLeft, transform.position, transform.eulerAngles);
		Vector3 thisTopRight = RotatePointAroundPivot (topRight, transform.position, transform.eulerAngles);
		Vector3 thisBottomRight = RotatePointAroundPivot (bottomRight, transform.position, transform.eulerAngles);

		snapPointsList.Add ("top", new StringVector3[] { new StringVector3("topLeft", thisTopLeft), new StringVector3("topRight", thisTopRight) });
		snapPointsList.Add ("bottom", new StringVector3[] { new StringVector3("bottomLeft", thisBottomLeft), new StringVector3("bottomRight", thisBottomRight) });
		snapPointsList.Add ("right", new StringVector3[] { new StringVector3("topRight", thisTopRight), new StringVector3("bottomRight", thisBottomRight) });
		snapPointsList.Add ("left", new StringVector3[] { new StringVector3("topLeft", thisTopLeft), new StringVector3("bottomLeft", thisBottomLeft) });

		return snapPointsList [closestSnapPoint.name];

	}

	float appendAxis (StringVector3 point, string axis) {
		if (axis == "z") {
			return point.value.z;
		} else {
			return point.value.x;
		}
	}

	string getClosestTargetSide() {

		List<StringVector3> snapPoints = new List<StringVector3>();
		BuildingSnap targetBuildingScript = nearestBuilding.GetComponent<BuildingSnap> ();

		Vector3 targetTopLeft = RotatePointAroundPivot (targetBuildingScript.topLeft, nearestBuilding.transform.position, nearestBuilding.transform.eulerAngles);
		Vector3 targetBottomLeft = RotatePointAroundPivot (targetBuildingScript.bottomLeft, nearestBuilding.transform.position, nearestBuilding.transform.eulerAngles);
		Vector3 targetTopRight = RotatePointAroundPivot (targetBuildingScript.topRight, nearestBuilding.transform.position, nearestBuilding.transform.eulerAngles);
		Vector3 targetBottomRight = RotatePointAroundPivot (targetBuildingScript.bottomRight, nearestBuilding.transform.position, nearestBuilding.transform.eulerAngles);

		snapPoints.Add (new StringVector3("topLeft", targetTopLeft));
		snapPoints.Add (new StringVector3("bottomLeft", targetBottomLeft));
		snapPoints.Add (new StringVector3("topRight", targetTopRight));
		snapPoints.Add (new StringVector3("bottomRight", targetBottomRight));

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

	StringVector3 getThisCornerToSnap() {

		/* Returns the correct corner on this object to snap to by comparing the two corner snap points positions */

		targetSide = getClosestTargetSide();
		StringVector3[] closestCorners = getClosestCorners ();

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
			return new StringVector3("null", Vector3.zero);
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

		StringVector3 thisCornerToSnap = getThisCornerToSnap ();

		Vector3 buildingLength = (originalSnapPoints [thisCornerToSnap.name] - originalSnapPoints ["center"]) * 10; // the distance between the closest point and the center, multiplied by 10 due local/world difference

		// Switch the buildingLength if the object rotates 90 or 180 degrees
		if (Mathf.Round((transform.rotation.eulerAngles.y / 90)) % 2 != 0) {
			Debug.Log ("running!");
			buildingLength = new Vector3 (buildingLength.z, 0, buildingLength.x);
		}

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
