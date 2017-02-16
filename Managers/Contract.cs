using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contract : MonoBehaviour {

    // Population increase contracts: records current population, requires higher population. Used for all major contracts.
    // Building construction contracts: minor contract. Rewards money. Requires contsruction of a certain number of building type.
    // Building demolition contracts: minor contract. Rewards money. Requires destruction of certain type of building.
    // Greenspace improvement contracts: minor contract, money reward. Requires amount of foliage to be added

<<<<<<< HEAD
    // Associated GameObjects
    GameObject stamp;
=======
>>>>>>> e54cae5f8b54fa59e90dd8bbfd2586f2a1956797

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

<<<<<<< HEAD
    void Awake()
    {
        active = false;
        //TODO: find children of contract, find stamp, set stamp
=======
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
>>>>>>> e54cae5f8b54fa59e90dd8bbfd2586f2a1956797
    }

    void Update()
    {
<<<<<<< HEAD
        CheckCompleted();
    }

    public static void IncrementPopulation(int numAdded)
=======
        Completed();
    }

    public void LocalIncrementPopulation(int numAdded)
>>>>>>> e54cae5f8b54fa59e90dd8bbfd2586f2a1956797
    {
        requiredPopulation -= numAdded;
    }

<<<<<<< HEAD
    public static void IncrementResidential()
=======
    public void LocalIncrementResidential()
>>>>>>> e54cae5f8b54fa59e90dd8bbfd2586f2a1956797
    {
        requiredResidential--;
    }

<<<<<<< HEAD
    public static void IncrementCommercial()
=======
    public void LocalIncrementCommercial()
>>>>>>> e54cae5f8b54fa59e90dd8bbfd2586f2a1956797
    {
        requiredCommercial--;
    }

<<<<<<< HEAD
    public static void IncrementIndustrial()
=======
    public void LocalIncrementIndustrial()
>>>>>>> e54cae5f8b54fa59e90dd8bbfd2586f2a1956797
    {
        requiredIndustrial--;
    }

<<<<<<< HEAD
    public static void IncrementFoliage()
=======
    public void LocalIncrementFoliage()
>>>>>>> e54cae5f8b54fa59e90dd8bbfd2586f2a1956797
    {
        requiredFoliage--;
    }

<<<<<<< HEAD
    public static void IncrementLeisure()
=======
    public void LocalIncrementLeisure()
>>>>>>> e54cae5f8b54fa59e90dd8bbfd2586f2a1956797
    {
        requiredLeisure--;
    }

<<<<<<< HEAD
    void Create(string incomingName, bool incomingMajor, string incomingDescShort, string incomingDescLong, string incomingType, int incomingLevel)
=======
    public void Create(string incomingName, bool incomingMajor, string incomingDescShort, string incomingDescLong, string incomingType, int incomingLevel)
>>>>>>> e54cae5f8b54fa59e90dd8bbfd2586f2a1956797
    // Sets basic info
    {
        name = incomingName;
        major = incomingMajor;
        descShort = incomingDescShort;
        descLong = incomingDescLong;
        type = incomingType;
        level = incomingLevel;
    }

<<<<<<< HEAD
    void AssignRequirements(string incomingReq)
=======
    public void AssignRequirements(string incomingReq)
>>>>>>> e54cae5f8b54fa59e90dd8bbfd2586f2a1956797
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

<<<<<<< HEAD
    void Sign()
    // activates contract
    {
        active = true;
        stamp.setActive(true);
        EnableRespawn();
    }

    bool Completed()
=======
    public void Sign()
    // activates contract
    {
        //contractManager.incrementPopulation 
    }

    public bool Completed()
>>>>>>> e54cae5f8b54fa59e90dd8bbfd2586f2a1956797
    // checks and returns whether the contract is completed (all requirements satisfied)
    {
        return (requiredPopulation <= 0 && requiredResidential <= 0 && requiredCommercial <= 0 && requiredIndustrial <= 0 && requiredFoliage <= 0 && requiredLeisure <= 0);
    }
<<<<<<< HEAD
=======


>>>>>>> e54cae5f8b54fa59e90dd8bbfd2586f2a1956797
}
