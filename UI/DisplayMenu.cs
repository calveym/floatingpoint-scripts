using UnityEngine;
using UnityEngine.UI;
using VRTK;
using System.Collections;

public class DisplayMenu : MonoBehaviour
{

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

    private void Start()
    // Sets listeners and deactivates all panels at start
    {
        events = GetComponent<VRTK_ControllerEvents>();
        GetComponent<VRTK_ControllerEvents>().TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouchStart);
        GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);
        itemGenerator = GameObject.Find("LeftController").GetComponent<ItemGenerator>();
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
		// Debug.Log(Model1.transform.parent);
    }

    int GetPressedButton(float position)
    {
        if (position < -0.33f)
        {
            return 1;
        }
        else if (-0.33f <= position && position < 0.33f)
        {
            return 2;
        }
        else if (position >= 0.66f)
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
            Vector2 position = SteamVR_Controller.Input(2).GetAxis();
            
            if(GetPressedButton(position.x) != pressedButton)
            {
                ButtonPressed(GetPressedButton(position.x));
                SwapModels();
            }
            yield return null;
        }
    }

    void DeactivateAll()
    // Deactivates all UI elements
    {
		// Deactivates
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
    // TODO: create model swapping logic
    {
        Destroy(Model1.GetComponent<SpawnerCube>().currentObject);
        Destroy(Model2.GetComponent<SpawnerCube>().currentObject);
        Destroy(Model3.GetComponent<SpawnerCube>().currentObject);
        Destroy(Model4.GetComponent<SpawnerCube>().currentObject);
        Destroy(Model5.GetComponent<SpawnerCube>().currentObject);
    }


}
