using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class TooltipManager : MonoBehaviour {

    public delegate void UpdateTooltips();
    public UpdateTooltips updateTooltips;

    public DisplayUI displayUI;
    List<GameObject> nearestBuildings;
    GameObject headset;
    GameObject rightController;
    GameObject testSphere;
    MeshRenderer rend;
    int tick;

    public static bool pressed;

	// Use this for initialization
	void Start () {
        headset = GameObject.Find("Headset");
        testSphere = GameObject.Find("TestSphere");
        rightController = GameObject.Find("RightController");
        rightController.GetComponent<VRTK_ControllerEvents>().ButtonOnePressed += new ControllerInteractionEventHandler(EnableObjectTooltip);
        rightController.GetComponent<VRTK_ControllerEvents>().ButtonOneReleased += new ControllerInteractionEventHandler(DisableObjectTooltip);
    }

    public void TestTooltip()
    // Used in conjunction with test runner to create tooltips without headset and controllers
    {
        tick = 0;
        pressed = true;
        nearestBuildings = U.FindNearestBuildings(testSphere.transform.position, 10f);
        foreach (GameObject building in nearestBuildings)
        {
            EnableTooltip(building);
        }
        StartCoroutine("SecondTick");
    }

    void EnableObjectTooltip (object sender, ControllerInteractionEventArgs e)
    {
        tick = 0;
        pressed = true;
        nearestBuildings = U.FindNearestBuildings(rightController.transform.position, 10f);
        foreach (GameObject building in nearestBuildings)
        {
            EnableTooltip(building);
        }
        StartCoroutine("SecondTick");
    }

    public void EnableTooltip(GameObject building)
    {
        tick = 0;
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

    public void TestDisableTooltip()
    // Used with test runner to disable tooltips without controllerss
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
        while(pressed && tick <= 5)
        {
            if(updateTooltips != null)
            {
                updateTooltips();
                tick++;
            }
            yield return new WaitForSeconds(1);
        }
    }
}
