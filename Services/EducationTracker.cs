using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autelia.Serialization;

[SelectionBase]
public class EducationTracker : ServiceTrackerBase {

    Education education;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {if (Serializer.IsDeserializing)	return;if (Serializer.IsLoading)	return;
        base.Start();

        education = ReferenceManager.instance.education;
    }

    public override void AddService()
    {
        base.AddService();
        education.AddEducation(this);
        education.addLocalEducation += DoEffect;
        education.servicePayment += PayForService;
    }

    protected override void DoEffect()
    {
        base.DoEffect();
        if (surroundingBuildings.Count <= amount)
        {
            foreach (GameObject building in surroundingBuildings)
            {
                if (building != gameObject && building.tag == "industrial" || building.tag == "commercial" || building.tag == "residential")
                {
                    building.GetComponent<ItemTracker>().education = true;
                }
            }
        }
        else if (amount < surroundingBuildings.Count)
        {
            for (int i = 0; i < amount; i++)
            {
                if (surroundingBuildings[i] != gameObject && surroundingBuildings[i].tag == "industrial" || surroundingBuildings[i].tag == "commercial" || surroundingBuildings[i].tag == "residential")
                {
                    surroundingBuildings[i].GetComponent<ItemTracker>().education = true;
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
        education.AddCost(cost);
    }
}
