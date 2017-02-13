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
  public int requiredPopulationChange;
  public string requiredBuildingType;
  public int requiredBuildingAmount;
  public string requiredBuildingAction;

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
    req = incomingReq;
  }


}
