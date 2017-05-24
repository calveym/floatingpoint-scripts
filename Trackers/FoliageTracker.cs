using System.Collections;
using VRTK;
using UnityEngine;
using UnityEngine.Events;
using System;

public class FoliageTracker : ComponentSnap {

    [Tooltip("Amount of happiness to add, sets the same property on the happiness affector")]
    float affectAmount;
    private ItemManager itemManager;
    [Tooltip("Object's happiness affector")]
    public HappinessAffector happinessAffector;
    [Tooltip("Material to set sphere to during grab action")]
    public Material sphereMaterial;

    protected override void Start()
    {
        base.Start();
        if(!itemManager)
        {
            itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();
        }
        happinessAffector = GetComponent<HappinessAffector>();
        happinessAffector.enabled = true;
        radius = happinessAffector.radius;
        happinessAffector.affectAmount = affectAmount;
        itemManager.addFoliage((int)affectAmount, gameObject);
    }

    protected override void SetSphereMaterial()
    {
        sphereScript.SetSphereMaterial(sphereMaterial);
    }
}
