using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ServiceTrackerBase : SphereObject {


    [Tooltip("Capacity of Service")]
    [Range(0, 100)]
    public int amount;  // Amount of effect of service
    [Tooltip("Type of service [power/health/police/education/fire]")]
    public string type;  // Type of service, used in logic
    [Range(-50, 50)]
    [Tooltip("EcoTick cost of service")]
    public float cost;

    protected List<GameObject> surroundingBuildings;
    public int numSurroundingBuildings;

    protected override void Start () {
        base.Start();

        economyManager = ReferenceManager.instance.economyManager;
        EconomyManager.ecoTick += PayForService;
 	}

    protected virtual void DoEffect()
    {
        surroundingBuildings = U.FindNearestBuildings(transform.position, radius);
        numSurroundingBuildings = surroundingBuildings.Count - 1;  // Minus one to remove the tracker itself
    }

    protected abstract void AddService();

    protected abstract void PayForService();  // Adds price to relevant manager in overrider class
}
