using System.Collections;
using VRTK;
using UnityEngine;
using UnityEngine.Events;

public class FoliageTracker : VRTK_InteractableObject {

    float radius;
    float affectAmount;
    static ItemManager itemManager;
    public HappinessAffector happinessAffector;

    VRTK_InteractableObject interact;
    GameObject spherePrefab;
    GameObject sphere;
    Vector3 oldPosition;
    bool grabbed;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        if(!itemManager)
        {
            itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();
        }
        happinessAffector = GetComponent<HappinessAffector>();
        happinessAffector.enabled = true;
        radius = happinessAffector.radius;
        affectAmount = happinessAffector.affectAmount;
        itemManager.addFoliage((int)affectAmount, gameObject);
        spherePrefab = GameObject.Find("SpherePrefab");

        InteractableObjectGrabbed += new InteractableObjectEventHandler(DoFoliageGrabStart);
        InteractableObjectUngrabbed += new InteractableObjectEventHandler(DoFoliageGrabEnd);
    }

    public void DoFoliageGrabStart(object sender, InteractableObjectEventArgs e)
    {
        grabbed = true;
        AttachSphere();
    }

    public void DoFoliageGrabEnd(object sender, InteractableObjectEventArgs e)
    {
        if(grabbed)
        {
            grabbed = false;
            DetachSphere();
            oldPosition = transform.position;
        }
    }

    void AttachSphere()
    {
        sphere = Instantiate(spherePrefab, new Vector3(transform.position.x, 10.1f, transform.position.z), Quaternion.identity);
        sphere.transform.localScale = new Vector3(radius, 0.1f, radius);
        sphere.GetComponent<Sphere>().LinkSphere(gameObject);
    }

    void DetachSphere()
    {
        sphere.GetComponent<Sphere>().UnlinkSphere();
    }
}
