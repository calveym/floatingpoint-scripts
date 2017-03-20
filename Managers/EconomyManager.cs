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
	public float goods;
	public float historicGoods;

    // Tracked items
    int residentialCap;
    int commercialCap;
    int industrialCap;

    // Declares public variables
    public int residentialTaxRate;
    public int commercialTaxRate;
    public int industrialTaxRate;
    public int rawIncome; // Gross
    bool keepUpdating;

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
        keepUpdating = true;
        StartCoroutine("SlowUpdate");
    }

    void LateUpdate()
    // Resets total income for next frame
    {

    }

    IEnumerator SlowUpdate()
    {
        while (keepUpdating)
        {
            updateBalance();
            updateIncome();

            ResidentialTracker.historicResidentialIncome = ResidentialTracker.totalResidentialIncome;
            CommercialTracker.historicCommercialIncome = CommercialTracker.totalCommercialIncome;
            IndustrialTracker.historicIndustrialIncome = IndustrialTracker.totalIndustrialIncome;

			goods = IndustrialTracker.allGoods;

			IndustrialTracker.allGoods = 0;
            ResidentialTracker.totalResidentialIncome = 0;
            CommercialTracker.totalCommercialIncome = 0;
            IndustrialTracker.totalIndustrialIncome = 0;
            yield return new WaitForSeconds(1);
        }
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
        return ResidentialTracker.totalResidentialIncome * (1 + 0.05f * residentialTaxRate);
	}

  float calculateCommercialIncome()
  // Tax income for all commercial buildings
  {
      if (commercialCap == 0 || population == 0)
      {
          return 0;
      }
      else
      {
          return CommercialTracker.totalCommercialIncome * (1 + 0.05f * commercialTaxRate);
      }
  }

  float calculateIndustrialIncome()
  // Tax income for industrial buildings
  {
      if (industrialCap == 0)
      {
          return 0;
      }
      else
      {
          return IndustrialTracker.totalIndustrialIncome * (1 + 0.05f * industrialTaxRate);
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
