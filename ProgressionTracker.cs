using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ProgressionTracker : MonoBehaviour {

    public bool usable;

    public void Enable()
    {
        usable = true;
        if(gameObject.GetComponent<ItemTracker>().grabbableObject == true)
        {
            gameObject.GetComponent<VRTK_InteractableObject>().isGrabbable = true;
        }
    }

}
