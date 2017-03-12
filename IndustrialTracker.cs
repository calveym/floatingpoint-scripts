using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndustrialTracker : ItemTracker {
// Manages individual stats of each industrial building.

    public int visitors;
    public int lifetimeVisitors;

    public int goodsProduced;
    public int goodsConsumed;

    public void TryGiveJob()
    {
        AddUsers(1);
    }
}
