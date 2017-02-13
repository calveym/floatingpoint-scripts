using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractManager : MonoBehaviour {

	public Contract[] allContracts;
	public Contract[] majorContracts;
	public Contract[] minorContracts;
	public Contract[] acceptedContracts;
	public Contract[] declinedContracts;
	public Contract[] completedContracts;
	public Contract[] failedContracts;

	EconomyManager economyManager;
	HappinessManager happinessManager;
	ItemManager itemManager;
	PopulationManager populationManager;

	void Awake()
	{

	}

	void Update()
	{
		CheckNumContracts();
		CheckContractsCompleted();
	}

	void CheckNumContracts()
	{

	}

	void CheckContractsCompleted()
	{
		// for every accepted contract, run it's Completed() function. If true, carry out reward.
	}

	void GenerateNewContract()
	{

	}


}
