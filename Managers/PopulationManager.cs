using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour {

	ItemManager itemManager;
    HappinessManager happinessManager;
    List<ItemTracker> residentialTrackers;
    List<ItemTracker> emptyResidential;
    List<int> numEmptyResidential;

	float newPopulationSpawn;
	float populationIncreaseRate;
	float residentialDemand;
    float happiness;

    public int population;
    public int unallocatedPopulation;
    public int totalPopulation;

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
        residentialTrackers = itemManager.residentialTrackers;
        totalPopulation = population + unallocatedPopulation;
        happiness = happinessManager.happiness;

        UpdateVacancies();
    }

    void UpdateVacancies()
    // Updates list of properties with vacancies
    {
        emptyResidential = new List<ItemTracker>();
        numEmptyResidential = new List<int>();
        if (residentialTrackers != null)
        {
            for (int i = 0; i < residentialTrackers.Count; i++)
            {
                if (residentialTrackers[i].NumVacancies() > 0 && residentialTrackers[i].usable == true)
                {
                    emptyResidential.Add(residentialTrackers[i]);
                    numEmptyResidential.Add(residentialTrackers[i].NumVacancies());
                }
            }
        }
    }

    void RunChecks()
    // Runs all checks
    {
        TryIncreasePopulation();
        TryAllocateUsers();
    }

    void TryAllocateUsers()
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
				//GameObject.Find("Managers").GetComponent<ContractManager>().incrementPopulation(numAdded);
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

    public int AvailableResidential()
    // Returns amount of available housing
    {
        return residentialCap - population;
    }
}
