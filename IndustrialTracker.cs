using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndustrialTracker : ItemTracker {
// Manages individual stats of each industrial building.

    public int visitors;
    public int lifetimeVisitors;

    public float goodsProduced;

    void Update()
    {
        if(!updateStarted)
        {
            StartCoroutine("UpdateSecond");
        }
    }

    void ProduceGoods()
    {
        goodsProduced = users * (landValue / 5) * availableTransportation;
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
        // TODO:
    }

    IEnumerator UpdateSecond()
    // Updates values once per second
    {
        updateStarted = true;
        if (!usable || !validPosition)
        {
            yield return new WaitForSeconds(10);
        }
        while (usable && validPosition)
        {
            UpdateLandValue();
            UpdateTransportationValue();
            ProduceGoods();
            income = goodsProduced;
            totalIndustrialIncome += income;
            yield return new WaitForSeconds(1);
        }
    }
}
