using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;

public class ItemTracker : MonoBehaviour {

    // Declare variables
    public PopulationManager populationManager;
    public EconomyManager economyManager;
    public ItemManager itemManager;

    public static float totalIncome;
    public string type;
    public int capacity;
    public float income;
    public int users;
    public bool usable;
    public bool grabbableObject;
    public bool validPosition;
    GameObject tooltip;

    public float landValue;

    private void Awake()
    // Sets start variables
    {
        landValue = 10f; // TODO: Make better calculations based on accessibility to various ameneties.
        populationManager = GameObject.Find("Managers").GetComponent<PopulationManager>();
        economyManager = GameObject.Find("Managers").GetComponent<EconomyManager>();
        itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();

        usable = false;
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

    public void AddUsers(int numAdded)
    {
        users += numAdded;
    }

    void EnableObjectTooltip(object sender, ControllerInteractionEventArgs e)
    // Enables, resets position and resets text for object tooltips
    {
        if(tooltip != null)
        {
            Destroy(tooltip);
        }
        tooltip = Instantiate(GameObject.Find("ObjectTooltip"), gameObject.transform);
        Debug.Log(tooltip);

        tooltip.GetComponent<VRTK_ObjectTooltip>().UpdateText("Income: " + income.ToString());
        tooltip.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        tooltip.transform.position = gameObject.transform.position + new Vector3(0f, 2.5f, 0f);
        tooltip.transform.localScale = new Vector3(100f, 100f, 100f);
        tooltip.transform.LookAt(GameObject.Find("Camera (eye)").transform);
    }

    void DisableObjectTooltip(object sender, ControllerInteractionEventArgs e)
    // Removes object tooltips
    {
        Destroy(tooltip.gameObject);
    }

    void OverCapacity()
    // Returns true if more users than capacity
    {
        if (users > capacity)
        {
            DeallocateUsers(capacity -= 1);
        }
    }

    public void setValues(string typeInput, int pressedButton)
    // Type setter
    {
        type = typeInput;
		capacity = (pressedButton * pressedButton + 1);
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
        if(type == "res")
        {
            DeallocateUsers(users);
        }
        else if(type == "com" || type == "ind")
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
            itemManager.residentialTrackers.Add(gameObject.GetComponent<ResidentialTracker >());
            GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().ButtonOnePressed += new ControllerInteractionEventHandler(EnableObjectTooltip);
            GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().ButtonOneReleased += new ControllerInteractionEventHandler(DisableObjectTooltip);
        }
        else if(type == "commercial")
        {
            itemManager.addCommercial(capacity, gameObject);
            GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().ButtonOnePressed += new ControllerInteractionEventHandler(EnableObjectTooltip);
            GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().ButtonOneReleased += new ControllerInteractionEventHandler(DisableObjectTooltip);
        }
        else if(type == "industrial")
        {
            itemManager.addIndustrial(capacity, gameObject);
            GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().ButtonOnePressed += new ControllerInteractionEventHandler(EnableObjectTooltip);
            GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().ButtonOneReleased += new ControllerInteractionEventHandler(DisableObjectTooltip);
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
}
