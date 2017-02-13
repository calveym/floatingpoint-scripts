using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contract: MonoBehaviour {

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

  // Requirements, start as null, assigned to false
  public bool completed;
  public int requiredPopulation;
  public int requiredResidential;
  public int requiredCommercial;
  public int requiredIndustrial;
  public int requiredFoliage;
  public int requiredLeisure;
  public string requirements

  public string rewardType;
  public int rewardAmount;

  void Awake()
  {

  }

  void Update()
  {
    CheckCompleted();
  }

  void Create(string incomingName, string incomingDescShort, string incomingDescLong, string incomingType)
  {
    name = incomingName;
    descShort = incomingDescShort;
    descLong = incomingDescLong;
    type = incomingType;
  }

  void AssignRequirements(string incomingReq)
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

  void Sign()
  // activates contract
  {

  }

  bool Completed()
  // checks and returns whether the contract is completed (all requirements satisfied)
  {
    return false;
  }


}
