using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;

public class FoliageTracker : MonoBehaviour {

    VRTK_InteractableObject interact;
    public bool working;

    public void StartFoliage()
    {
        StartCoroutine("FoliageTick");
    }
    
    public void UpgradeBuildings()
    {
        Vector3 center = transform.position;
        float radius = 1.5f;
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

    public IEnumerator FoliageTick()
    {
        while(working)
        {
            UpgradeBuildings();
            yield return new WaitForSeconds(10);
        }
    }
}
