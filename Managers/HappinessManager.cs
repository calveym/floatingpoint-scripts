using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappinessManager : MonoBehaviour {

    ItemManager itemManager;
    PopulationManager populationManager;
    EconomyManager economyManager;

    public float happiness;
    int population;
    int residentialCap;
    int industrialCap;
    int commercialCap;
    int leisureCap;
    int foliageCap;
    int availableJobs;
    int availableResidential;

    float jobHappiness;
    float foliageHappiness;
    float leisureHappiness;
    float taxHappiness;

    void Start ()
    // Set values pre-initialization
    {
        itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();
        populationManager = GameObject.Find("Managers").GetComponent<PopulationManager>();
        economyManager = GameObject.Find("Managers").GetComponent<EconomyManager>();
    }

	void Update ()
    // Set values pre-initialization
    {
        UpdateValues();

        jobHappiness = SetJobHappiness();
        foliageHappiness = SetFoliageHappiness();
        leisureHappiness = SetLeisureHappiness();
        taxHappiness = SetTaxHappiness();

        SetHappiness();
	}

    void UpdateValues()
    {
        population = populationManager.population;
        foliageCap = itemManager.foliageCap;
        availableResidential = populationManager.AvailableResidential();
        residentialCap = itemManager.residentialCap;
        commercialCap = itemManager.commercialCap;
        industrialCap = itemManager.industrialCap;
    }

    void SetHappiness()
    // Sets happiness value from components
    {
        happiness = jobHappiness + foliageHappiness + leisureHappiness + taxHappiness;
    }

    void AvailableJobs()
    // Job availability
    {
        return industrialCap + commercialCap - population;
    }

    float SetJobHappiness()
    // Job happiness algorithm
    {
        availableJobs = AvailableJobs();
        if(availableJobs != 0 && availableResidential != 0)
        {
            if (availableResidential / availableJobs <= 1)
            {
                return 25;
            }
            {
                return (availableJobs / availableResidential * 25); //TODO: FIX THIS- goes above 25
            }
        }
        else
        {
            return 0;
        }

    }

    float SetFoliageHappiness()
    // Foliage happiness algorithm
    {
        if (population != 0)
        {
            return foliageCap / (population * 10);
        }
        else return 25;
    }

    float SetLeisureHappiness()
    // Leisure happiness algorithm
    {
        if(population != 0)
        {
            return leisureCap / population;
        }
    }

    float SetTaxHappiness()
    // IDEA: Make this more of an overall metric of cost of living
    {
        return 100 - ((economyManager.residentialTaxRate * 2) + economyManager.industrialTaxRate + economyManager.commercialTaxRate) / 4;
    }
}
