using UnityEngine;
using System.Collections;
using System;

public class Residential : BuildingBase
{

    int unemployedUsers;

    protected override void RequestUsers()
    {
        if(populationManager.AvailablePopulation())
        {
            int maxAdd = capacity - users;
            if (populationManager.unallocatedPopulation < maxAdd)
                maxAdd = populationManager.unallocatedPopulation;

            users += maxAdd;
            unemployedUsers += maxAdd;
            populationManager.ConfirmHoused(maxAdd);
        }
    }

    protected override void UpdateSurroundingValues()
    {
        surroundingBuildings = U.FindNearestBuildings(gameObject.transform.position, 5f);
        surroundingResidential = U.ReturnResidentialTrackers(surroundingBuildings);
        surroundingCommercial = U.ReturnCommercialTrackers(surroundingBuildings);
        surroundingIndustrial = U.ReturnIndustrialTrackers(surroundingBuildings);
    }
}
