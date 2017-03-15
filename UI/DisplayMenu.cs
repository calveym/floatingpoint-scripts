using UnityEngine;
using UnityEngine.UI;
using VRTK;
using System.Collections;

public class DisplayMenu : MonoBehaviour
{
    int tier;

    // Add the three panels that make up the buttons
    public GameObject panel;
    public GameObject Model1;
    public GameObject Model2;
    public GameObject Model3;
    public GameObject Model4;
    public GameObject Model5;
    public GameObject Clipboard;

    VRTK_ControllerEvents events;
    ItemGenerator itemGenerator;
    int previousPressedButton;
    int pressedButton;
    public GameObject wireframeModels;
    public GameObject controllerLeft;

    private delegate int GetPressedButton(float position);
    GetPressedButton getPressedButton;

    private void Start()
    // Sets listeners and deactivates all panels at start
    {
        SetTier(3);
        events = GetComponent<VRTK_ControllerEvents>();
        GetComponent<VRTK_ControllerEvents>().TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouchStart);
        GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);
        itemGenerator = GameObject.Find("LeftController").GetComponent<ItemGenerator>();
        Debug.Log(getPressedButton(1.9f));
        DeactivateAll();
    }

    void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
    // TouchpadTouched event
    {
        ActivatePanel();
    }


    void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
    // TouchpadReleased event
    {
        DeactivateAll();
    }

    void ActivatePanel()
    // Activates panel and models
    {
        ButtonPressed(1);
        StartCoroutine("UpdateTouchpadAxis");
        Model1.SetActive(true);
        Model2.SetActive(true);
        Model3.SetActive(true);
        Model4.SetActive(true);
        Model5.SetActive(true);
        Clipboard.SetActive(true);
    }
    
    public void SetTier(int newTier)
    {
        tier = newTier;
        if(newTier == 1)
        {
            getPressedButton = TierOnePosition;
        }
        else if(newTier == 2)
        {
            getPressedButton = TierTwoPosition;
        }
        else if(newTier == 3)
        {
            getPressedButton = TierThreePosition;
        }
    }

    int TierOnePosition(float position)
    {
        return 1;
    }

    int TierTwoPosition(float position)
    {
        if(position < 0f)
        {
            return 1;
        }
        else if(position >= 0)
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }

    int TierThreePosition(float position)
    {
        if (position < -0.33f)
        {
            return 1;
        }
        else if (-0.33f <= position && position < 0.33f)
        {
            return 2;
        }
        else if (position >= 0.33f)
        {
            return 3;
        }
        else
        {
            return 0;
        }
    }

    IEnumerator UpdateTouchpadAxis()
    // Coroutine for updating touchpad axis. Runs once per frame.
    {
        while (events.touchpadTouched)
        {
            //Vector2 position = SteamVR_Controller.Input(2).GetAxis();
            Vector2 position = events.GetTouchpadAxis();

            if (getPressedButton(position.x) != pressedButton)
            {
                ButtonPressed(getPressedButton(position.x));
                SwapModels();
            }
            yield return null;
        }
    }

    void DeactivateAll()
    // Deactivates all UI elements
    {
        panel.SetActive(false);
        Model1.SetActive(false);
        Model2.SetActive(false);
        Model3.SetActive(false);
        Model4.SetActive(false);
        Model5.SetActive(false);
        Clipboard.SetActive(false);
    }

    public void ButtonPressed(int button)
    {
        pressedButton = button;
        panel.SetActive(true);
    }

    public GameObject InitiateSpawn(GameObject initiator)
    // Adds pressedButton to spawn logic
    {
        return itemGenerator.StartSpawn(pressedButton, initiator);
    }

    void SwapModels()
    {
        Destroy(Model1.GetComponent<SpawnerCube>().currentObject);
        Destroy(Model2.GetComponent<SpawnerCube>().currentObject);
        Destroy(Model3.GetComponent<SpawnerCube>().currentObject);
        Destroy(Model4.GetComponent<SpawnerCube>().currentObject);
        Destroy(Model5.GetComponent<SpawnerCube>().currentObject);
    }
}
