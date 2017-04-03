using VRTK;
using VRTK.GrabAttachMechanics;
using UnityEngine;

public class SpawnerCube : MonoBehaviour
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
		GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>().AliasGrabOff += 
            new ControllerInteractionEventHandler(DoRemoveObject);
    }

    void Update()
    // Runs trigger and child checks
    {
        if (currentObject == null)
        {
            currentObject = displayMenu.InitiateSpawn(gameObject);
            Debug.Log(currentObject);
        }
        else if (transform.childCount > 1)
        {
            transform.DetachChildren();
            currentObject.transform.parent = gameObject.transform;
        }
        else if(TriggerCheck(0))
        {
            transform.DetachChildren();
            currentObject = displayMenu.InitiateSpawn(gameObject);
        }
        currentObject.transform.LookAt(mainCamera.transform);
    }

    void DoRemoveObject(object sender, ControllerInteractionEventArgs e)
    // Removes parents if leavingObject
    {
        if(waitRemove == true)
        {
            leavingObject.transform.parent = null;
            EnablePhysics(leavingObject);
            VRTK_ChildOfControllerGrabAttach grab = leavingObject.AddComponent<VRTK_ChildOfControllerGrabAttach>() as VRTK_ChildOfControllerGrabAttach;
            Destroy(leavingObject.GetComponent<VRTK_FixedJointGrabAttach>());
            grab.precisionGrab = true;
            leavingObject.GetComponent<ItemTracker>().SetUsable();
            waitRemove = false;
        }
	}

	void OnTriggerEnter()
	{
		++count;
	}

	void OnTriggerExit(Collider other)
    // Runs when an object leaves cube trigger. Activates new building spawn
	{
		--count;
		if(other.gameObject == currentObject && (other.gameObject.tag == "residential" || other.gameObject.tag == "commercial" || other.gameObject.tag == "industrial" || other.gameObject.tag == "foliage" || other.gameObject.tag == "leisure"))
		{
            Debug.Log("Running prematurely");
            leavingObject = other.gameObject;
            currentObject = displayMenu.InitiateSpawn(gameObject);
			leavingObject.layer = 0;
            waitRemove = true; // Initiates remove tracker, used when controller releases building in DoRemoveObject()
		}
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
