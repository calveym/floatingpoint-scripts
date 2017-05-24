using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PowerTracker : ServiceTrackerBase {

    Power power;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start () {
        base.Start();

        power = ReferenceManager.instance.power;
        AddService();
    }

    protected void AddService()
    {
        power.AddPower(this);
        power.addLocalPower += DoEffect;
        power.servicePayment += PayForService;
    }

    protected void DoEffect()
    {
        List<GameObject> surroundingBuildings = U.FindNearestBuildings(transform.position, radius);
        foreach(GameObject building in surroundingBuildings)
        {
            building.GetComponent<ItemTracker>().power = true;
        }
    }

    protected override void PayForService()
    {
        power.AddCost(cost);
    }
}
