using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SphereObject : VRTK_InteractableObject {

    protected bool grabbed;
    GameObject spherePrefab;
    GameObject sphere;
    Vector3 oldPosition;

    [Header("Main settings")]
    [Space(10)]
    [Range(0f, 50f)]
    [Tooltip("Range of service effect in global units")]
    public float radius = 10;  // Radius of effect

    protected virtual void Start()
    {
        spherePrefab = GameObject.Find("SpherePrefab");

        InteractableObjectGrabbed += new InteractableObjectEventHandler(DoGrabStart);
        InteractableObjectUngrabbed += new InteractableObjectEventHandler(DoGrabEnd);
    }

    public virtual void DoGrabStart(object sender, InteractableObjectEventArgs e)
    {
        grabbed = true;
        AttachSphere();
    }

    public virtual void DoGrabEnd(object sender, InteractableObjectEventArgs e)
    {
        if (grabbed)
        {
            grabbed = false;
            DetachSphere();
            oldPosition = transform.position;
        }
    }

    protected virtual void AttachSphere()
    {
        sphere = Instantiate(spherePrefab, new Vector3(transform.position.x, 10.1f, transform.position.z), Quaternion.identity);
        sphere.transform.localScale = new Vector3(radius, 0.1f, radius);
        sphere.GetComponent<Sphere>().LinkSphere(gameObject);
    }

    protected virtual void DetachSphere()
    {
        sphere.GetComponent<Sphere>().UnlinkSphere();
    }
}
