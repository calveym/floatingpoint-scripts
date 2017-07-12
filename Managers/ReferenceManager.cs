using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using CloudCity;

public class ReferenceManager : MonoBehaviour {

    public static ReferenceManager instance;

    [Space(10)]
    [Header("VR Components")]
    [Space(5)]
    public GameObject rightController;
    public GameObject leftController;
    public GameObject headset;
    public VRTK_ControllerEvents rightEvents;
    public VRTK_ControllerEvents leftEvents;

    public RoadGenerator roadGenerator;
    public TOD_Sky tod;

    [Space(10)]
    [Header("Managers")]
    [Space(5)]
    public PopulationManager populationManager;
    public EconomyManager economyManager;
    public HappinessManager happinessManager;
    public ItemManager itemManager;
    public SaveManager saveManager;
    public TooltipManager tooltipManager;
    public PopupManager popupManager;
    public TutorialManager tutorialManager;
    public AudioManager audioManager;
    public SpawnManager spawnManager;
    public BuildingNotificationManager buildingNotificationManager;
    public ProgressionManager progressionManager;

    [Space(10)]
    [Header("Services")]
    [Space(5)]
    public Power power;
    public Education education;
    public Health health;
    public Police police;
    public Fire fire;

    [Space(10)]
    [Header("Prefabs")]
    [Space(5)]
    public GameObject notificationPrefab;
    public GameObject spherePrefab;
    public GameObject serviceTooltipPrefab;
    public GameObject residentialTooltipPrefab;
    public GameObject commercialTooltipPrefab;
    public GameObject industrialTooltipPrefab;
    public GameObject foliageTooltipPrefab;
    public GameObject buildingTarget;

    [Space(10)]
    [Header("Income modification")]
    [Space(5)]
    public float residentialIncomeMultiplier = 0.52f;
    public float commercialIncomeMultiplier = 0.152f;
    public float industrialIncomeMultiplier = 0.6f;

    [Space(10)]
    [Header("Materials")]
    [Space(5)]
    public Material moneyGreen;
    public Material moneyRed;

    public int tick = 0;

    // Use this for initialization
    void Awake () {
		if(instance != this)
        {
            instance = this;
        }
	}

    private void Start()
    {
        rightController = GameObject.Find("RightController");
        leftController = GameObject.Find("LeftController");
        Invoke("EnableModels", 1f);

        leftEvents = leftController.GetComponent<VRTK_ControllerEvents>();
        rightEvents = rightController.GetComponent<VRTK_ControllerEvents>();
    }

    void EnableModels()
    {
        Debug.Log("Enabling");
        leftController.transform.parent.Find("Model").gameObject.SetActive(true);
        rightController.transform.parent.Find("Model").gameObject.SetActive(true);
        GameObject model = leftController.transform.parent.Find("Model").gameObject;
        Component[] all = model.GetComponents(typeof(Component));
        for (int i = 0; i < model.gameObject.transform.childCount; i++)
        {
            GameObject Go = model.gameObject.transform.GetChild(i).gameObject;
            Component[] temp = Go.GetComponents(typeof(Component));
        }
    }

    private void Update()
    {
        tick++;
    }
}
