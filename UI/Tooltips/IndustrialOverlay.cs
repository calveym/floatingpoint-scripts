using UnityEngine;
using System.Collections;

public class IndustrialOverlay : BuildingTooltipBase
{

    int numVisitors;
    float productionMulti;
    TextMesh productionText;

    protected override void Start()
    {
        base.Start();
        tracker = GetComponent<IndustrialTracker>();
        tooltipPrefab = ReferenceManager.instance.industrialTooltipPrefab;
    }

    protected override void UpdateReferences()
    {
        base.UpdateReferences();
        productionText = tooltip.transform.Find("ProductionText").GetComponent<TextMesh>();
    }

    protected override void UpdateText()
    {
        base.UpdateText();
        productionText.text = productionMulti.ToString();
    }

    protected override void UpdateValues()
    {
        base.UpdateValues();
        productionMulti = tracker.productionMulti;
    }
}
