using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autelia.Serialization;

public class Police : ServiceBase
{

    List<PoliceTracker> police;  // List of all police trackers
    public delegate void AddLocalPolice();
    public AddLocalPolice addLocalPolice;


    protected void Awake()
    {if (Serializer.IsLoading)	return;
        police = new List<PoliceTracker>();
    }

    protected override void ApplyGlobalDefecit()
    {
        throw new NotImplementedException();
        //TODO:A
    }

    protected override void ApplyGlobalSurplus()
    {
        throw new NotImplementedException();
        //TODO:A
    }

    public void AddPolice(PoliceTracker incomingPolice)
    {
        police.Add(incomingPolice);
    }

    protected override void RunAddLocalAmounts()
    {
        if(addLocalPolice != null)
        {
            addLocalPolice();
        }
    }

    protected override void DeductCost()
    {
        economyManager.SetPoliceExpense(cost);
    }

    protected override void ResetCommercial()
    {
        foreach (GameObject com in itemManager.commercial)
        {
            com.GetComponent<ItemTracker>().police = false;
        }
    }

    protected override void ResetIndustrial()
    {
        foreach (GameObject ind in itemManager.industrial)
        {
            ind.GetComponent<ItemTracker>().police = false;
        }
    }

    protected override void ResetResidential()
    {
        foreach (GameObject res in itemManager.residential)
        {
            res.GetComponent<ItemTracker>().police = false;
        }
    }
}
