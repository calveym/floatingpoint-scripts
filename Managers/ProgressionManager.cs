using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour {

    public int level;
    public bool airport;
    public bool train;

    GameObject firstIsland;
    GameObject secondIsland;
    Vector3 setPosition;
    bool inPosition;

    public void Start()
    {
        AddIsland();
        firstIsland = GameObject.Find("Island");
        secondIsland = GameObject.Find("SecondIsland");
        setPosition = new Vector3(1.9f, 0f, -58.8f);
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
