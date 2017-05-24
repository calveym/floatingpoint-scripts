using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public abstract class SphereObject : VRTK_InteractableObject {

    // Manager declarations
    protected EconomyManager economyManager;

    protected bool grabbed;
    GameObject spherePrefab;
    GameObject sphere;
    protected Sphere sphereScript;  // Attached sphere script

    [Header("Main settings")]
    [Space(10)]
    [Range(0f, 50f)]
    [Tooltip("Range of service effect in global units")]
    public float radius = 10;  // Radius of effect

    protected virtual void Start()
    {
        spherePrefab = GameObject.Find("SpherePrefab");
        economyManager = ReferenceManager.instance.economyManager;
        InteractableObjectGrabbed += new InteractableObjectEventHandler(DoGrabStart);
        InteractableObjectUngrabbed += new InteractableObjectEventHandler(DoGrabEnd);
    }

    public virtual void DoGrabStart(object sender, InteractableObjectEventArgs e)
    {
        grabbed = true;
        Grab();
    }

    public virtual void DoGrabEnd(object sender, InteractableObjectEventArgs e)
    {
        if (grabbed)
        {
            Ungrab();
        }
    }

    protected virtual void Grab()
    {
        AttachSphere();
    }

    protected virtual void Ungrab()
    {
        grabbed = false;
        DetachSphere();
    }

    protected virtual void AttachSphere()
    {
        sphere = Instantiate(spherePrefab, new Vector3(transform.position.x, 10.1f, transform.position.z), Quaternion.identity);
        sphere.transform.localScale = new Vector3(radius, 0.1f, radius);
        sphereScript = sphere.GetComponent<Sphere>();
        sphereScript.LinkSphere(gameObject);
        SetSphereMaterial();
    }

    protected abstract void SetSphereMaterial();

    protected virtual void DetachSphere()
    {
        sphere.GetComponent<Sphere>().UnlinkSphere();
    }
}
