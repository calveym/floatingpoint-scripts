using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EconomyManager : MonoBehaviour {

    // Declare other managers
    [SerializeField]
    ItemManager itemManager;
    [SerializeField]
    PopulationManager populationManager;
    [SerializeField]
    HappinessManager happinessManager;
    [SerializeField]
    AudioManager audioManager;

    // Declare variables
    [Header("Money")]
    [Range(0.0001f, 1)]
    [Tooltip("Multiplier for income to get per tick income")]
    public float incomeMultiplier = 0.1f;
    public float balance = 0;
    [SerializeField]
    int numRoads;
    [SerializeField]
    float income; // Net income, after expenses
    [SerializeField]
    int population;
    [SerializeField]
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
    [Tooltip("Total goods available")]
    public float goods;
    [Tooltip("Allow importing goods to make up for defecit")]
    public bool enableImport;
    [Tooltip("Allow exporting goods to make money from surplus")]
    public bool enableExport;
    [Tooltip("Maximum amount of goods that can be imported or exported, before bonuses")]
    public float maxTransfer;

    // Tracked items
    [SerializeField]
    int residentialCap;
    [SerializeField]
    int commercialCap;
    [SerializeField]
    int industrialCap;

    // Declares public variables
    [Space(10)]

    [Header("Taxes")]
    [Space(5)]
    public int residentialTaxRate;
    public int commercialTaxRate;
    public int industrialTaxRate;
    [Range(0, 1000)]
    [Tooltip("Initial income, added every ecoTick to income")]
    public int rawIncome = 200; // Gross
    bool keepUpdating;
    [Space(10)]
    [Header("Services")]
    [Space(5)]
    [SerializeField]
    [Tooltip("[READ ONLY]  Total expenses for running all services")]
    float serviceExpenses;
    [SerializeField]
    [Tooltip("[READ ONLY]  Total expenses for running power services")]
    float powerExpenses;
    [SerializeField]
    [Tooltip("[READ ONLY]  Total expenses for running education services")]
    float educationExpenses;
    [SerializeField]
    [Tooltip("[READ ONLY]  Total expenses for running health services")]
    float healthExpenses;
    [SerializeField]
    [Tooltip("[READ ONLY]  Total expenses for running police services")]
    float policeExpenses;
    [SerializeField]
    [Tooltip("[READ ONLY]  Total expenses for running fire services")]
    float fireExpenses;


    [SerializeField]
    [Tooltip("[READ ONLY]  Production multiplier, influenced by power availability")]
    float productionMultiplier;
    [Space(10)]
    [Header("Audio")]
    [Space(5)]
    public AudioClip purchaseSound;
    public AudioClip failSound;

    [DoNotSerialize]
    bool ticking = false;
    [DoNotSerialize]
    bool isDeserializing = false;

    int tick; // used to run once per second

    void Awake()
    // Finds instances of all objects and sets up values
    {
        residentialTaxRate = 15;
        commercialTaxRate = 15;
        industrialTaxRate = 15;
        income = rawIncome;
        keepUpdating = true;
    }

    private void Start()
    {
        if(!LevelSerializer.IsDeserializing)
        {
            tick = 0;
            itemManager = ReferenceManager.instance.itemManager;
            populationManager = ReferenceManager.instance.populationManager;
            happinessManager = ReferenceManager.instance.happinessManager;
            audioManager = ReferenceManager.instance.audioManager;
            CheckCoroutines();
        }
    }

    void Update()
    {
        if (keepUpdating && tick % 60 == 0)
            {
                ticking = true;
                Debug.Log("Running: " + itemManager);
                if (ecoTick != null)
                {
                    ecoTick();
                }
                if (foliageTick != null)
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
            }
        tick++;
    }

    [SerializeField]
    public delegate void TickDelegate();

    [SerializeField]
    public static TickDelegate ecoTick;  // Multicast delegate run once per economic tick
    [SerializeField]
    public static TickDelegate foliageTick;  // Multicast delegate for foliage, extracted to reduce potential crashes

    IEnumerator EconomicTick()
    {
        yield return new WaitForSeconds(0.5f);

    }

    void CheckCoroutines()
    {
        if(!ticking)
        {
            keepUpdating = true;
            ticking = false;
            StartCoroutine("EconomicTick");
        }
    }

    void TransferGoods()
    {
        UpdateGoods();
        goods += IndustrialTracker.allGoods * productionMultiplier;
        IndustrialTracker.allGoods = 0;

        if (goods < goodsConsumption && enableImport)
        {
            transferQueued = true;
            PrepareImport();
        }
        else if ((goods / 2) >= goodsConsumption && enableExport)
        {
            transferQueued = true;
            PrepareExport();
        }
        if (transferQueued)
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
        if (balance <= import * 1.25f)
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
        balance += income * incomeMultiplier;
    }

    void UpdateIncome()
    // Recalculates income
    {
        float roadExpenses = CalculateRoadExpenses();
        float residentialIncome = CalculateResidentialIncome();
        float commercialIncome = CalculateCommercialIncome();
        float industrialIncome = CalculateIndustrialIncome();
        serviceExpenses = powerExpenses + educationExpenses + healthExpenses + policeExpenses + fireExpenses;

        float expenses = roadExpenses + CalculateCapacityExpenses() + serviceExpenses;
        income = (rawIncome + residentialIncome + commercialIncome + industrialIncome - expenses);
    }

    public void SetPowerExpense(float newPowerExpenses)
    {
        powerExpenses = newPowerExpenses;
    }

    public void SetEducationExpense(float newEducationExpenses)
    {
        educationExpenses = newEducationExpenses;
    }

    public void SetHealthExpense(float newHealthExpenses)
    {
        healthExpenses = newHealthExpenses;
    }

    public void SetPoliceExpense(float newPoliceExpenses)
    {
        policeExpenses = newPoliceExpenses;
    }

    public void SetFireExpense(float newFireExpenses)
    {
        fireExpenses = newFireExpenses;
    }

    public void SetProduction(float newProduction)
    {
        productionMultiplier = newProduction;
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

    public void MakePurchase(float amount)
    // Modifies balance by "amount", plays sound
    {
        balance += amount;
        ReferenceManager.instance.audioManager.PlaySingle(purchaseSound);
    }

    public void FailedPurchase()
    {
        ReferenceManager.instance.audioManager.PlaySingle(failSound);
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
