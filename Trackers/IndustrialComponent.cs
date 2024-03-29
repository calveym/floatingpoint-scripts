using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autelia.Serialization;

[System.Serializable]
public class IndustrialComponent : ComponentSnap {

    [Header("Tracker functionality setup")]
    [Space(5)]

    [Range(0, 1)]
    [Tooltip("Production multiplier, added to total multiplier of industrials")]
    public float productionMulti;

    [Tooltip("Level at which item is unlocked")]
    public int level = 0;

    [Range(0, 100)]
    [Tooltip("ecoTick recurring cost")]
    public int baseCost;

    public bool usable;

    [Tooltip("Material to change sphere to on grab")]
    public Material industrialMaterial;

    IndustrialTracker linkedTracker;

    bool checkStop;
    Vector3 oldPosition;

    public delegate void StopCheck();
    public static StopCheck stopCheck;

    protected override void Start()
    {if (Serializer.IsDeserializing)	return;if (Serializer.IsLoading)	return;
        base.Start();

        checkStop = true;

Autelia.Coroutines.CoroutineController.StartCoroutine(this, "WaitForStop");
    }

    protected override void Ungrab()
    {
        base.Ungrab();

        checkStop = true;

Autelia.Coroutines.CoroutineController.StartCoroutine(this, "WaitForStop");
    }

    protected override void SetSphereMaterial()
    {
        sphereScript.SetSphereMaterial(industrialMaterial);
        
    }

    bool CheckStopped()
    // Returns if object moved since last check
    {
        if (transform.position != oldPosition)
        {
            oldPosition = transform.position;
            return false;
        }
        else return true;
    }

    void UnlinkIfMoving()
    // Method that is attached to delegate to check if stopped. If stopped, component is unlinked
    {
        if(!CheckStopped())
        {
            Unlink();
        }
    }

    void Link()
    // Saves industrial tracker link and informs tracker of bonus
    {
        stopCheck += UnlinkIfMoving;

        List<IndustrialTracker> surroundingIndustrials = U.ReturnIndustrialTrackers(U.FindNearestBuildings(transform.position, radius));
        if(surroundingIndustrials.Count >= 1)
        {
            linkedTracker = surroundingIndustrials[0];
            if(linkedTracker)
                linkedTracker.LinkComponent(this);
        }
    }

    void Unlink()
    // Unlinks component from industrial tracker and restarts linking process
    {
        stopCheck -= UnlinkIfMoving;
        linkedTracker.UnlinkComponent(this);

        checkStop = true;

Autelia.Coroutines.CoroutineController.StartCoroutine(this, "WaitForStop");
    }

    IEnumerator WaitForStop()
    // Checks position until stopped, links component once stopped.
    {
        while(checkStop)
        {
            if(CheckStopped())
            {
                checkStop = false;
                Link();
            }
        }
        yield return new WaitForSeconds(4.9f);
    }

    static IEnumerator StoppedCheck()
    // Runs delegate for checking that components haven't moved.
    {
        while(true)
        {
            if(stopCheck != null)
            {
                stopCheck();
            }
        }
    }
}
