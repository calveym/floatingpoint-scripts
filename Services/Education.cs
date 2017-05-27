using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Education : ServiceBase
{

    List<EducationTracker> education;
    public delegate void AddLocalEducation();
    public AddLocalEducation addLocalEducation;


    protected void Awake()
    {
        education = new List<EducationTracker>();
    }

    protected override void ApplyGlobalDefecit()
    {
        //TODO:A
    }

    protected override void ApplyGlobalSurplus()
    {
        //TODO:B
    }

    public void AddEducation(EducationTracker incomingEducation)
    {
        education.Add(incomingEducation);
    }

    protected override void RunAddLocalAmounts()
    {
        if(addLocalEducation != null)
        {
            addLocalEducation();
        }
    }

    protected override void DeductCost()
    {
        economyManager.SetEducationExpense(cost);
    }

    protected override void ResetResidential()
    {
        foreach(GameObject res in itemManager.residential)
        {
            res.GetComponent<ItemTracker>().education = false;
        }
    }

    protected override void ResetCommercial()
    {
        foreach(GameObject com in itemManager.commercial)
        {
            com.GetComponent<ItemTracker>().education = false;
        }
    }

    protected override void ResetIndustrial()
    {
        foreach(GameObject ind in itemManager.industrial)
        {
            ind.GetComponent<ItemTracker>().education = false;
        }
    }
}
