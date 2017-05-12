using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Sphere : MonoBehaviour
{
    GameObject parent;
    MeshRenderer rend;
    bool linked;

    void Awake()
    {

    }

    private void Start()
    {
        rend = gameObject.GetComponent<MeshRenderer>();
    }

    public void LinkSphere(GameObject connectObject)
    {
        parent = connectObject;
        linked = true;
        StartCoroutine("AdjustPosition");
    }

    public void UnlinkSphere()
    {
        rend.enabled = false;
        parent = null;
        linked = false;
        Destroy(gameObject);
    }

    public IEnumerator AdjustPosition()
    {
        while(linked)
        {
            if(rend == null)
            {
                rend = gameObject.GetComponent<MeshRenderer>();
            }
            rend.enabled = true;
            transform.position = new Vector3(parent.transform.position.x, 10.1f, parent.transform.position.z);
            transform.rotation = Quaternion.identity;
            yield return null;
        }
    }
}