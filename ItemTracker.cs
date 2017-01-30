using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTracker : MonoBehaviour {

    // Declare variables
    public string type;
    public int capacity;

    private void Update()
    {
        if(transform.parent == null)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;
        }
    }

    public void setValues(string typeInput, int pressedButton)
    // Type accessor
    {
        type = typeInput;
		capacity = pressedButton;
    }
}
