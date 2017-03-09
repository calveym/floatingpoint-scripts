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

    LevelOne levelUpOne;

    LevelTwo levelUpTwo;

    LevelThree levelUpThree;

    public void Start()
    {
        displayMenu = GameObject.Find("LeftController").GetComponent<DisplayMenu>();
        firstIsland = GameObject.Find("Island");
        secondIsland = GameObject.Find("SecondIsland");
        setPosition = new Vector3(1.9f, 0f, -58.8f);
        // AddIsland();
    }

    void UnlockBuildingTier(int tier)
    {
        displayMenu.SetTier(tier);
    }

    public void AddAirport()
    // TODO
    {

    }

    public void AddTrain()
    // TODO
    {

    }

    public void AddIsland()
    {
        Debug.Log("Adding island");
        inPosition = false;
        IEnumerator coroutine = MoveIsland(secondIsland, secondIsland.transform.position, setPosition);
        StartCoroutine(coroutine);
    }

    IEnumerator MoveIsland(GameObject movingIsland, Vector3 source, Vector3 target)
    {
        while(!inPosition)
        {
            Debug.Log("Not in position"); 
            movingIsland.transform.position = target * Time.deltaTime * 0.2f;
        }
        if(movingIsland.transform.position == target)
        {
            inPosition = true;
        }
        yield return null;
    }
}
