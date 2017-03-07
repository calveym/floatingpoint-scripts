using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawableSurface : MonoBehaviour {

    public int maxInkCount;

    GameObject ink;
    List<GameObject> allInk;
    GameObject penTip;
    public static bool drawable;

	// Use this for initialization
	void Start ()
    {
        if(maxInkCount == null)
        {
            maxInkCount = 40;
        }
        drawable = true;
        allInk = new List<GameObject>();
        ink = GameObject.Find("Ink");
        penTip = GameObject.Find("PenTip");
	}

<<<<<<< HEAD
	// Update is called once per frame
	void Update ()
=======
    // Update is called once per frame
    void Update()
>>>>>>> 2b6ca68f847a0ab182182b8e351c4c3386891310
    {
        if (allInk.Count >= maxInkCount && gameObject.tag == "contract")
        {
            gameObject.GetComponent<Contract>().Sign();
            allInk.Clear();
        }
	}

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject == penTip)
        {
            Debug.Log(other.gameObject);
            GameObject newInk = Instantiate(ink, penTip.transform.position, Quaternion.Euler(0, 0, 0));
            allInk.Add(newInk);
            if(allInk.Count >= 40)
            {
                allInk.RemoveAt(0);
            }
        }
    }
}
