using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour {

	ItemManager itemManager;
    HappinessManager happinessManager;

    List<ItemTracker> residentialTrackers;
    List<ItemTracker> emptyResidential;
    List<int> numEmptyResidential;

	List<ItemTracker> commercialTrackers;
    List<ItemTracker> emptyCommercial;
    List<int> numEmptyCommercial;

	List<ItemTracker> industrialTrackers;
	List<ItemTracker> emptyIndustrial;
	List<int> numEmptyIndustrial;

	float newPopulationSpawn;
	float populationIncreaseRate;
	float residentialDemand;
    float happiness;

    public int population;
    public int unallocatedPopulation;
    public int totalPopulation;
	public int unemployedPopulation;

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
		if(unemployedPopulation > AvailableJobs())
		{
			// itemTrackers allocate nearest jobs to their users
		}
	}

	void FindCommercialJobs(int num)
	{
		for (int i = 0; i < AbailableJobs(); i++)
		{
			if(unemployedPopulation > 0)
			{
				int numAdded = 0;
				if(unemployedPopulation > numEmptyResidential[i])
				{
					numAdded = numEmptyResidential[i];
				}
				else if(unallocatedPopulation <= numEmptyResidential[i])
				{
					numAdded = unallocatedPopulation;
				}
				population += numAdded;
				unallocatedPopulation -= numAdded;
				numEmptyResidential[i] -= numAdded;
				emptyResidential[i].AddUsers(numAdded);
			}
			else break;
		}
	}

	void FindIndustrialJobs(int num)
	{
		for (int i = 0; i < num; i++)
		{
			if(unemployedPopulation > 0)
			{
				int numAdded = 0;
				if(unemployedPopulation > numEmptyResidential[i])
				{
					numAdded = numEmptyResidential[i];
				}
				else if(unallocatedPopulation <= numEmptyResidential[i])
				{
					numAdded = unallocatedPopulation;
				}
				population += numAdded;
				unallocatedPopulation -= numAdded;
				numEmptyResidential[i] -= numAdded;
				emptyResidential[i].AddUsers(numAdded);
			}
			else break;
		}
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

    public void DeallocateUsers(int numUsers)
    // Adds users back to unallocatedPopulation for future reallocation
    {
        population -= numUsers;
        unallocatedPopulation += numUsers;
    }

    int AvailableResidential()
    // Returns amount of available housing
    {
		return residentialCap - population;
    }

	int AvailableJobs()
	{
		return commercialCap + industrialCap;
	}
}
