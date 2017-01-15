using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour {


	DemandManager demandManager;
	ItemManager itemManager;

	float newPopulationSpawn;
	float populationIncreaseRate;
	float residentialDemand;
	int maxPopulation;

	public int population;

	void Awake () 
	// Set values pre-initialization
	{
		population = 0;
		populationIncreaseRate = 0;

		itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();
		demandManager = GameObject.Find("Managers").GetComponent<DemandManager>();
	}
	
	void Update ()
	// Updates values & calls increasePopulation if conditions are met
	{
		maxPopulation = itemManager.getMaxPop();
		getDemand();

		updatePopulationIncreaseRate();
		newPopulationSpawn += populationIncreaseRate * Time.deltaTime;

		if(newPopulationSpawn >= 1 && population < maxPopulation)
		{
			updatePopulation(Mathf.RoundToInt(newPopulationSpawn));
		} else if(newPopulationSpawn <= -1)
		{
			updatePopulation(Mathf.RoundToInt(newPopulationSpawn));
		}
	}

	void updatePopulation(int number)
	// Increments population by number
	{
		population += number;
		demandManager.incrementResidential(number);
	}

	void getDemand()
	{
		residentialDemand = demandManager.getDemand("residential");
	}

	void updatePopulationIncreaseRate()
	{
		populationIncreaseRate = residentialDemand;
	}
}
