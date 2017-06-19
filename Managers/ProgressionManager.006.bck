using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour {

    static ProgressionManager instance;

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
    public AudioClip levelUpSound;

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

    public delegate void LevelUp();

    LevelUp levelUp;

    public void Start()
    {
        level = 10;
        if(instance == null)
        {
            instance = this;
        }
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
        levelReq.Add(2, 25);
        levelReq.Add(3, 50);
        levelReq.Add(4, 100);
        levelReq.Add(5, 175);
        levelReq.Add(6, 225);
        levelReq.Add(7, 300);
        levelReq.Add(8, 500);
        levelReq.Add(9, 750);
        levelReq.Add(10, 1000);
        levelReq.Add(11, 1500);
        levelReq.Add(12, 2000);
        levelReq.Add(13, 3000);
        levelReq.Add(14, 5000);
        levelReq.Add(15, 7500);
        levelReq.Add(16, 100000);

        StartCoroutine("SlowUpdate");
    }
    
    void CheckLevelUp()
    {
        // Perform check to see whether next level that returns false from levelInfo can be completed.
        int currentLevelReq;
        levelReq.TryGetValue(level + 1, out currentLevelReq);
        if(populationManager.totalPopulation >= currentLevelReq)
        {
            PerformLevelUp(level + 1);
        }
    }

    void PerformLevelUp(int newLevel)
    {
        string levelUpString = string.Format("Level up! You are now Level {0} You have unlocked some new buildings!",
                                             level);
        PopupManager.Popup(levelUpString);

        if (levelUp != null)
            levelUp();
        AudioManager.instance.PlaySingle(levelUpSound);
        level = newLevel;
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

    public static string ToNextLevel()
    {
        int currentLevelReq;
        instance.levelReq.TryGetValue(level + 1, out currentLevelReq);
        return (currentLevelReq - instance.populationManager.population).ToString();
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
            yield return new WaitForSeconds(1);
        }
    }
}
