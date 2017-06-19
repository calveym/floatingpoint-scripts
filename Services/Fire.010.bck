using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autelia.Serialization;

public class Fire : ServiceBase
{

    List<FireTracker> fire;
    public delegate void AddLocalFire();
    public AddLocalFire addLocalFire;


    protected void Awake()
    {if (Serializer.IsLoading)	return;
        fire = new List<FireTracker>();
    }

    protected override void ApplyGlobalDefecit()
    {
        //TODO:A
    }

    protected override void ApplyGlobalSurplus()
    {
        //TODO:A
    }

    public void AddFire(FireTracker incomingFire)
    {
        fire.Add(incomingFire);
    }

    protected override void RunAddLocalAmounts()
    {
        if (addLocalFire != null)
        {
            addLocalFire();
        }
    }

    protected override void DeductCost()
    {
        economyManager.SetFireExpense(cost);
    }

    protected override void ResetCommercial()
    {
        foreach (GameObject com in itemManager.commercial)
        {
            com.GetComponent<ItemTracker>().fire = false;
        }
    }

    protected override void ResetIndustrial()
    {
        foreach (GameObject ind in itemManager.industrial)
        {
            ind.GetComponent<ItemTracker>().fire = false;
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
