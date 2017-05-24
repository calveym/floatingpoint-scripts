using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : Service {
    

    // Use this for initialization
    protected override void Start () {
        base.Start();
        serviceUpdater += ApplyEffect;
	}

    void ApplyEffect()
    // Checks amount against demand, applies relevant bonuses
    {
        if(amount < demand)
        {
            ApplyGlobalDefecit();
        }
        else if(demand < amount)
        {
            ApplyGlobalSurplus();
        }
    }

    void ApplyGlobalDefecit()
    {

    }

    void ApplyGlobalSurplus()
    {

    }
}
