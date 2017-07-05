using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CloudCity;

public class AddRoad : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.Find("Island").GetComponent<RoadGenerator>().AddRoad(transform.position, gameObject);
	}
}
