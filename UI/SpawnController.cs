using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using Autelia.Serialization;

public class SpawnController : MonoBehaviour
{

    SpawnManager spawnManager;  // Reference saved to help spawn in new building on enable of old
    EconomyManager economyManager;
    ThumbTracker thumb;

    int level;
    public int unit;  // Refers to which sphere this is on
    public bool showingBuilding;
    bool selected;  // Used to stop coroutine
    bool selectedStarted;  // Used to tell if coroutine already running- for starting

    GameObject containedBuilding;  // Building currently spawned in sphere. This reference will be used to delete it later on.
    Vector3 oldScale;
    Renderer containedRenderer;  // Used to find size for scale up/ down
    float scaleFactor;
    int containedType;  // contained building type
    int menuSelection;  // What the type should be
    DisplayUI displayUI;
    float price;


    bool disablePurchase;
    public Material disablePurchaseMaterial;

    // Trackers for enabling/ disabling
    ResidentialTracker res;
    CommercialTracker com;
    IndustrialTracker ind;
    CommercialTracker off;
    IndustrialComponent indc;
    FoliageTracker fol;
    ServiceTrackerBase serv;
    ItemTracker tracker;


    private void Start()
    {
        if (Serializer.IsLoading) return;
        displayUI = transform.parent.transform.parent.GetComponent<DisplayUI>();
        economyManager = GameObject.Find("Managers").GetComponent<EconomyManager>();
        thumb = transform.parent.transform.parent.GetComponent<ThumbTracker>();
    }

    private void Update()
    {
        if (selected)
        {
            PerformSelect();
        }
    }

    public GameObject ReturnContainedBuilding()
    {
        return containedBuilding;
    }

    public void EnableBuilding()
    // Call this to spawn the building 
    {
        if (!disablePurchase && economyManager.GetBalance() > price && containedBuilding)
        {
            economyManager.MakePurchase(price);
            SizeForPlay();
            DeselectBuilding();
            if (containedBuilding.tag == "industrial" || containedBuilding.tag == "industrialComponent")
            // Smoke deactivation
            {
                foreach (Transform child in containedBuilding.transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
            else if (containedBuilding.tag == "service")
            {
                serv.AddService();
            }
            EnablePhysics();
            if (containedType == 0)
            {
                EnableResidential();
            }
            else if (containedType == 1)
            {
                EnableCommercial();
            }
            else if (containedType == 2)
            {
                EnableIndustrial();
            }
            else if (containedType == 3)
            {
                EnableCommercial();
            }
            else if (containedType == 4)
            {
                EnableComponent();
            }
            else if (containedType == 5)
            {
                EnableFoliage();
            }
            else if (containedType == 6)
            {
                EnableService();
            }
            // Spawns new from here!!!!!
            showingBuilding = false;
            spawnManager.SpawnUIBuildings(displayUI.GetSelection(), thumb.angleIncrement);
        }
        else
        {
            economyManager.FailedPurchase();
        }
    }

    public void DeleteBuilding()
    {
        selected = false;
        selectedStarted = false;
        if (showingBuilding)
        {
            Destroy(containedBuilding);
            showingBuilding = false;
        }
    }

    void SelectBuilding()
    // TODO: Display building stats in displayMenu here
    // Start slow building rotation here
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
        selected = true;
        PerformSelect();

    }

    void PerformSelect()
    {
        List<string> sendList = new List<string>();
        sendList.Add(FancyType());
        sendList.Add(FancyCapacity());
        sendList.Add(FancyLevel());
        sendList.Add(FancyWeekCost());
        sendList.Add(FancyBuyCost());
        displayUI.SendSelectedText(sendList);
        containedBuilding.transform.Rotate(new Vector3(0, 2f, 0));
    }

    void DeselectBuilding()
    {
        selected = false;
        //containedBuilding.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
    }

    public void DisableBuilding(SpawnManager sm, GameObject newBuilding)
    // Called on every new building
    {
        if (!spawnManager)
        {
            spawnManager = sm;
        }

        disablePurchase = false;
        UpdateContainedBuilding(newBuilding);
        SetTracker();
        if (containedBuilding.tag != "industrialComponent" && containedBuilding.tag != "foliage")
            containedBuilding.GetComponent<RoadSnap>().SetupSnap();
        CheckLevel();
        DisablePhysics();
        SizeForMenu();

        if (unit == 2)
        {
            containedBuilding.transform.localScale *= 1.3f;

            SelectBuilding();
        }

        if (newBuilding.tag == "industrial" || newBuilding.tag == "industrialComponent")
        // Smoke deactivation
        {
            foreach (Transform child in newBuilding.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    void CheckLevel()
    {
        RetrieveLevel();
        //Debug.Log("Level: " + level);
        if (level > ProgressionManager.level)
        {
            disablePurchase = true;
            containedBuilding.GetComponent<Renderer>().material = disablePurchaseMaterial;
        }
    }

    void RetrieveLevel()
    {
        switch (containedType)
        {
            case 0:
                level = res.level;
                break;
            case 1:
                level = com.level;
                break;
            case 2:
                level = ind.level;
                break;
            case 3:
                level = off.level;
                break;
            case 4:
                level = indc.level;
                break;
            case 5:
                level = 2;
                break;
            case 6:
                level = 1;
                break;
        }
    }

    public Vector3 GetInstantiatePosition()
    {
        return transform.position - (new Vector3(0f, 0.3f, 0));
    }

    public bool Empty()
    {
        return !showingBuilding;
    }

    void EnableResidential()
    {
        price = res.buyCost;
        res.usable = true;
    }

    void EnableCommercial()
    {
        if (com)
        {
            price = com.buyCost;
            com.usable = true;
        }
        else if (off)
        {
            price = off.buyCost;
            off.usable = true;
        }
    }

    void EnableIndustrial()
    {
        price = ind.buyCost;
        ind.usable = true;
    }

    void EnableComponent()
    {
        price = indc.buyCost;
        indc.usable = true;
    }

    void EnableFoliage()
    {
        price = fol.buyCost;
        fol.happinessAffector.usable = true;
    }

    void EnableService()
    {
        price = serv.buyCost;
        serv.active = true;
    }

    void DisablePhysics()
    {
        containedBuilding.transform.parent = this.transform;
        Rigidbody rb = containedBuilding.GetComponent<Rigidbody>();
        containedBuilding.GetComponent<VRTK_InteractableObject>().isGrabbable = false;
        containedBuilding.GetComponent<BoxCollider>().enabled = false;
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    void EnablePhysics()
    {
        containedBuilding.transform.parent = null;
        Rigidbody rb = containedBuilding.GetComponent<Rigidbody>();
        containedBuilding.GetComponent<VRTK_InteractableObject>().isGrabbable = true;
        containedBuilding.GetComponent<BoxCollider>().enabled = true;
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    void UpdateContainedBuilding(GameObject newBuilding)
    {
        if (!displayUI)
        {
            displayUI = transform.parent.transform.parent.gameObject.GetComponent<DisplayUI>();
        }
        menuSelection = displayUI.GetSelection();
        containedBuilding = newBuilding;

        containedRenderer = containedBuilding.GetComponent<Renderer>();
        containedType = menuSelection;
        showingBuilding = true;
    }

    void SetTracker()
    {
        ClearTrackers();
        tracker = containedBuilding.GetComponent<ItemTracker>();
        if (containedType == 0)
        {
            res = containedBuilding.GetComponent<ResidentialTracker>();
            res.usable = false;
        }
        else if (containedType == 1)
        {
            com = containedBuilding.GetComponent<CommercialTracker>();
            com.usable = false;
        }
        else if (containedType == 2)
        {
            ind = containedBuilding.GetComponent<IndustrialTracker>();
            ind.usable = false;
        }
        else if (containedType == 3)
        {
            off = containedBuilding.GetComponent<CommercialTracker>();
            off.usable = false;
        }
        else if (containedType == 4)
        {
            indc = containedBuilding.GetComponent<IndustrialComponent>();
            indc.usable = false;
        }
        else if (containedType == 5)
        {
            fol = containedBuilding.GetComponent<FoliageTracker>();
            fol.active = false;
        }
        else if (containedType == 6)
        {
            serv = containedBuilding.GetComponent<ServiceTrackerBase>();
            serv.active = true;
        }
    }

    void ClearTrackers()
    {
        res = null;
        com = null;
        ind = null;
        off = null;
        indc = null;
        fol = null;
        serv = null;
        tracker = null;
    }

    void SizeForMenu()
    {
        if (containedRenderer)
        {
            Vector3 size = containedRenderer.bounds.size;
            if (size.x > size.y && size.x > size.z)
            {
                // size x 
                scaleFactor = size.x * 2f;
            }
            else if (size.y > size.x && size.y > size.z)
            {
                // size y
                scaleFactor = size.y * 2f;
            }
            else if (size.z > size.x && size.z > size.y)
            {
                // size z
                scaleFactor = size.z * 2f;
            }
            oldScale = containedBuilding.transform.localScale;
            containedBuilding.transform.localScale *= (1 / scaleFactor);
        }
        else
        {
            oldScale = containedBuilding.transform.localScale;
            containedBuilding.transform.localScale *= (1 / 2);
        }
    }

    void SizeForPlay()
    {
        if (unit == 2)
        {
            containedBuilding.transform.localScale = oldScale;
        }
    }

    string FancyType()
    {
        if (tracker != null)
            return tracker.buildingName;
        else if (containedBuilding.tag == "industrialComponent")
            return "Industrial Upgrade";
        else if (containedBuilding.tag == "service")
            return "Service";
        else if (containedBuilding.tag == "foliage")
        {
            return "Foliage";
        }
        else return " ";
    }

    string FancyCapacity()
    {
        switch (containedType)
        {
            case 0:
                return "Capacity: " + res.capacity;
            case 1:
                return "Capacity: " + com.capacity;
            case 2:
                return "Capacity: " + ind.capacity;
            case 3:
                return "Capacity: " + off.capacity;
            case 4:
                return "Production: " + indc.productionMulti;
            case 5:
                return "Range: " + fol.radius;
            case 6:
                return "Max Buildings Covered: " + serv.amount;
        }
        return "";
    }

    string FancyLevel()
    {
        switch (containedType)
        {
            case 0:
                return "Level: " + res.level;
            case 1:
                return "Level: " + com.level;
            case 2:
                return "Level: " + ind.level;
            case 3:
                return "Level: " + off.level;
            case 4:
                return "Level: " + indc.level;
            case 5:
                return "Increase amount: " + fol.GetAffectAmount();
            case 6:
                return "Level: " + serv.level;
        }
        return "";
    }

    string FancyWeekCost()
    {
        switch (containedType)
        {
            case 0:
                return "Weekly cost: $" + res.baseCost;
            case 1:
                return "Weekly cost: $" + com.baseCost;
            case 2:
                return "Weekly cost: $" + ind.baseCost;
            case 3:
                return "Weekly cost: $" + off.baseCost;
            case 4:
                return "Weekly cost: $" + indc.baseCost;
            case 5:
                return "";
            case 6:
                return "Weekly cost: $" + serv.cost;
        }
        return "";
    }

    string FancyBuyCost()
    {
        switch (containedType)
        {
            case 0:
                return "Buy Cost: $" + res.buyCost;
            case 1:
                return "Buy Cost: $" + com.buyCost;
            case 2:
                return "Buy Cost: $" + ind.buyCost;
            case 3:
                return "Buy Cost: $" + off.buyCost;
            case 4:
                return "Buy Cost: $" + indc.buyCost;
            case 5:
                return "";
            case 6:
                return "Buy Cost: $" + serv.buyCost;
        }
        return "";
    }
}
