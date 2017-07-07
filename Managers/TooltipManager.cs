using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using Autelia.Serialization;

public class TooltipManager : MonoBehaviour {

    public delegate void UpdateTooltips();
    public UpdateTooltips updateTooltips;

    List<GameObject> nearestBuildings;
    GameObject headset;
    GameObject rightController;
    Transform stareat;
    [Tooltip("Tooltips spawn from here if no controller present")]
    public GameObject testTooltipObject;
    [Tooltip("Use headset if true, else use assigned object")]
    public bool useHeadset;

    public static bool pressed;

	// Use this for initialization
	void Start () {
        headset = ReferenceManager.instance.headset;
        rightController = ReferenceManager.instance.rightController;
        rightController.GetComponent<VRTK_ControllerEvents>().ButtonOnePressed += new ControllerInteractionEventHandler(EnableObjectTooltip);
        rightController.GetComponent<VRTK_ControllerEvents>().ButtonOneReleased += new ControllerInteractionEventHandler(DisableObjectTooltip);
    }

    private void Update()
    {
        if (ReferenceManager.instance.tick % 45 == 0 && updateTooltips != null)
        {
            updateTooltips();
        }
    }

    void EnableObjectTooltip(object sender, ControllerInteractionEventArgs e)
    {
        StartTooltips();
    }

    void DisableObjectTooltip(object sender, ControllerInteractionEventArgs e)
    {
        DisableTooltips();
    }

    public void StartTooltips()
    // Used in conjunction with test runner to create tooltips without headset and controllers
    {
        pressed = true;
        if(useHeadset)
        // Used during play
        {
            stareat = headset.transform;
            nearestBuildings = U.FindNearestBuildings(rightController.transform.position, 10f);
        }
        else
        // For testing
        {
            stareat = testTooltipObject.transform;
            nearestBuildings = U.FindNearestBuildings(testTooltipObject.transform.position, 10f);
        }
        foreach (GameObject building in nearestBuildings)
        {
            EnableTooltips(building);
        }
    }

    void EnableTooltips(GameObject building)
    {
        if (building.tag == "residential" || building.tag == "service" || building.tag == "industrial" || building.tag == "commercial")
        {
            building.GetComponent<TooltipBase>().EnableTooltip(stareat.transform.position - new Vector3(0, 8f, 0));

        }
    }

    public void DisableTooltips()
    // Used with test runner to disable tooltips without controllerss
    {
        pressed = false;
        foreach (GameObject building in nearestBuildings)
        {
            if (building.tag == "residential" || building.tag == "service" || building.tag == "industrial" || building.tag == "commercial")
            {
                building.GetComponent<TooltipBase>().DisableTooltip();
            }
        }
    }
}
