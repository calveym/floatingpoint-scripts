using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour {

    DisplayMenu displayMenu;
    PopulationManager populationManager;
    PopupManager popupManager;

    public bool allowAddAirport;
    public bool allowAddTrain;
    public bool allowRemoveMountain;
    public bool allowAddIsland;

    public int level;
    int pop;

    public bool airport;
    public bool train;
    GameObject airport;
    GameObject train;
    TrainManager trainManager;
    AirportManager AirportManager;

    GameObject firstIsland;
    GameObject secondIsland;
    Vector3 setPosition;
    float islandLerp;
    bool inPosition;

    public delegate void LevelOne();
    public delegate void LevelTwo();
    public delegate void LevelThree();
    public static LevelOne levelOne;
    public static LevelTwo levelTwo;
    public static LevelThree levelThree;
    public const int LEVEL_ONE_REQ;
    public const int LEVEL_TWO_REQ;
    public const int LEVEL_THREE_REQ;


    public void Start()
    {
        LEVEL_ONE_REQ = 20;
        LEVEL_TWO_REQ = 50;
        LEVEL_THREE_REQ = 200;
        islandLerp = 0f;
        airport = false;
        train = false;
        airport = GameObject.Find("Airport");
        train = GameObject.Find("Train");
        popupManager = GameObject.Find("Popup");
        airportManager = GameObject.Find("Managers").GetComponent<AirportManager>();
        trainManager = GameObject.Find("Managers").GetComponent<TrainManager>();
        displayMenu = GameObject.Find("LeftController").GetComponent<DisplayMenu>();
        firstIsland = GameObject.Find("Island");
        secondIsland = GameObject.Find("SecondIsland");
        setPosition = new Vector3(1.9f, 0f, -58.8f);
        populationManager = GameObject.Find("Managers").GetComponent<PopulationManager>();
        SetupLevels();
        AddIsland();
    }

    void Update()
    {
        pop = populationManager.totalPopulation;
        RunChecks();
    }

    void TryIncreaseLevel()
    {
        if(level == 0 && pop > LEVEL_ONE_REQ)
        {
            levelOne();
        }
        else if(level == 1 && pop > LEVEL_TWO_REQ)
        {
            levelTwo();
        }
        else if(level == 2 > && pop > LEVEL_THREE_REQ)
        {
            levelThree();
        }
    }

    void SetupLevels()
    {
        levelTwo += UnlockBuildingTier;
        levelTwo += AllowRemoveMountains;
        levelThree += UnlockBuildingTier;
        levelThree += AllowAddIsland;
    }

    void UnlockBuildingTier()
    {
        displayMenu.SetTier(level + 1);
    }

    void AllowAddAirport()
    {
        allowAddAirport = true;
    }

    void AllowAddTrain()
    {
        allowAddTrain = true;
    }

    public void AllowRemoveMountains()
    {
        GameObject[] mountains = GameObject.FindGameObjectsWithTag("mountain");
        for (int i = 0; i < mountains.Length; i++)
        {
            mountains[i].GetComponent<ProgressionTracker>().Enable();
        }
    }

    public void AllowAddIsland()
    {
        allowAddIsland = true;
    }

    public void AddAirport()
    {
        if (allowAddAirport)
        {
            airport.SetActive(true);
            airportManager.StartService();
        }
    }

    public void AddTrain()
    {
        if(allowAddTrain)
        {
            train.SetActive(true);
            trainManager.StartService();
        }
    }

    public void AddIsland()
    {
        inPosition = false;
        StartCoroutine(MoveIsland(secondIsland, secondIsland.transform.position, setPosition));
    }

    IEnumerator MoveIsland(GameObject movingIsland, Vector3 source, Vector3 target)
    {
        while(inPosition == false)
        {
            source = Vector3.Lerp(source, target, islandLerp);
            if(islandLerp <= 1)
            {
                islandLerp += 0.002f;
            }
            else
            {
                inPosition = true;
            }
            yield return null;
        }
    }
}
