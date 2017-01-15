using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[System.Serializable]
public class LoadWheelsInfo
{
    public WheelCollider leftWheel;
    public GameObject leftWheelMesh;
    public WheelCollider rightWheel;
    public GameObject rightWheelMesh;
    public bool motor;
    public bool steering;
}

public class CarController : MonoBehaviour 
{

    public List<LoadWheelsInfo> wheelsInfo;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public GameObject steeringWheel;
    public VRTK_ControllerEvents rightController;
    public VRTK_ControllerEvents leftController;

	private float motor = 0;
	private float steering = -5;

    public void FixedUpdate()
    {
    	motor = 0;
        steering = 0;

		setMotorTorque ();
		setRightSteering ();
		setLeftSteering();
		applyWheelMotions();
	}

	public void setMotorTorque()
	{
		if (rightController.triggerTouched)
		{
			motor = maxMotorTorque * (rightController.GetTriggerAxis() * 5);
		}

		if (leftController.triggerTouched)
		{
			motor = maxMotorTorque * -(leftController.GetTriggerAxis() * 5);
		}
	}

	public void setRightSteering()
	{
		float rightAngle = rightController.transform.localRotation.y;


		if (rightAngle > 0.57 && rightAngle < 0.7)
		{
			steering = (maxSteeringAngle * (rightController.transform.localRotation.y + 0.6f)) * 100 * 3 - 350;
            Debug.Log(maxSteeringAngle * (rightController.transform.localRotation.y + 0.6f));
        }

		if (rightAngle > 0.7)
		{
			steering = 39;
		}
	}

	public void setLeftSteering()
	{
		float leftAngle = leftController.transform.localRotation.y;

        if (leftAngle < 0.45 && leftAngle > 0.06)
		{
			steering = ((maxSteeringAngle * (leftController.transform.localRotation.y * -1 + 1.61f)) * 100 * 3 - 350) * -1;
            Debug.Log(leftAngle);
        }

		if (leftAngle < 0.05)
		{
			steering = -11.4f;
		}
	}

	public void rotateSteeringWheel()
	{
		Vector3 eulerAngles = transform.rotation.eulerAngles;
		eulerAngles = new Vector3(0, eulerAngles.y, steering * -1);
		steeringWheel.transform.rotation = Quaternion.Euler(eulerAngles);
	}

	public void ApplyLocalPositionToVisuals(LoadWheelsInfo wheelPair)
	{
		WheelCollider[] wheelColliders = new WheelCollider[] { wheelPair.leftWheel, wheelPair.rightWheel };
		GameObject[] wheelMeshes = new GameObject[] { wheelPair.leftWheelMesh, wheelPair.rightWheelMesh };

		for (int i = 0; i < wheelColliders.Length; i++) {
			Vector3 position;
			Quaternion rotation;
			wheelColliders[i].GetWorldPose(out position, out rotation);
			wheelMeshes[i].transform.position = position;
			wheelMeshes[i].transform.rotation = rotation;
			wheelMeshes[i].transform.Rotate(0, -90, 0);
		}
	}

	public void applyWheelMotions()
	{
		foreach (LoadWheelsInfo wheelPair in wheelsInfo)
		{
			if (wheelPair.steering == true)
			{
				wheelPair.leftWheel.steerAngle = steering;
				wheelPair.rightWheel.steerAngle = steering;
			}

			if (wheelPair.motor == true)
			{
				wheelPair.leftWheel.motorTorque = motor;
				wheelPair.rightWheel.motorTorque = motor;
			}

			ApplyLocalPositionToVisuals(wheelPair);
			rotateSteeringWheel();
		}
	}
}
