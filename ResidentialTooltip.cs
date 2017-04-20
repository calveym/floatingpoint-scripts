using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class ResidentialTooltip : MonoBehaviour
{

    GameObject tooltip;
    ResidentialTracker residentialTracker;
    public bool buttonPressed;
    bool referencesUpdated;

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
        EnableObjectTooltip();
    }

    public void UpdateValues()
    {
        if(referencesUpdated == false)
        {
            UpdateReferences();
        }
        else
        {
            UpdateText();
        }

    }

    void UpdateText()
    {
        Debug.Log("Text attempting to update");
        titleText.text = residentialTracker.FancyTitle();
        incomeText.text = residentialTracker.FancyIncome();
        capacityText.text = residentialTracker.FancyCapacity();
        happinessText.text = residentialTracker.FancyHappiness();
        landValueText.text = residentialTracker.FancyLandValue();
    }

    void UpdateReferences()
    {
        titleText = transform.FindChild("TitleText").GetComponent<Text>();
        incomeText = transform.FindChild("IncomeText").GetComponent<Text>();
        capacityText = transform.FindChild("CapacityText").GetComponent<Text>();
        happinessText = transform.FindChild("HappinessText").GetComponent<Text>();
        landValueText = transform.FindChild("LandValueText").GetComponent<Text>();
        referencesUpdated = true;
    }

    public void EnableObjectTooltip()
    // Enables, resets position and resets text for object tooltips
    {
        if (tooltip != null)
        {
            Destroy(tooltip);
        }
        tooltip = Instantiate(GameObject.Find("ResidentialTooltip"), gameObject.transform);
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
        GameObject.Find("Managers").GetComponent<TooltipManager>().updateTooltips -= UpdateValues;
        Destroy(tooltip.gameObject);
        referencesUpdated = false;
    }

}
