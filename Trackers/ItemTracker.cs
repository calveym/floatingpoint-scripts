using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;

public class ItemTracker : MonoBehaviour {

    // Declare variables
    public PopulationManager populationManager;
    public EconomyManager economyManager;
    public ItemManager itemManager;
    public LandValue land;
    public float availableTransportation;

    public static float totalResidentialIncome;
    public static float historicResidentialIncome;
    public static float totalCommercialIncome;
    public static float historicCommercialIncome;
    public static float totalIndustrialIncome;
    public static float historicIndustrialIncome;

    public float baseCost;
    public float localHappiness;
    public int level;
    public string type;
    public int capacity;
    public float income;
    public int users;
    public bool usable;
    public float numSnappedRoads;
    public bool updateStarted;
    public bool grabbableObject;
    public bool validPosition;
    public GameObject tooltip;
    public float addedHappiness;

    public float landValue;

    private void Start()
    // Sets start variables
    {
        availableTransportation = 1;
        landValue = 10f;
        land = GetComponent<LandValue>();
        populationManager = GameObject.Find("Managers").GetComponent<PopulationManager>();
        economyManager = GameObject.Find("Managers").GetComponent<EconomyManager>();
        itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();

        //usable = false;
    }

    void EnablePhysics()
    // Enables physics if object has no parent
    {
        if (transform.parent == null && gameObject.tag != "road")
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;
        }
    }

    public void UpdateLandValue()
    {
        Debug.Log(land);
        landValue = land.RecalculateLandValue();
        landValue += users;
        landValue += numSnappedRoads;
        if (capacity == users)
        {
            landValue += 7;
        }
    }

    public void UpdateTransportationValue()
    // Updates available transportation depending on road presence and airport and train availability
    {
        availableTransportation = RoadAccess() + ProgressionManager.Airport() + ProgressionManager.Train();
    }

    public void UpdateHappiness()
    //  Sets local happiness level based on surroundings
    {
        localHappiness = addedHappiness;
        addedHappiness = 0;
    }

    float RoadAccess()
    {
        return 1;
    }

    public void AddUsers(int numAdded)
    {
        users += numAdded;
    }

    void OverCapacity()
    // Returns true if more users than capacity
    {
        if (users > capacity)
        {
            DeallocateUsers(capacity -= 1);
        }
    }

    public void setValues()
    // Type setter
    {
        type = gameObject.tag;
    }

    void DeallocateUsers(int numUsers)
    // Returns unallocated users to the populationManager
    {
        populationManager.DeallocateUsers(numUsers, type);
        users -= numUsers;
    }

    public void RemoveAllUsers()
    // Removes all local and related itemManager users
    {
        if(type == "residential")
        {
            DeallocateUsers(users);
        }
        else if(type == "commercial" || type == "industrial")
        {
            // TODO: Write employment deallocation function in popman
        }
    }

    public int NumEmpty()
    // Returns number of available spaces
    {
        return capacity - users;
    }

    public int GetCapacity()
    // Returns building capacity
    {
        return capacity;
    }

    public void SetUsable()
    // Separate parts called when objects are enabled. Add additional setup calls here
    {
        usable = true;
        if(type == "residential")
        {
            itemManager.addResidential(capacity, gameObject);
            populationManager.QueueUpdates();
            //GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().ButtonOnePressed += new ControllerInteractionEventHandler(EnableObjectTooltip);
            //GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().ButtonOneReleased += new ControllerInteractionEventHandler(DisableObjectTooltip);
        }
        else if(type == "commercial")
        {
            itemManager.addCommercial(capacity, gameObject);

            populationManager.QueueUpdates();
            //GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().ButtonOnePressed += new ControllerInteractionEventHandler(EnableObjectTooltip);
            //GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().ButtonOneReleased += new ControllerInteractionEventHandler(DisableObjectTooltip);
        }
        else if(type == "industrial")
        {
            itemManager.addIndustrial(capacity, gameObject);
            populationManager.QueueUpdates();
            //GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().ButtonOnePressed += new ControllerInteractionEventHandler(EnableObjectTooltip);
            //GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().ButtonOneReleased += new ControllerInteractionEventHandler(DisableObjectTooltip);
        }
        else if(type == "foliage")
        {
            itemManager.addFoliage(capacity, gameObject);
        }
        else if(type == "leisure")
        {
            itemManager.addLeisure(capacity, gameObject);
        }
    }

    public void ModifyHappiness(float amount, string trigger)
    {
        addedHappiness += amount;
        if(trigger == "industrialReduce" && type != "industrial")
        {
            //PopupManager.Popup("Warning!");
            //PopupManager.Popup("Factories reducing happiness");
        }
    }
}
