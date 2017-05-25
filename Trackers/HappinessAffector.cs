using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappinessAffector : MonoBehaviour {

    [Header("Default Settings")]
    [Space(5)]
    [Range(0, 100)]
    [Tooltip("[Optional Override]  Radius of happiness effect")]
    public float radius;
    [Range(0, 20)]
    [Tooltip("Amount of happiness added per foliage tick to surrounding buildings")]
    public int affectAmount;
    [Tooltip("Type of effect \n[industrialReduce/ foliage]")]
    public string affectType;

    public bool usable;

    private void Awake()
    {
        usable = false;
    }

    // Use this for initialization
    void Start () {
        EconomyManager.foliageTick += StartAffect;
	}
	
    public void StartAffect()
    {
        if(usable)
        {
            List<GameObject> surroundingBuildings = U.FindNearestBuildings(transform.position, radius);
            foreach (GameObject building in surroundingBuildings)
            {
                if (building.tag == "residential")
                {
                    if (affectType == "industrialReduce")
                    {
                        building.GetComponent<ResidentialTracker>().ModifyHappiness(affectAmount, "industrialReduce");
                    }
                    else
                    {
                        building.GetComponent<ResidentialTracker>().ModifyHappiness(affectAmount, "");
                    }
                }
                else if (building.tag == "industrial")
                {
                    building.GetComponent<IndustrialTracker>().ModifyHappiness(affectAmount, "");
                }
                else if (building.tag == "commercial")
                {
                    building.GetComponent<CommercialTracker>().ModifyHappiness(affectAmount, "");
                }
                else if (building.tag == "leisure")
                {
                    building.GetComponent<LeisureTracker>().ModifyHappiness(affectAmount, "");
                }
            }
        }
    }
}
