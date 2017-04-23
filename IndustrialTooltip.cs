using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndustrialTooltip : MonoBehaviour {

    GameObject tooltip;
    IndustrialTracker industrialTracker;
    public bool buttonPressed;
    bool referencesUpdated;
    Transform stareat;

    int happiness;

    // Text refs
    Text titleText;
    Text incomeText;
    Text happinessText;
    Text capacityText;
    // Text componentAText; Used as subtitle
    Text componentBText;
    Text componentCText;
    Text componentDText;

    SpriteRenderer dead;
    SpriteRenderer happy;
    SpriteRenderer veryHappy;
    SpriteRenderer passive;
    SpriteRenderer angry;

	// Use this for initialization
	void Start () {
        industrialTracker = GetComponent<IndustrialTracker>();
        referencesUpdated = false;
        stareat = GameObject.Find("Camera (eye)").transform;
    }

    public void UpdateValues () {
        if (referencesUpdated == false && TooltipManager.pressed == true)
        {
            UpdateReferences();
        }
        else if (referencesUpdated == true && TooltipManager.pressed == true)
        {
            UpdateText();
            UpdateHappiness();
        }
    }

    void UpdateText()
    {
        titleText.text = industrialTracker.FancyTitle();
        incomeText.text = industrialTracker.FancyIncome();
        capacityText.text = industrialTracker.FancyCapacity();
        componentBText.text = industrialTracker.goodsCapacityMulti.ToString();
        componentCText.text = industrialTracker.productionMulti.ToString();
        componentDText.text = industrialTracker.sellAmountMulti.ToString();
    }

    void UpdateHappiness()
    {
        int newHappiness = industrialTracker.FancyHappiness();
        if(newHappiness != happiness)
        {
            SetHappiness(newHappiness);
        }
    }

    void SetHappiness(int newHappiness)
    {
        happiness = newHappiness;
        if(happiness == 0)
        {
            DisableSprites();
            dead.enabled = true;
        }
        else if(happiness == 1)
        {
            DisableSprites();
            angry.enabled = true;
        }
        else if(happiness == 2)
        {
            DisableSprites();
            passive.enabled = true;
        }
        else if (happiness == 3)
        {
            DisableSprites();
            happy.enabled = true;
        }
        else if (happiness == 4)
        {
            DisableSprites();
            veryHappy.enabled = true;
        }
    }

    void DisableSprites()
    {
        dead.enabled = false;
        happy.enabled = false;
        veryHappy.enabled = false;
        passive.enabled = false;
        angry.enabled = false;
    }

    void UpdateReferences()
    {
        titleText = tooltip.transform.Find("Canvas/TitleText").GetComponent<Text>();
        incomeText = tooltip.transform.Find("Canvas/IncomeText").GetComponent<Text>();
        capacityText = tooltip.transform.Find("Canvas/CapacityText").GetComponent<Text>();
        componentBText = tooltip.transform.Find("Canvas/StorageText").GetComponent<Text>();
        componentCText = tooltip.transform.Find("Canvas/PdoructionText").GetComponent<Text>();
        componentDText = tooltip.transform.Find("Canvas/SalesText").GetComponent<Text>();
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
