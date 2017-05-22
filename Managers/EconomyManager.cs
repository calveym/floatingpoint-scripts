using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EconomyManager : MonoBehaviour {

    // Declare other managers
	public ItemManager itemManager;
	public PopulationManager populationManager;
    public HappinessManager happinessManager;

    // Declare variables
	float balance;
	int numRoads;
	float income; // Net income, after expenses
	int population;
    float happiness;

    [SerializeField]
    float goodsProduction;
    [SerializeField]
    float goodsConsumption;
    [SerializeField]
    bool transferQueued;
    float export;
    float import;
    float netGoodsTransfered;

    [Header("Goods")]
    public float goods;
    public float historicGoods;
    [Tooltip("Allow importing goods to make up for defecit")]
    public bool enableImport;
    [Tooltip("Allow exporting goods to make money from surplus")]
    public bool enableExport;
    [Tooltip("Maximum amount of goods that can be imported or exported")]
    public float maxTransfer;

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
        income = rawIncome;
        keepUpdating = true;
    }

    private void Start()
    {
        itemManager = ReferenceManager.instance.itemManager;
        populationManager = ReferenceManager.instance.populationManager;
        happinessManager = ReferenceManager.instance.happinessManager;
        StartCoroutine("EconomicTick");
    }

    public delegate void TickDelegate();

    public static TickDelegate ecoTick;  // Multicast delegate run once per economic tick
    public static TickDelegate foliageTick;  // Multicast delegate for foliage, extracted to reduce potential crashes

    IEnumerator EconomicTick()
    {
        while (keepUpdating)
        {

            if (ecoTick != null)
            {
                ecoTick();
            }
            if(foliageTick != null)
            {
                foliageTick();
            }

            ResidentialTracker.historicResidentialIncome = ResidentialTracker.totalResidentialIncome;
            CommercialTracker.historicCommercialIncome = CommercialTracker.totalCommercialIncome;
            IndustrialTracker.historicIndustrialIncome = IndustrialTracker.totalIndustrialIncome;

            UpdateIncome();
            TransferGoods();
            UpdateBalance();

            ResidentialTracker.totalResidentialIncome = 0;
            CommercialTracker.totalCommercialIncome = 0;
            IndustrialTracker.totalIndustrialIncome = 0;

            yield return new WaitForSeconds(2);
        }
    }

    void TransferGoods()
    {
        UpdateGoods();
        goods += IndustrialTracker.allGoods;
        IndustrialTracker.allGoods = 0;

        if(goods < goodsConsumption && enableImport)
        {
            transferQueued = true;
            PrepareImport();
        }
        else if((goods / 2) >= goodsConsumption && enableExport)
        {
            transferQueued = true;
            PrepareExport();
        }
        if(transferQueued)
            DoGoodsTransfer();
    }

    void UpdateGoods()
    {
        goodsConsumption = itemManager.residentialCap * happinessManager.happiness;
        goodsProduction = itemManager.industrialCap * happinessManager.happiness;
    }

    void DoGoodsTransfer()
    {
        transferQueued = false;
        goods -= export;
        goods += import;

        income -= import * 1.25f;
        income += export * 0.75f;
    }

    void PrepareImport()
    // Imports required amount
    {
        import = goodsConsumption - goods;
        if(balance <= import * 1.25f)
        {
            import = 0;
        }        
    }

    void PrepareExport()
    // Exports maximum amount
    {
        import = 0;
        export = goodsProduction - goodsConsumption;
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
		balance += income * Time.deltaTime * 100;
	}

	void UpdateIncome()
	// Recalculates income
	{
		float roadExpenses = CalculateRoadExpenses();
		float residentialIncome = CalculateResidentialIncome();
        float commercialIncome = CalculateCommercialIncome();
        float industrialIncome = CalculateIndustrialIncome();

        float expenses = roadExpenses + CalculateCapacityExpenses();
        income = (rawIncome + residentialIncome + commercialIncome + industrialIncome - expenses);
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

    public void SellGoods(float numGoods)
    {
        goods -= numGoods;
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

    public string FancyIncome()
    {
        return "+ " + income.ToString();
    }

    public string FancyBalance()
    {
        return "Balance: $" + Mathf.Round(balance).ToString();
    }
    
    public string FancyGoods()
    {
        return "Goods produced: " + goods;
    }

    public string FancyGoodsConsumed()
    {
        return "Goods consumed: ";
    }
}
