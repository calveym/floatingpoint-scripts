using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MayorsOffice : MonoBehaviour
{

    public bool atOffice;

    void Start()
    {
        atOffice = false;
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "GameController")
        {
            TeleportToMayorsOffice();
        }
    }

    void TeleportToMayorsOffice()
    {
        Debug.Log("Runninging");
        atOffice = true;
        GameObject.Find("[CameraRig]").transform.position = new Vector3(-11.1f, 0f, 63.8f);
    }
}
