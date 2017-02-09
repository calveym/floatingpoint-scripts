using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DrawRoad : MonoBehaviour {

    public List<Vector3> hitPositions;
    VRTK_ControllerEvents rightControllerEvents;
    VRTK_ControllerEvents leftControllerEvents;

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
        Vector2[] points = ProcessHitPositions(hitPositions);
        RemoveHitPositions();
        GenerateMeshGeometry(points);
    }

    IEnumerator RecordHitPositions()
    // Coroutine for recording hit positions. Runs once every frame, records positions to list
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
	
    Vector2[] ProcessHitPositions(List<Vector3> hitPositions)
    {
        Vector2 pointOne; // Defines bezier start point
        Vector2 pointTwo; // Defines bezier start angle
        Vector2 pointThree; // Defines bezier end angle
        Vector2 pointFour; // Defines bezier end point

        if (hitPositions.Count >= 4)
        {
            pointOne = GeneratePointOne(hitPositions);
            pointTwo = GeneratePointTwo(hitPositions);
            pointThree = GeneratePointThree(hitPositions);
            pointFour = GeneratePointFour(hitPositions);
        }
        Vector2[] points = new Vector2[4];

        return points;
    }

    Vector2 GeneratePointOne(List<Vector3> hitPositions)
    {
        return hitPositions[0];
    }

    Vector2 GeneratePointTwo(List<Vector3> hitPositions)
    {
        return hitPositions[1];
    }

    Vector2 GeneratePointThree(List<Vector3> hitPositions)
    {
        return hitPositions[-2];
    }

    Vector2 GeneratePointFour(List<Vector3> hitPositions)
    {
        return hitPositions[-1];
    }

    void RemoveHitPositions()
    {

    }

    void GenerateMeshGeometry(Vector2[] points)
    {

    }
}
