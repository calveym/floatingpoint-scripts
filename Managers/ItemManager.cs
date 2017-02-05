using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

	// Declare other managers
	EconomyManager economyManager;
	RoadGenerator roadGenerator;

    // Declare number of object types
	public List<GameObject> residential = new List<GameObject>();
	public List<GameObject> commercial = new List<GameObject>();
	List<GameObject> industrial = new List<GameObject>();
    public List<ItemTracker> residentialTrackers = new List<ItemTracker>();
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

	public int getNumRoads()
	// Returns number of roads on the map
	{
		return roadGenerator.roadPositions.Count;
	}

	public void addResidential(int capacity, GameObject newObject)
	// Adds a residential building to the total
	{
		residentialCap += (capacity);
		numResidential += 1;
		residential.Add(newObject);
	}

	public void addCommercial(int capacity, GameObject newObject)
	// Adds a commercial building to the total
	{
		commercialCap += (capacity);
		numCommercial += 1;
		commercial.Add(newObject);
    }

    public void addIndustrial(int capacity, GameObject newObject)
	// Adds an industrial building to the total
	{
		industrialCap += (capacity);
		numIndustrial += 1;
		industrial.Add(newObject);
    }

    public void addLeisure(int capacity, GameObject newObject)
    // Add a leisure building
	{
		leisureCap += (capacity);
		numLeisure += 1;
		leisure.Add(newObject);
	}

	public void addFoliage(int capacity, GameObject newObject)
    // Add foliage
	{
		foliageCap += (capacity);
		numFoliage += 1;
		foliage.Add(newObject);
	}

    public void removeResidential(GameObject removeObject)
    // Removes residential building
    {
        residentialCap -= removeObject.GetComponent<ItemTracker>().GetCapacity();
        removeObject.GetComponent<ItemTracker>().RemoveUsers();
        numResidential--;
        residential.Remove(removeObject);
    }

    public void removeCommercial(GameObject removeObject)
    // Removes commercial building
    {
        commercialCap -= removeObject.GetComponent<ItemTracker>().GetCapacity();
        removeObject.GetComponent<ItemTracker>().RemoveUsers();
        numCommercial--;
        commercial.Remove(removeObject);
    }

    public void removeIndustrial(GameObject removeObject)
    // Remove an industrial building
    {
        industrialCap -= removeObject.GetComponent<ItemTracker>().GetCapacity();
        numIndustrial--;
        industrial.Remove(removeObject);
    }

    public void removeLeisure(GameObject removeObject)
    // Remove a leisure building
    {
        leisureCap -= removeObject.GetComponent<ItemTracker>().GetCapacity();
        numLeisure--;
        leisure.Remove(removeObject);
    }

    public void removeFoliage(GameObject removeObject)
    // Remove foliage
    {
        foliageCap -= removeObject.GetComponent<ItemTracker>().GetCapacity();
        numLeisure--;
        leisure.Remove(removeObject);
    }

	public int getMaxPop()
	// Returns current maximum population
	{
		return residentialCap;
	}

}
