using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentSnap : RoadSnap {

	public void getNearbyBuildings()
    {
        hitColliders = Physics.OverlapSphere(transform.position, 1.5f, layerMask);

        if (hitColliders.Length == 0)
        {
            nearestBuilding = null;
            Destroy(targetBox);
        }
        else
        {
            foreach (Collider hitcol in hitColliders)
            {
                if (hitcol.gameObject.layer == buildingLayer && hitcol != GetComponent<Collider>())
                {

                    setToNearestBuilding(hitcol);

                    closestTargetSnapPoint = getClosestTargetSnapPoint();
                    closestSnapPoint = getClosestSnapPoint();

                    useCornerSnapPoints = shouldUseCornerSnapPoints();
                    // Debug.Log ("corner: " + useCornerSnapPoints);

                    drawTargetBox();

                    // Debug.Log ("FOUND HIT: " + nearestBuilding);

                }
                else
                {
                    // Debug.Log ("Not building: " + hitcol);
                }
            }
        }
    }
}
