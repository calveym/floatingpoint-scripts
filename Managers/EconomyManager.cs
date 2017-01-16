using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EconomyManager : MonoBehaviour {


	ItemManager itemManager;
	PopulationManager populationManager;

	float balance;
	public int rawIncome; // Gross income
	int numRoads;
	float income; // Net income, after expenses
	int population;
	int buildings;

	void Awake () 
	{
		balance = 1000;
		itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();
		populationManager = GameObject.Find("Managers").GetComponent<PopulationManager>();
	}

	void Start () 
	{
		income = rawIncome;
	}

	void Update () 
	{
		numRoads = itemManager.getNumRoads();
		getPopulation();

		updateBalance();
		updateIncome();
	}

	void updateBalance() 
	// Reduces balance by income and time
	{
		balance += income * Time.deltaTime;
	}

	void updateIncome()
	// Recalculates income
	{
		float roadExpenses = calculateRoadExpenses();
		float populationIncome = calculatePopulationIncome();

		income = rawIncome + populationIncome - roadExpenses;
	}

	float calculatePopulationIncome()
	// TODO: Make a more interesting algorithm here
	{
		return population;
	}

	float calculateRoadExpenses() 
	// Calculates how much is spent on road maintenance
	{
		float rawRoadIncome = numRoads / 5;

		return rawRoadIncome;
	}

	void reduceBalance(float amount)
	// Decreases balance by "amount"
	{
		balance -= amount;
	}

	void getPopulation()
	// Retrieves population from the pop manager
	{
		population = populationManager.population;
	}
}
