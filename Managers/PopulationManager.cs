using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour {


	DemandManager demandManager;
	ItemManager itemManager;
    HappinessManager happinessManager;

	float newPopulationSpawn;
	float populationIncreaseRate;
	float residentialDemand;
	int maxPopulation;
    float happiness;

    public int population;
    int residentialCap;
    int industrialCap;
    int commercialCap;

	void Awake () 
	// Set values pre-initialization
	{
		population = 0;
		populationIncreaseRate = 0;

		itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();
        happinessManager = GameObject.Find("Managers").GetComponent<HappinessManager>();
		demandManager = GameObject.Find("Managers").GetComponent<DemandManager>();
	}
	
	void Update ()
	// Updates values & calls increasePopulation if conditions are met
	{   
        if (population < itemManager.getMaxPop())
        {
            IncrementPopulation();
        }
	}

    void IncrementPopulation()
    {
        happiness = happinessManager.happiness;
        residentialDemand += happiness * Time.deltaTime;
    if (residentialDemand >= 1 )
        {
            ++population;
            --residentialDemand;
        }
    }

    public int AvailableResidential()
    {
        return residentialCap - population;
    }
}
