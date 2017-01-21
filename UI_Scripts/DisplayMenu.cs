    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using VRTK;

    public class DisplayMenu : MonoBehaviour
    {
    // Add the three panels that make up the buttons    
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;

    private void Start()
    // Sets listeners and deactivates all panels at start
    {
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);
        GetComponent<VRTK_ControllerEvents>().TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouchStart);
        GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);
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
        Vector2 position = GetComponent<VRTK_ControllerEvents>().GetTouchpadAxis();
        deactivatePanel(position);
    }

    void activatePanel(Vector2 position)
    // Activates correct panel
    {
        if (position.x < 0.33f)
        {
            panel1.SetActive(true);
        }
        else if (0.33f <= position.x && position.x < 0.66f)
        {
            panel2.SetActive(true);
        }
        else if (position.x >= 0.66f)
        {
            panel3.SetActive(true);
        }
    }

    void deactivatePanel(Vector2 position)
    // Deactivates all panels
    {
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(false);
    }
}


