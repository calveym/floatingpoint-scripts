using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappinessManager : MonoBehaviour {

    public int numSent;
    public float addedHappiness;
    public float happiness;

    bool keepUpdating;

    public void SendHappiness(int newHappiness)
    {
        addedHappiness += newHappiness;
        numSent++;
    }

    void Start()
    {
        happiness = 0;
        keepUpdating = true;

Autelia.Coroutines.CoroutineController.StartCoroutine(this, "UpdateHappiness");
    }

    IEnumerator UpdateHappiness()
    {
        while(keepUpdating)
        {
            if (numSent != 0)
            {
                happiness = addedHappiness / numSent;
            }
            else happiness = 1;

            numSent = 0;
            addedHappiness = 0;
            yield return new WaitForSeconds(2);
        }
    }
}








