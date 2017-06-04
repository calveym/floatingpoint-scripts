using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class HealthTracker : ServiceTrackerBase {

    Material sphereMaterial;
    Health health;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        health = ReferenceManager.instance.health;
        AddService();
    }

    protected override void AddService()
    {
        health.AddHealth(this);
        health.addLocalHealth += DoEffect;
        health.servicePayment += PayForService;
    }

    protected override void DoEffect()
    {
        base.DoEffect();
        if(surroundingBuildings.Count <= amount)
        {
            foreach (GameObject building in surroundingBuildings)
            {
                if (building != gameObject && building.tag == "industrial" || building.tag == "commercial" || building.tag == "industrial")
                {
                    ItemTracker tempTracker = building.GetComponent<ItemTracker>();
                    if(tempTracker)
                        tempTracker.health = true;
                }
            }
        }
        else if (amount < surroundingBuildings.Count)
        {
            for (int i = 0; i < amount; i++)
            {
                if (surroundingBuildings[i] != gameObject)
                {
                    surroundingBuildings[i].GetComponent<ItemTracker>().health = true;
                }
            }
        }
    }

    protected override void SetSphereMaterial()
    {
        sphereScript.SetSphereMaterial(sphereMaterial);
    }

    protected override void PayForService()
    {
        health.AddCost(cost);
    }
}
