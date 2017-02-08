using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DrawRoad : MonoBehaviour {

    public List<Vector3> hitPositions;
    VRTK_ControllerEvents rightControllerEvents;
    VRTK_ControllerEvents leftControllerEvents;

    Vector2 pointOne;
    Vector2 pointTwo;
    Vector2 pointThree;
    Vector2 pointFour;

    // Use this for initialization
    void Start () {
        hitPositions = new List<Vector3>();
        rightControllerEvents = GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>();

        rightControllerEvents.TriggerClicked +=
            new ControllerInteractionEventHandler(DoTriggerClick);
        rightControllerEvents.TriggerReleased +=
            new ControllerInteractionEventHandler(DoTriggerRelease);
    }

    void DoTriggerClick(object sender, ControllerInteractionEventArgs e)
    {
        StartCoroutine(RecordHitPositions());
    }

    void DoTriggerRelease(object sender, ControllerInteractionEventArgs e)
    {
        StopCoroutine(RecordHitPositions());
        ProcessHitPositions(hitPositions);
        RemoveHitPositions();
        GenerateMeshGeometry();
    }

    IEnumerator RecordHitPositions()
    {
        while (rightControllerEvents.triggerClicked)
        {
            RaycastHit hit;
            Physics.Raycast(rightControllerEvents.gameObject.transform.position, Vector3.forward, out hit);

            if (hit.transform.position.y == 100f) // makes sure that ray hits table
            {
                hitPositions.Add(hit.transform.position); // adds ray hit position to list
            }

            yield return null;
        }
    }
	
    void ProcessHitPositions(List<Vector3> hitPositions)
    {
        pointOne = GeneratePointOne(hitPositions);
        pointTwo = GeneratePointTwo(hitPositions);
        pointThree = GeneratePointThree(hitPositions);
        pointFour = GeneratePointFour(hitPositions);
    }

    Vector2 GeneratePointOne(List<Vector3> hitPositions)
    {
        return new Vector2(0f, 0f);
    }

    Vector2 GeneratePointTwo(List<Vector3> hitPositions)
    {
        return new Vector2(0f, 0f);
    }

    Vector2 GeneratePointThree(List<Vector3> hitPositions)
    {
        return new Vector2(0f, 0f);
    }

    Vector2 GeneratePointFour(List<Vector3> hitPositions)
    {
        return new Vector2(0f, 0f);
    }

    void RemoveHitPositions()
    {

    }

    void GenerateMeshGeometry()
    {

    }
}
