using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autelia.Serialization;
using CloudCity;

public class Health : ServiceBase
{

    List<HealthTracker> health;
    public delegate void AddLocalHealth();
    public AddLocalHealth addLocalHealth;


    protected void Awake()
    {if (Serializer.IsLoading)	return;
        health = new List<HealthTracker>();
    }

    protected override void ApplyGlobalDefecit()
    {
        // TODO:A
    }

    protected override void ApplyGlobalSurplus()
    {
        // TODO:A
    }

    public void AddHealth(HealthTracker incomingHealth)
    {
        health.Add(incomingHealth);
    }

    protected override void RunAddLocalAmounts()
    {
        if(addLocalHealth != null)
        {
            addLocalHealth();
        }
    }

    protected override void DeductCost()
    {
        economyManager.SetHealthExpense(cost);
    }

    protected override void ResetCommercial()
    {
        foreach (GameObject com in itemManager.commercial)
        {
            com.GetComponent<ItemTracker>().health = false;
        }
    }

    protected override void ResetIndustrial()
    {
        foreach (GameObject ind in itemManager.industrial)
        {
            ind.GetComponent<ItemTracker>().health = false;
        }
    }

    protected override void ResetResidential()
    {
        foreach (GameObject res in itemManager.residential)
        {
            res.GetComponent<ItemTracker>().health = false;
        }
    }
}
