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

        spherePrefab = GameObject.Find("SpherePrefab");
        objectUsed = false;
    }

    public void ComponentGrabStart(object sender, ControllerInteractionEventArgs e)
    {
        if (IsGrabbed() == true)
        {
            objectUsed = true;
            AttachSphere();
        }
    }
    
    public void ComponentGrabRelease(object sender, ControllerInteractionEventArgs e)
    {
        if (objectUsed == true)
        {
            objectUsed = false;
            DetachSphere();
            GetNearestBuilding();
            if(nearestBuilding)
            {
                ClosestFound(nearestBuilding);
            }
        }
    }

	public void GetNearestBuilding()
    {
        hitColliders = Physics.OverlapSphere(transform.position, 1.5f, 8);

        if (hitColliders.Length == 0)
        {
            nearestBuilding = null;
        }
        else
        {
            foreach (Collider hitcol in hitColliders)
            {
                if(gameObject.tag == "industrial")
                {
                    potentialTracker = hitcol.gameObject.GetComponent<IndustrialTracker>();
                    if (potentialTracker != null && hitcol != GetComponent<Collider>() && potentialTracker.level == component.level)
                    {
                        nearestBuilding = hitcol.gameObject;
                    }
                }
                else if (gameObject.tag == "foliage")
                {
                    nearestBuilding = hitcol.gameObject;
                }
            }
        }
    }

    void AttachSphere()
    {
        sphere = Instantiate(spherePrefab, new Vector3(transform.position.x, 10.1f, transform.position.z), Quaternion.identity);
        if (gameObject.tag == "foliage")
        {
            sphere.GetComponent<Renderer>().material = foliageMaterial;
        }
        if (gameObject.tag == "industrial")
        {
            sphere.GetComponent<Renderer>().material = industrialMaterial;
        }
        sphere.GetComponent<Sphere>().LinkSphere(gameObject);
    }

    void DetachSphere()
    {
        sphere.GetComponent<Sphere>().UnlinkSphere();
        Destroy(sphere.gameObject);
    }

    void ClosestFound(GameObject closest)
    {
        if(component.economyManager.GetBalance() >= component.cost)
        {
            if(closest.tag == "industrial")
            {
                component.LinkComponent(potentialTracker);
                PurchaseComponent();
            }
            else if(closest.tag == "foliage")
            {
                PurchaseComponent();
                closest.gameObject.GetComponent<FoliageTracker>().StartFoliage();
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
        Pay(component.cost);
    }

    IEnumerator MarkIndustrial()
    {
        while (objectUsed)
        {

            yield return null;
        }
    }
}
