using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ServiceTrackerBase : SphereObject {


    [Tooltip("Capacity of Service")]
    [Range(100, 20000)]
    public int amount;  // Amount of effect of service
    [Tooltip("Type of service [power/health/police/education/fire]")]
    public string type;  // Type of service, used in logic
    [Range(0, 10000)]
    [Tooltip("EcoTick cost of service")]
    public int cost;
    [Tooltip("Initial purchase cost of service")]
    public int buyCost;
    [Tooltip("Level at which this building is unlocked")]
    public int level;
    public Material sphereMaterial;

    protected List<GameObject> surroundingBuildings;
    public int numSurroundingBuildings;

    protected override void Start () {
        base.Start();

        economyManager = ReferenceManager.instance.economyManager;
 	}

    protected virtual void DoEffect()
    {
        surroundingBuildings = U.FindNearestBuildings(transform.position, radius);
        numSurroundingBuildings = surroundingBuildings.Count - 1;  // Minus one to remove the tracker itself
    }

    public virtual void AddService()
    {
        EconomyManager.ecoTick += PayForService;
    }

    protected abstract void PayForService();  // Adds price to relevant manager in overrider class
}
