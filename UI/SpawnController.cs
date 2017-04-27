using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

    SpawnManager spawnManager;  // Reference saved to help spawn in new building on enable of old
    ThumbTracker thumb;

    public int unit;  // Refers to which sphere this is on
    public bool showingBuilding;

    GameObject containedBuilding;  // Building currently spawned in sphere. This reference will be used to delete it later on.
    Renderer containedRenderer;  // Used to find size for scale up/ down
    float scaleFactor;
    int containedType;  // contained building type
    int menuSelection;  // What the type should be
    DisplayUI displayUI; 

    // Trackers for enabling/ disabling
    ResidentialTracker res;
    CommercialTracker com;
    IndustrialTracker ind;
    CommercialTracker off;
    IndustrialComponent indc;
    FoliageTracker fol;


    private void Start()
    {
        displayUI = GameObject.Find("UI").GetComponent<DisplayUI>();
    }

    public GameObject ReturnContainedBuilding()
    {
        return containedBuilding;
    }

    public void EnableBuilding()
    // Call this to spawn the building 
    {
        SizeForPlay();
        EnablePhysics();
        if (unit == 2)
        {
            DeselectBuilding();
        }
        if (containedType == 0)
        {
            EnableResidential();
        }
        else if(containedType == 1)
        {
            EnableCommercial();
        }
        else if(containedType == 2)
        {
            EnableIndustrial();
        }
        else if(containedType == 3)
        {
            EnableCommercial();
        }
        else if(containedType == 4)
        {
            EnableComponent();
        }
        else if(containedType == 5)
        {
            EnableFoliage();
        }

        // Spawns new from here!!!!!
        showingBuilding = false;
        spawnManager.SpawnUIBuildings(displayUI.GetSelection(), unit + thumb.angleIncrement);
    }

    void SelectBuilding()
    // TODO: Display building stats in displayMenu here
    // TODO: Start slow building rotation here
    {

    }

    void DeselectBuilding()

    {

    }

    public void DisableBuilding(SpawnManager sm, GameObject newBuilding)
    {
        if(!spawnManager)
        {
            spawnManager = sm;
        }
        UpdateContainedBuilding(newBuilding);

        DisablePhysics();
        SizeForMenu();

        SetTracker();
        if(unit == 2)
        {
            SelectBuilding();
        }
    }

    public bool Empty()
    {
        return !showingBuilding;
    }

    void EnableResidential()
    {
        res.usable = true;
    }

    void EnableCommercial()
    {
        com.usable = true;
    }

    void EnableIndustrial()
    {
        ind.usable = true;
    }

    void EnableComponent()
    {
        indc.usable = true;
    }

    void EnableFoliage()
    {
        fol.usable = true;
    }

    void DisablePhysics()
    {
        containedBuilding.transform.parent = this.transform;
        Rigidbody rb = containedBuilding.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    void EnablePhysics()
    {
        containedBuilding.transform.parent = null;
        Rigidbody rb = containedBuilding.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    void UpdateContainedBuilding(GameObject newBuilding)
    {
        menuSelection = displayUI.GetSelection();
        containedBuilding = newBuilding;

        containedRenderer = containedBuilding.GetComponent<Renderer>();
        containedType = menuSelection;
    }

    void SetTracker()
    {
        ClearTrackers();
        if (containedType == 0)
        {
            res = containedBuilding.GetComponent<ResidentialTracker>();
            Debug.Log("Res: " + res);
            res.usable = false;
        }
        else if(containedType == 1)
        {
            com = containedBuilding.GetComponent<CommercialTracker>();
            com.usable = false;
        }
        else if(containedType == 2)
        {
            ind = containedBuilding.GetComponent<IndustrialTracker>();
            ind.usable = false;
        }
        else if(containedType == 3)
        {
            off = containedBuilding.GetComponent<CommercialTracker>();
            off.usable = false;
        }
        else if(containedType == 4)
        {
            indc = containedBuilding.GetComponent<IndustrialComponent>();
            indc.usable = false;
        }
        else if(containedType == 5)
        {
            fol = containedBuilding.GetComponent<FoliageTracker>();
            fol.usable = false;
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
    }

    void SizeForMenu()
    {
        Vector3 size = containedRenderer.bounds.size;
        if (size.x > size.y && size.x > size.z)
        {
            // size x 
            scaleFactor = size.x;
        }
        else if (size.y > size.x && size.y > size.z)
        {
            // size y
            scaleFactor = size.y;
        }
        else if (size.z > size.x && size.z > size.y)
        {
            // size z
            scaleFactor = size.z; 
        }
        containedBuilding.transform.localScale *= (1 / scaleFactor) * 0.5f;
    }

    void SizeForPlay()
    {
        containedBuilding.transform.localScale *= scaleFactor * 1f / 0.75f;
    }
}
