using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;

public class ItemTracker : MonoBehaviour {

    // Declare variables
    public PopulationManager populationManager;
    public HappinessManager happinessManager;
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

    public float buyCost;
    public float baseCost;
    public int level;
    public string type;
    public bool usable;
    public bool grabbableObject;

    public int capacity;
    public float income;
    public int users;
    public float numSnappedRoads;
    public bool updateStarted;
    public bool validPosition;
    public float foliageIncome;
    public float landValue;

    public float localHappiness; // Happiness based on number of nice surroundings
    public float fillRateHappiness; // Happiness based on fill rate (users / cap * 40)
    public float addedHappiness; // Temporary tracking figure
    public float currentHappiness;  // Used to affect longterm happiness- longerm happiness tends towards this number
    public float longtermHappiness; // Most important happiness figure, should be used in most significant income, landvalue etc calculations
    public int happinessState;

    public void Start()
    // Sets start variables
    {
        longtermHappiness = 0;
        availableTransportation = 1;
        landValue = 10f;
        land = gameObject.GetComponent<LandValue>();
        happinessManager = GameObject.Find("Managers").GetComponent<HappinessManager>();
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
        if(land == null)
        {
            land = gameObject.GetComponent<LandValue>();
            Debug.Log("Getting land value" + gameObject.name);
        }
        landValue = land.RecalculateLandValue();
        landValue += users * 1.2f;
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

    public void UpdateLocalHappiness()
    //  Sets local happiness level based on surroundings
    {
        Debug.Log("Added Happiness: " + addedHappiness);
        if(addedHappiness < 40)
        {
            localHappiness = addedHappiness;
        }
        else
        {
            localHappiness = 40;
        }
        addedHappiness = 0;

        if(capacity != 0)
        {
            fillRateHappiness = (users / capacity) * 20;
        }
        happinessManager.SendHappiness(happinessState);
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

    public void CalculateLongtermHappiness()
    // makes the longterm happiness tend towards the current happiness
    {
        if(currentHappiness - longtermHappiness < 0.2f)
        {
            longtermHappiness = currentHappiness;
        }
        else
        {
            longtermHappiness += (currentHappiness - longtermHappiness) * 0.1f;
        }
    }

    public void CalculateHappinessState()
    {
        if (longtermHappiness < 20)
        {
            happinessState = 1;
        }
        else if (longtermHappiness < 60 && longtermHappiness >= 20)
        {
            happinessState = 2;
        }
        else if (longtermHappiness < 80 && longtermHappiness >= 60)
        {
            happinessState = 3;
        }
        else if (longtermHappiness >= 80)
        {
            happinessState = 4;
        }
    }
}
