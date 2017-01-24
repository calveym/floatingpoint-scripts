using UnityEngine;
using UnityEngine.UI;
using VRTK;

    public class DisplayMenu : MonoBehaviour
    {
    // Add the three panels that make up the buttons    
    public GameObject panel;
    public GameObject Model1;
    public GameObject Model2;
    public GameObject Model3;
    public GameObject Model4;
    public GameObject Model5;

    public Button B1;
    public Button B2;
    public Button B3;

    VRTK_ControllerEvents events;
    int pressedButton;

    private void Start()
    // Sets listeners and deactivates all panels at start
    {
        deactivateAll();
        GetComponent<VRTK_ControllerEvents>().TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouchStart);
        GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);
        events = GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>();
    }

    void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
    // TouchpadTouched event
    {
        Vector2 position = GetComponent<VRTK_ControllerEvents>().GetTouchpadAxis();
        activatePanel(position);
    }


    void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
    // TouchpadReleased event
    {
        Vector2 position = events.GetTouchpadAxis();
        deactivateAll();
    }

    void activatePanel(Vector2 position)
    // Activates panel and models
    {
        Model1.SetActive(true);
        Model2.SetActive(true);
        Model3.SetActive(true);
        Model4.SetActive(true);
        Model5.SetActive(true);

        Debug.Log(position);

        if (position.x < 0.33f)
        {
            buttonOnePressed();
        }
        else if (0.33f <= position.x && position.x < 0.66f)
        {
            buttonTwoPressed();
        }
        else if (position.x >= 0.66f)
        {
            buttonThreePressed();
        }
    }

    void deactivateAll()
    // Deactivates all UI elements   
    {
        panel.SetActive(false);
        Model1.SetActive(false);
        Model2.SetActive(false);
        Model3.SetActive(false);
        Model4.SetActive(false);
        Model5.SetActive(false);
        pressedButton = 0;
    }

    public void buttonOnePressed()
    {
        Debug.Log("Button 1 pressed");
        pressedButton = 1;
        B1.Select();
        panel.SetActive(true);
    }

    public void buttonTwoPressed()
    {
        Debug.Log("Button 2 pressed");
        pressedButton = 2;
        B2.Select();
        panel.SetActive(true);
    }

    public void buttonThreePressed()
    {
        Debug.Log("Button 3 pressed");
        pressedButton = 3;
        B3.Select();
        panel.SetActive(true);
    }
}


