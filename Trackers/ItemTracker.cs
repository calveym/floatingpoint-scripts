using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;

public class ItemTracker : MonoBehaviour {

    // Declare variables
    protected PopulationManager populationManager;
    protected HappinessManager happinessManager;
    protected EconomyManager economyManager;
    protected ItemManager itemManager;

    [Header("Components")]
    [Space(5)]
    public LandValue land;
    public float availableTransportation;
    public float numSnappedRoads;

    public static float totalResidentialIncome;
    public static float historicResidentialIncome;
    public static float totalCommercialIncome;
    public static float historicCommercialIncome;
    public static float totalIndustrialIncome;
    public static float historicIndustrialIncome;
    [Space(10)]

    [Header("Basic attributes")]
    [Space(5)]
    [Tooltip("Note- currently not used")]
    public int level = 0;
    [Tooltip("Initial purchase price")]
    public float buyCost;
    [Tooltip("Upkeep cost, deducted each ecoTick from income")]
    public float baseCost;
    public string type;
    public bool grabbableObject;
    [Tooltip("Maximum number of users")]
    public int capacity;

    [Header("Instance attributes")]
    [Space(5)]
    public bool usable;
    public float income;
    public int users;
    public bool updateStarted;
    public bool validPosition;
    public float landValue;
    [Space(10)]

    [Header("Unhappiness")]
    [Space(5)]
    [Range(-20f, 40f)]
    public float localHappiness; // Happiness based on number of nice surroundings

    [Range(0f, 40f)]
    [Tooltip("Amount of unhappiness required before users vacate")]
    public float requiredUnhappiness = 10;

    [Range(0f, 100f)]
    [Tooltip("Amount of happiness required to decrease unhappiness")]
    public float unhappinessMaximum = 60;  // Used in unhappiness check calculations

    [Range(0f, 50f)]
    [Tooltip("Amount of happiness required to increase unhappiness")]
    public float unhappinessMinimum = 40;  // Used in unhappiness check calculations

    protected bool movingOut;  // Used to control moving out coroutine
    [Space(10)]

    [Header("Happiness")]
    [Space(5)]
    public float fillRateHappiness; // Happiness based on fill rate (users / cap * 40)
    public float addedHappiness; // Temporary tracking figure
    public float currentHappiness;  // Used to affect longterm happiness- longerm happiness tends towards this number
    public float longtermHappiness; // Most important happiness figure, should be used in most significant income, landvalue etc calculations
    public int happinessState;

    protected int cumulativeUnhappiness = 0;  // Used to determine when people start moving out from unhappiness
    [Space(10)]

    [Header("Services")]
    [Space(5)]
    public bool power;
    public bool health;
    public bool education;
    public bool fire;
    public bool police;

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
        if(addedHappiness < 40 && addedHappiness > -20)
        {
            localHappiness = addedHappiness;
        }
        else if(addedHappiness >= 40)
        {
            localHappiness = 40;
        }
        else if(addedHappiness <= -20)
        {
            localHappiness = -20;
        }
        addedHappiness = 0;

        if(capacity != 0)
        {
            fillRateHappiness = ((float)users / (float)capacity) * 20;
        }
        happinessManager.SendHappiness(happinessState);
    }

    float RoadAccess()
    {
        return 1;
    }

    public virtual void AddUsers(int numAdded)
    {
        users += numAdded;
    }

    void OverCapacity()
    // Returns true if more users than capacity
    {
        if (users > capacity)
        {
            RemoveUsers(capacity -= 1);
        }
    }

    public void setValues()
    // Type setter
    {
        type = gameObject.tag;
    }

    protected void RemoveUsers(int numUsers)
    // Returns unallocated users to the populationManager
    {
        if(gameObject.tag == "residential")
        {
            populationManager.DeallocateUsers(numUsers, type);
            users -= numUsers;
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
            longtermHappiness += (currentHappiness - longtermHappiness) * 0.05f;
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

    protected IEnumerator MoveOut()
    {
        while (movingOut)
        {
            if (users >= 1)
            {
                RemoveUsers(1);
            }
            else movingOut = false;
            yield return new WaitForSeconds(2f);
        }
    }
}
