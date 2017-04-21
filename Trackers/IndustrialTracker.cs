using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndustrialTracker : ItemTracker {
    // Manages individual stats of each industrial building.

    GameObject markerPrefab;
    Marker marker;
    List<IndustrialComponent> components;
    List<float> sales; // List of recent sales counted in goodsSold

    public int visitors;
    public int lifetimeVisitors;

    public float goodsCapacity;  // obvious
    public float goodsProduced;  // Goods produced in last economic tick
    public float goodsOwned;  // All goods currently owned by this 
    public static float allGoods; // Base goods tracking figure for each economic tick

    public float productionAmount;  // Base production value
    public float sellPrice;  // Base sell price
    public float sellAmount;  // Base maximum amount sold per economy tick

    // Multipliers from components
    public float productionMulti;
    public float goodsCapacityMulti;
    public float sellPriceMulti;
    public float sellAmountMulti;
    public float capacityMulti;

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
        capacityMulti = 1;
    }

    new void Start()
    {
        base.Start();
        markerPrefab = GameObject.Find("MarkerPrefab");
    }

    void Update()
    {
        if (!updateStarted)
        {
            updateStarted = true;
            EconomyManager.ecoTick += UpdateSecond;
            GameObject.Find("Managers").GetComponent<ItemManager>().addIndustrial(capacity, gameObject);
        }
    }

    void ProduceGoods()
    {
        if(goodsOwned < goodsCapacity)
        {
            goodsProduced = users * localHappiness * productionAmount * productionMulti;
            if(goodsProduced + goodsOwned <= goodsCapacity)
            {
                goodsOwned += goodsProduced;
            }
            else
            {
                goodsProduced = goodsCapacity - goodsOwned;
                goodsOwned += goodsProduced;
            }
        }
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

    void SellGoods()
    // Calculates income from goods sale
    {
        if(goodsOwned > sellAmount)
        {
            goodsOwned -= sellAmount;
            allGoods += sellAmount;
            AddSale(sellAmount);
        }
        else if(goodsOwned < sellAmount)
        {
            allGoods += goodsOwned;
            AddSale(goodsOwned);
            goodsOwned = 0;
        }
        RemoveSale();
        income = goodsProduced * sellPrice;
        income -= baseCost;
        totalIndustrialIncome += goodsProduced;
    }

    void AddSale(float sellAmount)
    {
        sales.Add(sellAmount);
    }

    void RemoveSale()
    {
        if(sales.Count >= 60)
        {
            goodsSold += sales[sales.Count - 1];
            goodsSold -= sales[0];
            sales.RemoveAt(0);
        }
    }

    public void AddMarker()
    {
        marker = Instantiate(markerPrefab, transform.position + new Vector3(0f, 2f, 0f), transform.rotation, transform).GetComponent<Marker>();
        marker.StartRotation();
    }

    public void LinkComponent(IndustrialComponent component)
    // Sent from component, completes link
    {
        components.Add(component);
        RecalculateComponents();
    }

    void RecalculateComponents()
    {
        for(int i = 0; i < components.Count; i++)
        {
            if (components[i].type == "sellPrice" || components[i].type == "productionAmount" || components[i].type == "goodsCapacity" || components[i].type == "sellAmount" || components[i].type == "capacity")
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
        capacityMulti += component.capacityMulti;

    }

    void UpdateSecond()
    // Updates values once per second, economic tick
    {
        updateStarted = true;
        if (!usable || !validPosition)
        {
            return;
        }
        UpdateHappiness();
        UpdateLandValue();
        UpdateTransportationValue();
        ProduceGoods();
        SellGoods();
        totalIndustrialIncome += income;
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
        return "Income: $" + income + "/w";
    }

    public string FancyCapacity()
    {
        return "Users: " + users + "(" + capacity + ")";
    }

    public string FancyHappiness()
    {
        return "Happiness: " + localHappiness + "%";
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
}
