using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommercialTracker : ItemTracker {
    // Manages individual stats of each commercial building.

    int requiredSales = 4;
    float salesHappiness;

    public int visitors;
    public int lifetimeVisitors;

    public float goodsSold;
    public float goodsAvailable;
    bool checkEnable;

    List<ResidentialTracker> employees;



    new void Start()
    {
        base.Start();
        employees = new List<ResidentialTracker>();
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
        else if (!usable && !updateStarted)
        {
            checkEnable = true;
        }
    }

    public void Apply(float applicantLandValue, ResidentialTracker applicantTracker)
    {
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
            AcceptApplication(applicantTracker);
        }
        else RejectApplication(applicantTracker);
    }

    void AcceptApplication(ResidentialTracker applicantTracker)
    {
        if (applicantTracker.unemployedPopulation >= 1)
        {
            AddUsers(1);
            employees.Add(applicantTracker);
            applicantTracker.AcceptApplication();
        }
        else RejectApplication(applicantTracker);
    }

    void RejectApplication(ResidentialTracker applicantTracker)
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

    public void RemoveAllUsers()
    {
        foreach(ResidentialTracker res in employees)
        {
            res.unemployedPopulation++;
        }
    }

    void UpdateVisitors()
    {
        visitors = U.NumResidents(U.ReturnResidentialTrackers(U.FindNearestBuildings(transform.position, users * 2)));
    }

    void UpdateSalesHappiness()
    {
        if (capacity != 0 && requiredSales != 0)
        {
            salesHappiness = (goodsSold / capacity * requiredSales) * 40;
        }
        else salesHappiness = 0;
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
        income = visitors * (1 + (landValue * 0.01f)) * (happinessState) - baseCost;
        totalCommercialIncome += income;
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

    IEnumerator CheckEnable()
    {
        while (true)
        {
            if (checkEnable)
            {
                checkEnable = false;
                if (transform.parent.transform.parent.transform.parent.gameObject.name != "UI")
                {
                    usable = true;
                }
            }
            yield return new WaitForSeconds(5);
        }
    }
}
