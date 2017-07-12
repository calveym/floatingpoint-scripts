using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autelia.Serialization;
using CloudCity;

public class Power : ServiceBase
{

    List<PowerTracker> power;  // List of all power trackers
    public delegate void AddLocalPower();
    public AddLocalPower addLocalPower;


    protected void Awake()
    {
        power = new List<PowerTracker>();
    }

    protected override void ApplyGlobalDefecit()
    {
        economyManager.SetProduction(0.6f);
    }

    protected override void ApplyGlobalSurplus()
    {
        economyManager.SetProduction(1.2f);
    }

    public void AddPower(PowerTracker incomingPower)
    {
        power.Add(incomingPower);
    }

    protected override void RunAddLocalAmounts()
    {
        if(addLocalPower != null)
        {
            addLocalPower();
        }
    }

    protected override void DeductCost()
    {
        economyManager.SetPowerExpense(cost);
    }

    protected override void ResetCommercial()
    {
        foreach(GameObject com in itemManager.commercial)
        {
            if(com.tag == "commercial")
                com.GetComponent<ItemTracker>().power = false;
        }
    }

    protected override void ResetIndustrial()
    {
        foreach(GameObject ind in itemManager.industrial)
        {
            ind.GetComponent<ItemTracker>().power = false;
        }
    }

    protected override void ResetResidential()
    {
        foreach (GameObject res in itemManager.residential)
        {
            res.GetComponent<ItemTracker>().power = false;
        }
    }
}
