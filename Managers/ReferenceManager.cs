using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ReferenceManager : MonoBehaviour {

    public static ReferenceManager instance;

    [Space(10)]
    [Header("VR Components")]
    [Space(5)]
    public GameObject rightController;
    public GameObject leftController;
    public GameObject cameraEye;
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
        cameraEye = GameObject.Find("Headset");
        rightController = GameObject.Find("RightController");
        leftController = GameObject.Find("LeftController");
        leftEvents = leftController.GetComponent<VRTK_ControllerEvents>();
        rightEvents = rightController.GetComponent<VRTK_ControllerEvents>();
    }

    private void Update()
    {
        tick++;
    }
}
