using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class DisplayUI : MonoBehaviour {

    VRTK_ControllerEvents events;
    GameObject staticSpheres;
    GameObject wheelBase;
    GameObject canvas;
    ThumbTracker thumbTracker;

    GameObject menu;

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

    bool updateRequired;  // Used to trigger a change between buildings and menu
    bool displaying;  // controlls main Display coroutine
    public bool showBuildings;  // true if menu hidden and buildings shown
    public int showingBuildings;  // int referring to buildings category

    private void Awake()
    {
        showBuildings = false;
        menuSelection = 0;
        staticSpheres = transform.Find("StaticSpheres").gameObject;
        wheelBase = transform.Find("WheelBase").gameObject;
        canvas = transform.Find("Canvas").gameObject;

        menu = transform.Find("Canvas/Menu").gameObject;
        res = transform.Find("Canvas/Menu/Residential").gameObject.GetComponent<Image>();
        com = transform.Find("Canvas/Menu/Commercial").gameObject.GetComponent<Image>();
        ind = transform.Find("Canvas/Menu/Industrial").gameObject.GetComponent<Image>();
        off = transform.Find("Canvas/Menu/Office").gameObject.GetComponent<Image>();
        indc = transform.Find("Canvas/Menu/Component").gameObject.GetComponent<Image>();
        fol = transform.Find("Canvas/Menu/Foliage").gameObject.GetComponent<Image>();
    }

    private void Start()
    {
        thumbTracker = GetComponent<ThumbTracker>();
        events = GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>();
        events.TouchpadTouchStart += DoTouchpadTouch;
        events.TouchpadPressed += DoTouchpadPress;
        HideUI();
    }

    void DoTouchpadTouch(object sender, ControllerInteractionEventArgs e)
    {
        displaying = true;
        updateRequired = true;
        StartCoroutine("Display");
    }

    void DoTouchpadPress(object sender, ControllerInteractionEventArgs e)
    {
        showBuildings = !showBuildings;
        Debug.Log("Show buildings: " + showBuildings);
        thumbTracker.ForceStopTrackingAngle();
        thumbTracker.ForceStopTrackingPosition();
        thumbTracker.StartTracking();

        updateRequired = true;
    }

    void ShowMenu()
    {
        ShowUI();
        HideBuildings();
        menu.SetActive(true);
    }

    void HideMenu()
    {
        menu.SetActive(false);
    }

    void ShowBuildings()
    {
        ShowUI();
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
        if(menuSelection <= 4 && swipe > 0)
        {
            // swipe to the right
            menuSelection++;
            ResetMenuColors();
            updateRequired = true;
        }
        else if(menuSelection >= 1 && swipe < 0)
        {
            // swipe left
            menuSelection--;
            ResetMenuColors();
            updateRequired = true;
        }
        else
        {
            // if no valid swipe possible
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
                off.color = industrialColor;
                break;

            case 4:
                SetToBaseColor();
                indc.color = industrialColor;
                break;

            case 5:
                SetToBaseColor();
                fol.color = industrialColor;
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

    IEnumerator Display()
    {
        ResetMenuColors();
        while(displaying)
        {
            if (showBuildings && updateRequired)
            {
                ShowBuildings();
                updateRequired = false;
            }
            else if(!showBuildings && updateRequired)
            {
                ShowMenu();
                updateRequired = false;
            }
            yield return null;
        }
    }
}
