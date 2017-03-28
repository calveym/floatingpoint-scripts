using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

	// Declare other managers
	EconomyManager economyManager;
	RoadGenerator roadGenerator;
	ContractManager contractManager;

  // Declare number of object types
	public List<GameObject> residential = new List<GameObject>();
	public List<GameObject> commercial = new List<GameObject>();
	public List<GameObject> industrial = new List<GameObject>();
	public List<GameObject> leisure = new List<GameObject>();
	public List<GameObject> transport = new List<GameObject>();
	public List<GameObject> foliage = new List<GameObject>();
    public List<ResidentialTracker> residentialTrackers = new List<ResidentialTracker>();
	public List<CommercialTracker> commercialTrackers = new List<CommercialTracker>();
	public List<IndustrialTracker> industrialTrackers = new List<IndustrialTracker>();

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
		// contractmanager = GameObject.Find("Managers").GetComponent<ContractManager>();
	}

	public int getNumRoads()
	// Returns number of roads on the map
	{
		return roadGenerator.numRoads;
	}

	public void addResidential(int capacity, GameObject newObject)
	// Adds a residential building to the total
	{
		residentialCap += (capacity);
		numResidential += 1;
		residential.Add(newObject);
		residentialTrackers.Add(newObject.GetComponent<ResidentialTracker>());
	}

	public void addCommercial(int capacity, GameObject newObject)
	// Adds a commercial building to the total
	{
		commercialCap += (capacity);
		numCommercial += 1;
		commercial.Add(newObject);
		commercialTrackers.Add(newObject.GetComponent<CommercialTracker>());
    }

    public void addIndustrial(int capacity, GameObject newObject)
	// Adds an industrial building to the total
	{
		industrialCap += (capacity);
		numIndustrial += 1;
		industrial.Add(newObject);
		industrialTrackers.Add(newObject.GetComponent<IndustrialTracker>());
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
      removeObject.GetComponent<ItemTracker>().RemoveAllUsers();
      numResidential--;
      residential.Remove(removeObject);
  }

  public void removeCommercial(GameObject removeObject)
  // Removes commercial building
  {
      commercialCap -= removeObject.GetComponent<ItemTracker>().GetCapacity();
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
