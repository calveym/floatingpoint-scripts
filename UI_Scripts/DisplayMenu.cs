
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using VRTK;

    public class DisplayMenu : MonoBehaviour
    {
    public VRTK_ControllerEvents events;
    public GameObject panel;

    private void Start()
    {
        panel.SetActive(false);
    }

    void OnEnable()
    { 
            events.TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouched);
            events.TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadNotTouched);

    }

    void OnDisable()
    {
        events.TouchpadTouchStart -= new ControllerInteractionEventHandler(DoTouchpadTouched);
        events.TouchpadTouchEnd -= new ControllerInteractionEventHandler(DoTouchpadNotTouched);
    }




    void DoTouchpadTouched(object sender, ControllerInteractionEventArgs e)
    {
        panel.SetActive(true);
    }


    void DoTouchpadNotTouched(object sender, ControllerInteractionEventArgs e)
    {
        panel.SetActive(false);
    }

}


