using System;
using System.Collections;
using UnityEngine;

public class CommercialTracker : ItemTracker {
    // Manages individual stats of each commercial building.

    int requiredSales = 4;
    float salesHappiness;

    public int visitors;
    public int lifetimeVisitors;

    public float goodsSold;
    public float goodsAvailable;

    new void Start()
    {
        base.Start();
        EconomyManager.ecoTick += UpdateSecond;
    }

    void Update()
    {
        if (!updateStarted && usable)
        {
            updateStarted = true;
            EconomyManager.ecoTick += UpdateSecond;
            GameObject.Find("Managers").GetComponent<ItemManager>().addCommercial(capacity, gameObject);
        }
        else if (updateStarted && !usable)
        {
            updateStarted = false;
            EconomyManager.ecoTick -= UpdateSecond;
        }
    }

    public void Apply(float applicantLandValue, int residentID, ResidentialTracker applicantTracker)
    {
        Debug.Log("Commercial receiving job application");
        // TODO: the application is considered by the tracker, and the value of residential land has
        // an impact on the final decision, along with an element of chance
        System.Random rand = new System.Random(); //reuse this if you are generating many
        double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
        double u2 = 1.0 - rand.NextDouble();
        double randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log((float)u1)) *
                     Mathf.Sin(2.0f * Mathf.PI * (float)u2); //random normal(0,1)
        double randNormal =
                     landValue - 5 + 5 * randStdNormal; //random normal(mean,stdDev^2)
        if (applicantLandValue > randNormal && usable && users < capacity)
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
        // TODO
    }

    void SellGoods()
    {
        goodsSold = users * happinessState + (1f + visitors * 0.01f);
        if(goodsSold > goodsAvailable)
        {
            goodsSold = goodsAvailable;
        }

        if (IndustrialTracker.allGoods >= capacity)
        {
            IndustrialTracker.allGoods -= capacity;
            goodsAvailable = capacity;
        }
        else if(IndustrialTracker.allGoods < capacity && IndustrialTracker.allGoods > 0)
        {
            goodsAvailable = IndustrialTracker.allGoods;
            IndustrialTracker.allGoods = 0;
        }
    }

    void UpdateVisitors()
    {
        visitors = populationManager.population - populationManager.unemployedPopulation; // TODOA: MAKE THIS ONLY LOCAL
    }

    void UpdateSalesHappiness()
    {
        if(capacity != 0 && requiredSales != 0)
        {
            salesHappiness = (goodsSold / capacity * requiredSales) * 40;
        }
    }

    void UpdateSecond()
    // Updates values once per second
    {
        updateStarted = true;
        if (!usable || !validPosition)
        {
            return;
        }
        UpdateLocalHappiness();
        UpdateSalesHappiness();
        UpdateHappiness();
        UpdateLandValue();
        UpdateTransportationValue();
        UpdateVisitors();
        SellGoods();
        income = goodsSold;
        totalIndustrialIncome += income;
    }

    void UpdateHappiness()
    {
        currentHappiness = localHappiness + salesHappiness + fillRateHappiness;
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
        return ValidPosition();
    }

    public string FancyGoods()
    {
        return "Goods sold: " + goodsSold;
    }

    public string FancyVisitors()
    {
        return "Visitors: " + visitors;
    }

    public string FancyLandValue()
    {
        return "Land Value: $" + landValue;
    }
}
