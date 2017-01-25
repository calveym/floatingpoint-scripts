using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

	// Declare other managers
	EconomyManager economyManager;
	RoadGenerator roadGenerator;

    // Declare number of object types
	List<GameObject> residential = new List<GameObject>();
	List<GameObject> commercial = new List<GameObject>();
	List<GameObject> industrial = new List<GameObject>();
	List<GameObject> leisure = new List<GameObject>();
	List<GameObject> transport = new List<GameObject>();
	List<GameObject> foliage = new List<GameObject>();

	int numResidential;
	int numCommercial;
	int numIndustrial;
	int numLeisure;
	int numRoads;
	int numFoliage;

	// Declare max capacity for object types
	public int commercialCap;
	public int residentialCap;
	public int industrialCap;
	public int transportCap;
	public int leisureCap;
	public int foliageCap;

	void Awake ()
	// Get manager instances
	{
		economyManager = GameObject.Find("Managers").GetComponent<EconomyManager>();
		roadGenerator = GameObject.Find("Island").GetComponent<RoadGenerator>();
	}

	void Update ()
    // Updates state
	{
		CheckOK();
	}

	void CheckOK()
	// TODO: add check to confirm that all objects are being tracked, update object arrays if any errors found
	{
	}

	void GetNumObjects()
	// uses gameobject arrays to calculate number of each tag type
	{
		numRoads = getNumRoads();
		numResidential = residential.Count;
		numCommercial = commercial.Count;
		numIndustrial = industrial.Count;
		numLeisure = leisure.Count;
	}

	public int getNumRoads()
	// Returns number of roads on the map
	{
		return(roadGenerator.roadPositions.Count);
	}

	public void addResidential(int capacity, GameObject newObject)
	// Adds a residential building to the total
	{
		residentialCap += capacity;
		numResidential += 1;
		residential.Add(newObject);
	}

	public void addCommercial(int capacity, GameObject newObject)
	// Adds a commercial building to the total
	{
		commercialCap += capacity;
		numCommercial += 1;
		commercial.Add(newObject);
	}

	public void addIndustrial(int capacity, GameObject newObject)
	// Adds an industrial building to the total
	{
		industrialCap += capacity;
		numIndustrial += 1;
		industrial.Add(newObject);
	}

	public void addLeisure(int capacity, GameObject newObject)
	{
		leisureCap += capacity;
		numLeisure += 1;
		leisure.Add(newObject);
	}

	public void addFoliage(int capacity, GameObject newObject)
	{
		foliageCap += capacity;
		numFoliage += 1;
		foliage.Add(newObject);
	}

	public int getMaxPop()
	// Returns current maximum population
	{
		return residentialCap;
	}
}
