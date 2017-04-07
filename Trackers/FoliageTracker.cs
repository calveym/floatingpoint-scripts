using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;

public class FoliageTracker : MonoBehaviour {

    VRTK_InteractableObject interact;
    bool objectUsed;
    GameObject sphere;
    bool active;
    bool firstGrab;

	// Use this for initialization
	void Start () {
        firstGrab = true;
        // Adds listeners for controller grab to both controllers
        GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().AliasGrabOn +=
            new ControllerInteractionEventHandler(DoGrabStart);
        GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>().AliasGrabOn +=
            new ControllerInteractionEventHandler(DoGrabStart);

        // Add listeners for controller release to both controllers
        GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().AliasGrabOff +=
            new ControllerInteractionEventHandler(DoGrabRelease);
        GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>().AliasGrabOff +=
            new ControllerInteractionEventHandler(DoGrabRelease);

        sphere = GameObject.Find("FoliageSpherePrefab");
        interact = gameObject.GetComponent<VRTK_InteractableObject>();
        active = true;
    }

    void DoGrabStart(object sender, ControllerInteractionEventArgs e)
    // Grab start event listener
    {
        if (interact.IsGrabbed() == true)
        {
            AttachSphere();
            objectUsed = true;
            StartCoroutine("RotateSphere");
        }
    }

    void DoGrabRelease(object sender, ControllerInteractionEventArgs e)
    {
        if (objectUsed == true)
        {
            // Logic here
            StartCoroutine("FiveSecondTick");
            objectUsed = false;
        }
    }

    void AttachSphere()
    // Connects sphere prefab
    {
        sphere.transform.parent = gameObject.transform;
        sphere.transform.position = new Vector3(0f, 0f, 0f);
    }

    void ReleaseSphere()
    {
        sphere.transform.parent = null;
    }

    void UpgradeBuildings()
    {
        Vector3 center = transform.position;
        float radius = sphere.transform.localScale.x / 2;
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if(hitColliders[i].tag == "residential")
            {
                hitColliders[i].gameObject.GetComponent<ResidentialTracker>().AddFoliage(Size());
            }
            i++;
        }
    }

    float Size()
    {
        return transform.localScale.x * transform.localScale.y * transform.localScale.z;
    }

    IEnumerator FiveSecondTick()
    {
        while(active)
        {
            UpgradeBuildings();
            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator RotateSphere()
    {
        while(objectUsed)
        {
            sphere.transform.rotation = Quaternion.identity;
            yield return null;
        }
    }
}
