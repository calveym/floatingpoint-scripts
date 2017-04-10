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
        StartCoroutine("EconomicTick");
    }

    public delegate void TickDelegate();

    public static TickDelegate ecoTick;  // Multicast delegate run once per economic tick

    IEnumerator EconomicTick()
    {
        while (keepUpdating)
        {
            ecoTick();

            ResidentialTracker.historicResidentialIncome = ResidentialTracker.totalResidentialIncome;
            CommercialTracker.historicCommercialIncome = CommercialTracker.totalCommercialIncome;
            IndustrialTracker.historicIndustrialIncome = IndustrialTracker.totalIndustrialIncome;

            TransferGoods();
            UpdateIncome();
            UpdateBalance();


            ResidentialTracker.totalResidentialIncome = 0;
            CommercialTracker.totalCommercialIncome = 0;
            IndustrialTracker.totalIndustrialIncome = 0;

            yield return new WaitForSeconds(1);
        }
    }

    void TransferGoods()
    {
        goods = IndustrialTracker.allGoods;
        IndustrialTracker.allGoods = 0;
    }


    void SetCapacity()
    // Updates capacity from itemManager
    {
        residentialCap = itemManager.residentialCap;
        commercialCap = itemManager.commercialCap;
        industrialCap = itemManager.industrialCap;
    }

	void UpdateBalance()
	// Reduces balance by income and time
	{
		balance += income * Time.deltaTime;
	}

	void UpdateIncome()
	// Recalculates income
	{
		float roadExpenses = CalculateRoadExpenses();
		float residentialIncome = CalculateResidentialIncome();
        float commercialIncome = CalculateCommercialIncome();
        float industrialIncome = CalculateIndustrialIncome();

        float expenses = roadExpenses + CalculateCapacityExpenses();
		income = rawIncome + residentialIncome + commercialIncome + industrialIncome - expenses;
	}

    float CalculateCapacityExpenses()
    // Returns all expenses from capacity
    {
        return residentialCap + commercialCap + industrialCap;
    }

	float CalculateResidentialIncome()
	// Tax income from all residential properties
	{
        return ResidentialTracker.totalResidentialIncome * (1 + 0.05f * residentialTaxRate);
	}

	  float CalculateCommercialIncome()
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

	  float CalculateIndustrialIncome()
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

	float CalculateRoadExpenses()
	// Calculates how much is spent on road maintenance
	{
		return numRoads / 5;
	}

	public static void ChangeBalance(float amount)
	// Modifies balance by "amount"
	{
		GameObject.Find("Managers").GetComponent<EconomyManager>().balance += amount;
	}

	void SetPopulation()
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
