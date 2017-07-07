using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServiceTooltip : TooltipBase
{

    protected ServiceTrackerBase serviceTracker;  // This gameObject's service tracker

    [Tooltip("Material to set sphere to when showing area of effect")]
    public Material serviceMaterial;

    // Display variables
    string type;
    int demand;
    int amount;
    int numBuildings;
    float cost;

    Text typeText;
    Text demandText;
    Text amountText;
    Text numBuildingsText;
    Text costText;

    protected override void Start()
    {
        base.Start();

        serviceTracker = GetComponent<ServiceTrackerBase>();
    }

    public override void EnableTooltip(Vector3 stareat)
    {
        base.EnableTooltip(stareat);

        if(!serviceTracker.SphereAttached())
        {
            serviceTracker.AttachSphere();
        }
        serviceTracker.sphereScript.SetSphereMaterial(serviceMaterial);
    }

    public override void DisableTooltip()
    {
        base.DisableTooltip();

        serviceTracker.DetachSphere();
    }

    protected override void UpdateValues()
    {
        type = serviceTracker.type;
        demand = ServiceBase.FindDemand(type);
        amount = serviceTracker.amount;
        numBuildings = serviceTracker.numSurroundingBuildings;
        cost = serviceTracker.cost;

        base.UpdateValues();
    }

    protected override void UpdateText()
    {
        typeText.text = "" + type;
        demandText.text = "Overall Demand: " + demand;
        amountText.text = "Maximum Capacity: " + amount;
        costText.text = "Weekly Cost: $" + cost;
        numBuildingsText.text = "Buildings Covered: " + numBuildings;
    }

    protected override void UpdateReferences()
    {
        typeText = tooltip.transform.Find("Canvas/Type").GetComponent<Text>();
        demandText = tooltip.transform.Find("Canvas/Demand").GetComponent<Text>();
        amountText = tooltip.transform.Find("Canvas/Amount").GetComponent<Text>();
        numBuildingsText = tooltip.transform.Find("Canvas/NumBuildings").GetComponent<Text>();
        costText = tooltip.transform.Find("Canvas/Cost").GetComponent<Text>();
    }
}
