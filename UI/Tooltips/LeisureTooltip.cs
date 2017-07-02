using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class LeisureTooltip : MonoBehaviour
{

    GameObject tooltip;
    LeisureTracker leisureTracker;
    TooltipManager tooltipManager;
    public bool buttonPressed;

    // Use this for initialization
    void Start()
    {
        tooltipManager = GameObject.Find("Managers").GetComponent<TooltipManager>();
        leisureTracker = GetComponent<LeisureTracker>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateValues()
    {

    }

    public void EnableObjectTooltip()
    // Enables, resets position and resets text for object tooltips
    {
        if (tooltip != null)
        {
            Destroy(tooltip);
        }
        tooltip = Instantiate(GameObject.Find("LeisureTooltip"), gameObject.transform);
        Debug.Log(tooltip);

        tooltip.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        tooltip.transform.position = gameObject.transform.position + new Vector3(0f, 2.5f, 0f);
        tooltip.transform.localScale = new Vector3(100f, 100f, 100f);
        tooltip.transform.LookAt(GameObject.Find("Camera (eye)").transform);
        GameObject.Find("Managers").GetComponent<TooltipManager>().updateTooltips += UpdateValues;
        UpdateValues();
    }

    public void DisableObjectTooltip()
    // Removes object tooltips
    {
        Destroy(tooltip.gameObject);
        GameObject.Find("Managers").GetComponent<TooltipManager>().updateTooltips -= UpdateValues;
    }

}
