using System.Collections;
using UnityEngine;

public class Calibration : MonoBehaviour {

  void Awake()
  {
    GameObject.Find("Right Controller").GetComponent<VRTK_ControllerEvents>().TriggerClick += new ControllerInteractionEventHandler(DoTriggerClick);
  }

  void DoTriggerClick()
  {
    
  }
}
