using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemandManager : MonoBehaviour {

	float residentialDemand;
	float commercialDemand;
	float industrialDemand;

	void Start ()
	{
		residentialDemand = 1;
		commercialDemand = 1;
		industrialDemand = 1;
	}

    void Update()
    {
        // get the latest tracking information, then check residential demand - 
    } 

    void SetResidentialDemand()
    {

    }
}
