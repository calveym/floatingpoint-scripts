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

	// Use this for initialization
	void Start () {
        timePassed = 0;
        economyManager = GameObject.Find("Managers").GetComponent<EconomyManager>();
        incomeText = GameObject.Find("IncomeText").GetComponent<Text>();
        balanceText = GameObject.Find("BalanceText").GetComponent<Text>();
        populationText = GameObject.Find("PopulationText").GetComponent<Text>();
        happinessText = GameObject.Find("HappinessText").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(economyManager);
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
        happiness = economyManager.GetHappiness();
    } 

    void UpdateUIVariables()
    {
        incomeText.text = SetIncomeString();
        balanceText.text = SetBalanceString();
        populationText.text = population.ToString();
        happinessText.text = happiness.ToString();
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
