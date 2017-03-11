using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ink : MonoBehaviour {

    // Use this for initialization
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "PenTip")
        {
            DrawableSurface.drawable = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "PenTip")
        {
            DrawableSurface.drawable = true;
        }
    }
}
