using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndustrialTracker : ItemTracker {
    // Manages individual stats of each industrial building.

    int requiredProduction = 5;

    GameObject markerPrefab;
    Marker marker;
    List<IndustrialComponent> components;
    List<float> sales; // List of recent sales counted in goodsSold

    public float productionHappiness; // Happiness from reaching sales targets

    public int visitors;
    public int lifetimeVisitors;

    public float goodsCapacity;  // max amount of goods storeable
    public float goodsProduced;  // Goods produced in last economic tick
    public static float allGoods; // Base goods tracking figure for each economic tick

    public float sellPrice;  // Base sell price
    public float sellAmount;  // Base maximum amount sold per economy tick

    // Multipliers from components
    public float productionMulti;
    public float goodsCapacityMulti;
    public float sellPriceMulti;
    public float sellAmountMulti;

    float goodsSold;  // Number of goods sold last week

    void Awake()
    {
        sales = new List<float>();
        components = new List<IndustrialComponent>();
        goodsSold = 0;
        productionMulti = 1;
        goodsCapacityMulti = 1;
        sellPriceMulti = 1;
        sellAmountMulti = 1;
    }

    new void Start()
    {
        base.Start();
        markerPrefab = GameObject.Find("MarkerPrefab");
    }

    void Update()
    {
        if (!updateStarted && usable)
        {
            updateStarted = true;
            EconomyManager.ecoTick += UpdateSecond;
            GameObject.Find("Managers").GetComponent<ItemManager>().addIndustrial(capacity, gameObject);
        }
        else if (updateStarted && !usable)
        {
            updateStarted = false;
            EconomyManager.ecoTick -= UpdateSecond;
        }
    }

    void ProduceGoods()
    {
        goodsProduced = users * happinessState * productionMulti;

        income = goodsProduced * sellPrice * sellPriceMulti;
        income -= baseCost;
        allGoods += goodsProduced;
    }

    public void Apply(float applicantLandValue, int residentID, ResidentialTracker applicantTracker)
    {
        System.Random rand = new System.Random(); //reuse this if generating many
        double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
        double u2 = 1.0 - rand.NextDouble();
        double randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log((float)u1)) *
                     Mathf.Sin(2.0f * Mathf.PI * (float)u2); //random normal(0,1)
        double randNormal =
                     landValue - 5 + 5 * randStdNormal; //random normal(mean,stdDev^2)
        if (usable && users < capacity) // TODO: ADD LAND VALUE CHECKS BACK IN
        {
            AcceptApplication(residentID, applicantTracker);
        }
        else RejectApplication(residentID, applicantTracker);
    }

    void AcceptApplication(int residentID, ResidentialTracker applicantTracker)
    {
        if (!applicantTracker.IsEmployed(residentID))
        {
            AddUsers(1);
            applicantTracker.AcceptApplication(residentID);
        }
        else RejectApplication(residentID, applicantTracker);
    }

    void RejectApplication(int residentID, ResidentialTracker applicantTracker)
    {
        // TODO:
        Debug.Log("Applicant rejected!!!");
    }

    public void AddMarker()
    {
        marker = Instantiate(markerPrefab, transform.position + new Vector3(0f, 2f, 0f), transform.rotation, transform).GetComponent<Marker>();
        marker.transform.localScale *= 10;
        marker.StartRotation();
    }

    public void LinkComponent(IndustrialComponent component)
    // Sent from component, completes link
    {
        Debug.Log("Gettin linked eh@ " + gameObject.name);
        components.Add(component);
        RecalculateComponents();
    }

    void RecalculateComponents()
    {
        for(int i = 0; i < components.Count; i++)
        {
            if (components[i].type == "sellPrice" || components[i].type == "productionAmount" || components[i].type == "goodsCapacity" || components[i].type == "sellAmount")
            {
                AddBonus(components[i]);
            }
        }
    }

    void AddBonus(IndustrialComponent component)
    {
        sellPriceMulti += component.sellPriceMulti;
        productionMulti += component.productionMulti;
        goodsCapacityMulti += component.goodsCapacityMulti;
        sellAmountMulti += component.sellAmountMulti;
    }

    void UpdateProductionHappiness()
    {
        if (capacity != 0 && requiredProduction != 0)
        {
            productionHappiness = (goodsProduced / capacity * requiredProduction) * 40;
            if (productionHappiness > 40)
            {
                productionHappiness = 40; // Capped at 40
            }
        }
    }

    void UpdateSecond()
    // Updates values once per second, economic tick
    {
        updateStarted = true;
        if (!usable || !validPosition)
        {
            return;
        }
        UpdateLocalHappiness();
        UpdateProductionHappiness();
        UpdateHappiness();
        UpdateLandValue();
        UpdateTransportationValue();
        ProduceGoods();
        totalIndustrialIncome += income;
    }

    void UpdateHappiness()
    // Performs all necessary final happiness calculations, including longterm
    {
        currentHappiness = localHappiness + productionHappiness + fillRateHappiness;
        CalculateLongtermHappiness();
        CalculateHappinessState();
    }

    public string ValidPosition()
    {
        if (validPosition)
        {
            return "Active";
        }
        else return "Inactive";
    }

    public string FancyIncome()
    {
        return "Income: $" + Mathf.Round(income * 100) / 100 + "/w";
    }

    public string FancyCapacity()
    {
        return "Workers: " + users + " / " + capacity;
    }

    public int FancyHappiness()
    {
        return happinessState;
    }
    
    public string FancyTitle()
    {
        // TODO: NICE TITLES
        return ValidPosition();
    }

    public string FancyGoods()
    {
        return "Goods sold: " + goodsSold.ToString();
    }

    public string FancyLandValue()
    {
        return "Land Value: $" + landValue;
    }
    public void MarkWithComponent()
    {
        StartCoroutine("MarkWithComponent");
    }
}
