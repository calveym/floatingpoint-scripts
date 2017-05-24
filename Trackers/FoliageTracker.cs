using System.Collections;
using VRTK;
using UnityEngine;
using UnityEngine.Events;
using System;

public class FoliageTracker : ComponentSnap {

    [Range(0, 10)]
    int affectAmount;
    [Tooltip("Object's happiness affector")]
    public HappinessAffector happinessAffector;
    [Tooltip("Material to set sphere to during grab action")]
    public Material sphereMaterial;

    private ItemManager itemManager;


    protected override void Start()
    {
        base.Start();
        if(!itemManager)
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
        happinessAffector.affectAmount = affectAmount;
    }
}
