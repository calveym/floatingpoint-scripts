using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour {

    public static ReferenceManager instance;

    public GameObject rightController;
    public GameObject leftController;
    public GameObject cameraEye;

    public GameObject spherePrefab;
    public RoadGenerator roadGenerator;
    public TOD_Sky tod;

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

    public Power power;

    // Use this for initialization
    void Awake () {
		if(instance != this)
        {
            instance = this;
        }
	}
}
