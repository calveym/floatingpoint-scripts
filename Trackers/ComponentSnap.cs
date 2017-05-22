using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;

public class ComponentSnap : VRTK_InteractableObject {

    public Material blockedMaterial;
    public Material foliageMaterial;
    public Material industrialMaterial;

    GameObject spherePrefab;
    GameObject sphere;

    IndustrialComponent component;
    EconomyManager economyManager;
    IndustrialTracker potentialTracker;
    Material tempMaterial;
    GameObject nearestBuilding;
    public bool objectUsed;
    VRTK_InteractableObject interact;
    Collider[] hitColliders;

    void Start()
    {
        component = GetComponent<IndustrialComponent>();
        // Adds listeners for controller grab to both controllers
        GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().AliasGrabOn +=
            new ControllerInteractionEventHandler(ComponentGrabStart);
        GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>().AliasGrabOn +=
            new ControllerInteractionEventHandler(ComponentGrabStart);

        // Add listeners for controller release to both controllers
        GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().AliasGrabOff +=
            new ControllerInteractionEventHandler(ComponentGrabRelease);
        GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>().AliasGrabOff +=
            new ControllerInteractionEventHandler(ComponentGrabRelease);

        economyManager = ReferenceManager.instance.economyManager;
        spherePrefab = GameObject.Find("SpherePrefab");
        objectUsed = false;
    }

    public void ComponentGrabStart(object sender, ControllerInteractionEventArgs e)
    {
        if (IsGrabbed() == true)
        {
            objectUsed = true;
            AttachSphere();
            StartCoroutine("MarkIndustrial");
        }
    }
    
    public void ComponentGrabRelease(object sender, ControllerInteractionEventArgs e)
    {
        if (objectUsed == true)
        {
            if(sphere)
            {
                Debug.Log("Trying to detach");
                DetachSphere();
            }
            if(gameObject.tag == "industrialComponent")
            {
                nearestBuilding = U.ReturnIndustrialTrackers(U.FindNearestBuildings(transform.position, 5f))[0].gameObject;
            }
            else if(gameObject.tag == "foliage")
            {
                nearestBuilding = U.ReturnResidentialTrackers(U.FindNearestBuildings(transform.position, 5f))[0].gameObject;
            }
            if(nearestBuilding)
            {
                ClosestFound(nearestBuilding);
            }
            objectUsed = false;
        }
    }

    void AttachSphere()
    {
        sphere = Instantiate(spherePrefab, new Vector3(transform.position.x, 10.1f, transform.position.z), Quaternion.identity);
        if (gameObject.tag == "foliage")
        {
            sphere.GetComponent<Renderer>().material = foliageMaterial;
        }
        if (gameObject.tag == "industrialComponent")
        {
            sphere.GetComponent<Renderer>().material = industrialMaterial;
        }
        sphere.GetComponent<Sphere>().LinkSphere(gameObject);
    }

    void DetachSphere()
    {
        Debug.Log("Sphere: " + sphere);
        sphere.GetComponent<Sphere>().UnlinkSphere();
        Destroy(sphere.gameObject);
    }

    void ClosestFound(GameObject closest)
    {
        if(economyManager.GetBalance() >= component.buyCost)
        {
            if(closest.tag == "industrial")
            {
                PurchaseComponent();
            }
        }
        else
        {
            NotEnoughBalance();
        }
    }

    void Pay(float amount)
    {
        EconomyManager.ChangeBalance(0 - amount);
    }

    void NotEnoughBalance()
    {
        tempMaterial = component.gameObject.GetComponent<Renderer>().material;
        component.gameObject.GetComponent<Renderer>().material = blockedMaterial;
    }

    void PurchaseComponent()
    {
        component.LinkComponent(nearestBuilding.GetComponent<IndustrialTracker>());
        Pay(component.buyCost);
    }

    IEnumerator MarkIndustrial()
    {
        while (objectUsed)
        {
            List<GameObject> surroundingBuildings = U.FindNearestBuildings(transform.position, 1f);
            List<IndustrialTracker> surroundingIndustrial = U.ReturnIndustrialTrackers(surroundingBuildings);
            if(surroundingIndustrial.Count >= 1)
            {
                Debug.Log("Component: " + component);
                Debug.Log("Surrounding industrial: " + surroundingIndustrial.Count);
                component.FoundIndustrial(surroundingIndustrial[0].gameObject);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
