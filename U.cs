using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class U : object {

    public static List <GameObject> FindNearestBuildings (Vector3 position, float radius )
    {
        List<GameObject> returnObjects = new List<GameObject>();
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        Collider[] hitColliders = Physics.OverlapSphere(position, radius, layerMask);

        Debug.Log(hitColliders.Length);

        if (hitColliders.Length != 0)
        {
            foreach (Collider hitcol in hitColliders)
            {
                returnObjects.Add(hitcol.gameObject);
            }
        }
        else
        {
            returnObjects = null;
        }
        return returnObjects;
    }
}
