using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawableSurface : MonoBehaviour {

    GameObject ink;
    List<GameObject> allInk;
    GameObject penTip;


	// Use this for initialization
	void Start ()
    {
        allInk = new List<GameObject>();
        ink = GameObject.Find("Ink");
        penTip = GameObject.Find("PenTip");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (allInk.Count >= 100)
        {
            Destroy(allInk[0].gameObject);
            allInk.RemoveAt(0);
        }
	}

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == penTip)
        {
            GameObject newInk = Instantiate(ink, penTip.transform.position, Quaternion.Euler(0, 0, 0), gameObject.transform);
            allInk.Add(newInk);
        }
    }
}
