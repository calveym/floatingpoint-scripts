using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidentialTracker : ItemTracker {
// Manages specific residential functions.

    public int unemployedPopulation;
    public Dictionary <string, int> educationLevel; // maps individual residents to their education levels
    public Dictionary <string, int> age; // maps individual residents to their age

    void Update()
    {
        UpdateValues();
    }

    void UpdateValues()
    {
        income = users * (1 + 0.01f * economyManager.residentialTaxRate
        totalIncome += income;
    }

    void TryEmployPopulation()
    {
        int num;
        // TODO: Try to employ unemployedPopulation
        EmployPopulation(employmentLocation)
    }

    void EmployPopulation(GameObject employmentLocation)
    {
        if(employmentLocation.tag == "industrial")
        {
            employmentLocation.GetComponent<IndustrialTracker>();
        }
        else if(employmentLocation.tag == "commercial")
        {
            employmentLocation.GetComponent<CommercialTracker>();
        }
    }

    GameObject FindPreferredEmployment()
    {
        // TODO: Find employment that compromises distance / value
        // High land value employment areas should be preffered by all, but mostly obtained by
        // Citizens from higher land value areas/ with more education
        // Should return final gameobject of location
    }
}
