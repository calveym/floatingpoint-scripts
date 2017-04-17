using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandValue : MonoBehaviour {

    List<GameObject> surroundingBuildings;
    List<ResidentialTracker> surroundingResidential;
    List<CommercialTracker> surroundingCommercial;
    List<IndustrialTracker> surroundingIndustrial;
    List<LeisureTracker> surroundingLeisure;

    // Aggregate values
    float aggResidential;
    float aggCommercial;
    float aggIndustrial;
    float aggLeisure;

    float landValue;
    
    public float RecalculateLandValue()
    // Controls recalculation process
    {
        UpdateValues();
        CalculateAggregates();
        CalculateFinalValue();
        return landValue;
    }

    void CalculateFinalValue()
    {
        landValue = aggResidential + aggCommercial + aggIndustrial + aggLeisure;
    }

    void CalculateAggregates()
    {
        aggResidential = CalculateResidentialAggregate();
        aggCommercial = CalculateCommercialAggregate();
        aggIndustrial = CalculateIndustrialAggregate();
        aggLeisure = CalculateLeisureAggregate();
    }

    float CalculateResidentialAggregate()
    {
        float aggTemp = 0;
        foreach(ResidentialTracker res in surroundingResidential)
        {
            aggTemp += 1 / Vector3.Distance(transform.position, res.transform.position);
        }
        return aggTemp;
    }

    float CalculateCommercialAggregate()
    {
        float aggTemp = 0;
        foreach(CommercialTracker com in surroundingCommercial)
        {
            aggTemp += 1 / Vector3.Distance(transform.position, com.transform.position);
        }
        return aggTemp;
    }

    float CalculateIndustrialAggregate()
    {
        float aggTemp = 0;
        foreach(IndustrialTracker ind in surroundingIndustrial)
        {
            if(gameObject.tag == "industrial")
            {
                aggTemp += 1 / Vector3.Distance(transform.position, ind.transform.position);
            }
            else if(gameObject.tag == "residential")
            {
                aggTemp -= 1 / Vector3.Distance(transform.position, ind.transform.position);
            }
        }
        return aggTemp;
    }

    float CalculateLeisureAggregate()
    {
        float aggTemp = 0;
        foreach(LeisureTracker leis in surroundingLeisure)
        {
            aggTemp += 1 / Vector3.Distance(transform.position, leis.transform.position);
        }
        return aggTemp;
    }

    void UpdateValues()
    {
        surroundingBuildings = U.FindNearestBuildings(transform.position, 5f);
        surroundingResidential = U.ReturnResidentialTrackers(surroundingBuildings);
        surroundingCommercial = U.ReturnCommercialTrackers(surroundingBuildings);
        surroundingIndustrial = U.ReturnIndustrialTrackers(surroundingBuildings);
        surroundingLeisure = U.ReturnLeisureTrackers(surroundingBuildings); 
    }
}
