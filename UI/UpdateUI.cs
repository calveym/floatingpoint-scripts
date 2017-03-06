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

    Text incomeText;
    Text balanceText;
    Text populationText;
    Text happinessText;

    EconomyManager economyManager;
    HappinessManager happinessManager;

	// Use this for initialization
	void Start () {
        timePassed = 0;
        economyManager = GameObject.Find("Managers").GetComponent<EconomyManager>();
        incomeText = GameObject.Find("IncomeText").GetComponent<Text>();
        Debug.Log(GameObject.Find("IncomeText"));
        balanceText = GameObject.Find("BalanceText").GetComponent<Text>();
        populationText = GameObject.Find("PopulationText").GetComponent<Text>();
        happinessText = GameObject.Find("HappinessText").GetComponent<Text>();
        happinessManager = GameObject.Find("Managers").GetComponent<HappinessManager>();
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

    string SetPopulationString()
    {
        return "Population: " + population.ToString();
    }

    string SetHappinessString()
    {
        return "Happiness: " + happiness.ToString();
    }

    string SetIncomeString()
    {
        return "(" + IsPositive(income) + income.ToString() + ")";
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
