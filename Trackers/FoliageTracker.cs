using System.Collections;
using VRTK;
using UnityEngine;
using UnityEngine.Events;
using System;

public class FoliageTracker : SphereObject {

    float affectAmount;
    private ItemManager itemManager;
    [Tooltip("Object's happiness affector")]
    public HappinessAffector happinessAffector;
    [Tooltip("Material to set sphere to during grab action")]
    public Material sphereMaterial;

    VRTK_InteractableObject interact;


    protected override void Awake()
    {
        base.Awake();
    }

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
        affectAmount = happinessAffector.affectAmount;
        itemManager.addFoliage((int)affectAmount, gameObject);
    }

    protected override void SetSphereMaterial()
    {
        sphereScript.SetSphereMaterial(sphereMaterial);
    }
}
