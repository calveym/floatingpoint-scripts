using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using Autelia.Serialization;

public abstract class SphereObject : VRTK_InteractableObject {

    // Manager declarations
    protected EconomyManager economyManager;

    protected bool grabbed;
    GameObject spherePrefab;
    protected GameObject sphere;
    public Sphere sphereScript;  // Attached sphere script

    [Header("Main settings")]
    [Space(10)]
    [Tooltip("Identifies if building is active")]
    public bool active;
    [Range(0f, 50f)]
    [Tooltip("Range of service effect in global units")]
    public int radius;  // Radius of effect

    protected virtual void Start()
    {if (Serializer.IsLoading)	return;
        spherePrefab = ReferenceManager.instance.spherePrefab;
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

    public bool SphereAttached()
    {
        return sphere;
    }

    public virtual void AttachSphere()
    {
        if(!spherePrefab)
        {
            spherePrefab = ReferenceManager.instance.spherePrefab;
        }
        sphere = Instantiate(spherePrefab, new Vector3(transform.position.x, 10.1f, transform.position.z), Quaternion.identity);
        sphere.transform.localScale = new Vector3(radius, 0.1f, radius);
        sphereScript = sphere.GetComponent<Sphere>();
        sphereScript.LinkSphere(gameObject);
        SetSphereMaterial();
    }

    public virtual void DetachSphere()
    {
        sphere.GetComponent<Sphere>().UnlinkSphere();
    }

    protected abstract void SetSphereMaterial();
}
