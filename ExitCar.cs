using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ExitCar : MonoBehaviour {

	public VRTK_ControllerEvents events;
	public GameObject mainCamera;
    public GameObject mainLeftController;
    public GameObject mainRightController;
    public GameObject carCamera;

    void OnEnable()
	{ 
		events.ApplicationMenuPressed += new ControllerInteractionEventHandler(ExitVehicle);

	}

	void OnDisable()
	{
		events.ApplicationMenuPressed -= new ControllerInteractionEventHandler(ExitVehicle);
	}

	void ExitVehicle(object sender, ControllerInteractionEventArgs e)
    {
        mainCamera.SetActive(true);
        mainLeftController.SetActive(true);
        mainRightController.SetActive(true);
        carCamera.SetActive(false);    
	}
}
