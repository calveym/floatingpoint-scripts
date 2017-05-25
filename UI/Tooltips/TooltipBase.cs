using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TooltipBase : MonoBehaviour {

    protected TooltipManager tooltipManager;
    protected GameObject tooltip;
    protected bool referencesUpdated;

    protected virtual void Awake()
    {

    }

	protected virtual void Start()
    {
        tooltipManager = ReferenceManager.instance.tooltipManager;
        referencesUpdated = false;
    }

    public virtual void EnableTooltip(Transform stareat)
    {
        if (tooltip != null)
        {
            Destroy(tooltip);
        }
        tooltip = Instantiate(GameObject.Find("Tooltip"), gameObject.transform);
        tooltip.transform.position = gameObject.transform.position + new Vector3(0f, 4f, 0f);
        tooltip.transform.LookAt(2 * transform.position - stareat.position);
        tooltipManager.updateTooltips += UpdateValues;
        UpdateValues();
    }

    public virtual void DisableTooltip()
    {
        tooltipManager.updateTooltips -= UpdateValues;
        Destroy(tooltip.gameObject);
        referencesUpdated = false;
    }

    protected abstract void UpdateText();

    protected virtual void UpdateValues()
    {
        if (referencesUpdated == false)
        {
            referencesUpdated = true;
            UpdateReferences();
        }
        if (referencesUpdated == true)
        {
            UpdateText();
        }
    }

    protected abstract void UpdateReferences();
}
