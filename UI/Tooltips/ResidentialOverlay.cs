using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidentialOverlay : BuildingTooltipBase
{

    int numEmployed;
    TextMesh employmentText;

    protected override void UpdateReferences()
    {
        base.UpdateReferences();
        employmentText = tooltip.transform.Find("EmploymentText").GetComponent<TextMesh>();
    }

    protected override void UpdateText()
    {
        base.UpdateText();
        employmentText.text = numEmployed.ToString();
    }

    protected override void UpdateValues()
    {
        base.UpdateValues();
        numEmployed = tracker.users - tracker.unemployedPopulation;
    }
}
