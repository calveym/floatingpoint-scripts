using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autelia.Serialization;

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
    

    void Awake()
    {

    }

    void Start ()
    // Get manager instances
    {
        economyManager = GameObject.Find("Managers").GetComponent<EconomyManager>();
        populationManager = GameObject.Find("Managers").GetComponent<PopulationManager>();
        roadGenerator = GameObject.Find("Island").GetComponent<RoadGenerator>();
        if (Serializer.IsLoading)
        {
            ResetItems();
            return;
        }
	}

    public void AddTrackersToEcoTick()
    {
        foreach(ResidentialTracker res in residentialTrackers)
        {
            res.AddEcoTick();
        }
        foreach(CommercialTracker com in commercialTrackers)
        {
            com.AddEcoTick();
        }
    }

    public void ResetItems()
    {
        Autelia.Coroutines.CoroutineController.StartCoroutine(this, "ResetItemRoutine");
    }

    void GetResidential()
    {
        residentialTrackers = new List<ResidentialTracker>();
        residential = new List<GameObject>();
        ResidentialTracker[] residentials = GameObject.FindObjectsOfType<ResidentialTracker>();
        foreach(ResidentialTracker res in residentials)
        {
            residentialTrackers.Add(res);
            residential.Add(res.gameObject);
        }
        residentialCap = GetCapacity(residentialTrackers);
    }

    void GetCommercial()
    {
        commercialTrackers = new List<CommercialTracker>();
        commercial = new List<GameObject>();
        CommercialTracker[] commercials = GameObject.FindObjectsOfType<CommercialTracker>();
        foreach(CommercialTracker com in commercials)
        {
            commercialTrackers.Add(com);
            commercial.Add(com.gameObject);
        }
        commercialCap = GetCapacity(commercialTrackers);
    }

    void GetIndustrial()
    {
        industrialTrackers = new List<IndustrialTracker>();
        industrial = new List<GameObject>();
        IndustrialTracker[] industrials = GameObject.FindObjectsOfType<IndustrialTracker>();
        foreach(IndustrialTracker ind in industrials)
        {
            industrialTrackers.Add(ind);
            industrial.Add(ind.gameObject);
        }
        industrialCap = GetCapacity(industrialTrackers);
    }

    int GetCapacity(List<ResidentialTracker> items)
    {
        int returnValue = 0;
        foreach(ResidentialTracker item in items)
        {
            returnValue += item.capacity;
        }
        return returnValue;
    }

    int GetCapacity(List<CommercialTracker> items)
    {
        int returnValue = 0;
        foreach (CommercialTracker item in items)
        {
            returnValue += item.capacity;
        }
        return returnValue;
    }

    int GetCapacity(List<IndustrialTracker> items)
    {
        int returnValue = 0;
        foreach (IndustrialTracker item in items)
        {
            returnValue += item.capacity;
        }
        return returnValue;
    }

    public int getNumRoads()
	// Returns number of roads on the map
	{
		return roadGenerator.numRoads;
	}

	public void addResidential(int capacity, GameObject newObject)
	// Adds a residential building to the total
	{
        if(!residential.Contains(newObject))
        {
            residentialCap += (capacity);
            numResidential += 1;
            residential.Add(newObject);
            residentialTrackers.Add(newObject.GetComponent<ResidentialTracker>());
        }
	}

	public void addCommercial(int capacity, GameObject newObject)
	// Adds a commercial building to the total
	{
        if(!commercial.Contains(newObject))
        {
            commercialCap += (capacity);
            numCommercial += 1;
            commercial.Add(newObject);
            commercialTrackers.Add(newObject.GetComponent<CommercialTracker>());
        }
    }

    public void addIndustrial(int capacity, GameObject newObject)
	// Adds an industrial building to the total
	{
        if(!industrial.Contains(newObject))
        {
            industrialCap += (capacity);
            numIndustrial += 1;
            industrial.Add(newObject);
            industrialTrackers.Add(newObject.GetComponent<IndustrialTracker>());
        }
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
        Debug.Log("Removing...");
        ResidentialTracker temp = removeObject.GetComponent<ResidentialTracker>();
        EconomyManager.ecoTick -= temp.UpdateSecond;
        residentialCap -= removeObject.GetComponent<ResidentialTracker>().GetCapacity();
        temp.RemoveAllUsers();
        populationManager.DeallocateUsers(temp.users, "residential");
        numResidential--;
        economyManager.IssueRefund(temp.buyCost);
        residential.Remove(removeObject);
    }

    public void removeCommercial(GameObject removeObject)
    // Removes commercial building
    {
        CommercialTracker temp = removeObject.GetComponent<CommercialTracker>();
        EconomyManager.ecoTick -= temp.UpdateSecond;
        commercialCap -= temp.capacity;
        temp.RemoveAllUsers();
        populationManager.DeallocateUsers(temp.users, "commercial");
        numCommercial--;
        economyManager.IssueRefund(temp.buyCost);
        commercial.Remove(removeObject);
    }

    public void removeIndustrial(GameObject removeObject)
    // Remove an industrial building
    {
        IndustrialTracker temp = removeObject.GetComponent<IndustrialTracker>();
        EconomyManager.ecoTick -= temp.UpdateSecond;
        industrialCap -= temp.capacity;
        temp.RemoveAllUsers();
        populationManager.DeallocateUsers(temp.users, "commercial");
        numIndustrial--;
        economyManager.IssueRefund(temp.buyCost);
        industrial.Remove(removeObject);
    }

    public void removeLeisure(GameObject removeObject)
    // Remove a leisure building
    {
        leisureCap -= removeObject.GetComponent<ItemTracker>().GetCapacity();
        numLeisure--;
        economyManager.IssueRefund(100);
        leisure.Remove(removeObject);
    }

    public void removeFoliage(GameObject removeObject)
    // Remove foliage
    {
        foliageCap -= (int)removeObject.GetComponent<HappinessAffector>().affectAmount;
        numLeisure--;
        economyManager.IssueRefund(100);
        leisure.Remove(removeObject);
    }

	public int GetMaxPop()
	// Returns current maximum population
	{
		return residentialCap;
	}

    public int GetMaxJobs()
    {
        return industrialCap + commercialCap;
    }

    void Empty()
    {
    }

    public IEnumerator ResetItemRoutine()
    {
        while(Serializer.IsLoading)
            yield return new WaitForSeconds(1f);
        GetResidential();
        GetCommercial();
        GetIndustrial();
    }
}
