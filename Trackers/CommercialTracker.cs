using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autelia.Serialization;

[System.Serializable]
public class CommercialTracker : ItemTracker {
    // Manages individual stats of each commercial building.

    int requiredSales = 4;
    float salesHappiness;

    [Header("Instance Operation Variables")]
    public int range = 10;

    public float goodsAvailable;
    bool checkEnable;

    List<ResidentialTracker> employees;


    new void Start()
    {
        if (Serializer.IsLoading)
        {
            RemoveEcoTick();
            return;
        }
        base.Start();
        employees = new List<ResidentialTracker>();
    }

    void Update()
    {
        if (!updateStarted && usable && validPosition)
        {
            AddEcoTick();
        }
        else if (updateStarted && !usable && validPosition)
        {
            updateStarted = false;
            EconomyManager.ecoTick -= UpdateSecond;
        }
        else if (!usable && !updateStarted && validPosition)
        {
            checkEnable = true;
        }
        if (ReferenceManager.instance.tick % 5 == 0)
        {
            if (checkEnable)
            {
                checkEnable = false;
                if (transform.parent == null)
                {
                    usable = true;
                }
            }
        }
    }

    public void AddEcoTick()
    {
        EnableSnap();
        updateStarted = true;
        EconomyManager.ecoTick += UpdateSecond;
        ReferenceManager.instance.itemManager.addCommercial(capacity, gameObject);
    }

    void RemoveEcoTick()
    {
        updateStarted = false;
        EconomyManager.ecoTick -= UpdateSecond;
    }

    public void Apply(float applicantLandValue, ResidentialTracker applicantTracker)
    {
        // TODO: the application is considered by the tracker, and the value of residential land has
        // an impact on the final decision, along with an element of chance
        float landValueDifference = landValue - applicantLandValue;
        landValueDifference = Mathf.Abs(landValueDifference);
        if(landValueDifference < landValue / 3 && usable && users < capacity && !Serializer.IsLoading)
        {
            AcceptApplication(applicantTracker);
        }
        else if (Random.Range(0, 5) > 3 && !Serializer.IsLoading)
        {
            AcceptApplication(applicantTracker);
        }
        else RejectApplication(applicantTracker);
    }

    void AcceptApplication(ResidentialTracker applicantTracker)
    {
        if (!applicantTracker.isActiveAndEnabled)
            return;
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
        goodsSold = Random.Range(0, 10) * visitors;
        /*if(goodsSold > economyManager.goods)
        {
            goodsSold = economyManager.goods;
        }
        */
        economyManager.goods -= goodsSold;
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
        visitors = (int)(U.NumResidents(U.ReturnResidentialTrackers(U.FindNearestBuildings(transform.position, range))) * 0.2f);
    }

    void UpdateSalesHappiness()
    {
        if (capacity != 0 && requiredSales != 0)
        {
            salesHappiness = (goodsSold / capacity * requiredSales) * 40;
        }
        else salesHappiness = 0;
    }

    public void UpdateSecond()
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
        income = (goodsSold * (1 + (landValue * 0.01f)) * (longtermHappiness / 50)) * ReferenceManager.instance.commercialIncomeMultiplier - baseCost + users;
        totalCommercialIncome += income * 10; // the jg multipier
    }

    void UpdateHappiness()
    {
        currentHappiness = localHappiness + salesHappiness + fillRateHappiness + 20;
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
        return buildingName;
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
