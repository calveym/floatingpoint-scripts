using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRunner : MonoBehaviour
{

    GameObject manager;
    EconomyManager economyManager;
    PopulationManager populationManager;
    ItemGenerator itemGenerator;
    ItemManager itemManager;
    HappinessManager happinessManager;
    ProgressionManager progressionManager;

    ResidentialTracker[] residentialTrackers;
    CommercialTracker[] commercialTrackers;
    IndustrialTracker[] industrialTrackers;

    public GameObject resSpawnerCube;
    public GameObject indSpawnerCube;
    public GameObject comSpawnerCube;

    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("Managers");
        economyManager = manager.GetComponent<EconomyManager>();
        itemManager = manager.GetComponent<ItemManager>();
        happinessManager = manager.GetComponent<HappinessManager>();
        progressionManager = manager.GetComponent<ProgressionManager>();
        itemGenerator = GetComponent<ItemGenerator>();
        
        FindAllChildren();
        EnableAllChildren();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown("q")) FirstTest();
        //if (Input.GetKeyDown("w")) SecondTest();
        //if (Input.GetKeyDown("e")) ThirdTest();
        if (Input.GetKeyDown("r")) FourthTest();
        //if (Input.GetKeyDown("t")) FifthTest();
        //if (Input.GetKeyDown("y")) SixthTest();
        //if (Input.GetKeyDown("u")) SeventhTest();
        //if (Input.GetKeyDown("i")) EighthTest();
        //if (Input.GetKeyDown("o")) NinthTest();
    }

    void FirstTest()
    {
        Debug.Log("Add residential");
        GameObject newObj = itemGenerator.StartSpawn(1, resSpawnerCube);
        resSpawnerCube.GetComponent<FakeSpawnerCube>().RemoveObject(newObj);
    }

    void SecondTest()
    {
        Debug.Log("Add commercial");
        GameObject newObj = itemGenerator.StartSpawn(1, comSpawnerCube);
        comSpawnerCube.GetComponent<FakeSpawnerCube>().RemoveObject(newObj);
    }

    void ThirdTest()
    {
        Debug.Log("Add industrial");
        GameObject newObj = itemGenerator.StartSpawn(1, indSpawnerCube);
        indSpawnerCube.GetComponent<FakeSpawnerCube>().RemoveObject(newObj);
    }

    public void FourthTest()
    {
        Debug.Log("Test 4 Running");
        PopupManager.Popup("TEST POPUP WORKS!!!");
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
