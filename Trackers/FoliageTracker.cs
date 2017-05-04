using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;

public class FoliageTracker : VRTK_InteractableObject {

    VRTK_InteractableObject interact;
    GameObject spherePrefab;
    GameObject sphere;
    Vector3 oldPosition;
    bool working;
    public bool usable;
    bool grabbed;

    void Start()
    {
        spherePrefab = GameObject.Find("SpherePrefab");
        GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().AliasGrabOn +=
            DoFoliageGrabStart;
        GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>().AliasGrabOn +=
            DoFoliageGrabStart;

        // Add listeners for controller release to both controllers
        GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().AliasGrabOff +=
            DoFoliageGrabEnd;
        GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>().AliasGrabOff +=
            DoFoliageGrabEnd;

        usable = true;
    }
    
    public void DoFoliageGrabStart(object sender, ControllerInteractionEventArgs e)
    {
        if(IsGrabbed())
        {
            grabbed = true;
            AttachSphere();
        }
    }

    public void DoFoliageGrabEnd(object sender, ControllerInteractionEventArgs e)
    {
        if(grabbed)
        {
            DetachSphere();
            oldPosition = transform.position;
            working = true;
            StartCoroutine("FoliageTick");
        }
    }

    void AttachSphere()
    {
        sphere = Instantiate(spherePrefab, new Vector3(transform.position.x, 10.1f, transform.position.z), Quaternion.identity);
        sphere.GetComponent<Sphere>().LinkSphere(gameObject);
    }

    void DetachSphere()
    {
        sphere.GetComponent<Sphere>().UnlinkSphere();
        Destroy(sphere.gameObject);
    }

    public void UpgradeBuildings()
    {
        Vector3 center = transform.position;
        float radius = 5f;
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if(hitColliders[i].tag == "residential")
            {
                hitColliders[i].gameObject.GetComponent<ResidentialTracker>().ModifyHappiness(3f, "foliage");
            }
            i++;
        }
    }

    float Size()
    {
        return transform.localScale.x * transform.localScale.y * transform.localScale.z;
    }

    public IEnumerator FoliageTick()
    {
        while(working && usable)
        {
            UpgradeBuildings();
            yield return new WaitForSeconds(10);
        }
    }
}
