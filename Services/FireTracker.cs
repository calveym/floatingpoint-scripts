using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autelia.Serialization;
namespace CloudCity
{
    [SelectionBase]
    public class FireTracker : ServiceTrackerBase
    {

        Fire fire;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            fire = ReferenceManager.instance.fire;
        }

        public override void AddService()
        {
            base.AddService();
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
                        ItemTracker tempTracker = building.GetComponent<ItemTracker>();
                        if (tempTracker)
                            tempTracker.fire = true;
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
}