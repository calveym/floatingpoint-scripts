using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ServiceBase: MonoBehaviour {

    [Tooltip("Max required amount, calculated at runtime based on current total population")]
    public int demand;  // Main demand variable, translation of overall population
    [Tooltip("Total amount of service, calculated at runtime")]
    public int amount;  // Amount of service available


    protected float cost;  // All added costs from trackers
    protected float totalCost;  // Total cost, calculated with multiplier and cost, deducted from income
    [Header("Setup")]
    [Range(0, 1f)]
    [Tooltip("Cost per unit of population")]
    public float costMultiplier;  // used to calculate cost with amount
    [Tooltip("Enables tweaking of demand level")]
    public int demandMultiplier;[Tooltip("Multiplier for comparing amount to demand, for applying defecit/ surplus. \n [x < 1]: harder to stay out of defecit \n [x == 1]: Defecit if below demand\n [x > 1]: Easier to stay out of defecit")]
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
    {
        populationManager = ReferenceManager.instance.populationManager;
        economyManager = ReferenceManager.instance.economyManager;
        itemManager = ReferenceManager.instance.itemManager;
        serviceUpdater = UpdateServices;

        StartCoroutine("UpdateService");
    }

    protected void UpdateDemand()
    // Updates demand from population
    {
        demand = populationManager.totalPopulation * demandMultiplier;
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
            yield return new WaitForSeconds(15f);
        }
    }
}
