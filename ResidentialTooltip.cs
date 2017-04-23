using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class ResidentialTooltip : MonoBehaviour
{

    GameObject tooltip;
    ResidentialTracker residentialTracker;
    bool referencesUpdated;
    Transform stareat;


    // Text references
    Text titleText;
    Text incomeText;
    Text capacityText;
    Text happinessText;
    Text landValueText;

    // Use this for initialization
    void Start()
    {
        residentialTracker = GetComponent<ResidentialTracker>();
        referencesUpdated = false;
        stareat = GameObject.Find("Camera (eye)").transform;
    }

    public void UpdateValues()
    {
        if (referencesUpdated == false && TooltipManager.pressed == true)
        {
            UpdateReferences();
        }
        else if (referencesUpdated == true && TooltipManager.pressed == true)
        {
            UpdateText();
        }
    }

    void UpdateText()
    {
        titleText.text = residentialTracker.FancyTitle();
        incomeText.text = residentialTracker.FancyIncome();
        capacityText.text = residentialTracker.FancyCapacity();
        happinessText.text = residentialTracker.FancyHappiness();
        landValueText.text = residentialTracker.FancyLandValue();
    }

    void UpdateReferences()
    {
        titleText = tooltip.transform.Find("Canvas/TitleText").GetComponent<Text>();
        incomeText = tooltip.transform.Find("Canvas/IncomeText").GetComponent<Text>();
        capacityText = tooltip.transform.Find("Canvas/CapacityText").GetComponent<Text>();
        happinessText = tooltip.transform.Find("Canvas/HappinessText").GetComponent<Text>();
        landValueText = tooltip.transform.Find("Canvas/LandValueText").GetComponent<Text>();
        referencesUpdated = true;
    }

    public void EnableObjectTooltip()
    // Enables, resets position and resets text for object tooltips
    {
        Debug.Log("Residential tooltip attempting to enable");
        if (tooltip != null)
        {
            Destroy(tooltip);
        }
        tooltip = Instantiate(GameObject.Find("ResidentialTooltip"), gameObject.transform);

        tooltip.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        tooltip.transform.position = gameObject.transform.position + new Vector3(0f, 2.5f, 0f);
        tooltip.transform.localScale = new Vector3(10f, 10f, 10f);
        tooltip.transform.LookAt(2 * transform.position - stareat.position);
        GameObject.Find("Managers").GetComponent<TooltipManager>().updateTooltips += UpdateValues;
        UpdateReferences();
        UpdateText();
    }

    public void DisableObjectTooltip()
    // Removes object tooltips
    {
        GameObject.Find("Managers").GetComponent<TooltipManager>().updateTooltips -= UpdateValues;
        Destroy(tooltip.gameObject);
        referencesUpdated = false;
    }

}
