using System;
using System.Collections;
using UnityEngine;

public class CommercialTracker : ItemTracker {
// Manages individual stats of each commercial building.

    public void Apply(float applicantLandValue, int residentID, ResidentialTracker applicantTracker)
    {
        // TODO: the application is considered by the tracker, and the value of residential land has
        // an impact on the final decision, along with an element of chance
        System.Random rand = new System.Random(); //reuse this if you are generating many
        double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
        double u2 = 1.0 - rand.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                     Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
        double randNormal =
                     landValue - 5 + 5 * randStdNormal; //random normal(mean,stdDev^2)
        if (applicantLandValue > randNormal)
        {
            AcceptApplication(residentID, applicantTracker);
        }
        else RejectApplication(residentID, applicantTracker);
    }

    void AcceptApplication(int residentID, ResidentialTracker applicantTracker)
    {
        AddUsers(1);
        applicantTracker.AcceptApplication(residentID);
    }

    void RejectApplication(int residentID, ResidentialTracker appplicantTracker)
    {

    }
}
