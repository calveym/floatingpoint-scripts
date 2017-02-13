using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToOffice : MonoBehaviour
{

    public bool atOffice;

    private void Start()
    {
        atOffice = false;
    }

    private void Update()
    {
    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "[MayorsOffice]")
        {
            atOffice = true;
            GameObject.Find("[CameraRig]").transform.position += new Vector3(0f, -110f, 0f);
        }
        else if (other.gameObject.name == "OfficeDoor")
        {
            atOffice = false;
            GameObject.Find("[CameraRig]").transform.position += new Vector3(0f, 110f, 0f);
        }
    }
}
