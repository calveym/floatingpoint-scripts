using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Autelia.Serialization;

public class IndustrialTooltip : MonoBehaviour {

    GameObject tooltip;
    IndustrialTracker industrialTracker;
    bool referencesUpdated;
    Transform stareat;

    int happiness; // temporary value used to check if update required

    // Text refs
    Text titleText;
    Text incomeText;
    Text landValueText;
    Text capacityText;
    // Text componentAText; Used as subtitle
    Text goodsText;
    Text productionText;
    Text componentDText;

    // Happiness sprites
    SpriteRenderer dead;
    SpriteRenderer happy;
    SpriteRenderer veryHappy;
    SpriteRenderer passive;
    SpriteRenderer angry;

	// Use this for initialization
	void Start () {if (Serializer.IsDeserializing)	return;if (Serializer.IsLoading)	return;
        industrialTracker = GetComponent<IndustrialTracker>();
        referencesUpdated = false;
        stareat = GameObject.Find("Camera (eye)").transform;
    }

    public void UpdateValues () {
        if (referencesUpdated == false && TooltipManager.pressed == true)
        {
            UpdateReferences();
        }
        if (referencesUpdated == true && TooltipManager.pressed == true)
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
        landValueText.text = industrialTracker.FancyLandValue();
        goodsText.text = industrialTracker.goodsProduced.ToString();
        productionText.text = industrialTracker.productionMulti.ToString();
        //componentDText.text = industrialTracker.sellAmountMulti.ToString();
    }

    void UpdateHappiness()
    {
        int newHappiness = industrialTracker.FancyHappiness();
        SetHappiness(newHappiness);
    }

    void SetHappiness(int newHappiness)
    {
        if (dead == null)
        {
            UpdateReferences();
        }
        happiness = newHappiness;
        DisableSprites();
        if (happiness == 0)
        {
            dead.enabled = true;
        }
        else if(happiness == 1)
        {
            angry.enabled = true;
        }
        else if(happiness == 2)
        {
            passive.enabled = true;
        }
        else if (happiness == 3)
        {
            happy.enabled = true;
        }
        else if (happiness == 4)
        {
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
        landValueText = tooltip.transform.Find("Canvas/LandValueText").GetComponent<Text>();
        goodsText = tooltip.transform.Find("Canvas/GoodsText").GetComponent<Text>();
        productionText = tooltip.transform.Find("Canvas/ProductionText").GetComponent<Text>();
        //componentDText = tooltip.transform.Find("Canvas/SalesText").GetComponent<Text>();

        dead = tooltip.transform.Find("Canvas/Icons/Dead").GetComponent<SpriteRenderer>();
        happy = tooltip.transform.Find("Canvas/Icons/Happy").GetComponent<SpriteRenderer>();
        angry = tooltip.transform.Find("Canvas/Icons/Angry").GetComponent<SpriteRenderer>();
        passive = tooltip.transform.Find("Canvas/Icons/Passive").GetComponent<SpriteRenderer>();
        veryHappy = tooltip.transform.Find("Canvas/Icons/VeryHappy").GetComponent<SpriteRenderer>();
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
