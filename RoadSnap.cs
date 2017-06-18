using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RoadSnap : MonoBehaviour {

	public GameObject snapCube;
	public bool manualUse, manualSnap;	
	public GameObject targetBoxPrefab;
	public Material blockedMaterial;

	GameObject lastTargetBox;
	bool snapSetup = false;
	bool targetIsBlocked, snapObject, objectUsed;
	VRTK_InteractableObject interact;
	Bounds originalBounds;

	int roadLayer = 11;
	int layerMask;

	public void updateTargetIsBlocked(bool status) {
		targetIsBlocked = status;
	}

    void Start() {
		layerMask = 1 << roadLayer;
        interact = GetComponent<VRTK_InteractableObject>();
        //Debug.Log(gameObject + " " + interact);
        interact.InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbed);
        interact.InteractableObjectUngrabbed += new InteractableObjectEventHandler(DoGrabRelease);
	}

    public void SetupSnap()
    {
        snapSetup = true;
        GameObject clone = Instantiate(gameObject, Vector3.zero, Quaternion.identity);
        BoxCollider col = clone.GetComponent<BoxCollider>();
        col.enabled = true;
        originalBounds = col.bounds;
        Debug.Log("Original bounds at start: " + originalBounds.extents);
        Debug.Log("Anticlone bounds at start: " + gameObject.GetComponent<BoxCollider>().bounds.extents);
        SetBounds(originalBounds);
        Destroy(clone);
    }

    void SetBounds(Bounds setBounds)
    {
        GetComponent<SnapPoints>().SetupBounds(setBounds);
    }

    void ObjectGrabbed(object sender, InteractableObjectEventArgs e) {
        //Debug.Log("Grabbing");
		objectUsed = true;
		snapObject = false;
	}

	void DoGrabRelease(object sender, InteractableObjectEventArgs e)
    {
        //Debug.Log("Releasing");
        //gameObject.GetComponent<SnapPoints> ().enabled = false;
        if (!targetIsBlocked) {
			snapObject = true;
		}
	}
		
	void Update () {
		if (objectUsed || manualUse) {

			Main ();
		}
	}

	void Main() {
        //Debug.Log("Checking for snap");
		targetBoxPrefab.GetComponent<SnapPoints> ().bounds = originalBounds; // fixes the target box rotating with building

		List<GameObject> closestObjects = FindClosestObjects ();

		if (closestObjects.Count == 0)
			return;

		GameObject closestRoad = GetClosestRoad (closestObjects);

		KeyValuePair<string, Vector3> snapPoint = GetRoadSnapPoint (closestRoad);

		if (lastTargetBox != null)
			Destroy (lastTargetBox);
		
		if (ShouldObjectSnap(gameObject, snapPoint, closestRoad)) {
			GameObject targetBox = GenerateTargetBox ();

			targetBox.GetComponent<TargetCollisionCheck>().setNearestBuilding(closestRoad); // give target box nearest building for collision checking

			SetRotation (targetBox, closestRoad);
			SnapThis (targetBox, snapPoint, closestRoad);

			lastTargetBox = targetBox;

			if (snapObject || manualSnap) {
				SetRotation (gameObject, closestRoad);
				SnapThis (gameObject, snapPoint, closestRoad);

				manualSnap = false;
				objectUsed = false;
				snapObject = false;

				Destroy (lastTargetBox);
			}
		}
	}

	List<GameObject> FindClosestObjects () {
		
		Collider[] hitColliders = Physics.OverlapSphere (transform.position, 3f, layerMask); //  create a sphere to detect nearby objects

		List<GameObject> nearestObjects = new List<GameObject>();

		foreach (Collider hitcol in hitColliders) {
			if (hitcol.gameObject != gameObject) nearestObjects.Add (hitcol.gameObject);
		}

		return nearestObjects;
	}
		
	GameObject GetClosestRoad (List<GameObject> roads) {
		
		float closestDistance = 1000000;
		GameObject closestRoad = null;

		for (int i = 0; i < roads.Count; i++) {
			float newDistance = Vector3.Distance (transform.position, roads[i].transform.position);

			if (newDistance < closestDistance) {
				closestDistance = newDistance;
				closestRoad = roads[i];
			}
		}

		return closestRoad;
	}

	Dictionary<string, Vector3> GetVerticalPoints (GameObject road) {

		float halfWidth = 0.5f;

		Dictionary<string, Vector3> points = new Dictionary<string, Vector3>(); 

		if ((Mathf.Round(road.transform.rotation.eulerAngles.y) / 90) % 2 == 0) {
			points.Add("bottom", new Vector3 (road.transform.position.x, road.transform.position.y, road.transform.position.z + halfWidth));
			points.Add("top", new Vector3 (road.transform.position.x, road.transform.position.y, road.transform.position.z - halfWidth));
		} else {
			points.Add("bottom", new Vector3 (road.transform.position.x + halfWidth, road.transform.position.y, road.transform.position.z));
			points.Add("top", new Vector3 (road.transform.position.x - halfWidth, road.transform.position.y, road.transform.position.z));
		}

		return points;
	}

	KeyValuePair<string, Vector3> GetRoadSnapPoint (GameObject road) {

		Dictionary<string, Vector3> verticalPoints = GetVerticalPoints (road);

		float closestDistance = 1000000;
		KeyValuePair<string, Vector3> closestPoint = new KeyValuePair<string, Vector3>();

		foreach (KeyValuePair<string, Vector3> point in verticalPoints) {
			float newDistance = Vector3.Distance (transform.position, point.Value);

			if (newDistance < closestDistance) {
				closestDistance = newDistance;
				closestPoint = point;
			}
		}

		return closestPoint;
	}

	float CalculateDistance (Vector3 thisPoint, Vector3 targetPoint, bool useXAxis) {
		if (useXAxis) {
			return Mathf.Abs(targetPoint.x - thisPoint.x);
		} else {
			return Mathf.Abs(targetPoint.z - thisPoint.z);
		}
	}

	KeyValuePair<string, Vector3> GetClosestSnapPoint (GameObject objectToSnap, KeyValuePair<string, Vector3> roadSnapPoint, GameObject road) {

		Dictionary<string, Vector3> snapPoints = objectToSnap.GetComponent<SnapPoints> ().GetCurrentPoints ();

		float closestDistance = 1000000;
		KeyValuePair<string, Vector3> closestPoint = new KeyValuePair<string, Vector3>();

		bool useXAxis = !((Mathf.Round(road.transform.rotation.eulerAngles.y) / 90) % 2 == 0);

		foreach(KeyValuePair<string, Vector3> point in snapPoints) {

			float newDistance = CalculateDistance (point.Value, roadSnapPoint.Value, useXAxis);

			if (newDistance < closestDistance) {
				closestDistance = newDistance;
				closestPoint = point;
			}
		}

		return closestPoint;
	}

	GameObject GenerateTargetBox () {
		
		GameObject targetBox = (GameObject)Instantiate(targetBoxPrefab, transform.position, transform.rotation);

		if (targetIsBlocked) {
			targetBox.GetComponent<Renderer> ().material = blockedMaterial;
		}
			
		targetBox.GetComponent<TargetCollisionCheck> ().parentBuilding = gameObject; // tell the targetBox that this object is it's parent (uses it for calling updateTargetIsBlocked on this object)

		Vector3 thisSize = GetComponent<BoxCollider>().size;
		targetBox.transform.localScale = new Vector3 (thisSize.x / 10, 0.05f, thisSize.z / 10); // divides width and depth by 10 due to objects have x10 scale

		targetBox.transform.position = new Vector3(targetBox.transform.position.x, 10.025f, targetBox.transform.position.z); // adjust vertical axis as it is too low by default

		return targetBox;
	}

	bool ShouldObjectSnap(GameObject objectToSnap, KeyValuePair<string, Vector3> roadSnapPoint, GameObject road) {

		Dictionary<string, Vector3> snapPoints = gameObject.GetComponent<SnapPoints> ().GetCurrentPoints ();

		bool moreThanZ = objectToSnap.transform.position.z > road.transform.position.z;
		bool moreThanX = objectToSnap.transform.position.x > road.transform.position.x;	

		foreach(KeyValuePair<string, Vector3> point in snapPoints) {

			if ((Mathf.Round(road.transform.rotation.eulerAngles.y) / 90) % 2 == 0) {

				if ((point.Value.z < roadSnapPoint.Value.z) && moreThanZ || (point.Value.z > roadSnapPoint.Value.z) && !moreThanZ) {
					return false;
				}

			} else {

				if ((point.Value.x < roadSnapPoint.Value.x) && moreThanX || (point.Value.x > roadSnapPoint.Value.x) && !moreThanX) {
					return false;
				}
			}
		}
		return true;
	}
		

	void SetRotation(GameObject objectToRotate, GameObject targetObject) {

		GameObject emptyParent = new GameObject(); // this is needed to prevent the object scale being changed when parented

		targetObject.transform.parent = emptyParent.transform;
		objectToRotate.transform.parent = emptyParent.transform;

		float yAngle = Mathf.Round(objectToRotate.transform.localEulerAngles.y / 90) * 90;

		objectToRotate.transform.localRotation = Quaternion.Euler(0, yAngle, 0);

		objectToRotate.transform.parent = null;
		targetObject.transform.parent = null;

		Destroy (emptyParent);
	}

	void SnapThis (GameObject objectToSnap, KeyValuePair<string, Vector3> roadSnapPoint, GameObject road) {

		KeyValuePair<string, Vector3> closestSnapPoint = GetClosestSnapPoint (objectToSnap, roadSnapPoint, road);

		GameObject g1 = (GameObject)Instantiate (snapCube, closestSnapPoint.Value, objectToSnap.transform.rotation);

		objectToSnap.transform.parent = g1.transform;

		Vector3 snapPosition;

		if ((Mathf.Round(road.transform.rotation.eulerAngles.y) / 90) % 2 == 0) {
			snapPosition = new Vector3 (objectToSnap.transform.position.x, roadSnapPoint.Value.y, roadSnapPoint.Value.z);
		} else {
			snapPosition = new Vector3 (roadSnapPoint.Value.x, roadSnapPoint.Value.y, objectToSnap.transform.position.z);
		}

		g1.transform.position = snapPosition;

		objectToSnap.transform.parent = null;

		Destroy (g1);
	}
		
}
