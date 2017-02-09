using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawableSurface : MonoBehaviour {

    GameObject ink;
    List<GameObject> allInk;
    GameObject penTip;
    public static bool drawable;

	// Use this for initialization
	void Start ()
    {
        drawable = true;
        allInk = new List<GameObject>();
        ink = GameObject.Find("Ink");
        penTip = GameObject.Find("PenTip");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (allInk.Count >= 20 && gameObject.tag == "contract")
        {
            gameObject.GetComponent<Contract>().Sign();
        }
	}

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == penTip)
        {
            Debug.Log(other.gameObject);
            GameObject newInk = Instantiate(ink, penTip.transform.position, Quaternion.Euler(0, 0, 0));
            allInk.Add(newInk);
        }
    }
}
