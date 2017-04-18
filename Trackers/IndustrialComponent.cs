using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndustrialComponent : ItemTracker {

    public float goodsCapacity;
    public float sellAmount;
    public float sellPrice;
    public float productionAmount;
    //  See IndustrialTracker for definitions

    GameObject linkedBuilding;
    IndustrialTracker linkedTracker;
    GameObject tempLinkedBuilding;
    IndustrialTracker tempLinkedTracker;

    public float cost;  // Upfront purchase cost

    public void FoundIndustrial(GameObject found)
    // Runs every time new industrial building is found
    {
        tempLinkedBuilding = found;
        tempLinkedTracker = found.GetComponent<IndustrialTracker>();

        tempLinkedTracker.AddMarker();
    }

	public void LinkComponent(IndustrialTracker tracker)
    // Links component to tracker
    {
        linkedBuilding = tracker.gameObject;
        linkedTracker = tracker;
        tracker.LinkComponent(this);
    }
}
