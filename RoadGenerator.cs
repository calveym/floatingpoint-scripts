using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RoadGenerator : VRTK_InteractableObject {

	public GameObject controller;

	public GameObject smallRoadStraight;
	public GameObject smallRoadTurn;
	public GameObject smallRoadTJunction;
	public GameObject smallRoadXJunction;

	public Vector3 gridSize;

	private List<bool> surroundingRoads;
	private string surroundingRoadString;
	private RaycastHit hit;
	private Vector3 newPosition;
	private bool up;
	private bool down;
	private bool left;
	private bool right;
	private Dictionary<string, GameObject> roadObject;
	private Dictionary<string, Quaternion> roadRotation;

	public List<Vector3> roadPositions;

	public bool V3Equal(Vector3 a, Vector3 b)
    {
		return Vector3.SqrMagnitude(a - b) < 3.162f;
	}

	public void removeRoad(Vector3 position)
    {
		for(int i = 0; i < roadPositions.Count; i++)
        {
			if(V3Equal(roadPositions[i] , position))
            {
				roadPositions.RemoveAt(i);
			}
		}
	}

	void Start()
	{
		roadPositions = new List<Vector3>();

		Quaternion zero = new Quaternion(0, 0, 0, 1);
		Quaternion ninety = new Quaternion(0, 0.7071f, 0, 0.7071f);
		Quaternion opposite = new Quaternion(0, 1, 0, 0);
		Quaternion minusNinety = new Quaternion(0, -0.7071f, 0, 0.7071f);

		roadObject = new Dictionary<string, GameObject>();
		roadObject.Add("0000", smallRoadStraight);
		roadObject.Add("1000", smallRoadStraight);
		roadObject.Add("0100", smallRoadStraight);
		roadObject.Add("0010", smallRoadStraight);
		roadObject.Add("0001", smallRoadStraight);
		roadObject.Add("1100", smallRoadStraight);
		roadObject.Add("0011", smallRoadStraight);
		roadObject.Add("1010", smallRoadTurn);
		roadObject.Add("0101", smallRoadTurn);
		roadObject.Add("1001", smallRoadTurn);
		roadObject.Add("0110", smallRoadTurn);
		roadObject.Add("0111", smallRoadTJunction);
		roadObject.Add("1110", smallRoadTJunction);
		roadObject.Add("1011", smallRoadTJunction);
		roadObject.Add("1101", smallRoadTJunction);
		roadObject.Add("1111", smallRoadXJunction);


		roadRotation = new Dictionary<string, Quaternion>();
		roadRotation.Add("0000", zero);
		roadRotation.Add("1000", ninety);
		roadRotation.Add("0100", ninety);
		roadRotation.Add("0010", zero);
		roadRotation.Add("0001", zero);
		roadRotation.Add("1100", ninety);
		roadRotation.Add("0011", zero);
		roadRotation.Add("1010", zero);
		roadRotation.Add("0101", opposite);
		roadRotation.Add("1001", ninety);
		roadRotation.Add("0110", minusNinety);
		roadRotation.Add("0111", opposite);
		roadRotation.Add("1110", minusNinety);
		roadRotation.Add("1011", zero);
		roadRotation.Add("1101", ninety);
		roadRotation.Add("1111", zero);

		reDrawRoads();
	}

	public override void StartUsing (GameObject usingObject) {
		base.StartUsing (usingObject);
		if (Physics.Raycast (controller.transform.position, controller.transform.forward, out hit, 1000.0f)) {
			// Debug.Log(hit.point);
			round (hit.point, out newPosition);
			// Debug.Log(hit.point);
			roadPositions.Add(newPosition);
			destroy();
			reDrawRoads();
		}
	}

	private void round (Vector3 point, out Vector3 newPosition)
    {
		newPosition = new Vector3(Mathf.Round(point.x) * gridSize.x,
			10.01f,
			Mathf.Round(point.z) * gridSize.z);
	}

	private void findCorrectRoad(Vector3 newPosition, out GameObject correctRoad)
    {
		checkRoadUp (newPosition, out up);
		checkRoadDown(newPosition, out down);
		checkRoadLeft(newPosition, out left);
		checkRoadRight(newPosition, out right);

		List<bool> surroundingRoads = new List<bool> ();
		surroundingRoads.Add (up);
		surroundingRoads.Add (down);
		surroundingRoads.Add (left);
		surroundingRoads.Add (right);

		string surroundingRoadString = "";

		for(int x = 0; x < surroundingRoads.Count; x++)
        {
			if(surroundingRoads[x] == true) {
				surroundingRoadString += "1";
			} else {
				surroundingRoadString += "0";
			}
		}

		lookUp (surroundingRoadString, newPosition, out correctRoad);
	}

	private void checkRoadRight(Vector3 newPosition, out bool exists)
    {
		exists = false;
		for(int i = 0; i < roadPositions.Count; i++)
        {
			if(roadPositions[i] == new Vector3(newPosition.x + 1f, newPosition.y, newPosition.z))
            {
				exists = true;
			}
		}
	}

	private void checkRoadLeft(Vector3 newPosition, out bool exists)
    {
		exists = false;
		for(int i = 0; i < roadPositions.Count; i++)
        {
			if(roadPositions[i] == new Vector3(newPosition.x - 1f, newPosition.y, newPosition.z))
            {
				exists = true;
			}
		}
	}

	private void checkRoadDown(Vector3 newPosition, out bool exists)
    {

		exists = false;
		for(int i = 0; i < roadPositions.Count; i++) {
			if(roadPositions[i] == new Vector3(newPosition.x, newPosition.y, newPosition.z - 1f))
        {
				exists = true;
			}
		}
	}

	private void checkRoadUp(Vector3 newPosition, out bool exists)
    {
		exists = false;
		for(int i = 0; i < roadPositions.Count; i++)
        {
			if(roadPositions[i] == new Vector3(newPosition.x, newPosition.y, newPosition.z + 1f))
            {
				exists = true;
			}
		}
	}

	private void lookUp(string surroundingRoadString, Vector3 newPosition, out GameObject correctRoad)
    {
		GameObject correctRoadObject;
		Quaternion correctRoadRotation;
		roadObject.TryGetValue(surroundingRoadString, out correctRoadObject);
		roadRotation.TryGetValue(surroundingRoadString, out correctRoadRotation);
		correctRoad = Instantiate(correctRoadObject, newPosition, correctRoadRotation);
		correctRoad.tag = "road";
	}

	public void destroy()
    {
		// Debug.Log("Runs destroy:");
		GameObject[] allRoads;
		allRoads = GameObject.FindGameObjectsWithTag("road");
		for(int i = 0; i < allRoads.Length; i++)
        {
			Destroy(allRoads[i].gameObject);
		}
	}

	public void reDrawRoads()
    {
		// Debug.Log("Runs redraw:");
		GameObject correctRoad;
		for(int i = 0; i < roadPositions.Count; i++)
        {
			findCorrectRoad (roadPositions[i], out correctRoad);
		}
	}

    public bool CheckSurroundingRoads(Vector3 position)
    {
        return true;
    }

}
