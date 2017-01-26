
 ﻿using VRTK;
using UnityEngine;

public class SpawnerCube : MonoBehaviour
{
    DisplayMenu displayMenu;
    string modelName;
	int count;

    void Start()
    {
		count = 0;
        displayMenu = GameObject.Find("LeftController").GetComponent<DisplayMenu>();
        modelName = gameObject.name;
		displayMenu.InitiateSpawn(gameObject);
    }

	void OnTriggerEnter()
	{
		++count;
	}

	void OnTriggerExit(Collider other)
	{
		--count;
		if(IsTriggerEmpty() && (other.gameObject.tag == "residential" || other.gameObject.tag == "commercial" || other.gameObject.tag == "industrial" || other.gameObject.tag == "foliage" || other.gameObject.tag == "leisure"))
		{
			Debug.Log(other.gameObject.tag);

			EnablePhysics(other.gameObject);
			displayMenu.InitiateSpawn(gameObject);
			other.gameObject.layer = 0;

		}
	}

	void EnablePhysics(GameObject building)
	{
		building.GetComponent<Rigidbody>().useGravity = true;
		building.GetComponent<Rigidbody>().isKinematic = false;
		building.GetComponent<VRTK_InteractableObject>().isGrabbable = true;
	}

	bool IsTriggerEmpty()
	{
		return count == 0;
	}
}
