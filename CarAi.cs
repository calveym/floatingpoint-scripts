using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class CarAi : MonoBehaviour {

	public float speed = 0.5f;
	private float rotationSpeed = 10.0f;
	private Transform TargetNode;
	private Transform PrevNode;
	RaycastHit hit = new RaycastHit();

    Vector3 oldPosition;
    bool checkCar;
    bool firstTick;

    void Start()
    {
        checkCar = true;
        oldPosition = transform.position;
        firstTick = true;

        Autelia.Coroutines.CoroutineController.StartCoroutine(this, "CheckCar");
    }

	// Update is called once per frame
	void Update () {
        if(TargetNode)
        {
            Vector3 dir = TargetNode.position - transform.position;
            dir += new Vector3(0.001f, 0f, 0f);
            
            Quaternion newLookRotation = Quaternion.LookRotation(dir);

            if (Physics.Raycast(transform.position, Vector3.forward, out hit))
            {
                if (hit.collider.gameObject.tag == "Car")
                {
                    // Debug.Log ("hit");
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, TargetNode.position, Time.deltaTime * speed);
            if(transform.rotation != newLookRotation)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, newLookRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    void OnTriggerEnter(Collider Col)

    {
        switch (Col.gameObject.name)
        {
            case "StraightLeftStart":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("StraightLeftEnd");
                break;
            case "StraightRightStart":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("StraightRightEnd");
                break;
            case "TurnRightStart":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("TurnRight");
                break;
            case "TurnRight":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("TurnRightEnd");
                break;
            case "TurnLeftStart":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("TurnLeft");
                break;
            case "TurnLeft":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("TurnLeftEnd");
                break;
            case "TSectionRightStraightStart":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("TSectionTurnNodeRSS");
                PrevNode = Col.gameObject.transform.parent.gameObject.transform.Find("TSectionRightStraightStart");
                break;
            case "TSectionLeftStraightStart":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("TSectionTurnNodeLSS");
                PrevNode = Col.gameObject.transform.parent.gameObject.transform.Find("TSectionLeftStraightStart");
                break;
            case "TSectionRightStart":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("TSectionTurnNodeRS");
                PrevNode = Col.gameObject.transform.parent.gameObject.transform.Find("TSectionRightStart");
                break;
            case "TSectionTurnNodeRS":
                if (PrevNode.name == "TSectionRightStart")
                {
                    int num = Random.Range(0, 2);
                    if (num == 0)
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("TSectionLeftStraightEnd");
                        PrevNode = null;
                    }
                    else
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("TSectionRightStraightEnd");
                        PrevNode = null;
                    }
                }
                break;
            case "TSectionTurnNodeRSS":
                if (PrevNode.name == "TSectionRightStraightStart")
                {
                    int num = Random.Range(0, 2);
                    if (num == 0)
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("TSectionLeftEnd");
                        PrevNode = null;
                    }
                    else
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("TSectionRightStraightEnd");
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
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("TSectionLeftStraightEnd");
                        PrevNode = null;
                    }
                    else
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("TSectionLeftEnd");
                        PrevNode = null;
                    }
                }
                break;
            case "InterRightVerticalStart":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("TopLeftTurnNode");
                PrevNode = Col.gameObject.transform.parent.gameObject.transform.Find("InterRightVerticalStart");
                break;
            case "InterLeftVerticalStart":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("BottomRightTurnNode");
                PrevNode = Col.gameObject.transform.parent.gameObject.transform.Find("InterLeftVerticalStart");
                break;
            case "InterRightHorizontalStart":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("TopRightTurnNode");
                PrevNode = Col.gameObject.transform.parent.gameObject.transform.Find("InterRightHorizontalStart");
                break;
            case "InterLeftHorizontalStart":
                TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("BottomLeftTurnNode");
                PrevNode = Col.gameObject.transform.parent.gameObject.transform.Find("InterLeftHorizontalStart");
                break;
            case "TopLeftTurnNode":
                if (PrevNode.name == "InterRightVerticalStart")
                {
                    int num = Random.Range(0, 3);
                    if (num == 0)
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("InterRightVerticalEnd");
                        PrevNode = null;
                    }
                    else if (num == 1)
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("InterRightHorizontalEnd");
                        PrevNode = null;
                    }
                    else
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("InterLeftHorizontalEnd");
                        PrevNode = null;
                    }
                }
                break;
            case "TopRightTurnNode":
                if (PrevNode.name == "InterRightHorizontalStart")
                {
                    int num = Random.Range(0, 3);
                    if (num == 0)
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("InterRightVerticalEnd");
                        PrevNode = null;
                    }
                    else if (num == 1)
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("InterRightHorizontalEnd");
                        PrevNode = null;
                    }
                    else
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("InterLeftVerticalEnd");
                        PrevNode = null;
                    }
                }
                break;
            case "BottomRightTurnNode":
                if (PrevNode.name == "InterLeftVerticalStart")
                {
                    int num = Random.Range(0, 3);
                    if (num == 0)
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("InterLeftVerticalEnd");
                        PrevNode = null;
                    }
                    else if (num == 1)
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("InterRightHorizontalEnd");
                        PrevNode = null;
                    }
                    else
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("InterLeftHorizontalEnd");
                        PrevNode = null;
                    }
                }
                break;
            case "BottomLeftTurnNode":
                if (PrevNode.name == "InterLeftHorizontalStart")
                {
                    int num = Random.Range(0, 3);
                    if (num == 0)
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("InterRightVerticalEnd");
                        PrevNode = null;
                    }
                    else if (num == 1)
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("InterLeftVerticalEnd");
                        PrevNode = null;
                    }
                    else
                    {
                        TargetNode = Col.gameObject.transform.parent.gameObject.transform.Find("InterLeftHorizontalEnd");
                        PrevNode = null;
                    }
                }
                break;

            default:
                break;
        }
    }

    IEnumerator CheckCar()
    {
        while(checkCar)
        {
            if (Vector3.Distance(transform.position, oldPosition) <= 0.02f && !firstTick)
            {
                TrafficSpawner.instance.RemoveCar(gameObject);
            }
            firstTick = false;
            oldPosition = transform.position;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
