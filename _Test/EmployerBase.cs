using UnityEngine;
using System.Collections;
using System;
using Autelia.Serialization;

public abstract class EmployerBase : BuildingBase
{

    [Space(10)]
    [Header("Employer Attributes")]
    [Space(5)]
    public int employerRange;

    public bool ApplyForJob(float applicantLandValue, Residential applicantTracker)
    {
        float landValueDifference = landValue - applicantLandValue;
        landValueDifference = Mathf.Abs(landValueDifference);
        if (landValueDifference < (landValue / 3) && users < capacity && !Serializer.IsLoading)
        {
            return AcceptApplication(applicantTracker);
        }
        else if (UnityEngine.Random.Range(0, 5) > 3 && !Serializer.IsLoading)
        {
            return AcceptApplication(applicantTracker);
        }
        else return false;
    }

    protected virtual bool AcceptApplication(Residential applicantTracker)
    {
        return true;
    }

    protected override void RequestUsers()
    {
        
    }
}
