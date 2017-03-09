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

    LevelOne levelOne;

    LevelTwo levelTwo;

    LevelThree levelThree;


    public void Start()
    {
        displayMenu = GameObject.Find("LeftController").GetComponent<DisplayMenu>();
        firstIsland = GameObject.Find("Island");
        secondIsland = GameObject.Find("SecondIsland");
        setPosition = new Vector3(1.9f, 0f, -58.8f);
        levelTwo += UnlockBuildingTier;
        levelTwo += AllowRemoveMountains;
        levelThree += UnlockBuildingTier;
        // AddIsland();
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

        }
        else if(level == 1)
        {

        }
        else if(level == 2)
        {

        }
    }

    public void AddAirport()
    {

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
        Debug.Log("Running");
        inPosition = true;
        IEnumerator coroutine = MoveIsland(secondIsland, secondIsland.transform.position, setPosition);
        StartCoroutine(coroutine);
    }

    IEnumerator MoveIsland(GameObject movingIsland, Vector3 source, Vector3 target)
    {
        while(!inPosition)
        {
            movingIsland.transform.position = target * Time.deltaTime * 0.2f;
        }
        yield return null;
    }
}
