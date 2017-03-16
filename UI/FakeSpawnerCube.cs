using VRTK;
using VRTK.GrabAttachMechanics;
using UnityEngine;

public class FakeSpawnerCube : MonoBehaviour
{
    DisplayMenu displayMenu;
    ItemManager itemManager;

    int count;
    bool waitRemove;
    public GameObject currentObject;
    GameObject leavingObject;
    GameObject mainCamera;

    void Start()
    // Prepares state
    {
        count = 0;
        displayMenu = GameObject.Find("LeftController").GetComponent<DisplayMenu>();
        itemManager = GameObject.Find("Managers").GetComponent<ItemManager>();

        mainCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
    }

    void Update()
    // Runs trigger and child checks
    {
        if (currentObject == null)
        {
            //currentObject = displayMenu.InitiateSpawn(gameObject);
        }
        else if (transform.childCount > 1)
        {
            transform.DetachChildren();
            currentObject.transform.parent = gameObject.transform;
        }
        else if (TriggerCheck(0))
        {
            transform.DetachChildren();
            currentObject = displayMenu.InitiateSpawn(gameObject);
        }
    }

    public void RemoveObject(GameObject leavingObject)
    // Removes parents if leavingObject
    {
        leavingObject.transform.parent = null;
        EnablePhysics(leavingObject);
        leavingObject.layer = 0;
        VRTK_ChildOfControllerGrabAttach grab = leavingObject.AddComponent<VRTK_ChildOfControllerGrabAttach>() as VRTK_ChildOfControllerGrabAttach;
        Destroy(leavingObject.GetComponent<VRTK_FixedJointGrabAttach>());
        grab.precisionGrab = true;
        leavingObject.GetComponent<ItemTracker>().SetUsable();
        waitRemove = false;
    }

    void OnTriggerEnter()
    {
        ++count;
    }

    void EnablePhysics(GameObject building)
    // Enable physics for newly spawned building
    {
        building.GetComponent<Rigidbody>().useGravity = true;
        building.GetComponent<Rigidbody>().isKinematic = false;
    }

    bool TriggerCheck(int numObjects)
    // returns true if number of objects in trigger is equal to numObjects
    {
        return count == numObjects;
    }

    bool IsTriggerEmpty()
    // returns true if cube area is empty
    {
        return count == 0;
    }
}
