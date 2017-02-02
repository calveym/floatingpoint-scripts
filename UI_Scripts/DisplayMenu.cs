using UnityEngine;
using UnityEngine.UI;
using VRTK;
using System;

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
    int pressedButton;
	public GameObject wireframeModels;
	public GameObject controllerLeft;

    private void Start()
    // Sets listeners and deactivates all panels at start
    {
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
        Vector2 position = GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>().GetTouchpadAxis();
		// Debug.Log(GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>());

        Model1.SetActive(true);
        Model2.SetActive(true);
        Model3.SetActive(true);
        Model4.SetActive(true);
        Model5.SetActive(true);
        Clipboard.SetActive(true);

        if (position.x < -0.33f)
        {
            ButtonOnePressed();
        }
        else if (-0.33f <= position.x && position.x < 0.33f)
        {
            ButtonTwoPressed();
        }
        else if (position.x >= 0.66f)
        {
            ButtonThreePressed();
        }

		// Debug.Log(Model1.transform.parent);
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

    public void ButtonOnePressed()
    {
        // Debug.Log("Button 1 pressed");
        pressedButton = 1;
        SwapModels(1);
        panel.SetActive(true);
    }

    public void ButtonTwoPressed()
    {
        // Debug.Log("Button 2 pressed");
        pressedButton = 2;
        SwapModels(2);
        panel.SetActive(true);
    }

    public void ButtonThreePressed()
    {
        // Debug.Log("Button 3 pressed");
        pressedButton = 3;
        SwapModels(3);
        panel.SetActive(true);
    }

    public GameObject InitiateSpawn(GameObject initiator)
    // Adds pressedButton to spawn logic
    {
        return itemGenerator.StartSpawn(pressedButton, initiator);
    }

    void SwapModels(int modelNumber)
    // TODO: create model swapping logic
    {
    }


}
