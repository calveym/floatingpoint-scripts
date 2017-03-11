using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour {

    DisplayMenu displayMenu;

    public bool allowAddAirport;
    public bool allowAddTrain;
    public bool allowRemoveMountain;
    public bool allowAddIsland;

    public int level;
    public bool airport;
    public bool train;

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


    public void Start()
    {
        islandLerp = 0f;
        displayMenu = GameObject.Find("LeftController").GetComponent<DisplayMenu>();
        firstIsland = GameObject.Find("Island");
        secondIsland = GameObject.Find("SecondIsland");
        setPosition = new Vector3(1.9f, 0f, -58.8f);
        SetupLevels();
        AddIsland();
    }

    void SetupLevels()
    {
        levelTwo += UnlockBuildingTier;
        levelTwo += AllowRemoveMountains;
        levelThree += UnlockBuildingTier;
    }

    void UnlockBuildingTier()
    {
        displayMenu.SetTier(level + 1);
    }

    public void IncreaseLevel()
    // TODO: call this function when the contract requirements are fulfilled
    {
        if(level == 0)
        {
            levelOne();
        }
        else if(level == 1)
        {
            levelTwo();
        }
        else if(level == 2)
        {
            levelThree();
        }
    }

    void AllowAddAirport()
    {

    }

    void AllowAddTrain()
    {

    }

    public void AddAirport()
    {
        if (allowAddAirport)
        {

        }
    }

    public void AddTrain()
    {

    }

    public void AllowRemoveMountains()
    {
        GameObject[] mountains = GameObject.FindGameObjectsWithTag("mountain");
        for (int i = 0; i < mountains.Length; i++)
        {
            mountains[i].GetComponent<ProgressionTracker>().Enable();
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
