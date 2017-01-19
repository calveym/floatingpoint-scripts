using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EconomyManager : MonoBehaviour {

    // Declare other managers
	ItemManager itemManager;
	PopulationManager populationManager;

    // Declare variables
	float balance;
	int numRoads;
	float income; // Net income, after expenses
	int population;

    // Tracked items
    int residentialCap;
    int commercialCap;
    int industrialCap;
	
    // Declares public variables 
    public int residentialTaxRate;
    public int commercialTaxRate;
    public int industrialTaxRate;
    public int rawIncome; // Gross income

    void Awake ()
    // Finds instances of all objects and sets up values
    {
        residentialTaxRate = 5;
        commercialTaxRate = 5;
        industrialTaxRate = 5;
        rawIncome = 5;
		balance = 1000;
		itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();
		populationManager = GameObject.Find("Managers").GetComponent<PopulationManager>();
        income = rawIncome;
    }

	void Update ()
    // Updates state and recalculates balance and income
	{
		numRoads = itemManager.getNumRoads();
		getPopulation();
        getCapacity();
   
		updateBalance();
		updateIncome();
	}

    void getCapacity()
    // Updates capacity from itemManager
    {
        residentialCap = itemManager.residentialCap;
        commercialCap = itemManager.commercialCap;
        industrialCap = itemManager.industrialCap;
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
		float residentialIncome = calculateResidentialIncome();
        float commercialIncome = calculateCommercialIncome();
        float industrialIncome = calculateIndustrialIncome();

        float expenses = roadExpenses + calculateCapacityExpenses();
		income = rawIncome + residentialIncome + commercialIncome + industrialIncome - expenses;
	}

    float calculateCapacityExpenses()
    // Returns all expenses from capacity
    {
        return residentialCap + commercialCap + industrialCap;
    }

	float calculateResidentialIncome()
	// Tax income from all residential properties
	{
        if(residentialCap == 0)
        {
            return 0;
        } else
        {
            return population * residentialTaxRate;
        }
	}

    float calculateCommercialIncome()
    // Tax income for all commercial buildings
    {
        if (commercialCap == 0 || population == 0)
        {
            return 0;
        }
        else if(population >= commercialCap)
        {
            return commercialCap * commercialTaxRate;
        }
        else
        {
            return population * commercialTaxRate;
        }
    }

    float calculateIndustrialIncome()
    // Tax income for industrial buildings
    {
        if (industrialCap == 0)
        {
            return 0;
        }
        else if (population >= industrialCap)
        {
            return commercialCap * industrialTaxRate;
        }
        else
        {
            return population * industrialTaxRate;
        }
    }

	float calculateRoadExpenses() 
	// Calculates how much is spent on road maintenance
	{
		return numRoads / 5;
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
