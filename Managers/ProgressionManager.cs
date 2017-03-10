using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour {

    DisplayMenu displayMenu;

    public int level;
    public bool airport;
    public bool train;

    GameObject firstIsland;
    GameObject secondIsland;
    Vector3 setPosition;
    bool inPosition;

    public delegate void LevelOne();

    public delegate void LevelTwo();

    public delegate void LevelThree();

    static LevelOne levelOne;

    static LevelTwo levelTwo;

    static LevelThree levelThree;


    public void Start()
    {
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

    public void AddAirport()
    {

    }

    public void AddTrain()
    {

    }

    public static void AllowRemoveMountains()
    {
        GameObject[] mountains = GameObject.FindGameObjectsWithTag("mountain");
        for (int i = 0; i < mountains.Length; i++)
        {
            mountains[i].GetComponent<ProgressionTracker>().Enable();
        }
    }

    public void AddIsland()
    {
        Debug.Log("Running");
        inPosition = false;
        IEnumerator coroutine = MoveIsland(secondIsland, secondIsland.transform.position, setPosition);
        StartCoroutine(coroutine);
    }

    IEnumerator MoveIsland(GameObject movingIsland, Vector3 source, Vector3 target)
    {
        while(!inPosition)
        {
            movingIsland.transform.position = target * Time.deltaTime * 0.2f;
        }
        if(source == target)
        {
            inPosition = true;
        }
        yield return null;
    }
}
