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
    float happiness;

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
        residentialTaxRate = 15;
        commercialTaxRate = 15;
        industrialTaxRate = 15;
        rawIncome = 25;
		balance = 1000;
		itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();
		populationManager = GameObject.Find("Managers").GetComponent<PopulationManager>();
        income = rawIncome;
    }

	void Update ()
    // Updates state and recalculates balance and income
	{
		numRoads = itemManager.getNumRoads();
		setPopulation();
        setCapacity();
   
		updateBalance();
		updateIncome();
	}

    void setCapacity()
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
        return population * (1 + 0.01f * residentialTaxRate);
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
            return commercialCap * (1 + 0.01f * commercialTaxRate);
        }
        else
        {
            return population * (1 + 0.01f * commercialTaxRate);
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
            return industrialCap * (1 + 0.01f * industrialTaxRate);
        }
        else
        {
            return population * (1 + 0.01f * industrialTaxRate);
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

	void setPopulation()
	// Retrieves population from the pop manager
	{
		population = populationManager.population;
	}

    public float GetBalance()
    {
        return balance;
    }

    public float GetIncome()
    {
        return income;
    }

    public int GetPopulation()
    {
        return population;
    }

    public float GetHappiness()
    {
        return happiness;
    }
}
