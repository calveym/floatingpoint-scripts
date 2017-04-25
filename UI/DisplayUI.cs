using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUI : MonoBehaviour {

    GameObject staticSpheres;
    GameObject wheelBase;
    GameObject canvas;
    ThumbTracker thumbTracker;

    public Color baseColor;
    public Color residentialColor;
    public Color commercialColor;
    public Color industrialColor;
    public Color officeColor;
    public Color componentColor;
    public Color foliageColor;

    Image res;
    Image com;
    Image ind;
    Image off;
    Image indc;
    Image fol;

    int menuSelection;  // which of the 6 menu panels is selected, used in logic

    public bool showBuildings;
    public int showingBuildings;

    private void Awake()
    {
        showBuildings = false;
        staticSpheres = transform.Find("StaticSpheres").gameObject;
        wheelBase = transform.Find("WheelBase").gameObject;
        canvas = transform.Find("Canvas").gameObject;

        res = transform.Find("Canvas/Menu/Residential").gameObject.GetComponent<Image>();
        com = transform.Find("Canvas/Menu/Commercial").gameObject.GetComponent<Image>();
        ind = transform.Find("Canvas/Menu/Industrial").gameObject.GetComponent<Image>();
        off = transform.Find("Canvas/Menu/Office").gameObject.GetComponent<Image>();
        indc = transform.Find("Canvas/Menu/Office").gameObject.GetComponent<Image>();
        fol = transform.Find("Canvas/Menu/Office").gameObject.GetComponent<Image>();
    }

    private void Start()
    {
        thumbTracker = GetComponent<ThumbTracker>();
        HideUI();
    }

    void DoButtonClick()
    {
        showBuildings = !showBuildings;
        thumbTracker.ForceStopTrackingAngle();
        thumbTracker.ForceStopTrackingPosition();
        thumbTracker.StartTracking();
        if(showBuildings)
        {
            ShowBuildings();
        }
        else
        {
            ShowMenu();
        }
    }

    void ShowMenu()
    {
        HideBuildings();
        res.enabled = true;
        com.enabled = true;
        ind.enabled = true;
        off.enabled = true;
        indc.enabled = true;
        fol.enabled = true;
    }

    void HideMenu()
    {
        res.enabled = false;
        com.enabled = false;
        ind.enabled = false;
        off.enabled = false;
        indc.enabled = false;
        fol.enabled = false;
    }

    void ShowBuildings()
    {
        HideMenu();
        wheelBase.SetActive(true);
        staticSpheres.SetActive(true);
    }

    void HideBuildings()
    {
        wheelBase.SetActive(false);
        staticSpheres.SetActive(false);
    }

    public void SendSwipe(float swipe)
    {
        if(menuSelection < 5 && swipe > 0)
        {
            // swipe to the right
            menuSelection++;
            ResetMenuColors();
        }
        else if(menuSelection > 0 && swipe < 0)
        {
            // swipe left
            menuSelection--;
            ResetMenuColors();
        }
    }

    void ResetMenuColors()
    {
        switch(menuSelection)
        {
            case 0:
                SetToBaseColor();
                res.color = residentialColor;
                break;

            case 1:
                SetToBaseColor();
                com.color = commercialColor;
                break;

            case 2:
                SetToBaseColor();
                ind.color = industrialColor;
                break;

            case 3:
                SetToBaseColor();
                off.color = officeColor;
                break;

            case 4:
                SetToBaseColor();
                indc.color = officeColor;
                break;

            case 5:
                SetToBaseColor();
                fol.color = foliageColor;
                break;
        }
    }

    void SetToBaseColor()
    {
        res.color = baseColor;
        com.color = baseColor;
        ind.color = baseColor;
        off.color = baseColor;
        indc.color = baseColor;
        fol.color = baseColor;
    }

	public void ShowUI()
    {
        canvas.SetActive(true);
        wheelBase.SetActive(true);
        staticSpheres.SetActive(true);
    }

    public void HideUI()
    {
        canvas.SetActive(false);
        wheelBase.SetActive(false);
        staticSpheres.SetActive(false);
    }
}
