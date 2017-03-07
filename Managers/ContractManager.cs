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

    public delegate void IncrementPopulation(int numAdded);
    public delegate void IncrementResidential();
    public delegate void IncrementCommercial();
    public delegate void IncrementIndustrial();
    public delegate void IncrementFoliage();
    public delegate void IncrementLeisure();

    IncrementPopulation incrementPopulation;
    IncrementResidential incrementResidential;
    IncrementCommercial incrementCommercial;
    IncrementIndustrial incrementIndustrial;
    IncrementFoliage incrementFoliage;
    IncrementLeisure incrementLeisure;

    void Awake()
	{
		contractPrefab = GameObject.Find("ContractPrefab");
		economyManager = GameObject.Find("Managers").GetComponent<EconomyManager>();
		happinessManager = GameObject.Find("Managers").GetComponent<HappinessManager>();
		itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();
		populationManager = GameObject.Find("Managers").GetComponent<PopulationManager>();

		allContracts = new List<Contract>();
		// majorContracts = new List<Contract>;
		acceptedContracts = new List<Contract>();
		declinedContracts = new List<Contract>();
		completedContracts = new List<Contract>();
		failedContracts = new List<Contract>();
    }


	void Update()
	{
		CheckContractsCompleted();
	}

	public void ContractButtonPress()
	{
		if (allContracts.Count <= 1)
		{
			//GenerateContract();
		}
	}

	void CheckContractsCompleted()
	{
		for(int i = acceptedContracts.Count - 1; i > 0; i--)
		{
			if(acceptedContracts[i].Completed())
			{
				Contract contract = acceptedContracts[i];
				acceptedContracts.RemoveAt(i);
				completedContracts.Add(contract);
			}
		}
	}

	public void GenerateContract()
	{
        Debug.Log("Pressed");
		GameObject newContract = Instantiate(contractPrefab);
		Contract contract = newContract.GetComponent<Contract>();
		contract.Create("Test contract", false, "Increase population by 5", "The regional government has created a scheme to encourage population growth. Increase your population by 5 to complete", "0", 0);
		contract.AssignRequirements("500000");
		allContracts.Add(contract);
	}
}
