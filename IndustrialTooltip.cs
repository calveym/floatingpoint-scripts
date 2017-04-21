using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndustrialTooltip : MonoBehaviour {

    GameObject tooltip;
    GameObject canvas;
    IndustrialTracker industrialTracker;
    public bool buttonPressed;
    bool referencesUpdated;

    // Text refs
    Text titleText;
    Text incomeText;
    Text happinessText;
    Text capacityText;
    // Text componentAText; Used as subtitle
    Text componentBText;
    Text componentCText;
    Text componentDText;

	// Use this for initialization
	void Start () {
        industrialTracker = GetComponent<IndustrialTracker>();
        referencesUpdated = false;
    }

    public void UpdateValues () {
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
        titleText.text = industrialTracker.FancyTitle();
        incomeText.text = industrialTracker.FancyIncome();
        capacityText.text = industrialTracker.FancyCapacity();
        happinessText.text = industrialTracker.FancyHappiness();
        componentBText.text = industrialTracker.capacityMulti.ToString();
        componentCText.text = industrialTracker.productionMulti.ToString();
        componentCText.text = industrialTracker.sellAmountMulti.ToString();
    }

    void UpdateReferences()
    {
        titleText = tooltip.transform.Find("Canvas/TitleText").GetComponent<Text>();
        incomeText = tooltip.transform.Find("Canvas/IncomeText").GetComponent<Text>();
        capacityText = tooltip.transform.Find("Canvas/CapacityText").GetComponent<Text>();
        happinessText = tooltip.transform.Find("Canvas/HappinessText").GetComponent<Text>();
        componentBText = tooltip.transform.Find("Canvas/ComponentBText").GetComponent<Text>();
        componentCText = tooltip.transform.Find("Canvas/ComponentCText").GetComponent<Text>();
        componentDText = tooltip.transform.Find("Canvas/ComponentDText").GetComponent<Text>();
        referencesUpdated = true;
    }

    public void EnableObjectTooltip()
    // Enables, resets position and resets text for object tooltips
    {
        if (tooltip != null)
        {
            Destroy(tooltip);
        }
        tooltip = Instantiate(GameObject.Find("IndustrialTooltip"), gameObject.transform);

        tooltip.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        tooltip.transform.position = gameObject.transform.position + new Vector3(0f, 2.5f, 0f);
        tooltip.transform.localScale = new Vector3(10f, 10f, 10f);
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
