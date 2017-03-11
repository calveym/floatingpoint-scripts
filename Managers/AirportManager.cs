using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirportManager : MonoBehaviour {

    GameObject airport;
    public bool active;

    void Start()
    {
        airport = GameObject.Find("Airport");
    }

    void StartService()
    // TODO
    {
        active = true;
        // Start animations based on demand
    }

}
