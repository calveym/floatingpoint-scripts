using VRTK;
using UnityEngine;

public class SpawnerCube : MonoBehaviour
{
    DisplayMenu displayMenu;
    string modelName;
	int count;
    GameObject recentBuilding;

    void Start()
    // Prepares state
    {
		count = 0;
        displayMenu = GameObject.Find("LeftController").GetComponent<DisplayMenu>();
        modelName = gameObject.name;
		recentBuilding = displayMenu.InitiateSpawn(gameObject);

		GameObject.Find("LeftController").GetComponent<VRTK_ControllerEvents>().GripReleased += new ControllerInteractionEventHandler(DoGripReleased);
    }

    void DoGripReleased(object sender, ControllerInteractionEventArgs e)
	{
	    transform.DetachChildren();
	}

	void OnTriggerEnter()
	{
		++count;
	}

	void OnTriggerExit(Collider other)
    // Runs when an object leaves cube trigger. Activates new building spawn
	{
		--count;
		if(IsTriggerEmpty() && (other.gameObject.tag == "residential" || other.gameObject.tag == "commercial" || other.gameObject.tag == "industrial" || other.gameObject.tag == "foliage" || other.gameObject.tag == "leisure"))
		{
			EnablePhysics(other.gameObject);
			recentBuilding = displayMenu.InitiateSpawn(gameObject);
			other.gameObject.layer = 0;
		}
	}

	void EnablePhysics(GameObject building)
    // Enable physics for newly spawned building
	{
		building.GetComponent<Rigidbody>().useGravity = true;
		building.GetComponent<Rigidbody>().isKinematic = false;
		building.GetComponent<VRTK_InteractableObject>().isGrabbable = true;
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
