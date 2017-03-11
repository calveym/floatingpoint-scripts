using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour {

    public bool active;
    GameObject train;

    void Start()
    {
        train = GameObject.Find("Train");
    }

    void StartService()
    // TODO
    {
        active = true;
        // Start animations based on demand
    }

}
