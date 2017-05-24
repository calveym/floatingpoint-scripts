using System.Collections;
using VRTK;
using UnityEngine;
using UnityEngine.Events;

public class FoliageTracker : SphereObject {

    float affectAmount;
    static ItemManager itemManager;
    public HappinessAffector happinessAffector;

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
}
