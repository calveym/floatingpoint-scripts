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
        population = populationManager.population;
        availableResidential = populationManager.AvailableResidential();

        SetJobHappiness();
        SetFoliageHappiness();
        SetLeisureHappiness();
        SetTaxHappiness();

        SetHappiness();
        Debug.Log(happiness);
	}

    void SetHappiness()
    // Sets happiness value from components
    {
        Debug.Log(jobHappiness);
        Debug.Log(foliageHappiness);
        Debug.Log(leisureHappiness);
        Debug.Log(taxHappiness);
        happiness = jobHappiness + foliageHappiness + leisureHappiness + taxHappiness;
    }

    void SetJobs()
    // Job availability
    {
        availableJobs = industrialCap + commercialCap - population;
    }

    void SetJobHappiness()
    // Job happiness algorithm
    {
        SetJobs();
        if(availableJobs != 0 && availableResidential != 0)
        {
            if (availableResidential / availableJobs <= 1)
            {
                jobHappiness = 25;
            }
            {
                jobHappiness = (availableJobs / availableResidential * 25);
            }
        }
        else
        {
            jobHappiness = 0;
        }

    }

    void SetFoliageHappiness()
    // Foliage happiness algorithm
    {
        if (population != 0)
        {
            foliageHappiness = foliageCap / (population * 10);
        }
        else foliageHappiness = 25;
    }

    void SetLeisureHappiness()
    // Leisure happiness algorithm
    {
        if(population != 0)
        {
            leisureHappiness = leisureCap / population;
        }
    }

    void SetTaxHappiness()
    // IDEA: Make this more of an overall metric of cost of living
    {
        taxHappiness = 100 - ((economyManager.residentialTaxRate * 2) + economyManager.industrialTaxRate + economyManager.commercialTaxRate);
        taxHappiness = taxHappiness / 4;
    }
}








