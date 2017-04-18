using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Sphere : MonoBehaviour
{
    GameObject parent;
    bool linked;

    public void LinkSphere(GameObject connectObject)
    {
        parent = connectObject;
        linked = true;
        StartCoroutine("AdjustPosition");
    }

    public void UnlinkSphere()
    {
        Debug.Log("Trying to finish");
        parent = null;
        linked = false;
    }

    public IEnumerator AdjustPosition()
    {
        while(linked)
        {
            transform.position = new Vector3(parent.transform.position.x, 10.1f, parent.transform.position.z);
            transform.rotation = Quaternion.identity;
            yield return null;
        }
    }
}