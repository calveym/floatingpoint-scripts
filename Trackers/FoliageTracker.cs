using System.Collections;
using VRTK;
using UnityEngine;
using UnityEngine.Events;
using System;
using Autelia.Serialization;

public class FoliageTracker : ComponentSnap {

    [Range(0, 10)]
    int affectAmount;
    [Tooltip("Object's happiness affector")]
    public HappinessAffector happinessAffector;
    [Tooltip("Material to set sphere to during grab action")]
    public Material sphereMaterial;

    private ItemManager itemManager;


    public int GetAffectAmount()
    {
        return affectAmount;
    }

    protected override void Start()
    {if (Serializer.IsLoading)	return;
        base.Start();
        if (!happinessAffector)
        {
            happinessAffector = GetComponent<HappinessAffector>();
            affectAmount = happinessAffector.affectAmount;
        }
        if (!itemManager)
        {
            itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();
        }
    }

    protected override void Ungrab()
    // Extends component snap ungrab method by add
    {
        base.Ungrab();
        EnableHappinessAffector();
        itemManager.addFoliage((int)affectAmount, gameObject);
    }

    protected override void SetSphereMaterial()
    {
        sphereScript.SetSphereMaterial(sphereMaterial);
    }

    void EnableHappinessAffector()
    {
        if(!happinessAffector)
        {
            happinessAffector = GetComponent<HappinessAffector>();
        }
        happinessAffector.enabled = true;
        happinessAffector.radius = radius;
    }
}
