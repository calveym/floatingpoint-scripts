using UnityEngine;

public class CommercialOverlay : BuildingTooltipBase
{

    int numVisitors;
    TextMesh visitorText;

    protected override void Start()
    {
        base.Start();
        tooltipPrefab = ReferenceManager.instance.commercialTooltipPrefab;
        tracker = GetComponent<CommercialTracker>();
    }

    protected override void UpdateReferences()
    {
        base.UpdateReferences();
        visitorText = tooltip.transform.Find("VisitorText").GetComponent<TextMesh>();
    }

    protected override void UpdateText()
    {
        base.UpdateText();
        visitorText.text = numVisitors.ToString();
    }

    protected override void UpdateValues()
    {
        base.UpdateValues();
        numVisitors = tracker.visitors;
    }
}
