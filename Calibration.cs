using System.Collections;
using UnityEngine;
using VRTK;

public class Calibration : MonoBehaviour {

  GameObject rightController;
  GameObject leftController;
  float avgHeight;
  GameObject cube;

  void Awake()
  {
    cube = GameObject.Find("CalibrationCube");
    rightController = GameObject.Find("RightController");
    leftController = GameObject.Find("LeftController");
    rightController.GetComponent<VRTK_ControllerEvents>().TriggerClick += new ControllerInteractionEventHandler(DoTriggerClick);
  }

  void Update()
  {
    avgHeight = (leftController.transform.position.y + rightController.transform.position.y) / 2;
    cube.transform.position.y = avgHeight;
  }

  void DoTriggerClick(object sender, ControllerInteractionEventArgs e)
  {
    SetHeight(avgHeight);
  }

  public void SetHeight(float avgHeight)
  {
    //TODO: send avgHeight to main scene, alter [CameraRig] by avgHeight.y
  }
}
