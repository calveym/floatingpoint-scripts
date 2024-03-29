using System;
using System.Collections.Generic;
using UnityEngine;
using Autelia.Serialization;

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


    public float resUpdate;
    public float comUpdate;
    public float indUpdate;

    float happiness;
    List<string> firstNames;
    List<string> lastNames;

    public int population; // Population that are housed
    public int unallocatedPopulation; // Population that are not housed
    public int totalPopulation; // Total population
	public int unemployedPopulation; // Unemployed population- subset of "population", meaning they must be housed first to enter this category

    int residentialCap;
    int industrialCap;
    int commercialCap;

    void Awake()
    {
        population = 0;
        resUpdate = 0;
        indUpdate = 0;
        comUpdate = 0;
    }

    void Start ()
	// Set values pre-initialization
	{if (Serializer.IsLoading)	return;
        names = gameObject.GetComponent<NameGenerator>();
        firstNames = names.FirstNames();
        lastNames = names.LastNames();
        itemManager = ReferenceManager.instance.itemManager;
        happinessManager = ReferenceManager.instance.happinessManager;
        EconomyManager.ecoTick += PopulationUpdate;

        QueueUpdates();
    }

    public void PopulationUpdate ()
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
        resUpdate = 0;
        emptyResidential = new List<ResidentialTracker>();
        numEmptyResidential = new List<int>();
        residentialWithUnemployed = new List<ResidentialTracker>();
        numResidentialWithUnemployed = new List<int>();

        if (residentialTrackers != null)
        {
            foreach(ResidentialTracker residentialTracker in residentialTrackers)
            {
                // First add empty users
                if (residentialTracker.NumEmpty() > 0 && residentialTracker.usable == true)
                {
                    emptyResidential.Add(residentialTracker);
                    numEmptyResidential.Add(residentialTracker.NumEmpty());
                }
                // Then add employment
                if(residentialTracker.unemployedPopulation > 0)
                {
                    residentialWithUnemployed.Add(residentialTracker);
                    numResidentialWithUnemployed.Add(residentialTracker.unemployedPopulation);
                }
            }
        }
    }

    public void QueueUpdates()
    {
        resUpdate++;
        indUpdate++;
        comUpdate++;
    }

    void UpdateCommercialTrackers()
    {
        comUpdate = 0;
        emptyCommercial = new List<CommercialTracker>();
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
        indUpdate = 0;
        emptyIndustrial = new List<IndustrialTracker>();
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
    }

    void TryAllocatePopulation()
    // Run the check to allocate users
    {
        if(unallocatedPopulation > 0 && AvailableResidential() > 0 && emptyResidential != null)
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
                resUpdate++;
                population += numAdded;
				unemployedPopulation += numAdded;
                unallocatedPopulation -= numAdded;
                numEmptyResidential[i] -= numAdded;
                emptyResidential[i].AddUsers(numAdded);
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
        if (totalPopulation < itemManager.GetMaxPop())
        {
            //Debug.Log("Initial increase conditions met");
            IncreasePopulation();
        }
    }

    void IncreasePopulation()
    // Tries to increase population
    {
        if (totalPopulation < 2)
        {
            unallocatedPopulation++;
        }
        else if ((decimal)unemployedPopulation / (decimal)totalPopulation <= (decimal)0.1)
        {
            unallocatedPopulation += (IncomingPopulation());
        }
        else
        {
            //Debug.Log("Too much unemployment to increase");
        }
        resUpdate++;
    }

    int IncomingPopulation()
    {
        float maxWorkAmount = itemManager.GetMaxJobs() * 1.1f;
        float maxCapacity = itemManager.GetMaxPop();
        if(maxCapacity - totalPopulation <= 5)
        {
            return 1;
        }
        else if (maxWorkAmount - maxCapacity <= 3)
        {
            return (int)((maxCapacity - totalPopulation) * 0.2f);
        }
        else if (maxWorkAmount < maxCapacity - 1)
        {
            //Debug.Log("Option 1");
            return (int)((maxWorkAmount - totalPopulation) * 0.2f);
        }
        else if (maxWorkAmount > maxCapacity - 1)
        {
            //Debug.Log("Option 2");
            return (int)((maxCapacity - totalPopulation) * 0.2f);
        }
        else return (int)((maxCapacity - totalPopulation) * 0.2f);
    }

    public void DeallocateUsers(int numUsers, string type)
    // Adds users back to unallocatedPopulation for future reallocation
    {
		if(type == "residential")
		{
	        population -= numUsers;
	        unallocatedPopulation += numUsers;
		}
		else if(type == "industrial" || type == "commercial")
		{
            unemployedPopulation += numUsers;
            // TODO: need to inform resitrack of newly unemployed
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

    public float GetHappiness()
    {
        return happinessManager.happiness;
    }

    public string FancyTotalPopulation()
    {
        return "Total Population: " + totalPopulation.ToString();
    }

    public string FancyEmployedPopulation()
    {
        return "Employed: " + (totalPopulation - unemployedPopulation).ToString();
    }

    public string FancyUnemployedPopulation()
    {
        if (totalPopulation != 0)
        {
            return "Unemployed: %" + (unemployedPopulation / totalPopulation * 100).ToString();
        }
        else return "0 Unemployed";
    }

    public bool AvailablePopulation()
        // Returns true if unallocatedPopulation >= 1
    {
        if (unallocatedPopulation >= 1) return true;
        else return false;
    }

    public void ConfirmHoused(int numHoused)
    {
        population += numHoused;
        unallocatedPopulation -= numHoused;
    }
}
