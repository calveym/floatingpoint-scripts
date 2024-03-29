using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autelia.Serialization;

public abstract class ServiceBase: MonoBehaviour {

    [Header("Setup")]
    [Tooltip("Max required amount, calculated at runtime based on current total population")]
    public int demand;  // Main demand variable, translation of overall population
    [Tooltip("Total amount of service, calculated at runtime")]
    public int amount;  // Amount of service available
    [Tooltip("Service type, eg [power]")]
    public string type; // Type of service, set in inspector

    // STATIC DEMAND VARIABLES, FOR ACCESS. NOT SET HERE, TAKEN FROM RELEVANT service.demand
    [Header("Demand")]
    public static int powerDemand;
    public static int educationDemand;
    public static int healthDemand;
    public static int fireDemand;
    public static int policeDemand;
   

    protected float cost;  // All added costs from trackers
    protected float totalCost;  // Total cost, calculated with multiplier and cost, deducted from income
    [Header("Effects")]
    [Range(1f, 5f)]
    [Tooltip("Enables tweaking of demand level, 1 to stay default")]
    public int demandMultiplier = 1;
    [Tooltip("Multiplier for comparing amount to demand, for applying defecit/ surplus. \n [x < 1]: harder to stay out of defecit \n [x == 1]: Defecit if below demand\n [x > 1]: Easier to stay out of defecit")]
    [Range(0f, 2f)]
    public float minDemandMultiplier;

    protected float abandonWaitTime;  // Time before abandonment begins
    protected int maxHappinessEffect;  // Maximum possible effect on happiness

    protected PopulationManager populationManager;
    protected EconomyManager economyManager;
    protected ItemManager itemManager;

    protected bool serviceWorking = true;

    public delegate void ServiceUpdater();
    public delegate void ServicePayment();
    public ServiceUpdater serviceUpdater;
    public ServicePayment servicePayment;


    protected virtual void Start()
    {if (Serializer.IsLoading)	return;
        populationManager = ReferenceManager.instance.populationManager;
        economyManager = ReferenceManager.instance.economyManager;
        itemManager = ReferenceManager.instance.itemManager;
        serviceUpdater = UpdateServices;



Autelia.Coroutines.CoroutineController.StartCoroutine(this, "UpdateService");
    }

    protected void UpdateDemand()
    // Updates demand from population
    {
        demand = populationManager.totalPopulation * demandMultiplier;
        if (type == "power")
        {
            powerDemand = demand;
        }
        else if (type == "education")
        {
            educationDemand = demand;
        }
        else if (type == "health")
        {
            healthDemand = demand;
        }
        else if (type == "police")
        {
            policeDemand = demand;
        }
        else if(type == "fire")
        {
            fireDemand = demand;
        }
    }

    protected void UpdateServices()
    // Updates amount of service
    {
        amount = 0;  // Resets amount of service

        ResetLocalServices();  // Obvious
        RunAddLocalAmounts();  // Calls function that calls delegate, which local services subscribe to and return with AddAmount call
    }

    protected void ResetLocalServices()
    // Resets relevant service values to false;
    {
        ResetResidential();
        ResetCommercial();
        ResetIndustrial();
    }

    public void AddAmount(int addAmount)
    // called by local services
    {
        amount += addAmount;
    }


    protected void RunUpdates()
    // Updates base service figures
    {
        UpdateDemand();
        UpdateServices();
    }

    protected void ApplyGlobalEffect()
    // Checks amount against demand, applies relevant bonuses
    {
        if (amount < demand / minDemandMultiplier)
        {
            ApplyGlobalDefecit();
        }
        else if (demand <= amount)
        {
            ApplyGlobalSurplus();
        }
    }

    public void AddCost(float newCost)
    {
        cost += newCost;
    }

    public static int FindDemand(string type)
    // Generalised function that allows easy access of demand
    {
        if (type == "power")
        {
            return powerDemand;
        }
        else if(type == "education")
        {
            return educationDemand;
        }
        else if(type == "health")
        {
            return healthDemand;
        }
        else if(type == "fire")
        {
            return fireDemand;
        }
        else if(type == "police")
        {
            return policeDemand;
        }
        else return 0;
    }


    // Abstract members. All overriden by derived classes 
    protected abstract void ResetResidential();

    protected abstract void ResetCommercial();

    protected abstract void ResetIndustrial();

    protected abstract void RunAddLocalAmounts();

    protected abstract void ApplyGlobalDefecit();

    protected abstract void ApplyGlobalSurplus();

    protected abstract void DeductCost();

    // Coroutines
    protected IEnumerator UpdateService()
    // Runs full update, 15s cycle
    {
        yield return new WaitForSeconds(1f);
        while(serviceWorking)
        {
            cost = 0;
            RunUpdates();
            if (servicePayment != null)
            {
                cost = 0;
                servicePayment();
                DeductCost();
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
