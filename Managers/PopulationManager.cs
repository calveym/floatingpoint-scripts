using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour {

	ItemManager itemManager;
    HappinessManager happinessManager;

    List<ResidentialTracker> residentialTrackers;
    List<ResidentialTracker> emptyResidential; // List of residential trackers with vacancies
    List<int> numEmptyResidential; // Number of vacancies in each residential building
	List<ResidentialTracker> residentialWithUnemployed;
	list<int> numResidentialWithUnemployed;

	List<CommercialTracker> commercialTrackers;

	List<IndustrialTracker> industrialTrackers;

	float newPopulationSpawn;
	float populationIncreaseRate;
	float residentialDemand;
    float happiness;

    public int population; // Population that are housed
    public int unallocatedPopulation; // Population that are not housed
    public int totalPopulation; // Total population
	public int unemployedPopulation; // Unemployed population- subset of "population"

    int residentialCap;
    int industrialCap;
    int commercialCap;

	void Awake ()
	// Set values pre-initialization
	{
		population = 0;
		populationIncreaseRate = 0;

		itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();
        happinessManager = GameObject.Find("Managers").GetComponent<HappinessManager>();
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
		UpdateEmptyCommercial();
		UpdateEmptyIndustrial();
    }

    void UpdateEmptyResidential()
    // Updates list of properties with vacancies
    {
        emptyResidential = new List<ItemTracker>();
        numEmptyResidential = new List<int>();
        if (residentialTrackers != null)
        {
            for (int i = 0; i < residentialTrackers.Count; i++)
            {
                if (residentialTrackers[i].NumEmpty() > 0 && residentialTrackers[i].usable == true)
                {
                    emptyResidential.Add(residentialTrackers[i]);
                    numEmptyResidential.Add(residentialTrackers[i].NumEmpty());
                }
            }
        }
    }

	void UpdateEmptyCommercial()
    // Updates list of properties with vacancies
    {
        emptyCommercial = new List<ItemTracker>();
        numEmptyCommercial = new List<int>();
        if (commercialTrackers != null)
        {
            for (int i = 0; i < commercialTrackers.Count; i++)
            {
                if (commercialTrackers[i].NumEmptyCommercial() > 0 && commercialTrackers[i].usable == true)
                {
                    emptyCommercial.Add(commercialTrackers[i]);
                    numEmptyCommercial.Add(commercialTrackers[i].NumEmptyCommercial());
                }
            }
        }
    }

	void UpdateEmptyIndustrial()
    // Updates list of properties with vacancies
    {
        emptyIndustrial = new List<ItemTracker>();
        numEmptyIndustrial = new List<int>();
        if (industrialTrackers != null)
        {
            for (int i = 0; i < industrialTrackers.Count; i++)
            {
                if (industrialTrackers[i].NumEmptyIndustrial() > 0 && industrialTrackers[i].usable == true)
                {
                    emptyIndustrial.Add(industrialTrackers[i]);
                    numEmptyIndustrial.Add(industrialTrackers[i].NumEmptyIndustrial());
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
                emptyResidential[i].AddUsers(numAdded);
            }
        }
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
		if(unemployedPopulation >= AvailableJobs())
		{
			for(int i; i < unemployedPopulation; i++)
			{
				// TODO: a thing
			}
		}
		else
		{
			for(int i; i < AvailableJobs; i++)
			{
				// TODO: a thing
			}
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

    int AvailableResidential()
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
