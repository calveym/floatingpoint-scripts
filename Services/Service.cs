using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Service: MonoBehaviour {

    public int demand;  // Main demand variable, translation of overall population
    public int amount;  // Amount of service available
    public int demandMultiplier;  // 

    protected float cost;  // Amount of money the service costs every ecoTick
    [Range(0, 1f)]
    [Tooltip("Cost per unit of population")]
    public float costMultiplier;  // How much service costs per unit of population

    protected float abandonWaitTime;  // Time before abandonment begins
    protected int maxHappinessEffect;  // Maximum possible effect on happiness

    protected PopulationManager populationManager;
    protected EconomyManager economyManager;
    //protected ItemManager itemManager;

    protected bool serviceWorking = true;

    public delegate void ServiceUpdater();
    public delegate void AddLocalAmounts();
    public static ServiceUpdater serviceUpdater;
    public static AddLocalAmounts addLocalAmounts;

    List<Power> power;

    protected virtual void Start()
    {
        populationManager = ReferenceManager.instance.populationManager;
        economyManager = ReferenceManager.instance.economyManager;
        //itemManager = ReferenceManager.instance.itemManager;

        StartCoroutine("UpdateService");
    }

    protected void UpdateDemand()
    // Updates demand from population
    {
        demand = populationManager.totalPopulation * demandMultiplier;
    }

    protected void UpdateAmount()
    // Updates amount of service
    {
        amount = 0;
        addLocalAmounts();  // Calls delegate, which local services subscribe to and return with AddAmount call
    }

    public void AddAmount(int addAmount)
    // called by local services
    {

    }

    public void AddPower(Power incomingPower)
    {
        power.Add(incomingPower);
        RunUpdates();
    }

    void RunUpdates()
    // Updates base service figures
    {
        UpdateDemand();
        UpdateAmount();
    }

    protected IEnumerator UpdateService()
    // Queues updates
    {
        while(serviceWorking)
        {
            serviceUpdater();  // Adds individual services, retrieves total current service amounts
            RunUpdates();
            yield return new WaitForSeconds(15f);
        }
    }
}
