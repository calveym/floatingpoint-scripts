using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    // Small size residential buildings
    public GameObject res_a1_green;
    public GameObject res_a1_red;
    public GameObject res_a2_blue;
    public GameObject res_a2_red;
    public GameObject res_a3_red;
    public GameObject res_a3_tan;
    public GameObject res_a4_a;
    public GameObject res_a4_b;
    public GameObject res_a5;
    public GameObject res_a6_beige;
    public GameObject res_a6_blue;
    public GameObject res_a7_a;
    public GameObject res_a7_b;

    // Mid size residential buildings
    public GameObject res_b1_red;
    public GameObject res_b1_tan;
    public GameObject res_b2_orange;
    public GameObject res_b2_red;
    public GameObject res_b3_blue;
    public GameObject res_b3_brown;

    public static void Spawn(Vector3 targetPosition, int type, int unit)
    // Starts spawn process, enables static spawn calls
    {
        //GameObject.Find("Managers").GetComponent<SpawnManager>().StartSpawn(targetPosition, objectName);
    }

    void LoadAllBuildings()
    // Uses the name passed to the method to return the gameobject to instantiate
    {

    }
}
