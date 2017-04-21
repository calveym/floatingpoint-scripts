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
        if (referencesUpdated == false && buttonPressed == true)
        {
            UpdateReferences();
        }
        else if (referencesUpdated == true && buttonPressed == true)
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
        titleText = transform.Find("Canvas/TitleText").GetComponent<Text>();
        incomeText = transform.Find("Canvas/IncomeText").GetComponent<Text>();
        capacityText = transform.Find("Canvas/CapacityText").GetComponent<Text>();
        happinessText = transform.Find("Canvas/HappinessText").GetComponent<Text>();
        goodsText = transform.Find("Canvas/ComponentBText").GetComponent<Text>();
        visitorsText = transform.Find("Canvas/ComponentCText").GetComponent<Text>();
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
        Destroy(tooltip.gameObject);
        GameObject.Find("Managers").GetComponent<TooltipManager>().updateTooltips -= UpdateValues;
        referencesUpdated = false;
    }

}
