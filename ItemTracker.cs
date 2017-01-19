using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTracker : MonoBehaviour {

    // Declare variables
    public string type;
    public int capacity;

    public void addType(string typeInput)
    // Type accessor
    {
        type = typeInput;
    }

    public void addCapacity(int capInput)
    // Capacity accessor
    {
        capacity = capInput;
    }
}
 