using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PoliceTracker : ServiceTrackerBase
{

    Material sphereMaterial;
    Police police;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        police = ReferenceManager.instance.police;
        AddService();
    }

    protected override void AddService()
    {
        police.AddPolice(this);
        police.addLocalPolice += DoEffect;
        police.servicePayment += PayForService;
    }

    protected override void DoEffect()
    {
        base.DoEffect();
        if (surroundingBuildings.Count <= amount)
        {
            foreach (GameObject building in surroundingBuildings)
            {
                if (building != gameObject && building.tag == "industrial" || building.tag == "commercial" || building.tag == "industrial")
                {
                    ItemTracker tempTracker = building.GetComponent<ItemTracker>();
                    if(tempTracker)
                        tempTracker.police = true;
                }
            }
        }
        else if (amount < surroundingBuildings.Count)
        {
            for (int i = 0; i < amount; i++)
            {
                if (surroundingBuildings[i] != gameObject)
                {
                    surroundingBuildings[i].GetComponent<ItemTracker>().police = true;
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
        police.AddCost(cost);
    }
}
