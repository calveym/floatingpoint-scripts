using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class TooltipManager : MonoBehaviour {

    public delegate void UpdateTooltips();
    public UpdateTooltips updateTooltips;

    List<GameObject> nearestBuildings;
    GameObject rightController;
    public GameObject indicator;
    MeshRenderer rend;

    public static bool pressed;

	// Use this for initialization
	void Start () {
        rend = indicator.GetComponent<MeshRenderer>();
        rightController = GameObject.Find("RightController");
        rightController.GetComponent<VRTK_ControllerEvents>().ButtonOnePressed += new ControllerInteractionEventHandler(EnableObjectTooltip);
        rightController.GetComponent<VRTK_ControllerEvents>().ButtonOneReleased += new ControllerInteractionEventHandler(DisableObjectTooltip);
    }

    void EnableObjectTooltip (object sender, ControllerInteractionEventArgs e)
    {
        pressed = true;
        nearestBuildings = U.FindNearestBuildings(rightController.transform.position, 10f);
        rend.enabled = true;
        foreach(GameObject building in nearestBuildings)
        {
            EnableTooltip(building);
        }
        StartCoroutine("SecondTick");
    }

    public void EnableTooltip(GameObject building)
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
        rend.enabled = false;
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
            if(updateTooltips != null)
            {
                updateTooltips();
            }
            yield return new WaitForSeconds(1);
        }
    }

}
