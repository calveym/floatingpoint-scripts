﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class FireTracker : ServiceTrackerBase {

    Material sphereMaterial;
    Fire fire;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        fire = ReferenceManager.instance.fire;
        AddService();
    }

    protected override void AddService()
    {
        fire.AddFire(this);
        fire.addLocalFire += DoEffect;
        fire.servicePayment += PayForService;
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
                    building.GetComponent<ItemTracker>().fire = true;
                }
            }
        }
        else if (amount < surroundingBuildings.Count)
        {
            for (int i = 0; i < amount; i++)
            {
                if (surroundingBuildings[i] != gameObject)
                {
                    surroundingBuildings[i].GetComponent<ItemTracker>().fire = true;
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
        fire.AddCost(cost);
    }
}
