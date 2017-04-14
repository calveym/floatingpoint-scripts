using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ResidentialTracker : ItemTracker {
    // Manages specific residential functions.

    public int unemployedPopulation;
    public Dictionary <int, int> educationLevel; // maps individual residents to their education levels
    public Dictionary <int, int> age; // maps individual residents to their age
    public Dictionary <int, bool> employed; // matches residents with employment status
    public Dictionary<int, string> residents; // matches slots to residents
    List<int> takenIDs; // List of currently active IDs for finding new ones

    float foliage;
    float foliageIncome;

    void Start()
    {
        educationLevel = new Dictionary<int, int>();
        age = new Dictionary<int, int>();
        employed = new Dictionary<int, bool>();
        residents = new Dictionary<int, string>();
        takenIDs = new List<int>();
        foliage = 0;
    }

    void Update()
    {
        if (!updateStarted)
        {
            updateStarted = true;
            EconomyManager.ecoTick += UpdateSecond;
            GameObject.Find("Managers").GetComponent<ItemManager>().addResidential(capacity, gameObject);
        }
    }

    public void AddUsers(int numUsers, List<string> names, int newAge)
    // Adds numUsers to users if capacity is not exceeded
    {
        if (numUsers + users <= capacity)
        {
            users += numUsers;
            for(int i = 0; i < numUsers; i++)
            {
                int newID = FindAvailableID();
                educationLevel.Add(newID, 0);
                age.Add(newID, newAge);
                employed.Add(newID, false);
                unemployedPopulation++;
                residents.Add(newID, names[i]);
                takenIDs.Add(newID);
            }
        }
        else
        {
            Debug.Log("ERROR: user mismatch, aborting");
            populationManager.QueueUpdates();
        }
    }

    int FindAvailableID()
    // Finds next lowest available resident ID
    {
        int availableID = users + 1;
        for(int i = 0; i <= takenIDs.Count; i++)
        {
            if (!takenIDs.Contains(i))
            {
                availableID = i;
                break;
            }
        }
        return availableID;
    }

    public void AcceptApplication(int acceptedApplicantID)
    {
        employed[acceptedApplicantID] = true;
        unemployedPopulation--;
        populationManager.unemployedPopulation--;
        populationManager.QueueUpdates();
    }

    public void TryEmployWorker()
    // Called by populationManager
    {
        int unemployedID = employed.FirstOrDefault(x => x.Value == false).Key;
        GameObject employmentLocation = FindPreferredEmployment();
        ApplyForJob(employmentLocation, unemployedID);
    }

    void ApplyForJob(GameObject targetLocation, int residentID)
    {
        if(targetLocation.tag == "commercial")
        {
            targetLocation.GetComponent<CommercialTracker>().Apply(landValue, residentID, this);
        }
        else if (targetLocation.tag == "industrial")
        {
            targetLocation.GetComponent<IndustrialTracker>().Apply(landValue, residentID, this);
        }
    }

    GameObject FindPreferredEmployment()
    {
        List<IndustrialTracker> firstFiveIndustrial = populationManager.emptyIndustrial.Take(5).ToList();
        List<CommercialTracker> firstFiveCommercial = populationManager.emptyCommercial.Take(5).ToList();
        List<GameObject> allPotentialLocations = new List<GameObject>();

        for(int i = 0; i < firstFiveCommercial.Count; i++)
        {
            allPotentialLocations.Add(firstFiveCommercial[i].gameObject);
        }
        for(int i = 0; i < firstFiveIndustrial.Count; i++)
        {
            allPotentialLocations.Add(firstFiveIndustrial[i].gameObject);
        }
        Dictionary<GameObject, float> distances = AnalyzeDistances(allPotentialLocations);
        Dictionary<GameObject, float> landValues = AnalyzeValues(allPotentialLocations);
        Dictionary<GameObject, float> aggregate = CreateAggregate(allPotentialLocations, distances, landValues);
        GameObject finalEmploymentLocation = aggregate.FirstOrDefault(x => x.Value == aggregate.Values.Max()).Key;
 
        return finalEmploymentLocation;
    }

    Dictionary<GameObject, float> CreateAggregate(List<GameObject> allObjects, Dictionary<GameObject, float> distances, Dictionary<GameObject, float> landValues)
    // Creates aggregate land value and distance calculation
    {
        Dictionary<GameObject, float> returnObject = new Dictionary<GameObject, float>();
        for(int i = 0; i < allObjects.Count; i++)
        {
            if (returnObject.ContainsKey(allObjects[i]) == false)
            {
                returnObject.Add(allObjects[i], (1 / distances[allObjects[i]] + landValues[allObjects[i]]));
            }
        }
        return returnObject;
    }

    Dictionary<GameObject, float> AnalyzeValues(List<GameObject> trialObjects)
    {
        Dictionary<GameObject, float> returnObject = new Dictionary<GameObject, float>();
        for (int i = 0; i < (trialObjects.Count); i++)
        {
            if(trialObjects[i].tag == "commercial")
            {
                if (returnObject.ContainsKey(trialObjects[i]) == false)
                {
                    returnObject.Add(trialObjects[i], trialObjects[i].GetComponent<CommercialTracker>().landValue);
                }
            }
            else if (trialObjects[i].tag == "industrial")
            {
                if (returnObject.ContainsKey(trialObjects[i]) == false)
                {
                    returnObject.Add(trialObjects[i], trialObjects[i].GetComponent<IndustrialTracker>().landValue);
                }
            }
        }
        return returnObject;
    }

    Dictionary<GameObject, float> AnalyzeDistances(List<GameObject> trialObjects)
    {
        Dictionary<GameObject, float> returnObject = new Dictionary<GameObject, float>();
        for(int i = 0; i < trialObjects.Count; i++)
        {
            if(returnObject.ContainsKey(trialObjects[i]) == false)
            {
                returnObject.Add(trialObjects[i], Vector3.Distance(transform.position, trialObjects[i].transform.position));
            }
        }
        return returnObject;
    }

    public bool IsEmployed(int ID)
    {
        return employed[ID];
    }

    void UpdateSecond()
    // Updates values once per second
    {
        updateStarted = true;
        if(!usable || !validPosition)
        {
            return;
        }
        UpdateLandValue();
        UpdateTransportationValue();
        UpdateHappiness();
        CalculateIncome();
    }

    void CalculateIncome()
    {
        income = users * availableTransportation * landValue / 5;
        income += foliageIncome;
        income -= baseCost;
        totalResidentialIncome += income;
    }

    public void AddFoliage(float addAmount)
    {
        foliage += addAmount;
    }

}
