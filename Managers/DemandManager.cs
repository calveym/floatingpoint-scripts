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
	
	void Update () 
	{
		updateDemand();
	}

	public float getDemand(string type)
	// Returns demand, for use by other classes
	{
		if(type == "residential")
		{
			return residentialDemand;
		} else if(type == "commercial")
		{
			return commercialDemand;
		} else if(type == "industrial")
		{
			return industrialDemand;
		} else 
		{
		return 0f;
		}
	}

	void updateDemand()
	// Creates new demand values. TODO: do this.
	{
		
	}

	public void incrementResidential(int number)
	// Updates residential demand after population change
	{
		if(number > 0)
		{
			residentialDemand -= 0.1f;
		} else if(number < 0)
		{
			residentialDemand += 0.1f;
		}
	}
}
