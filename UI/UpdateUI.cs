using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UpdateUI : MonoBehaviour {

    float balance;
    float income;
    float happiness;
    int population;

    float timePassed;

    public Text incomeText;
    public Text balanceText;
    public Text populationText;
    public Text happinessText;
    GameObject manager;

    EconomyManager economyManager;
    HappinessManager happinessManager;

	// Use this for initialization
	void Start () {
        timePassed = 0;
        manager = GameObject.Find("Managers");
        economyManager = manager.GetComponent<EconomyManager>();
        happinessManager = manager.GetComponent<HappinessManager>();
	}
	
	// Update is called once per frame
	void Update () {
        timePassed += Time.deltaTime;
        if(timePassed >= 1)
        {
            UpdateManagerVariables();
            UpdateUIVariables();
            timePassed = 0;
        }
	}

    void UpdateManagerVariables()
    {
        balance = economyManager.GetBalance();
        income = economyManager.GetIncome();
        population = economyManager.GetPopulation();
        happiness = happinessManager.happiness;
    } 

    void UpdateUIVariables()
    {
        incomeText.text = SetIncomeString();
        balanceText.text = SetBalanceString();
        populationText.text = SetPopulationString();
        happinessText.text = SetHappinessString();
    }

    public string SetPopulationString()
    {
        return "Population: " + population.ToString();
    }

    string SetHappinessString()
    {
        return "Happiness: " + happiness.ToString();
    }

    string SetIncomeString()
    {
        string newString = "(" + IsPositive(income) + income.ToString() + ")";
        return newString;   
    }

    string SetBalanceString()
    {
        return balance.ToString();
    }

    string IsPositive(float value)
    {
        if (value > 0)
        {
            return "+";
        }
        else if (value == 0)
        {
            return "";
        }
        else {
            return "-";
        }
    }
}
