using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Autelia.Serialization;

[System.Serializable]
public class ResidentialTracker : ItemTracker {
    // Manages specific residential functions.

    public int employmentHappiness;

    float foliage;
    bool checkEnable;

    new void Start()
    {
        base.Start();
        foliage = 0;

        if (Serializer.IsLoading)
            RemoveEcoTick();
        //StartCoroutine("CheckEnable");
    }

    void Update()
    {
        if (!updateStarted && usable && validPosition)
        {
            AddEcoTick();
        }
        else if(updateStarted && !usable && validPosition)
        {
            updateStarted = false;
            EconomyManager.ecoTick -= UpdateSecond;
        }
        else if(!usable && !updateStarted && validPosition)
        {
            checkEnable = true;
        }
        if (ReferenceManager.instance.tick % 5 == 0)
        {
            if (checkEnable)
            {
                checkEnable = false;
                if (transform.parent == null)
                {
                    usable = true;
                }
            }
        }
        if(ReferenceManager.instance.tick % 10 == 0)
        {
            UpdateUnhappiness();
        }
    }

    public void AddEcoTick()
    {
        EnableSnap();
        updateStarted = true;
        EconomyManager.ecoTick += UpdateSecond;
        itemManager.addResidential(capacity, gameObject);
    }

    void RemoveEcoTick()
    {
        updateStarted = false;
        EconomyManager.ecoTick -= UpdateSecond;
    }

    public override void AddUsers(int numUsers)
    // Adds numUsers to users if capacity is not exceeded
    {
        if (numUsers + users <= capacity)
        {
            base.AddUsers(numUsers);
            unemployedPopulation += numUsers;
        }
        else
        {
            Debug.Log("ERROR: user mismatch, aborting");
            populationManager.QueueUpdates();
        }
    }                     

    public void RemoveAllUsers()
    {
        // TODO: Inform employment location of vacancies
    }

    public void AcceptApplication()
    {
        unemployedPopulation--;
        populationManager.unemployedPopulation--;
        populationManager.QueueUpdates();
    }

    void CheckEmployment()
    {
        int amountToEmploy = 0;
        if (unemployedPopulation >= 1 && unemployedPopulation < 10)
            amountToEmploy = 1;
        else if (unemployedPopulation >= 10 && unemployedPopulation < 50)
            amountToEmploy = 9;
        else if (unemployedPopulation >= 50 && unemployedPopulation < 250)
            amountToEmploy = 49;
        else if (unemployedPopulation >= 250)
            amountToEmploy = 99;


        for (int i = 0; i < amountToEmploy; i++)
        {
            TryEmployWorker();
        }
    }

    void TryEmployWorker()
    // Called by populationManager
    {
        GameObject employmentLocation = FindPreferredEmployment();
        if (employmentLocation)
        {
            ApplyForJob(employmentLocation);
        }
    }

    void ApplyForJob(GameObject targetLocation)
    {
        if(targetLocation.tag == "commercial")
        {
            targetLocation.GetComponent<CommercialTracker>().Apply(landValue, this);
        }
        else if (targetLocation.tag == "industrial")
        {
            targetLocation.GetComponent<IndustrialTracker>().Apply(landValue, this);
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
                returnObject.Add(allObjects[i], (distances[allObjects[i]] + landValues[allObjects[i]]));
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
                returnObject.Add(trialObjects[i], 1 / Vector3.Distance(transform.position, trialObjects[i].transform.position));
            }
        }
        return returnObject;
    }

    public void UpdateSecond()
    // Updates values once per second
    {
        Debug.Log("Tick");
        updateStarted = true;
        if(!usable || !validPosition)
        {
            return;
        }
        CheckEmployment();
        UpdateLocalHappiness();
        UpdateEmploymentHappiness();
        UpdateHappiness();
        UpdateLandValue();
        UpdateTransportationValue();
        CalculateIncome();
    }

    void UpdateEmploymentHappiness()
    {
        if (capacity != 0)
        {
            employmentHappiness = (users - unemployedPopulation) / capacity * 20;
        }
        else employmentHappiness = 0;
    }

    void UpdateHappiness()
    {
        currentHappiness = localHappiness + employmentHappiness + fillRateHappiness + 20;
        CalculateLongtermHappiness();
        CalculateHappinessState();
    }

    void CalculateIncome()
    {
        income = users * (1 + (landValue * 0.01f)) * (longtermHappiness / 50) * ReferenceManager.instance.residentialIncomeMultiplier;
        income -= baseCost;
        totalResidentialIncome += income;
    }

    void UpdateUnhappiness()
    {
        if(currentHappiness < unhappinessMinimum && cumulativeUnhappiness < requiredUnhappiness)
        {
            cumulativeUnhappiness++;
        }
        else if(currentHappiness >= unhappinessMaximum && cumulativeUnhappiness > 0)
        {
            cumulativeUnhappiness--;
        }
    }

    void CheckUnhappiness()
    {
        if(cumulativeUnhappiness >= requiredUnhappiness)
        {
            movingOut = true;

Autelia.Coroutines.CoroutineController.StartCoroutine(this, "MoveOut");
        }
    }

    public string ValidPosition()
    {
        if (validPosition)
        {
            return "Active";
        }
        else return "Inactive";
    }

    public string FancyIncome()
    {
        return "Income: $" + Mathf.Round(income * 100) / 100 + "/w";
    }

    public string FancyCapacity()
    {
        return "Residents: " + users + " / " + capacity;
    }

    public int FancyHappiness()
    {
        return happinessState;
    }

    public string FancyLandValue()
    {
        return "Land Value: $" + landValue;
    }

    public string FancyTitle()
    {
        return buildingName;
    }
}