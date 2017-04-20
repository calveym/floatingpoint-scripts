using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class CommercialTooltip : MonoBehaviour
{

    GameObject tooltip;
    CommercialTracker commercialTracker;
    public bool buttonPressed;
    LineRenderer line;
    bool referencesUpdated;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonPressed)
        {
            if (line == null)
            {
                line = transform.FindChild("Line").GetComponent<LineRenderer>();
            }
            line.SetPosition(0, tooltip.transform.position);
            line.SetPosition(1, commercialTracker.transform.position);
        }
    }

    public void UpdateValues()
    {
        if (referencesUpdated == false)
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
        titleText.text = commercialTracker.FancyTitle();
        incomeText.text = commercialTracker.FancyIncome();
        capacityText.text = commercialTracker.FancyCapacity();
        happinessText.text = commercialTracker.FancyHappiness();
        goodsText.text = commercialTracker.FancyGoods();
        visitorsText.text = commercialTracker.FancyVisitors();
    }

    void UpdateReferences()
    {
        titleText = transform.FindChild("TitleText").GetComponent<Text>();
        incomeText = transform.FindChild("IncomeText").GetComponent<Text>();
        capacityText = transform.FindChild("CapacityText").GetComponent<Text>();
        happinessText = transform.FindChild("HappinessText").GetComponent<Text>();
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
        Destroy(tooltip.gameObject);
        GameObject.Find("Managers").GetComponent<TooltipManager>().updateTooltips -= UpdateValues;
        referencesUpdated = false;
    }

}
