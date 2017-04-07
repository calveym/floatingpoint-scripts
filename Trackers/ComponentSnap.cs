using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;

public class ComponentSnap : RoadSnap {

    IndustrialComponent component;
    IndustrialTracker potentialTracker;
    Material tempMaterial;

    void Start()
    {
        component = GetComponent<IndustrialComponent>();

        // Add listeners for controller release to both controllers
        GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().AliasGrabOff +=
            new ControllerInteractionEventHandler(DoGrabRelease);
        GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>().AliasGrabOff +=
            new ControllerInteractionEventHandler(DoGrabRelease);
    }

    void DoGrabRelease(object sender, ControllerInteractionEventArgs e)
    {
        if (objectUsed == true)
        {
            if (!targetIsBlocked)
            {
                component.LinkComponent(potentialTracker);
                objectToPlace = gameObject;
                checkForNearbyBuilding();
            }
            objectUsed = false;
        }
    }

	public void getNearbyBuildings()
    {
        hitColliders = Physics.OverlapSphere(transform.position, 1.5f, layerMask);

        if (hitColliders.Length == 0)
        {
            nearestBuilding = null;
            Destroy(targetBox);
        }
        else
        {
            foreach (Collider hitcol in hitColliders)
            {
                potentialTracker = hitcol.gameObject.GetComponent<IndustrialTracker>();
                if (potentialTracker != null && hitcol != GetComponent<Collider>() && potentialTracker.level == component.level)
                {
                    BuildingFound(hitcol);
                }
            }
        }
    }

    void BuildingFound(Collider hitcol)
    {
        if(component.economyManager.GetBalance() >= component.cost)
        {
            component.gameObject.GetComponent<Renderer>().material = tempMaterial; // messy
            PurchaseComponent();
            component.FoundIndustrial(hitcol.gameObject);
            setToNearestBuilding(hitcol);

            closestTargetSnapPoint = getClosestTargetSnapPoint();
            closestSnapPoint = getClosestSnapPoint();

            useCornerSnapPoints = shouldUseCornerSnapPoints();
            drawTargetBox();
        }
        else
        {
            NotEnoughBalance();
        }
        
    }

    void NotEnoughBalance()
    {
        tempMaterial = component.gameObject.GetComponent<Renderer>().material;
        component.gameObject.GetComponent<Renderer>().material = blockedMaterial;
    }

    void PurchaseComponent()
    {
        component.economyManager.ChangeBalance(component.cost);
    }
}
