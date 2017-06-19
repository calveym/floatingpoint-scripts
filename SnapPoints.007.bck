using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoints : MonoBehaviour {
	
	public Dictionary<string, Vector3> originalPoints;
	public GameObject snapCube;

	public Bounds bounds;

	void Start () {
		originalPoints = GetCurrentPoints ();
        if(gameObject.tag == "road")
    		bounds = GetComponent<BoxCollider> ().bounds;
		originalPoints.Add ("center", bounds.center);
	}

    public void SetupBounds(Bounds newBounds)
    {
        if(gameObject.layer == 8)
        {
            bounds = newBounds;
        }
    }

	public Dictionary<string, Vector3> GetCurrentPoints () {
		Vector3 center = GetComponent<BoxCollider> ().bounds.center;

		float yAxis = transform.position.y, halfWidth = (bounds.size.x / 2), halfDepth = (bounds.size.z / 2);
			
		Dictionary<string, Vector3> currentPoints = new Dictionary<string, Vector3>()
		{
			{ "top", new Vector3(center.x, yAxis, center.z - halfDepth) },
			{ "right", new Vector3(center.x - halfWidth, yAxis, center.z) },
			{ "bottom", new Vector3(center.x, yAxis, center.z + halfDepth) },
			{ "left", new Vector3(center.x  + halfWidth, yAxis, center.z) },
		};

		/* 
		currentPoints.Add ("topRight", new Vector3 (currentPoints["right"].x, yAxis, currentPoints["top"].z));
		currentPoints.Add ("topLeft", new Vector3 (currentPoints["left"].x, yAxis, currentPoints["top"].z));
		currentPoints.Add ("bottomRight", new Vector3 (currentPoints ["right"].x, yAxis, currentPoints ["bottom"].z));
		currentPoints.Add ("bottomLeft", new Vector3 (currentPoints ["left"].x, yAxis, currentPoints ["bottom"].z));
		*/

		List<string> keys = new List<string> (currentPoints.Keys);

		foreach (string key in keys) {
			currentPoints[key] = RotatePointAroundPivot (currentPoints[key], transform.position, transform.eulerAngles);
			//GameObject g1 = (GameObject)Instantiate (snapCube, currentPoints[key], transform.rotation);

		}
			
		return currentPoints;
	}

	private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
		Vector3 dir = point - pivot; // get point direction relative to pivot
		dir = Quaternion.Euler(angles) * dir; // rotate it
		point = dir + pivot; // calculate rotated point
		return point; // return it
	}
		
}
