using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autelia.Serialization;

[SelectionBase]
public class PowerTracker : ServiceTrackerBase
{

    Power power;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start ()
    {
        base.Start();

        power = ReferenceManager.instance.power;
        AddService();
    }

    public override void AddService()
    {
        base.AddService();
        power.AddPower(this);
        power.addLocalPower += DoEffect;
        power.servicePayment += PayForService;
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
                        tempTracker.power = true;
                }
            }
        }
        else if(amount < surroundingBuildings.Count)
        {
            for(int i = 0; i < amount; i++)
            {
                if (surroundingBuildings[i] != gameObject && surroundingBuildings[i].tag == "industrial" || surroundingBuildings[i].tag == "commercial" || surroundingBuildings[i].tag == "industrial")
                {
                    surroundingBuildings[i].GetComponent<ItemTracker>().power = true;
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
        power.AddCost(cost);
    }
}
