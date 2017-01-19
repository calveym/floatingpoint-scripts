using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

	// Declare other managers
	EconomyManager economyManager;
	RoadGenerator roadGenerator;

    // Declare number of object types
    GameObject[] residential;
    GameObject[] commercial;
    GameObject[] industrial;
    GameObject[] transport;
    GameObject[] leisure;
	int numResidential;
	int numCommercial;
	int numIndustrial;
	int numRoads;

	// Declare max capacity for object types
	public int commercialCap;
	public int residentialCap;
	public int industrialCap;

	void Awake ()
	// Get manager instances
	{
		economyManager = GameObject.Find("Managers").GetComponent<EconomyManager>();
		roadGenerator = GameObject.Find("Island").GetComponent<RoadGenerator>();
	}

	void Update ()
    // Updates state
	{
		numRoads = getNumRoads();
        residential = GameObject.FindGameObjectsWithTag("residential");
        commercial = GameObject.FindGameObjectsWithTag("commercial");
        industrial = GameObject.FindGameObjectsWithTag("industrial");
        transport = GameObject.FindGameObjectsWithTag("transport");
        leisure = GameObject.FindGameObjectsWithTag("leisure");
	}

	public int getNumRoads()
	// Returns number of roads on the map
	{
		return(roadGenerator.roadPositions.Count);
	}

	public void addResidential(int capacity)
	// Adds a residential building to the total
	{
		numResidential += 1;
		residentialCap += capacity;
	}

	public void addCommercial(int capacity)
	// Adds a commercial building to the total
	{
		numCommercial += 1;
		commercialCap += capacity;
	}

	public void addIndustrial(int capacity)
	// Adds an industrial building to the total
	{
		numIndustrial += 1;
		industrialCap += 1;
	}

	public int getMaxPop()
	// Returns current maximum population
	{
		return residentialCap;
	}
}
