using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndustrialComponent : ItemTracker {

    public float goodsCapacityMulti;
    public float sellAmountMulti;
    public float sellPriceMulti;
    public float productionMulti;
    //  See IndustrialTracker for definitions

    GameObject linkedBuilding;
    IndustrialTracker linkedTracker;
    GameObject tempLinkedBuilding;
    IndustrialTracker tempLinkedTracker;

    new void Start()
    {
        base.Start();
    }

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
