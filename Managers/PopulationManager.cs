using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour {


	DemandManager demandManager;
	ItemManager itemManager;
    HappinessManager happinessManager;
    List<ItemTracker> residentialTrackers;
    List<ItemTracker> vacantResidential;
    List<int> numVacancies;

	float newPopulationSpawn;
	float populationIncreaseRate;
	float residentialDemand;
    float happiness;

    public int population;
    public int unallocatedPopulation;
    public int totalPopulation;
    public int populationCap;

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
		demandManager = GameObject.Find("Managers").GetComponent<DemandManager>();
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
        populationCap = itemManager.residentialCap;
        totalPopulation = population + unallocatedPopulation;
        happiness = happinessManager.happiness;

        UpdateVacancies();
    }

    void UpdateVacancies()
    // Updates list of properties with vacancies
    {
        vacantResidential = null;
        if (residentialTrackers != null)
        {
            for (int i = 0; i < residentialTrackers.Count; i = i + 1)
            {
                if (residentialTrackers[i].NumVacancies() > 0)
                {
                    vacantResidential.Add(residentialTrackers[i]);
                    numVacancies.Add(residentialTrackers[i].NumVacancies());
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
            for (int i = 0; i < vacantResidential.Count; i += 1)
            {
                int numAdded = 0;
                if(unallocatedPopulation > numVacancies[i])
                {
                    numAdded = numVacancies[i];
                }
                else if(unallocatedPopulation < numVacancies[i] && unallocatedPopulation > 0)
                {
                    numAdded = unallocatedPopulation;
                }
                vacantResidential[i].AddUsers(numVacancies[i]);
                numVacancies[i] -= numAdded;
            }
        }
    }

    void TryIncreasePopulation()
    // Run the check to increase population
    {
        if (population < itemManager.getMaxPop())
        {
            IncreasePopulation();
        }
    }

    void IncreasePopulation()
    // Tries to increase population
    {
        happiness = happinessManager.happiness;
        residentialDemand += happiness * Time.deltaTime;
    if (residentialDemand >= 1 )
        {
            ++unallocatedPopulation;
            --residentialDemand;
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
