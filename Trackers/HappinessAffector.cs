using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappinessAffector : MonoBehaviour {

    public float radius;
    public float affectAmount;
    public string affectType;

	// Use this for initialization
	void Start () {
        EconomyManager.ecoTick += StartAffect;	
	}
	
    public void StartAffect()
    {
        List<GameObject> surroundingBuildings = FindSurroundingBuildings();
        foreach (GameObject building in surroundingBuildings)
        {
            if (building.tag == "residential")
            {
                if(affectType == "industrialReduce")
                {
                    building.GetComponent<ResidentialTracker>().ModifyHappiness(affectAmount, "industrialReduce");
                }
                else
                {
                    building.GetComponent<ResidentialTracker>().ModifyHappiness(affectAmount, "");
                }
            }
            else if(building.tag == "industrial")
            {
                building.GetComponent<IndustrialTracker>().ModifyHappiness(affectAmount, "");
            }
            else if(building.tag == "commercial")
            {
                building.GetComponent<CommercialTracker>().ModifyHappiness(affectAmount, "");
            }
            else if(building.tag == "leisure")
            {
                building.GetComponent<LeisureTracker>().ModifyHappiness(affectAmount, "");
            }
        }
    }

	List<GameObject> FindSurroundingBuildings()
    {
        List<GameObject> returnList = new List<GameObject>();
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, layerMask);
        if (hitColliders.Length > 0)
        {
            foreach (Collider hitcol in hitColliders)
            {
                returnList.Add(hitcol.gameObject);
            }
        }

        return returnList;
    }
}
