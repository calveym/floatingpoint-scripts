using System.Collections;
using System.Collections.Generic;
using VRTK;
using UnityEngine;

public abstract class ComponentSnap : SphereObject {

    [Range(0, 10000)]
    [Tooltip("Component purchase cost")]
    public int buyCost;  // Purchase cost of component

    // Snapping variables
    Vector3 startPosition;
    Vector3 endPosition;

    Quaternion startRotation;
    Vector3 initialEndRotation;

    [Range(0, 1)]
    float snapAmount;
    bool targetClear;  // Target location clear

    protected override void Grab()
    {
        base.Grab();

        targetClear = false;
    }

    protected override void Ungrab()
    {
        base.Ungrab();

        InitiateSnap();
    }

    void InitiateSnap()
    // Starts process of snapping to ground
    {
        if (CheckAvailableSpace() && transform.position.y <= 12)
        {
            startPosition = transform.position;
            endPosition = new Vector3(startPosition.x, 10.01f, startPosition.z);
            startRotation = transform.rotation;
            targetClear = true;
            snapAmount = 0;
            U.DisablePhysics(gameObject);
            StartCoroutine("Snap");
        }
    }

    bool CheckAvailableSpace()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 5f);
        if (hit.collider.gameObject.name == "Island")
        {
            return true;
        }
        else return false;
    }

    IEnumerator Snap()
    // Lerps object
    {
        while(targetClear && snapAmount <= 1)
        {
            snapAmount += 0.05f;
            Debug.Log("Start rotation: " + startRotation);
            Debug.Log("IDentity: " + Quaternion.identity);
            if(snapAmount <= 0.9f)
            {
                transform.rotation = Quaternion.Slerp(startRotation, Quaternion.identity, snapAmount + 0.1f);
            }
            transform.position = Vector3.Lerp(startPosition, endPosition, snapAmount);
            yield return null;
        }
        if(snapAmount >= 1)
        {
            U.EnablePhysics(gameObject);
        }
    }
}
