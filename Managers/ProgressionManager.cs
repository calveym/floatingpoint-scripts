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

    public static int level;
    public bool allowLevelUp;
    int pop;

    public static bool airportBought;
    public static bool trainBought;
    GameObject airport;
    GameObject train;
    TrainManager trainManager;
    AirportManager airportManager;

    GameObject firstIsland;
    GameObject secondIsland;
    Vector3 setPosition;
    float islandLerp;
    bool inPosition;

    Dictionary<int, int> levelReq; // Requirement for population to level up
    Dictionary<int, bool> levelInfo; // Stores which levels have been unlocked

    public delegate void LevelUp();

    LevelUp levelUp;

    public void Start()
    {
        islandLerp = 0f;
        airportBought = false;
        trainBought = false;
        airport = GameObject.Find("Airport");
        train = GameObject.Find("Train");
        popupManager = GameObject.Find("Managers").GetComponent<PopupManager>();
        airportManager = GameObject.Find("Managers").GetComponent<AirportManager>();
        trainManager = GameObject.Find("Managers").GetComponent<TrainManager>();
        displayMenu = GameObject.Find("LeftController").GetComponent<DisplayMenu>();
        firstIsland = GameObject.Find("Island");
        secondIsland = GameObject.Find("SecondIsland");
        setPosition = new Vector3(1.9f, 0f, -58.8f);
        populationManager = GameObject.Find("Managers").GetComponent<PopulationManager>();
        AddIsland();
        levelReq = new Dictionary<int, int>();
        levelReq.Add(1, 10);
        levelReq.Add(2, 100);
        levelReq.Add(3, 250);
        levelReq.Add(4, 500);
        levelReq.Add(5, 1000);
        levelReq.Add(6, 2000);
        levelReq.Add(7, 3200);
        levelReq.Add(8, 5000);
        levelReq.Add(9, 7000);
        levelReq.Add(10, 12000);
        levelReq.Add(11, 18000);
        levelReq.Add(12, 25000);
        levelReq.Add(13, 34000);
        levelReq.Add(14, 46000);
        levelReq.Add(15, 60000);
        level = 14;
        StartCoroutine("SlowUpdate");
    }
    
    public void CheckLevelUp()
    {
        // Perform check to see whether next level that returns false from levelInfo can be completed.
        int currentLevelReq;
        levelReq.TryGetValue(level + 1, out currentLevelReq);
        if(populationManager.totalPopulation * 0.01  >= currentLevelReq)
        {
            PerformLevelUp(level + 1);
        }
    }

    void PerformLevelUp(int newLevel)
    {
        if(level + 1 == newLevel)
        {
            level = newLevel;
            levelInfo[level] = true;
        }
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

    IEnumerator SlowUpdate()
    {
        while(true)
        {
            if(allowLevelUp)
            {
                CheckLevelUp();
            }
            yield return new WaitForSeconds(5);
        }
    }

    public static float Airport()
    {
        if (airportBought)
        {
            return 1;
        }
        else return 0;
    }

    public static float Train()
    {
        if (trainBought)
        {
            return 1;
        }
        else return 0;
    }
}
