using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : ServiceBase
{

    List<PowerTracker> power;  // List of all power trackers
    public delegate void AddLocalPower();
    public AddLocalPower addLocalPower;


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
        RunUpdates();
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
        List<GameObject> allCom = itemManager.commercial;
        foreach(GameObject com in allCom)
        {
            com.GetComponent<ItemTracker>().power = false;
        }
    }

    protected override void ResetIndustrial()
    {
        List<GameObject> allInd = itemManager.industrial;
        foreach(GameObject ind in allInd)
        {
            ind.GetComponent<ItemTracker>().power = false;
        }
    }

    protected override void ResetResidential()
    {
        List<GameObject> allRes = itemManager.residential;
        foreach (GameObject res in allRes)
        {
            res.GetComponent<ItemTracker>().power = false;
        }
    }
}
