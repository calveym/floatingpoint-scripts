using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;
using Autelia.Serialization;

public class DisplayUI : MonoBehaviour {

    SpawnManager spawnManager;
    VRTK_ControllerEvents events;
    EconomyManager economyManager;
    PopulationManager populationManager;
    ProgressionManager progressionManager;
    GameObject staticSpheres;
    GameObject wheelBase;
    GameObject canvas;
    GameObject leftController;
    ThumbTracker thumbTracker;

    GameObject menu;

    [Header("Menu Colors")]
    [Space(5)]
    [Tooltip("Menu panel base color; menu panels are reset to this every swipe")]
    public Color baseColor;
    public Color residentialColor;
    public Color commercialColor;
    public Color industrialColor;
    public Color officeColor;
    public Color componentColor;
    public Color foliageColor;
    public Color serviceColor;

    [Space(10)]
    [Header("UI References")]
    [Space(5)]
    public GameObject globalStats;
    public GameObject buildingStats;

    public Text globalIncome;
    public Text globalBalance;
    public Text globalHappiness;
    public Text globalTotalPopulation;
    public Text globalUnemployedPopulation;
    public Text globalLevel;
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

    public AnimationClip openMenu;
    public Animation menuAnimation;

    Image res;
    Image com;
    Image ind;
    Image off;
    Image indc;
    Image fol;
    Image serv;

    int menuSelection;  // which of the 6 menu panels is selected, used in logic

    List<string> text;
    bool updateRequired;  // Used to trigger a change between buildings and menu
    [Tooltip("Controls main display coroutine")]
    public bool displaying;  // controlls main Display coroutine
    [Tooltip("True if buildings and not menu are showing")]
    public bool showBuildings;  // true if menu hidden and buildings shown
    [Tooltip("True if no presses before touch")]
    public bool firstTouch;
    public bool ignoreFirstTouch;

    bool showingGlobalStats;

    bool interruptRelease;  // Interrupts release if another touch happens before releasedelay

    private void Awake()
    {
        firstTouch = true;
        showBuildings = false;
        displaying = false;
        showingGlobalStats = false;
        menuSelection = 0;
    }

    private void Start()
    {if (Serializer.IsDeserializing)	return;if (Serializer.IsLoading)	return;
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
        serv = transform.Find("Canvas/Menu/Service").gameObject.GetComponent<Image>();
        baseColor = fol.color;

        spawnManager = ReferenceManager.instance.spawnManager;
        economyManager = ReferenceManager.instance.economyManager;
        populationManager = ReferenceManager.instance.populationManager;
        progressionManager = ReferenceManager.instance.progressionManager;
        thumbTracker = GetComponent<ThumbTracker>();
        events = GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>();
        events.TouchpadTouchStart += DoTouchpadTouch;
        events.TouchpadPressed += DoTouchpadPress;
        events.TouchpadTouchEnd += DoTouchpadRelease;
        HideUI();
    }

    public int GetSelection()
    {
        return menuSelection;
    }

    void DoTouchpadTouch(object sender, ControllerInteractionEventArgs e)
    {
        if(!displaying)
        {
            interruptRelease = true;
            updateRequired = true;
            ShowUI();
            ShowGlobalStats();
            HideBuildings();
            HideMenu();
            showingGlobalStats = true;

Autelia.Coroutines.CoroutineController.StartCoroutine(this, "UpdateGlobalStats");
        }

        else if(displaying)
        {
            thumbTracker.StartTracking();
        }
    }

    void DoTouchpadRelease(object sender, ControllerInteractionEventArgs e)
    {
        if(!displaying && showingGlobalStats)
        {
            HideUI();
        }
    }

    void DoTouchpadPress(object sender, ControllerInteractionEventArgs e)
    {
        U.LeftPulse(750);
        U.PlayClick();
        interruptRelease = true;
        updateRequired = true;
        if (firstTouch)
        {
            displaying = true;
            showingGlobalStats = true;
            ShowUI();
            showBuildings = false;
            firstTouch = false;
            HideBuildings();
            ShowMenu();
            thumbTracker.StartTracking();
            ResetMenuColors();

Autelia.Coroutines.CoroutineController.StartCoroutine(this, "UpdateGlobalStats");
        }

        else if(!firstTouch)
        {
            if(TopButton())
                // Go in/ select
            {
                if(showBuildings)
                {
                    spawnManager.PurchaseBuilding();
                }
                else
                {
                    ShowBuildings();
                    showingGlobalStats = false;

Autelia.Coroutines.CoroutineController.StartCoroutine(this, "UpdateBuildings");
                }
            }
            else if(!TopButton())
                // Go back/ hide
            {
                if(showBuildings)
                    // Show menu
                {
                    showBuildings = false;
                    showingGlobalStats = true;
                    ShowMenu();
                    thumbTracker.ForceStopTrackingAngle();

Autelia.Coroutines.CoroutineController.StartCoroutine(this, "UpdateGlobalStats");
                }
                else if(!showBuildings)
                    // Hide UI
                {
                    thumbTracker.ForceStopTrackingAngle();
                    menuSelection = 0;
                    showingGlobalStats = false;
                    showBuildings = false;
                    firstTouch = true;
                    displaying = false;
                    HideUI();
                }
            }
        }
    }

    bool TopButton()
    {
        if (events.GetTouchpadAxis().y > 0)
        {
            return true;
        }
        else return false;
    }

    public void SendSwipe(float swipe)
    {
        if(menuSelection <= 5 && swipe > 0)
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
                res.color = residentialColor;
                break;

            case 1:
                com.color = commercialColor;
                break;

            case 2:
                ind.color = industrialColor;
                break;

            case 3:
                off.color = residentialColor;
                break;

            case 4:
                indc.color = componentColor;
                break;

            case 5:
                fol.color = foliageColor;
                break;
            case 6:
                serv.color = commercialColor;
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
        serv.color = baseColor;
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
        DisableSprites();
        HideGlobalStats();

        staticSpheres.SetActive(true);
        spawnManager.SpawnUIBuildings(menuSelection, thumbTracker.angleIncrement);
        showBuildings = true;

        buildingType.enabled = true;
        buildingWeeklyCost.enabled = true;
        buildingCapacity.enabled = true;
        buildingBuyCost.enabled = true;
        buildingLevel.enabled = true;
    }

    void ShowMenu()
    {
        HideBuildings();
        ShowGlobalStats();
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
        globalIncome.enabled = false;
        globalBalance.enabled = false;
        globalTotalPopulation.enabled = false;
        globalUnemployedPopulation.enabled = false;
        globalLevel.enabled = false;
        globalGoodsConsumed.enabled = false;
    }

    void ShowGlobalStats()
    {
        globalIncome.enabled = true;
        globalBalance.enabled = true;
        globalTotalPopulation.enabled = true;
        globalUnemployedPopulation.enabled = true;
        globalLevel.enabled = true;
        globalGoodsConsumed.enabled = true;
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
        globalLevel.text = "Level: " + ProgressionManager.level;
        globalGoodsConsumed.text = "Level Up Population: " + ProgressionManager.ToNextLevel();
        SetHappiness();
    }

    IEnumerator UpdateGlobalStats()
    {
        while (showingGlobalStats)
        {
            UpdateGlobalText();
            yield return null;
        }
    }

    IEnumerator UpdateBuildings()
    {
        while(displaying && showBuildings)
        {
            UpdateBuildingText();
            yield return null;
        }
    }
}
