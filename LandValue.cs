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

    void Awake()
    {
        // Debug.Log("Waking up");
    }

    private void Start()
    {
        GetComponent<ItemTracker>().SetLandValue(this);
    }

    public float RecalculateLandValue()
    // Controls recalculation process
    {
        UpdateValues();
        CalculateAggregates();
        return CalculateFinalValue();
    }

    float CalculateFinalValue()
    {
        return aggResidential + aggCommercial + aggIndustrial + aggLeisure;
    }

    void CalculateAggregates()
    {
        aggResidential = CalculateResidentialAggregate() * 5;
        aggCommercial = CalculateCommercialAggregate() * 5;
        aggIndustrial = CalculateIndustrialAggregate() * 7;
        // aggLeisure = CalculateLeisureAggregate();
    }

    float CalculateResidentialAggregate()
    {
        float aggTemp = surroundingResidential.Count;
        return aggTemp;
    }

    float CalculateCommercialAggregate()
    {
        float aggTemp = surroundingCommercial.Count;
        return aggTemp;
    }

    float CalculateIndustrialAggregate()
    {
        float aggTemp = 0;
        foreach(IndustrialTracker ind in surroundingIndustrial)
        {
            if(gameObject.tag == "industrial")
            {
                aggTemp ++;
            }
            else if(gameObject.tag == "residential")
            {
                aggTemp --;
            }
        }
        return aggTemp;
    }

    float CalculateLeisureAggregate()
    {
        float aggTemp = surroundingLeisure.Count;
        return aggTemp;
    }

    void UpdateValues()
    {
        surroundingBuildings = U.FindNearestBuildings(gameObject.transform.position, 10f);
        surroundingResidential = U.ReturnResidentialTrackers(surroundingBuildings);
        surroundingCommercial = U.ReturnCommercialTrackers(surroundingBuildings);
        surroundingIndustrial = U.ReturnIndustrialTrackers(surroundingBuildings);
        surroundingLeisure = U.ReturnLeisureTrackers(surroundingBuildings);
    }
}
