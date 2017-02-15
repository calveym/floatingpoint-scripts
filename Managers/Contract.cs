using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contract : MonoBehaviour {

    // Population increase contracts: records current population, requires higher population. Used for all major contracts.
    // Building construction contracts: minor contract. Rewards money. Requires contsruction of a certain number of building type.
    // Building demolition contracts: minor contract. Rewards money. Requires destruction of certain type of building.
    // Greenspace improvement contracts: minor contract, money reward. Requires amount of foliage to be added


    // Base information
    public string name;
    public bool major;
    public string descShort;
    public string descLong;
    public string type;
    public int level;
    public bool active;

    // Requirements, start as null, assigned to false
    public int requiredPopulation;
    public int requiredResidential;
    public int requiredCommercial;
    public int requiredIndustrial;
    public int requiredFoliage;
    public int requiredLeisure;
    public string requirements;

    public string rewardType;
    public int rewardAmount;

    ContractManager contractManager;

    private void Awake()
    {
        requiredPopulation = 0;
        requiredResidential = 0;
        requiredCommercial = 0;
        requiredIndustrial = 0;
        requiredLeisure = 0;
        requiredFoliage = 0;
        contractManager = GameObject.Find("Managers").GetComponent<ContractManager>();
    }

    void Update()
    {
        Completed();
    }

    public void LocalIncrementPopulation(int numAdded)
    {
        requiredPopulation -= numAdded;
    }

    public void LocalIncrementResidential()
    {
        requiredResidential--;
    }

    public void LocalIncrementCommercial()
    {
        requiredCommercial--;
    }

    public void LocalIncrementIndustrial()
    {
        requiredIndustrial--;
    }

    public void LocalIncrementFoliage()
    {
        requiredFoliage--;
    }

    public void LocalIncrementLeisure()
    {
        requiredLeisure--;
    }

    public void Create(string incomingName, bool incomingMajor, string incomingDescShort, string incomingDescLong, string incomingType, int incomingLevel)
    // Sets basic info
    {
        name = incomingName;
        major = incomingMajor;
        descShort = incomingDescShort;
        descLong = incomingDescLong;
        type = incomingType;
        level = incomingLevel;
    }

    public void AssignRequirements(string incomingReq)
    // Sets requirements
    {
        requirements = incomingReq;
        DecodeRequirements();
    }

    void DecodeRequirements()
    // translates input string of reqs into ints, and sets required vars
    {
        requiredPopulation = requirements[0];
        requiredResidential = requirements[1];
        requiredCommercial = requirements[2];
        requiredIndustrial = requirements[3];
        requiredFoliage = requirements[4];
        requiredLeisure = requirements[5];
    }

    public void Sign()
    // activates contract
    {
        //contractManager.incrementPopulation 
    }

    public bool Completed()
    // checks and returns whether the contract is completed (all requirements satisfied)
    {
        return (requiredPopulation <= 0 && requiredResidential <= 0 && requiredCommercial <= 0 && requiredIndustrial <= 0 && requiredFoliage <= 0 && requiredLeisure <= 0);
    }


}
