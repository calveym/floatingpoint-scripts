using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RoadDeleter : VRTK_InteractableObject {

	GameObject controller;
	RoadGenerator roadGenerator;

	void Start ()
	{
		controller = GameObject.Find("Controller (left)");
		roadGenerator = GameObject.Find("Island").GetComponent<RoadGenerator>();
	}

	public override void StartUsing (GameObject usingObject)
	{
		base.StartUsing (usingObject);
		Physics.Raycast (controller.transform.position, controller.transform.forward, out RaycastHit hit, 1000.0f);
		if (hit.transform.gameObject.tag == "road")
		{
			roadGenerator.RemoveRoad(roadGenerator.Round(hit.point));
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
}
