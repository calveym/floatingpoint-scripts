using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;

public abstract class ComponentSnap : SphereObject {

    [Range(0, 10000)]
    [Tooltip("Component purchase cost")]
    public int cost;  // Purchase cost of component

    protected override void Grab()
    {
        base.Grab();

        Purchase();
    }

    protected override void Ungrab()
    {
        base.Ungrab();

        InitiateSnap();
        // method overriden in derived class to start effect here
    }

    void InitiateSnap()
    {
        // TODO
    }

    void Purchase()
    // Deduct one off purchase cost
    {
        economyManager.MakePurchase(cost);
    }
}
