using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTracker : MonoBehaviour {

    public string type;
    public int capacity;

    public void addType(string typeInput)
    {
        type = typeInput;
    }

    public void addCapacity(int capInput)
    {
        capacity = capInput;
    }
}
