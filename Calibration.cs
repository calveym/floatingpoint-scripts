using System.Collections;
using UnityEngine;
using VRTK;

public class Calibration : MonoBehaviour {

  void Awake()
  {
    GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().TriggerClicked += new ControllerInteractionEventHandler(DoTriggerClick);
  }

  void DoTriggerClick(object sender, ControllerInteractionEventArgs e)
  {
    
  }
}
