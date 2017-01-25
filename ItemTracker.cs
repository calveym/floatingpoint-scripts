using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTracker : MonoBehaviour {

    // Declare variables
    public string type;
    public int capacity;

    public void setValues(string typeInput, int pressedButton)
    // Type accessor
    {
        type = typeInput;
		capacity = pressedButton;
    }
}
