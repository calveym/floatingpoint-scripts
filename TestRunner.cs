using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRunner : MonoBehaviour
{

    GameObject manager;
    EconomyManager economyManager;
    PopulationManager populationManager;
    ItemManager itemManager;
    HappinessManager happinessManager;
    ProgressionManager progressionManager;

    ResidentialTracker[] residentialTrackers;
    CommercialTracker[] commercialTrackers;
    IndustrialTracker[] industrialTrackers;

    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("Managers");
        economyManager = manager.GetComponent<EconomyManager>();
        itemManager = manager.GetComponent<ItemManager>();
        happinessManager = manager.GetComponent<HappinessManager>();
        progressionManager = manager.GetComponent<ProgressionManager>();
        

        FindAllChildren();
        EnableAllChildren();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1")) FirstTest();
        if (Input.GetKeyDown("2")) SecondTest();
        if (Input.GetKeyDown("3")) ThirdTest();
        if (Input.GetKeyDown("4")) FourthTest();
        if (Input.GetKeyDown("5")) FifthTest();
        if (Input.GetKeyDown("6")) SixthTest();
        if (Input.GetKeyDown("7")) SeventhTest();
        if (Input.GetKeyDown("8")) EighthTest();
        if (Input.GetKeyDown("9")) NinthTest();
    }

    void FirstTest()
    {
        Debug.Log("Test 1 Running");
    }

    void SecondTest()
    {
        Debug.Log("Test 2 Running");
    }

    void ThirdTest()
    {
        Debug.Log("Test 3 Running");
    }

    void FourthTest()
    {
        Debug.Log("Test 4 Running");
    }

    void FifthTest()
    {
        Debug.Log("Test 5 Running");
    }

    void SixthTest()
    {
        Debug.Log("Test 6 Running");
    }

    void SeventhTest()
    {
        Debug.Log("Test 7 Running");
    }

    void EighthTest()
    {
        Debug.Log("Test 8 Running");
    }

    void NinthTest()
    {
        Debug.Log("Test 9 Running");
    }

    void FindAllChildren()
    {
        residentialTrackers = transform.GetAllComponentsInChildren<ResidentialTracker>();
        commercialTrackers = transform.GetAllComponentsInChildren<CommercialTracker>();
        industrialTrackers = transform.GetAllComponentsInChildren<IndustrialTracker>();
    }

    void EnableAllChildren()
    {
        for(int i = 0; i < residentialTrackers.Length; i++)
        {
            residentialTrackers[i].SetUsable();
        }
        for(int i = 0; i < commercialTrackers.Length; i++)
        {
            commercialTrackers[i].SetUsable();
        }
        for(int i = 0; i < industrialTrackers.Length; i++)
        {
            industrialTrackers[i].SetUsable();
        }
    }
}
