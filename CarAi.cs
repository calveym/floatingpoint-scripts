using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAi : MonoBehaviour {

	public float speed = 0.5f;
	private float rotationSpeed = 10.0f;
	private Transform TargetNode;
	private Transform PrevNode;
	RaycastHit hit = new RaycastHit();

	// Update is called once per frame
	void Update () {
		Vector3 dir = TargetNode.position - transform.position;
		Quaternion newLookRotation = Quaternion.LookRotation(dir);

		if (Physics.Raycast (transform.position, Vector3.forward, out hit)) {
			if (hit.collider.gameObject.tag == "Car") {
				// Debug.Log ("hit");
			}
		}
		transform.position = Vector3.MoveTowards (transform.position, TargetNode.position, Time.deltaTime * speed);
		transform.rotation = Quaternion.Slerp (transform.rotation, newLookRotation, rotationSpeed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider Col)

    {
        switch(Col.gameObject.name)
        {
            case "StraightLeftStart":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild("StraightLeftEnd");
                break;
            case "StraightRightStart":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild("StraightRightEnd");
                break;
            case "TurnRightStart":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild("TurnRight");
                break;
            case "TurnRight":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild("TurnRightEnd");
                break;
            case "TurnLeftStart":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild("TurnLeft");
                break;
            case "TurnLeft":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild("TurnLeftEnd");
                break;
            case "TSectionRightStraightStart":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild("TSectionTurnNodeRSS");
                PrevNode = Col.gameObject.transform.parent.gameObject.transform.FindChild("TSectionRightStraightStart");
                break;
            case "TSectionLeftStraightStart":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild("TSectionTurnNodeLSS");
                PrevNode = Col.gameObject.transform.parent.gameObject.transform.FindChild("TSectionLeftStraightStart");
                break;
            case "TSectionRightStart":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild("TSectionTurnNodeRS");
                PrevNode = Col.gameObject.transform.parent.gameObject.transform.FindChild("TSectionRightStart");
                break;
            case "TSectionTurnNodeRS":
                if(PrevNode.name == "TSectionRightStart")
                {
                    int num = Random.Range(0, 2);
                    if (num == 0)
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild("TSectionLeftStraightEnd");
                        PrevNode = null;
                    }
                    else
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild("TSectionRightStraightEnd");
                        PrevNode = null;
                    }
                }
                break;
            case "TSectionTurnNodeRSS":
                if(PrevNode.name == "TSectionRightStraightStart")
                {
                    int num = Random.Range(0, 2);
                    if (num == 0)
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild("TSectionLeftEnd");
                        PrevNode = null;
                    }
                    else
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild("TSectionRightStraightEnd");
                        PrevNode = null;
                    }
                }
                break;
            case "TSectionTurnNodeLSS":
                if (PrevNode.name == "TSectionLeftStraightStart")
                {
                    int num = Random.Range(0, 2);
                    if (num == 0)
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild("TSectionLeftStraightEnd");
                        PrevNode = null;
                    }
                    else
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild("TSectionLeftEnd");
                        PrevNode = null;
                    }
                }
                break;
                case ""
            default:
                Destroy(gameObject);
                break;
        }



        













		// Intersection

		if (Col.gameObject.name == "InterRightVerticalStart") {
			TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild ("TopLeftTurnNode");
			PrevNode = Col.gameObject.transform.parent.gameObject.transform.FindChild ("InterRightVerticalStart");
		}

		if (Col.gameObject.name == "InterLeftVerticalStart") {
			TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild ("BottomRightTurnNode");
			PrevNode = Col.gameObject.transform.parent.gameObject.transform.FindChild ("InterLeftVerticalStart");
		}

		if (Col.gameObject.name == "InterRightHorizontalStart") {
			TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild ("TopRightTurnNode");
			PrevNode = Col.gameObject.transform.parent.gameObject.transform.FindChild ("InterRightHorizontalStart");
		}

		if (Col.gameObject.name == "InterLeftHorizontalStart") {
			TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild ("BottomLeftTurnNode");
			PrevNode = Col.gameObject.transform.parent.gameObject.transform.FindChild ("InterLeftHorizontalStart");
		}

		if (Col.gameObject.name == "TopLeftTurnNode" && PrevNode.name == "InterRightVerticalStart") {
			int num = Random.Range (0, 3);
			if (num == 0) {
				TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild ("InterRightVerticalEnd");
				PrevNode = null;
			} else if (num == 1) {
				TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild ("InterRightHorizontalEnd");
				PrevNode = null;
			} else {
				TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild ("InterLeftHorizontalEnd");
				PrevNode = null;
			}
		}

		if (Col.gameObject.name == "TopRightTurnNode" && PrevNode.name == "InterRightHorizontalStart") {
			int num = Random.Range (0, 3);
			if (num == 0) {
				TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild ("InterRightVerticalEnd");
				PrevNode = null;
			} else if (num == 1) {
				TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild ("InterRightHorizontalEnd");
				PrevNode = null;
			} else {
				TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild ("InterLeftVerticalEnd");
				PrevNode = null;
			}
		}

		if (Col.gameObject.name == "BottomRightTurnNode" && PrevNode.name == "InterLeftVerticalStart") {
			int num = Random.Range (0, 3);
			if (num == 0) {
				TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild ("InterLeftVerticalEnd");
				PrevNode = null;
			} else if (num == 1) {
				TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild ("InterRightHorizontalEnd");
				PrevNode = null;
			} else {
				TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild ("InterLeftHorizontalEnd");
				PrevNode = null;
			}
		}

		if (Col.gameObject.name == "BottomLeftTurnNode" && PrevNode.name == "InterLeftHorizontalStart") {
			int num = Random.Range (0, 3);
			if (num == 0) {
				TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild ("InterRightVerticalEnd");
				PrevNode = null;
			} else if (num == 1) {
				TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild ("InterLeftVerticalEnd");
				PrevNode = null;
			} else {
				TargetNode = Col.gameObject.transform.parent.gameObject.transform.FindChild ("InterLeftHorizontalEnd");
				PrevNode = null;
			}
		}
	}

}
