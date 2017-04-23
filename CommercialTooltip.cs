using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class CommercialTooltip : MonoBehaviour
{

    GameObject tooltip;
    CommercialTracker commercialTracker;
    bool referencesUpdated;
    Transform stareat;

    // Text references
    Text titleText;
    Text incomeText;
    Text capacityText;
    Text happinessText;
    Text goodsText;
    Text visitorsText;

    // Use this for initialization
    void Start()
    {
        commercialTracker = GetComponent<CommercialTracker>();
        referencesUpdated = false;
        stareat = GameObject.Find("Camera (eye)").transform;
    }

    public void UpdateValues()
    {
        Debug.Log("referencesUpdated: " + referencesUpdated);
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
        titleText.text = commercialTracker.FancyTitle();
        incomeText.text = commercialTracker.FancyIncome();
        capacityText.text = commercialTracker.FancyCapacity();
        happinessText.text = commercialTracker.FancyHappiness();
        goodsText.text = commercialTracker.FancyGoods();
        visitorsText.text = commercialTracker.FancyVisitors();
    }

    void UpdateReferences()
    {
        titleText = tooltip.transform.Find("Canvas/TitleText").GetComponent<Text>();
        incomeText = tooltip.transform.Find("Canvas/IncomeText").GetComponent<Text>();
        capacityText = tooltip.transform.Find("Canvas/CapacityText").GetComponent<Text>();
        happinessText = tooltip.transform.Find("Canvas/HappinessText").GetComponent<Text>();
        goodsText = tooltip.transform.Find("Canvas/ComponentBText").GetComponent<Text>();
        visitorsText = tooltip.transform.Find("Canvas/ComponentCText").GetComponent<Text>();
        referencesUpdated = true;
    }

    public void EnableObjectTooltip()
    // Enables, resets position and resets text for object tooltips
    {
        if (tooltip != null)
        {
            Destroy(tooltip);
        }
        tooltip = Instantiate(GameObject.Find("CommercialTooltip"), gameObject.transform);

        tooltip.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        tooltip.transform.position = gameObject.transform.position + new Vector3(0f, 2.5f, 0f);
        tooltip.transform.localScale = new Vector3(10f, 10f, 10f);
        tooltip.transform.LookAt(2 * transform.position - stareat.position);
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
