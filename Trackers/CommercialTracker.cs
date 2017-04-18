using System;
using System.Collections;
using UnityEngine;

public class CommercialTracker : ItemTracker {
    // Manages individual stats of each commercial building.

    public int visitors;
    public int lifetimeVisitors;

    public float goodsSold;
    public float goodsAvailable;

    void Awake()
    {
        EconomyManager.ecoTick += UpdateSecond;
    }

    void Update()
    {
        if (!updateStarted)
        {
            StartCoroutine("UpdateSecond");
        }
    }

    public void Apply(float applicantLandValue, int residentID, ResidentialTracker applicantTracker)
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
        if (visitors > goodsAvailable)
        {
            goodsSold = goodsAvailable;
        }
        else goodsSold = visitors;
    }

    void UpdateVisitors()
    {
        visitors = populationManager.population - populationManager.unemployedPopulation;
    }

    void UpdateSecond()
    // Updates values once per second
    {
        updateStarted = true;
        if (!usable || !validPosition)
        {
            return;
        }
        UpdateLandValue();
        UpdateTransportationValue();
        UpdateVisitors();
        goodsAvailable = capacity;
        SellGoods();
        income = goodsSold;
        totalIndustrialIncome += income;
    }
}
