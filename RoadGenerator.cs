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
    private RaycastHit hit;

	Dictionary<string, GameObject> roadObject; // Contains surroundingRoadString - object mapping
	Dictionary<string, Quaternion> roadRotation; // Contains surroundingRoadString - rotation mapping

	public Dictionary<Vector3, GameObject> roads; // Dictionary of all road positions and objects
	public Dictionary<Vector3, string> surroundingRoads; // Dictionary of all road positions and associated surroundingRoadString

	void Start()
	// Initiates all of the dictionaries for lookups and all other variables that need to be initialized
	{
		roads = new Dictionary<Vector3, GameObject>();
		surroundingRoads = new Dictionary<Vector3, string>();

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

	public override void StartUsing (GameObject usingObject)
	// Runs when object is used by vrtk controller
	{
		base.StartUsing (usingObject);
		if (Physics.Raycast (controller.transform.position, controller.transform.forward, out hit, 1000.0f))
        {
			Vector3 rounded = Round(hit.point);

            GameObject roundedValue;
            roads.TryGetValue(rounded, out roundedValue);
            if (roundedValue == null)
			{
				DrawRoad(rounded);
				RedrawLocalRoads(rounded);
			}
		}
	}

	void DrawRoad(Vector3 position)
	// Handles road creation process
	{
		roads.Add(position, CreateRoadObject(position));
		surroundingRoads.Add(position, CheckSurroundingRoads(position));
	}

	public void RedrawLocalRoads(Vector3 position)
	// Redraw only the surrounding roads
	{
		if(CheckRoadRight(position) == "1")
		{
			ConfirmRoad(new Vector3(position.x + 1f, position.y, position.z));
		}
		if (CheckRoadLeft(position) == "1")
		{
			ConfirmRoad(new Vector3(position.x - 1f, position.y, position.z));
		}
		if (CheckRoadUp(position) == "1")
		{
			ConfirmRoad(new Vector3(position.x, position.y, position.z + 1f));
		}
		if (CheckRoadDown(position) == "1")
		{
			ConfirmRoad(new Vector3(position.x, position.y, position.z - 1f));
		}
	}

	void ConfirmRoad(Vector3 position)
	// Redraw road if surroundings change
	{
        string oldSurroundingString;
        surroundingRoads.TryGetValue(position, out oldSurroundingString);
		if(oldSurroundingString != CheckSurroundingRoads(position))
		{
			RemoveRoad(position);
			DrawRoad(position);
		}
	}

	GameObject CreateRoadObject(Vector3 newPosition)
	// Creates road instance using surrounding road check
  {
		string surroundingRoadString = CheckSurroundingRoads(newPosition);
		return InstantiateRoad (surroundingRoadString, newPosition);
	}

	string CheckSurroundingRoads(Vector3 newPosition)
	// Returns string in the format "1111" - "0000" reflecting surrounding roads
	{
        return CheckRoadUp(newPosition) + CheckRoadDown(newPosition) + CheckRoadLeft(newPosition) + CheckRoadRight(newPosition);
	}

	string CheckRoadRight(Vector3 newPosition)
	// Returns 1 if road directly right exists
	{
        GameObject returnObject;
        string returnValue = roads.TryGetValue(new Vector3(newPosition.x + 1f, newPosition.y, newPosition.z), out returnObject) ? "1" : "0";
        return returnValue;
	}

	string CheckRoadLeft(Vector3 newPosition)
	// Returns 1 if road directly left exists
	{
        GameObject returnObject;
        string returnValue = roads.TryGetValue(new Vector3(newPosition.x - 1f, newPosition.y, newPosition.z), out returnObject) ? "1" : "0";
        return returnValue;
    }

	string CheckRoadDown(Vector3 newPosition)
	// Returns 1 if road directly down exists
	{
        GameObject returnObject;
        string returnValue = roads.TryGetValue(new Vector3(newPosition.x, newPosition.y, newPosition.z - 1f), out returnObject) ? "1" : "0";
        return returnValue;
    }

	string CheckRoadUp(Vector3 newPosition)
	// Returns 1 if road directly up exists
	{
        GameObject returnObject;
        string returnValue = roads.TryGetValue(new Vector3(newPosition.x, newPosition.y, newPosition.z + 1f), out returnObject) ? "1" : "0";
        return returnValue;
    }

	GameObject InstantiateRoad(string surroundingRoadString, Vector3 newPosition)
	// Uses surrounding road information to instantiate a find a new road
    {
        GameObject correctRoad;
		GameObject correctRoadObject;
		Quaternion correctRoadRotation;
		roadObject.TryGetValue(surroundingRoadString, out correctRoadObject);
		roadRotation.TryGetValue(surroundingRoadString, out correctRoadRotation);
		correctRoad = Instantiate(correctRoadObject, newPosition, correctRoadRotation);
		correctRoad.tag = "road";
		return correctRoad;
	}

	bool V3Equal(Vector3 a, Vector3 b)
	// Returns true if two input vector3s are equal to within set tolerance
    {
		return Vector3.SqrMagnitude(a - b) < 3.162f;
	}

	public void RemoveRoad(Vector3 position)
	// Handles removing road at position
    {
        GameObject destroyRoad;
        roads.TryGetValue(position, out destroyRoad);
        Debug.Log(position);
		roads.Remove(position);
		surroundingRoads.Remove(position);
        Destroy(destroyRoad);
	}

	public Vector3 Round (Vector3 point)
	// Rounds to nearest 0.1f
    {
		Vector3 newPosition = new Vector3(Mathf.Round(point.x),
			10.01f,
			Mathf.Round(point.z));
		return newPosition;
	}
}
