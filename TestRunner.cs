using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autelia.Serialization;

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

    public delegate void First();
    public static First first;

    public delegate void Second();
    public static Second second;

    public delegate void Third();
    public static Third third;

    public delegate void Fourth();
    public static Fourth fourth;

    TOD_Sky tod;

    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("Managers");
        economyManager = manager.GetComponent<EconomyManager>();
        itemManager = manager.GetComponent<ItemManager>();
        happinessManager = manager.GetComponent<HappinessManager>();
        progressionManager = manager.GetComponent<ProgressionManager>();
        itemGenerator = GetComponent<ItemGenerator>();

        tod = ReferenceManager.instance.tod;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("z")) FirstTest();
        if (Input.GetKeyDown("x")) SecondTest();
        if (Input.GetKeyDown("c")) ThirdTest();
        if (Input.GetKeyDown("v")) FourthTest();
        if (Input.GetKeyDown("t")) FifthTest();
        if (Input.GetKeyDown("y")) SixthTest();
        if (Input.GetKeyDown("u")) SeventhTest();
        //if (Input.GetKeyDown("i")) EighthTest();
        //if (Input.GetKeyDown("o")) NinthTest();
        if (Input.GetKeyDown("[7]")) SetTime(0);
        if (Input.GetKeyDown("[8]")) SetTime(5f);
        if (Input.GetKeyDown("[9]")) SetTime(6);
        if (Input.GetKeyDown("[4]")) SetTime(7.5f);
        if (Input.GetKeyDown("[5]")) SetTime(10);
        if (Input.GetKeyDown("[6]")) SetTime(12.5f);
        if (Input.GetKeyDown("[1]")) SetTime(15f);
        if (Input.GetKeyDown("[2]")) SetTime(17.5f);
        if (Input.GetKeyDown("[3]")) SetTime(20);

    }

    void SetTime(float time)
    {
        if (!tod)
            tod = ReferenceManager.instance.tod;
        tod.Cycle.Hour = time;
    }

    void FirstTest()
    {
        Debug.Log("Test 1");
        first();
    }

    void SecondTest()
    {
        Debug.Log("Test 2");
        second();
    }

    void ThirdTest()
    {
        Debug.Log("Test 3");
        third();
    }

    public void FourthTest()
    {
        Debug.Log("Test 4");
        fourth();
    }

    void FifthTest()
    {
        Debug.Log("Test 5 Running");
        manager.GetComponent<TooltipManager>().DisableTooltips();
    }

    void SixthTest()
    {
        Debug.Log("Test 6 Running");
        TooltipManager.pressed = true;
        manager.GetComponent<TooltipManager>().StartTooltips();
    }

    void SeventhTest()
    {
        Debug.Log("Test 7 Running");
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    void EighthTest()
    {
        Debug.Log("Test 8 Running");
    }

    void NinthTest()
    {
        Debug.Log("Test 9 Running");
    }
}
