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

    // Use this for initialization
    void Start () {
        itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();
        populationManager = GameObject.Find("Managers").GetComponent<PopulationManager>();
        economyManager = GameObject.Find("Managers").GetComponent<EconomyManager>();
    }
	
	// Update is called once per frame
	void Update () {
        population = populationManager.population;
        availableResidential = populationManager.AvailableResidential();

        SetJobHappiness();
        SetFoliageHappiness();
        SetLeisureHappiness();
        SetTaxHappiness();

        SetHappiness();
	}

    void SetHappiness()
    {
        happiness = jobHappiness + foliageHappiness + leisureHappiness + taxHappiness;
    }

    void SetJobs()
    {
        availableJobs = industrialCap + commercialCap - population;
    }

    void SetJobHappiness()
    {
        SetJobs();
        if (availableJobs / availableResidential >= 1)
        {
            jobHappiness = 25;
        }
        else
        {
            jobHappiness = (availableJobs / availableResidential * 25);
        }

    }

    void SetFoliageHappiness()
    {
        foliageHappiness = foliageCap / (population * 10);
    }

    void SetLeisureHappiness()
    {
        leisureHappiness = leisureCap / population;
    }

    void SetTaxHappiness()
    // IDEA: Make this more of an overall metric of cost of living
    {
        taxHappiness = 100 - ((economyManager.residentialTaxRate * 2) + economyManager.industrialTaxRate + economyManager.commercialTaxRate);
    }
}








