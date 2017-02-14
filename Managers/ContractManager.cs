using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractManager : MonoBehaviour {

	public List<Contract> allContracts;
	// public List<Contract> majorContracts;
	public List<Contract> acceptedContracts;
	public List<Contract> declinedContracts;
	public List<Contract> completedContracts;
	public List<Contract> failedContracts;

	GameObject contractPrefab; // Basic contract- contains the model, gameObject, blank text fields. GenerateContract adds the contract component.

	EconomyManager economyManager;
	HappinessManager happinessManager;
	ItemManager itemManager;
	PopulationManager populationManager;

	void Awake()
	{
		contractPrefab = GameObject.Find("ContractPrefab");
		economyManager = GameObject.Find("EconomyManager");
		happinessManager = GameObject.Find("HappinessManager");
		itemManager = GameObject.Find("itemManager");
		populationManager = GameObject.Find("populationManager");

		allContracts = new List<Contract>;
		// majorContracts = new List<Contract>;
		acceptedContracts = new List<Contract>;
		declinedContracts = new List<Contract>;
		completedContracts = new List<Contract>;
		failedContracts = new List<Contract>;
	}

	void Update()
	{
		CheckNumContracts();
		CheckContractsCompleted();
	}

	void CheckNumContracts()
	{
		if (allContracts.Count <= 1)
		{
			GenerateContract();
		}
	}

	void CheckContractsCompleted()
	{
		for(i = acceptedContracts.Count - 1; i > 0; i--)
		{
			if(acceptedContracts[i].Completed())
			{
				Contract contract = acceptedContracts[i];
				acceptedContracts.RemoveAt(i);
				completedContracts.Add(contract);
			}
		}
	}

	void GenerateContract()
	{
		GameObject newContract = Instantiate(ContractPrefab);
		Contract contract = newContract.GetComponent<Contract>();
		contract.Create("Test contract", false, "Increase population by 5", "The regional government has created a scheme to encourage population growth. Increase your population by 5 to complete", "0", "0");
		contract.AssignRequirements("500000");
		allContracts.Add(contract);
	}
}
