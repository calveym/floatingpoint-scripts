using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EconomyManager : MonoBehaviour {


	ItemManager itemManager;
	PopulationManager populationManager;

	float balance;
	public int rawIncome;
	int numRoads;
	float income;
	int population;
	int buildings;

	void Awake () 
	{
		balance = 1000;
		itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();
		populationManager = GameObject.Find("Managers").GetComponent<PopulationManager>();
	}

	// Use this for initialization
	void Start () 
	{
		income = rawIncome;
	}
	
	// Update is called once per frame
	void Update () 
	{
		numRoads = itemManager.getNumRoads();
		getPopulation();

		updateBalance();
		updateIncome();
	}

	void updateBalance() 
	{
		balance += income * Time.deltaTime;
	}

	void updateIncome()
	{
		float roadExpenses = calculateRoadExpenses();
		float populationIncome = calculatePopulationIncome();

		income = rawIncome + populationIncome - roadExpenses;
	}

	float calculatePopulationIncome()
	{
		return population;
	}

	float calculateRoadExpenses() 
	{
		float rawRoadIncome = numRoads / 5;

		return rawRoadIncome;
	}

	void reduceBalance(float amount)
	{
		balance -= amount;
	}

	void getPopulation()
	{
		population = populationManager.population;
	}
}
