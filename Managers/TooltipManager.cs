using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class TooltipManager : MonoBehaviour {

    public delegate void UpdateTooltips();
    public UpdateTooltips updateTooltips;
    List<GameObject> nearestBuildings;
    GameObject rightController; 

    bool pressed;

	// Use this for initialization
	void Start () {
        GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().ButtonOnePressed += new ControllerInteractionEventHandler(EnableObjectTooltip);
        GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().ButtonOneReleased += new ControllerInteractionEventHandler(DisableObjectTooltip);
        rightController = GameObject.Find("RightController");
    }

    void EnableObjectTooltip (object sender, ControllerInteractionEventArgs e)
    {
        pressed = true;
        Debug.Log("enabledobjecttooooooltip");
        nearestBuildings = U.FindNearestBuildings(rightController.transform.position, 20f);
        foreach(GameObject building in nearestBuildings)
        {
            EnableTooltip(building);
        }
    }

    void EnableTooltip(GameObject building)
    {   
        if(building.tag == "residential")
        {
            building.GetComponent<ResidentialTooltip>().EnableObjectTooltip();
        }
        if(building.tag == "commercial")
        {
            building.GetComponent<CommercialTooltip>().EnableObjectTooltip();

        }
        if (building.tag == "industrial")
        {
            building.GetComponent<IndustrialTooltip>().EnableObjectTooltip();

        }
        if (building.tag == "leisure")
        {
            building.GetComponent<LeisureTooltip>().EnableObjectTooltip();
        }
    }

    void DisableObjectTooltip(object sender, ControllerInteractionEventArgs e)
    {
        pressed = false;
        foreach (GameObject building in nearestBuildings)
        {
            if (building.tag == "residential")
            {
                building.GetComponent<ResidentialTooltip>().DisableObjectTooltip();
            }
            if (building.tag == "commercial")
            {
                building.GetComponent<CommercialTooltip>().DisableObjectTooltip();

            }
            if (building.tag == "industrial")
            {
                building.GetComponent<IndustrialTooltip>().DisableObjectTooltip();

            }
            if (building.tag == "leisure") 
            {
                building.GetComponent<LeisureTooltip>().DisableObjectTooltip();
            } 
        }
    }

    IEnumerator SecondTick()
    {
        while(pressed)
        {
            updateTooltips();
            yield return new WaitForSeconds(1);
        }
    }

}
