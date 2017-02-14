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

	private string surroundingRoads;
	private RaycastHit hit;
	private Vector3 newPosition;
	private bool up;
	private bool down;
	private bool left;
	private bool right;
	private Dictionary<string, GameObject> roadObject;
	private Dictionary<string, Quaternion> roadRotation;

	public List<Vector3> roadPositions;

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
	}

	public void StartUsing (GameObject usingObject) {
		base.StartUsing (usingObject);
		if (Physics.Raycast (controller.transform.position, controller.transform.forward, out hit, 1000.0f)) {
			roadPositions.Add(Round(hit.point));
			destroy();
			redrawRoads();
		}
	}

	public Vector3 Round (Vector3 point)
    {
		newPosition = new Vector3(Mathf.Round(point.x * 100) / 100,
			10.01f,
			Mathf.Round(point.z * 100) / 100);
		return newPosition;
	}

	private GameObject findCorrectRoad(Vector3 newPosition)
    {
		surroundingRoads = checkRoadUp(newPosition) + checkRoadDown(newPosition) + checkRoadLeft(newPosition) + checkRoadRight(newPosition);
		return lookUp (surroundingRoadString, newPosition);
	}

	private bool checkRoadRight(Vector3 newPosition)
	{
		return (roadPositions.Contains(new Vector3(newPosition.x + 1f, newPosition.y, newPosition.z)) ? "1" : "0"
	}

	private bool checkRoadLeft(Vector3 newPosition)
	{
		return (roadPositions.Contains(new Vector3(newPosition.x - 1f, newPosition.y, newPosition.z)) ? "1" : "0"
	}

	private bool checkRoadDown(Vector3 newPosition)
	{
		return (roadPositions.Contains(new Vector3(newPosition.x, newPosition.y, newPosition.z - 1f)) ? "1" : "0"
	}

	private bool checkRoadUp(Vector3 newPosition)
	{
		return (roadPositions.Contains(new Vector3(newPosition.x, newPosition.y, newPosition.z + 1f)) ? "1" : "0"
	}


	private GameObject lookUp(string surroundingRoadString, Vector3 newPosition)
  {
		GameObject correctRoadObject;
		Quaternion correctRoadRotation;
		roadObject.TryGetValue(surroundingRoadString, out correctRoadObject);
		roadRotation.TryGetValue(surroundingRoadString, out correctRoadRotation);
		correctRoad = Instantiate(correctRoadObject, newPosition, correctRoadRotation);
		correctRoad.tag = "road";
		return correctRoad;
	}

	public bool V3Equal(Vector3 a, Vector3 b)
  {
		return Vector3.SqrMagnitude(a - b) < 3.162f;
	}

	public void removeRoad(Vector3 position)
  {
		roadPositions.Remove(position);
	}

	public void destroy()
  {
		GameObject[] allRoads;
		allRoads = GameObject.FindGameObjectsWithTag("road");
		for(int i = 0; i < allRoads.Length; i++)
        {
			Destroy(allRoads[i].gameObject);
		}
	}

	public void redrawRoads()
  {
		for(int i = 0; i < roadPositions.Count; i++)
        {
			findCorrectRoad (roadPositions[i]);
		}
	}

	public void redraw
}
