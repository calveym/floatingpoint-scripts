using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingBase : PersonContainer
{
    
    protected PopulationManager populationManager;
    protected HappinessManager happinessManager;
    protected EconomyManager economyManager;
    protected ItemManager itemManager;

    [Header("Basic Attributes")]
    [Space(5)]

    [Tooltip("Enable or disable BuildingUpdate from running")]
    public int usable;
    [Tooltip("Level required to purchase")]
    public int level = 0;
    [Tooltip("Cost to purchase")]
    public int buyCost;
    [Tooltip("Cost to maintain")]
    public int baseCost;
    [Tooltip("Building name, as shown to player on tooltips")]
    public string buildingName;
    [Tooltip("Maximum user capacity")]
    new public int capacity;

    [Space(10)]
    [Header("Generated Attributes")]
    [Space(5)]
    public float income;
    new public int users;
    public int idleUsers;
    public bool snapped;
    public float landValue = 10;
    public float desirability;  // Defines how likely people are to move in/ out

    [Space(10)]
    [Header("Happiness")]
    [Space(5)]
    float fillRateHappiness;  // Happiness based on user-cap ratio
    float addedHappiness;  // Happiness added or detracted by surroundings, tracking figure
    float localHappiness;  // Converted surrounding happiness
    float salesHappiness;  // Happiness from selling

    public float currentHappiness = 0;
    public float longtermHappiness = 0;
    public int happinessState;

    [Space(10)]
    [Header("Services")]
    [Space(5)]
    public bool power;
    public bool health;
    public bool education;
    public bool fire;
    public bool police;

    protected List<GameObject> surroundingBuildings;
    protected List<ResidentialTracker> surroundingResidential;
    protected List<CommercialTracker> surroundingCommercial;
    protected List<IndustrialTracker> surroundingIndustrial;
    private float aggResidential;
    private float aggCommercial;
    private float aggIndustrial;

    void Start()
    {
        populationManager = ReferenceManager.instance.populationManager;
        economyManager = ReferenceManager.instance.economyManager;
        happinessManager = ReferenceManager.instance.happinessManager;
        itemManager = ReferenceManager.instance.itemManager;
    }

    public virtual void BuildingUpdate()
    // Main building update loop
    {
        CheckUsers();
        LandValue();
        UpdateHappiness();
    }

    protected virtual void CheckUsers()
    // Initiates user increase if not full
    {
        if (!Full())
            RequestUsers();
    }

    protected abstract void RequestUsers();

    protected virtual void LandValue()
    // Updates land value figures
    {
        UpdateSurroundingValues();
        CalculateAggregates();
        landValue = aggResidential + aggCommercial + aggIndustrial;
        landValue += users / capacity * 30f;
        if (education)
            landValue += 25;
        if (capacity == users)
            landValue += 7;
    }

    protected abstract void UpdateSurroundingValues();

    void CalculateAggregates()
    {
        aggResidential = CalculateResidentialAggregate() * 5;
        aggCommercial = CalculateCommercialAggregate() * 8;
        aggIndustrial = CalculateIndustrialAggregate() * 3;
    }

    float CalculateResidentialAggregate()
    {
        return surroundingResidential.Count;
    }

    float CalculateCommercialAggregate()
    {
        return surroundingCommercial.Count;
    }

    float CalculateIndustrialAggregate()
    {
        return surroundingIndustrial.Count;
    }

    protected virtual void UpdateHappiness()
    {
        UpdateLocalHappiness();
        currentHappiness = localHappiness + salesHappiness + fillRateHappiness;
        CalculateLongtermHappiness();
        CalculateHappinessState();

        happinessManager.SendHappiness(happinessState);
    }

    public void UpdateLocalHappiness()
    //  Sets local happiness level based on surroundings
    {
        if (addedHappiness < 40 && addedHappiness > -20)
        {
            localHappiness = addedHappiness;
        }
        else if (addedHappiness >= 40)
        {
            localHappiness = 40;
        }
        else if (addedHappiness <= -20)
        {
            localHappiness = -20;
        }
        addedHappiness = 0;

        if (capacity != 0)
        {
            fillRateHappiness = ((float)users / (float)capacity) * 20;
        }
    }

    public void CalculateLongtermHappiness()
    // makes the longterm happiness tend towards the current happiness
    {
        if (currentHappiness - longtermHappiness < 0.2f)
        {
            longtermHappiness = currentHappiness;
        }
        else
        {
            longtermHappiness += (currentHappiness - longtermHappiness) * 0.05f;
        }
    }

    public void CalculateHappinessState()
    {
        if (longtermHappiness < 20)
        {
            happinessState = 1;
        }
        else if (longtermHappiness < 60 && longtermHappiness >= 20)
        {
            happinessState = 2;
        }
        else if (longtermHappiness < 80 && longtermHappiness >= 60)
        {
            happinessState = 3;
        }
        else if (longtermHappiness >= 80)
        {
            happinessState = 4;
        }
    }

    public void ModifyHappiness(float amount, string trigger)
    {
        addedHappiness += amount;
    }
}
 