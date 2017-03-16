using System;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour {

	ItemManager itemManager;
    HappinessManager happinessManager;
    NameGenerator names;

    List<ResidentialTracker> residentialTrackers;
    List<ResidentialTracker> emptyResidential; // List of residential trackers with vacancies
    List<int> numEmptyResidential; // Number of vacancies in each residential building
	List<ResidentialTracker> residentialWithUnemployed;
	List<int> numResidentialWithUnemployed;

	List<CommercialTracker> commercialTrackers;
    public List<CommercialTracker> emptyCommercial;

	List<IndustrialTracker> industrialTrackers;
    public List<IndustrialTracker> emptyIndustrial;

	float newPopulationSpawn;
	float populationIncreaseRate;
	float residentialDemand;
    float happiness;
    List<string> firstNames;
    List<string> lastNames;

    public int population; // Population that are housed
    public int unallocatedPopulation; // Population that are not housed
    public int totalPopulation; // Total population
	public int unemployedPopulation; // Unemployed population- subset of "population"

    int residentialCap;
    int industrialCap;
    int commercialCap;

    void Awake()
    {
        names = gameObject.GetComponent<NameGenerator>();
        itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();
        happinessManager = GameObject.Find("Managers").GetComponent<HappinessManager>();
        population = 0;
        populationIncreaseRate = 0;
    }

    void Start ()
	// Set values pre-initialization
	{
        firstNames = names.FirstNames();
        lastNames = names.LastNames();
	}

	void Update ()
	// Updates values & calls increasePopulation if conditions are met
	{
        UpdateValues();
        RunChecks();
	}

    void UpdateValues()
    // Retrieves necessary values from other managers
    {
        residentialCap = itemManager.residentialCap;
		commercialCap = itemManager.commercialCap;
		industrialCap = itemManager.industrialCap;
        residentialTrackers = itemManager.residentialTrackers;
		commercialTrackers = itemManager.commercialTrackers;
		industrialTrackers = itemManager.industrialTrackers;
        totalPopulation = population + unallocatedPopulation;
        happiness = happinessManager.happiness;

        UpdateEmptyResidential();
        UpdateCommercialTrackers();
        UpdateIndustrialTrackers();
    }

    void UpdateEmptyResidential()
    // Updates list of properties with vacancies
    {
        emptyResidential = new List<ResidentialTracker>();
        numEmptyResidential = new List<int>();
        residentialWithUnemployed = new List<ResidentialTracker>();
        numResidentialWithUnemployed = new List<int>();

        if (residentialTrackers != null)
        {
            for (int i = 0; i < residentialTrackers.Count; i++)
            {
                if (residentialTrackers[i].NumEmpty() > 0 && residentialTrackers[i].usable == true)
                {
                    emptyResidential.Add(residentialTrackers[i]);
                    numEmptyResidential.Add(residentialTrackers[i].NumEmpty());
                }
                if(residentialTrackers[i].unemployedPopulation > 0)
                {
                    residentialWithUnemployed.Add(residentialTrackers[i]);
                    numResidentialWithUnemployed.Add(residentialTrackers[i].unemployedPopulation);
                }
            }
        }
    }

    void UpdateCommercialTrackers()
    {
        if(commercialTrackers != null)
        {
            for(int i = 0; i < commercialTrackers.Count; i++)
            {
                if(commercialTrackers[i].NumEmpty() > 0)
                {
                    emptyCommercial.Add(commercialTrackers[i]);
                }
            }
        }
    }

    void UpdateIndustrialTrackers()
    {
        if(industrialTrackers != null)
        {
            for(int i = 0; i < industrialTrackers.Count; i++)
            {
                if(industrialTrackers[i].NumEmpty() > 0)
                {
                    emptyIndustrial.Add(industrialTrackers[i]);
                }
            }
        }
    }
    
    void RunChecks()
    // Runs all checks
    {
        TryIncreasePopulation();
        TryAllocatePopulation();
		TryFindJob();
    }

    void TryAllocatePopulation()
    // Run the check to allocate users
    {
        if(unallocatedPopulation > 0 && AvailableResidential() > 0)
        {
            for (int i = 0; i < emptyResidential.Count; i++)
            {
                int numAdded = 0;
                if(unallocatedPopulation > numEmptyResidential[i])
                {
                    numAdded = numEmptyResidential[i];
                }
                else if(unallocatedPopulation <= numEmptyResidential[i])
                {
                    numAdded = unallocatedPopulation;
                }
                population += numAdded;
				unemployedPopulation += numAdded;
                unallocatedPopulation -= numAdded;
                numEmptyResidential[i] -= numAdded;
                emptyResidential[i].AddUsers(numAdded, GenerateNames(numAdded), 20);
            }
        }
    }

    List<string> GenerateNames(int numAdded)
    {
        List<string> names = new List<string>();
        for(int i = 0; i < numAdded; i++)
        {
            System.Random r = new System.Random();
            int firstIndex = r.Next(names.Count);
            int secondIndex = r.Next(names.Count);
            string createdName = firstNames[firstIndex] + lastNames[secondIndex];
            names.Add(createdName);
        }
        return names;
    }

    void TryIncreasePopulation()
    // Run the check to increase population
    {
        if (totalPopulation < itemManager.getMaxPop())
        {
            IncreasePopulation();
        }
    }

	void TryFindJob()
	// TODO: allocate to either industrial or commercial, select type first, then allocate
	{
		if(unemployedPopulation > 0 && AvailableJobs() > 0)
		{
			FindJob();
		}
	}

	void FindJob()
	{
		for(int i = 0; i < residentialWithUnemployed.Count; i++)
        {
            residentialWithUnemployed[i].TryEmployWorker();
        }
		// itemTrackers allocate nearest jobs to their users
	}

    void IncreasePopulation()
    // Tries to increase population
    {
        happiness = happinessManager.happiness;
        residentialDemand += happiness * Time.deltaTime * 0.05f;
        if (residentialDemand >= 1 )
        {
            unallocatedPopulation++;
            residentialDemand--;
        }
    }

    public void DeallocateUsers(int numUsers, string type)
    // Adds users back to unallocatedPopulation for future reallocation
    {
		if(type == "res")
		{
	        population -= numUsers;
	        unallocatedPopulation += numUsers;
		}
		else if(type == "ind" || type == "com")
		{

		}
    }

    public int AvailableResidential()
    // Returns amount of available housing
    {
		return residentialCap - population;
    }

	int AvailableJobs()
	// Returns total number of available jobs
	{
		return commercialCap + industrialCap;
	}
}
