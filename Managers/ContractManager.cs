using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractManager : MonoBehaviour {

	public List<Contract> allContracts;
	public List<Contract> majorContracts;
	public List<Contract> pendingContracts;
	public List<Contract> activeContracts;
	public List<Contract> declinedContracts;
	public List<Contract> completedContracts;
	public List<Contract> failedContracts;

	GameObject ejector;
	GameObject button;
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
		ejector = GameObject.Find("Ejector");

		allContracts = new List<Contract>;
		pendingContracts = new List<Contract>;
		majorContracts = new List<Contract>;
		activeContracts = new List<Contract>;
		declinedContracts = new List<Contract>;
		completedContracts = new List<Contract>;
		failedContracts = new List<Contract>;
	}

	void Update()
	{
		CheckContractsCompleted();
	}

	public void ContractButtonPress()
	{
		GameObject newContract = Instantiate(ContractPrefab, ejector.transform.position);
		Contract contract = newContract.GetComponent<Contract>();
		contract.Create("Test contract", false, "Increase population by 5", "The regional government has created a scheme to encourage population growth. Increase your population by 5 to complete", "0", "0");
		contract.AssignRequirements("500000");
		pendingContracts.Add(contract);
		allContracts.Add(contract);
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
}
