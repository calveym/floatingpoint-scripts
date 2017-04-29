using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class DisplayUI : MonoBehaviour {

    VRTK_ControllerEvents events;
    EconomyManager economyManager;
    PopulationManager populationManager;
    GameObject staticSpheres;
    GameObject wheelBase;
    GameObject canvas;
    ThumbTracker thumbTracker;

    GameObject menu;

    public Color baseColor;
    public Color residentialColor;
    public Color commercialColor;
    public Color industrialColor;
    public Color officeColor;
    public Color componentColor;
    public Color foliageColor;

    public GameObject globalStats;
    public GameObject buildingStats;

    public Text globalIncome;
    public Text globalBalance;
    public Text globalHappiness;
    public Text globalTotalPopulation;
    public Text globalUnemployedPopulation;
    public Text globalGoodsProduced;
    public Text globalGoodsConsumed;

    public Text buildingType;
    public Text buildingCapacity;
    public Text buildingWeeklyCost;
    public Text buildingBuyCost;
    public Text buildingLevel;

    // Happiness sprites
    public SpriteRenderer dead;
    public SpriteRenderer happy;
    public SpriteRenderer veryHappy;
    public SpriteRenderer passive;
    public SpriteRenderer angry;

    Image res;
    Image com;
    Image ind;
    Image off;
    Image indc;
    Image fol;

    int menuSelection;  // which of the 6 menu panels is selected, used in logic

    List<string> text;
    bool updateRequired;  // Used to trigger a change between buildings and menu
    bool displaying;  // controlls main Display coroutine
    public bool showBuildings;  // true if menu hidden and buildings shown
    bool firstTouch;
    public bool ignoreFirstTouch;

    private void Awake()
    {
        showBuildings = true;
        menuSelection = 0;
        staticSpheres = transform.Find("StaticSpheres").gameObject;
        wheelBase = transform.Find("WheelBase").gameObject;
        canvas = transform.Find("Canvas").gameObject;

        menu = transform.Find("Canvas/Menu").gameObject;
        res = transform.Find("Canvas/Menu/Residential").gameObject.GetComponent<Image>();
        com = transform.Find("Canvas/Menu/Commercial").gameObject.GetComponent<Image>();
        ind = transform.Find("Canvas/Menu/Industrial").gameObject.GetComponent<Image>();
        off = transform.Find("Canvas/Menu/Office").gameObject.GetComponent<Image>();
        indc = transform.Find("Canvas/Menu/Component").gameObject.GetComponent<Image>();
        fol = transform.Find("Canvas/Menu/Foliage").gameObject.GetComponent<Image>();
    }

    private void Start()
    {
        economyManager = GameObject.Find("Managers").GetComponent<EconomyManager>();
        populationManager = economyManager.populationManager;
        thumbTracker = GetComponent<ThumbTracker>();
        events = GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>();
        events.TouchpadTouchStart += DoTouchpadTouch;
        events.TouchpadPressed += DoTouchpadPress;
        HideUI();
    }

    public int GetSelection()
    {
        return menuSelection;
    }

    void DoTouchpadTouch(object sender, ControllerInteractionEventArgs e)
    {
        updateRequired = true;
        firstTouch = true;
        if (!displaying)
        {
            displaying = true;
            StartCoroutine("Display");
        }
    }

    void DoTouchpadPress(object sender, ControllerInteractionEventArgs e)
    {
        firstTouch = false;
        ignoreFirstTouch = true;
        updateRequired = true;
        showBuildings = !showBuildings;
        thumbTracker.ForceStopTrackingAngle();
        thumbTracker.ForceStopTrackingPosition();
        thumbTracker.StartTracking();

        if(!displaying)
        {
            displaying = true;
            StartCoroutine("Display");
        }
    }

    public void SendSwipe(float swipe)
    {
        if(menuSelection <= 4 && swipe > 0)
        {
            // swipe to the right
            menuSelection++;
            ResetMenuColors();
            updateRequired = true;
        }
        else if(menuSelection >= 1 && swipe < 0)
        {
            // swipe left
            menuSelection--;
            ResetMenuColors();
            updateRequired = true;
        }
        else
        {
            // if no valid swipe possible
        }
    }

    public void SendSelectedText(List<string> newText)
    {
        text = newText;
        UpdateBuildingText();
    }

    void ResetMenuColors()
    {
        SetToBaseColor();
        switch(menuSelection)
        {
            case 0:
                SetToBaseColor();
                res.color = residentialColor;
                break;

            case 1:
                SetToBaseColor();
                com.color = commercialColor;
                break;

            case 2:
                SetToBaseColor();
                ind.color = industrialColor;
                break;

            case 3:
                SetToBaseColor();
                off.color = residentialColor;
                break;

            case 4:
                SetToBaseColor();
                indc.color = componentColor;
                break;

            case 5:
                SetToBaseColor();
                fol.color = foliageColor;
                break;
        }
    }

    void SetToBaseColor()
    {
        res.color = baseColor;
        com.color = baseColor;
        ind.color = baseColor;
        off.color = baseColor;
        indc.color = baseColor;
        fol.color = baseColor;
    }

	public void ShowUI()
    {
        canvas.SetActive(true);
        wheelBase.SetActive(true);
        staticSpheres.SetActive(true);
    }

    public void HideUI()
    {
        canvas.SetActive(false);
        wheelBase.SetActive(false);
        staticSpheres.SetActive(false);
    }

    void ShowBuildings()
    {
        HideMenu();

        wheelBase.SetActive(true);
        staticSpheres.SetActive(true);

        buildingType.enabled = true;
        buildingWeeklyCost.enabled = true;
        buildingCapacity.enabled = true;
        buildingBuyCost.enabled = true;
        buildingLevel.enabled = true;
    }

    void ShowMenu()
    {
        HideBuildings();
        menu.SetActive(true);
    }

    void HideMenu()
    {
        menu.SetActive(false);
    }

    void HideBuildings()
    {
        wheelBase.SetActive(false);
        staticSpheres.SetActive(false);

        buildingType.enabled = false;
        buildingWeeklyCost.enabled = false;
        buildingCapacity.enabled = false;
        buildingBuyCost.enabled = false;
        buildingLevel.enabled = false;
    }

    void SetHappiness()
    {
        float happiness = populationManager.GetHappiness();
        DisableSprites();
        if (happiness == 0)
        {
            dead.enabled = true;
        }
        else if (happiness > 0 && happiness <= 2)
        {
            angry.enabled = true;
        }
        else if (happiness > 2 && happiness <= 3)
        {
            passive.enabled = true;
        }
        else if (happiness > 3 && happiness <= 4)
        {
            happy.enabled = true;
        }
        else if (happiness > 4)
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

    void HideGlobalStats()
    {
        DisableSprites();
        globalIncome.enabled = false;
        globalBalance.enabled = false;
        globalTotalPopulation.enabled = false;
        globalUnemployedPopulation.enabled = false;
        globalGoodsProduced.enabled = false;
        globalGoodsConsumed.enabled = false;
    }

    void ShowGlobalStats()
    {
        SetHappiness();
        globalIncome.enabled = true;
        globalBalance.enabled = true;
        globalTotalPopulation.enabled = true;
        globalUnemployedPopulation.enabled = true;
        globalGoodsProduced.enabled = true;
        globalGoodsConsumed.enabled = true;
        UpdateGlobalText();
    }

    void UpdateBuildingText()
    {
        buildingType.text = text[0];
        buildingCapacity.text = text[1];
        buildingLevel.text = text[2];
        buildingWeeklyCost.text = text[3];
        buildingBuyCost.text = text[4];
    }

    void UpdateGlobalText()
    {
        globalIncome.text = economyManager.FancyIncome();
        globalBalance.text = economyManager.FancyBalance();
        globalTotalPopulation.text = populationManager.FancyTotalPopulation();
        globalUnemployedPopulation.text = populationManager.FancyUnemployedPopulation();
        globalGoodsProduced.text = economyManager.FancyGoodsProduced();
        globalGoodsConsumed.text = economyManager.FancyGoodsConsumed();
        SetHappiness();
    }

    IEnumerator Display()
    {
        ResetMenuColors();
        while(firstTouch && displaying && !ignoreFirstTouch)
        {
            ShowUI();
            HideMenu();
            HideBuildings();
            ShowGlobalStats();
            yield return null;
        }
        while(displaying && ignoreFirstTouch && !firstTouch)
        {
            if (showBuildings && updateRequired)
            {
                Debug.Log("Showing buildings");
                ShowUI();
                HideGlobalStats();
                ShowBuildings();
                UpdateBuildingText();
                updateRequired = false;
            }
            else if(showBuildings && !updateRequired)
            {
                UpdateBuildingText();
            }
            else if(!showBuildings && updateRequired)
            {
                Debug.Log("Showing menu");
                ShowUI();
                ShowMenu();
                ShowGlobalStats();
                UpdateGlobalText();
                updateRequired = false;
            }
            else if(!showBuildings && !updateRequired)
            {
                UpdateGlobalText();
            }
            yield return null;
        }
        if(!displaying)
        {
            Debug.Log("Hiding");
            HideUI();
            HideMenu();
            HideGlobalStats();
            HideBuildings();
            yield return null;
        }
    }

    public IEnumerator ReleaseDelay()
    {
        float touchTime = 0f;
        while(touchTime < 3)
        {
            ignoreFirstTouch = true;
            touchTime += Time.deltaTime;
            yield return null;
        }
        ignoreFirstTouch = false;
        displaying = false;
        HideUI();
        HideMenu();
        HideGlobalStats();
        HideBuildings();
    }
}
