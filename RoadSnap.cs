using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;

public class RoadSnap : MonoBehaviour {

	VRTK_InteractableObject interact;
	bool objectUsed;
	RoadGenerator roadGenerator;
	public GameObject cube;
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

	void Update() {
		// checks if object is used, and if there is a nearby object with the matching tag
		if (objectUsed) {
			hitColliders = Physics.OverlapSphere (transform.position, 1.5f);
			foreach (Collider hitcol in hitColliders) {
				if (hitcol.CompareTag ("residential")) {
					nearestBuilding = hitcol.gameObject;
				} else {
					nearestBuilding = null;
				}
			}
		}
	}

	void Start ()
	{
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
		roadGenerator = GameObject.Find("Island").GetComponent<RoadGenerator>();
		objectUsed = false;
	}

	void DoGrabRelease(object sender, ControllerInteractionEventArgs e)
	// Grab end event listener
	{
		if (objectUsed == true) {
			if (nearestBuilding) {
				gameObject.GetComponent<BoxCollider> ().enabled = false;
				setPosition ();
				gameObject.GetComponent<BoxCollider> ().enabled = true;
			} else {
				GetComponent<BoxCollider> ().enabled = true;
			}

			objectUsed = false;

		} 
	}

	void DoGrabStart(object sender, ControllerInteractionEventArgs e)
	// Grab start event listener
	{
		if(interact.IsGrabbed() == true)
		{
			// gameObject.GetComponent<Collider> ().enabled = false;
			objectUsed = true;

		}
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

		// set x position
		transform.position = new Vector3(targetPosition.x + distanceToMoveX, targetPosition.y, targetPosition.z);
		//gameObject.GetComponent<Collider> ().enabled = true;

		// set z position, and align x axis
		//transform.position = new Vector3(transform.position.x + pointDifference, targetPosition.y, targetPosition.z - distanceToMoveZ);


	}

	void InitiateSnapCheck()
	// Checks if object can snap
	{
		if(roadGenerator.CheckSurroundingRoads(transform.position))
		{
			// Debug.Log ("Running!");
			Vector3 newPosition;
			roadGenerator.Round(transform.position, out newPosition);
			// transform.position = newPosition;
			// transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		}
	}
}
