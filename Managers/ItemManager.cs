using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

	// Declare other managers
	EconomyManager economyManager;
	RoadGenerator roadGenerator;

	// Declare number of object types
	int numResidential;
	int numCommercial;
	int numIndustrial;
	int numRoads;

	// Declare max capacity for object types
	int commercialCap;
	int residentialCap;
	int industrialCap;


	void Awake ()
	// Get manager instances
	{
		economyManager = GameObject.Find("Managers").GetComponent<EconomyManager>();
		roadGenerator = GameObject.Find("Island").GetComponent<RoadGenerator>();
	}

	void Update ()
	{
		numRoads = getNumRoads();
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
