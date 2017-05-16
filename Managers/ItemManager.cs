using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    // Declare other managers
    PopulationManager populationManager;
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

    public delegate void FiveSecondUpdate();
    public static FiveSecondUpdate fiveUpdate;

    public delegate void TenSecondUpdate();
    public static TenSecondUpdate tenUpdate;

    void Awake()
    {

    }

    void Start ()
	// Get manager instances
	{
        fiveUpdate += Empty;
        tenUpdate += Empty;
		economyManager = GameObject.Find("Managers").GetComponent<EconomyManager>();
        populationManager = GameObject.Find("Managers").GetComponent<PopulationManager>();
		roadGenerator = GameObject.Find("Island").GetComponent<RoadGenerator>();
        // contractmanager = GameObject.Find("Managers").GetComponent<ContractManager>();
        StartCoroutine("FiveUpdate");
        StartCoroutine("TenUpdate");
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
        ResidentialTracker temp = removeObject.GetComponent<ResidentialTracker>();
        residentialCap -= removeObject.GetComponent<ResidentialTracker>().GetCapacity();
        temp.RemoveAllUsers();
        populationManager.DeallocateUsers(temp.users, "residential");
        numResidential--;
        residential.Remove(removeObject);
    }

    public void removeCommercial(GameObject removeObject)
    // Removes commercial building
    {
        CommercialTracker temp = removeObject.GetComponent<CommercialTracker>();
        commercialCap -= temp.capacity;
        temp.RemoveAllUsers();
        populationManager.DeallocateUsers(temp.users, "commercial");
        numCommercial--;
        commercial.Remove(removeObject);
    }

    public void removeIndustrial(GameObject removeObject)
    // Remove an industrial building
    {
        IndustrialTracker temp = removeObject.GetComponent<IndustrialTracker>();
        industrialCap -= temp.capacity;
        temp.RemoveAllUsers();
        populationManager.DeallocateUsers(temp.users, "commercial");
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
        foliageCap -= (int)removeObject.GetComponent<HappinessAffector>().affectAmount;
        numLeisure--;
        leisure.Remove(removeObject);
    }

	public int getMaxPop()
	// Returns current maximum population
	{
		return residentialCap;
	}

    void Empty()
    {
    }

    IEnumerator FiveUpdate()
    {
        while(true)
        {
            fiveUpdate();
            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator TenUpdate()
    {
        while(true)
        {
            tenUpdate();
            yield return new WaitForSeconds(10);
        }
    }
}
