using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappinessManager : MonoBehaviour {

    public int numSent;
    public float addedHappiness;
    public float happiness;

    bool keepUpdating = true;

    public void SendHappiness(int newHappiness)
    {
        addedHappiness += newHappiness;
    }

    void Start()
    {
        StartCoroutine("UpdateHappiness");
    }

    IEnumerator UpdateHappiness()
    {
        while(keepUpdating)
        {
            happiness = addedHappiness / numSent;
            numSent = 0;
            addedHappiness = 0;
            yield return new WaitForSeconds(5);
        }
    }
}








