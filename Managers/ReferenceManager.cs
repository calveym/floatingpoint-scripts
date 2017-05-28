using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour {

    public static ReferenceManager instance;

    public GameObject rightController;
    public GameObject leftController;
    public GameObject cameraEye;

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

    [Space(10)]
    [Header("Income modification")]
    [Space(5)]
    public float residentialIncomeMultiplier = 0.32f;
    public float commercialIncomeMultiplier = 1.42f;
    public float industrialIncomeMultiplier = 0.6f;

    // Use this for initialization
    void Awake () {
		if(instance != this)
        {
            instance = this;
        }
	}
}
